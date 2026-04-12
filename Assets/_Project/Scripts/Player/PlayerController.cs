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

    private Animator _anim;

    private Ray _ray;
    private RaycastHit hit;

    private void Awake()
    {
        if (_cam == null) _cam = Camera.main;
        if (_meshAgent == null) _meshAgent = GetComponent<NavMeshAgent>();
        if (_anim == null) _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!GameUIManager.Instance.isPaused)
        {
            _ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(_ray, out hit))
            {
                _walking = true;
                _running = false;
                _meshAgent.speed = _walkingSpeed;
                _meshAgent.SetDestination(hit.point);
            }
        }
    }

    public void OnDoubleClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!GameUIManager.Instance.isPaused)
        {
            _ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(_ray, out hit))
            {
                _running = true;
                _walking = false;
                _meshAgent.speed = _runningSpeed;
                _meshAgent.SetDestination(hit.point);
            }
        }
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

    public void FootStepSound()
    {


    }


}
