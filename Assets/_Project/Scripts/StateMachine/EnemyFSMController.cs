using UnityEngine;
using UnityEngine.AI;

public class EnemyFSMController : MonoBehaviour
{
    [SerializeField] private BaseFSMState _initialState;

    [SerializeField] private LifeController _lifeController;

    private BaseFSMState _currentState;

    public NavMeshAgent agent;
    public Animator anim;

    bool _deathStarted = false;
    //bool isAlive = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (_lifeController == null) _lifeController = GetComponent<LifeController>();

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
        if (_initialState != null) ChangeState(_initialState);

        EnemiesManager.Instance.RegistEnemy(this);
    }

    private void Update()
    {
        if (_currentState == null) return;

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

    public void DestroyGOEnemy()
    {
        Destroy(gameObject);
    }

    private void StartDeathAnimation()
    {
        if (_deathStarted) return;

        _deathStarted = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.enabled = false;
        }

        anim.SetBool("walking", false);

        anim.SetBool("isDying", true);
    }

    public void OnDefeated()
    {
        //isAlive = false;

        EnemiesManager.Instance.RemoveEnemy(this);

        AudioManager.Instance.PlaySFX("DeathSound");

        StartDeathAnimation();
    }


}