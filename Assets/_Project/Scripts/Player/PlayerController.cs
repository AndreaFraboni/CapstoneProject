using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private NavMeshPath _navMeshPath;
    [SerializeField] private Camera _cam;

    [SerializeField] private float _walkingSpeed = 2f;
    [SerializeField] private float _runningSpeed = 6f;

    [SerializeField] private bool _walking = false;
    [SerializeField] private bool _running = false;

    [SerializeField] private LifeController _lifeController;

    [SerializeField] private PlayerInput _playerInput;

    private Animator _anim;

    private Ray _ray;
    private RaycastHit hit;

    public bool isAlive = true;

    private bool _deathStarted = false;

    private bool _clickForWalk = false;
    private bool _clickForRun = false;

    private void Awake()
    {
        if (_cam == null) _cam = Camera.main;
        if (_meshAgent == null) _meshAgent = GetComponent<NavMeshAgent>();
        if (_anim == null) _anim = GetComponentInChildren<Animator>();
        if (_lifeController == null) _lifeController = GetComponent<LifeController>();
    }

    private void OnEnable()
    {
        if (_lifeController != null) _lifeController.OnDefeated += OnDefeated;
    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnDefeated -= OnDefeated;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!isAlive) return;
        if (!context.started) return;
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) return;

        _clickForWalk = true;
    }

    public void OnDoubleClick(InputAction.CallbackContext context)
    {
        if (!isAlive) return;
        if (!context.performed) return;
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) return;

        _clickForRun = true;
    }

    private void Update()
    {
        if (!isAlive) return;
        if (!GameManager.Instance.IsPlaying()) return;
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.Paused) return;

        HandleMove();
        HandleAnimation();
    }

    private void HandleMove()
    {
        if (!_clickForWalk && !_clickForRun) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
        {
            _clickForWalk = false;
            _clickForRun = false;
            return;
        }

        _ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(_ray, out hit))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 2.0f, NavMesh.AllAreas))//https://docs.unity3d.com/540/Documentation/ScriptReference/NavMesh.SamplePosition.html
            {
                _walking = _clickForWalk;
                _running = _clickForRun;

                if (_clickForWalk) _meshAgent.speed = _walkingSpeed;
                if (_clickForRun) _meshAgent.speed = _runningSpeed;

                _meshAgent.ResetPath();
                _meshAgent.SetDestination(hit.point);
            }
        }
        _clickForWalk = false;
        _clickForRun = false;

    }

    private void HandleAnimation()
    {
        float speed = _meshAgent.velocity.magnitude;
        if (speed > 0.1f)
        {
            _anim.SetBool("walking", _walking);
            _anim.SetBool("running", _running);
        }

        if (speed < 0.1f)
        {
            _anim.SetBool("walking", false);
            _anim.SetBool("running", false);
        }
    }

    public void DestroyGOPlayer()
    {
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }

    private void StartDeathAnimation()
    {
        if (_deathStarted) return;

        _deathStarted = true;

        if (_meshAgent != null)
        {
            _meshAgent.isStopped = true;
            _meshAgent.ResetPath();
            _meshAgent.velocity = Vector3.zero;
            _meshAgent.enabled = false;
        }

        _walking = false;
        _running = false;
        _anim.SetBool("walking", false);
        _anim.SetBool("running", false);

        _anim.SetBool("isDying", true);
    }

    public void OnDefeated()
    {
        isAlive = false;
        AudioManager.Instance.PlaySFX("DeathSound");
        StartDeathAnimation();
    }
}
