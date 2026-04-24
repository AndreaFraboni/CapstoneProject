using UnityEngine;
using UnityEngine.AI;

public class EntFSMController : MonoBehaviour
{
    [SerializeField] private EntBaseFSMState _initialState;
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private float _detectionRadius = 6f;
    [SerializeField] private int _physicalDamage = 100;

    public HandHitBox HandHitbox;
    private EntBaseFSMState _currentState;
    public NavMeshAgent agent;
    public Animator anim;
    bool _deathStarted = false;
    public bool isAlive = true;
    public bool IsAttacking = false;

    public Transform CurrentTarget { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (_lifeController == null) _lifeController = GetComponent<LifeController>();
        if (HandHitbox == null) HandHitbox = GetComponentInChildren<HandHitBox>();

        EntBaseFSMState[] states = GetComponentsInChildren<EntBaseFSMState>();
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
        if (HandHitbox) HandHitbox.physicalDamage = _physicalDamage;

        if (_initialState != null)
        {
            ChangeState(_initialState);
        }
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

    public void ChangeState(EntBaseFSMState newState)
    {
        if (newState == null) return;
        if (_currentState == newState) return;

        if (_currentState != null)
            _currentState.OnStateExit();

        _currentState = newState;
        _currentState.OnStateEnter();
    }

    public void SetCurrentTarget(Transform target)
    {
        CurrentTarget = target;
    }

    public bool HasCurrentTarget()
    {
        if (CurrentTarget == null) return false;
        return true;
    }

    public void ValidateCurrentTarget()
    {
        if (CurrentTarget == null)
        {
            CurrentTarget = FindNearestEnemy();
            return;
        }

        EnemyFSMController enemy = CurrentTarget.GetComponent<EnemyFSMController>();
        if (enemy == null)
        {
            CurrentTarget = null;
            CurrentTarget = FindNearestEnemy();
            return;
        }

        if (!enemy.isAlive)
        {
            CurrentTarget = null;
            CurrentTarget = FindNearestEnemy();
            return;
        }

        float distance = Vector3.Distance(transform.position, CurrentTarget.position);
        if (distance > _detectionRadius)
        {
            CurrentTarget = null;
            CurrentTarget = FindNearestEnemy();
        }


    }

    private Transform FindNearestEnemy()
    {
        GameObject NearstEnemyFounded = null;
        float nearstDistance = _detectionRadius;

        if (EnemiesManager.Instance == null)
        {
            Debug.LogError("EnemiesManager.Instance is NULL!");
            return null;
        }

        if (EnemiesManager.Instance.listEnemies.Count == 0)
        {
            //Debug.LogWarning("EnemiesManager.Instance is EMPTY!");
            return null;
        }

        foreach (EnemyFSMController currentEnemy in EnemiesManager.Instance.listEnemies)
        {
            if (currentEnemy == null) continue;
            if (!currentEnemy.isAlive) continue;

            float distance = Vector3.Distance(transform.position, currentEnemy.transform.position);
            if (distance < nearstDistance)
            {
                nearstDistance = distance;
                NearstEnemyFounded = currentEnemy.gameObject;
            }
        }
        if (NearstEnemyFounded != null)
            return NearstEnemyFounded.transform;

        return null;
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
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("DeathSound");
        StartDeathAnimation();
    }


    public void DestroyGOEnt()
    {
        Destroy(gameObject);
    }


    public void StopAttack()
    {
        IsAttacking = false;
        if (anim == null) return;
        anim.SetBool("isAttacking", false);
    }





}
