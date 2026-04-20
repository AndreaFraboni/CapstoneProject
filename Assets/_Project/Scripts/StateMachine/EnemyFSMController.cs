using UnityEngine;
using UnityEngine.AI;

public class EnemyFSMController : MonoBehaviour
{
    [SerializeField] private BaseFSMState _initialState;
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private Transform _mainTarget;

    [SerializeField] private LayerMask _otherTargetsLayer;
    [SerializeField] private float _DetectionRadius = 6f;

    [SerializeField] private int _physicalDamage = 50;

    public EnemyHandHitBox enemyHandHitbox;

    private BaseFSMState _currentState;

    public NavMeshAgent agent;
    public Animator anim;

    bool _deathStarted = false;

    public bool isAlive = true;
    public bool IsAttacking = false;

    public Transform MainTarget => _mainTarget;
    public Transform CurrentTarget { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (_lifeController == null) _lifeController = GetComponent<LifeController>();

        if (enemyHandHitbox == null)
        {
            enemyHandHitbox = GetComponentInParent<EnemyHandHitBox>();
        }

        BaseFSMState[] states = GetComponentsInChildren<BaseFSMState>();

        foreach (var state in states)
        {
            state.Setup(this);

            foreach (var transition in state.GetTransitions())
            {
                transition.Setup(this);
            }
        }
    }

    private void OnEnable()
    {
        if (_lifeController != null) _lifeController.OnDefeated += OnDefeated;
    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnDefeated -= OnDefeated;
    }


    private void Start()
    {
        CurrentTarget = _mainTarget;

        if (enemyHandHitbox) enemyHandHitbox.physicalDamage = _physicalDamage;

        if (_initialState != null) ChangeState(_initialState);

        EnemiesManager.Instance.RegistEnemy(this);
    }

    private void Update()
    {
        if (_currentState == null || !isAlive) return;

        ValidateCurrentTarget();

        _currentState.StateUpdate();

        HandleAnimation();

        foreach (var t in _currentState.GetTransitions())
        {
            if (t.IsConditionMet())
            {
                ChangeState(t.GetTargetState());
                break;
            }
        }
    }

    private void HandleAnimation()
    {
        if (anim == null || agent == null) return;

        float speed = agent.velocity.magnitude;
        if (speed > 0.1f)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

    }

    public void ChangeState(BaseFSMState newState)
    {
        if (newState == null) return;
        if (_currentState == newState) return;

        if (_currentState != null)
            _currentState.OnStateExit();

        _currentState = newState;
        _currentState.OnStateEnter();
    }




    public void SetMainTarget(Transform target)
    {
        _mainTarget = target;
        if (CurrentTarget == null || CurrentTarget == _mainTarget) CurrentTarget = target;
    }
    public void SetCurrentTarget(Transform target)
    {
        if (target == null) return;
        CurrentTarget = target;
    }

    public void ResetToMainTarget()
    {
        CurrentTarget = MainTarget;
    }

    public bool HasCurrentTarget()
    {
        if (CurrentTarget == null) return false;
        return true;
    }

    public bool IsCurrentTargetMainTarget()
    {
        return CurrentTarget == MainTarget;
    }

    public void ValidateCurrentTarget()
    {
        if (CurrentTarget == null)
        {
            ResetToMainTarget();
            return;
        }
    }



    public void DestroyGOEnemy()
    {
        Destroy(gameObject);
    }

    private void StartDeathAnimation()
    {
        if (_deathStarted) return;

        isAlive = false;
        _deathStarted = true;
        IsAttacking = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.enabled = false;
        }

        if (anim != null)
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("walking", false);
            anim.SetBool("isDying", true);
        }
    }

    public void OnDefeated()
    {
        EnemiesManager.Instance.RemoveEnemy(this);
        AudioManager.Instance.PlaySFX("DeathSound");
        StartDeathAnimation();
    }

    public void StartPlayAttackAnimation()
    {
        if (anim == null || IsAttacking || _deathStarted) return;
        IsAttacking = true;
        AudioManager.Instance.PlaySFX("EnemyRoar");
        anim.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        IsAttacking = false;
        if (anim == null) return;
        anim.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _DetectionRadius);
    }

}