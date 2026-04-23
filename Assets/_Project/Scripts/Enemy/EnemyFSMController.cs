using UnityEngine;
using UnityEngine.AI;

public class EnemyFSMController : MonoBehaviour
{
    [SerializeField] private BaseFSMState _initialState;
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private Transform _mainTarget;

    [SerializeField] private LayerMask _otherTargetsLayer;
    [SerializeField] private float _detectionRadius = 6f;

    [SerializeField] private int _physicalDamage = 50;

    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _blueGemPrefab;

    [SerializeField] private int _numberOfCoinsBonus = 5;
    [SerializeField] private int _numberOfBlueGemBonus = 5;
    [SerializeField] private float _bonusDistanceFromSpawnPoint = 1f;
    [SerializeField] private float _bonusHeightOnTerrain = 0.25f;

    public EnemyHandHitBox enemyHandHitbox;

    private BaseFSMState _currentState;

    public NavMeshAgent agent;
    public Animator anim;

    bool _deathStarted = false;

    public bool isAlive = true;
    public bool IsAttacking = false;

    public Transform MainTarget => _mainTarget;
    public Transform CurrentTarget { get; private set; }

    private Collider[] hitColliders;
    private const int maxColliders = 10;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (_lifeController == null) _lifeController = GetComponent<LifeController>();

        if (enemyHandHitbox == null)
        {
            enemyHandHitbox = GetComponentInParent<EnemyHandHitBox>();
        }

        hitColliders = new Collider[maxColliders];

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

        if (EnemiesManager.Instance != null) EnemiesManager.Instance.RegistEnemy(this);

    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnDefeated -= OnDefeated;
    }

    public void ResetEnemy(Transform target)
    {
        isAlive = true;
        _deathStarted = false;
        IsAttacking = false;

        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }

        if (anim != null)
        {
            anim.SetBool("isDying", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("walking", false);
        }

        _lifeController.SetHp(50);

        _mainTarget = target;
        CurrentTarget = _mainTarget;

        if (_initialState != null) ChangeState(_initialState);
    }

    private void Start()
    {
        if (enemyHandHitbox) enemyHandHitbox.physicalDamage = _physicalDamage;
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
        //Debug.Log("CurrentTarget -> " + target.name);
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
        if (CurrentTarget == null) CurrentTarget = _mainTarget;
    }

    private void SpawnBonus()
    {
        AudioManager.Instance.PlaySFXAtPoint("BonusGame", this.transform.position);

        for (int i = 0; i < _numberOfCoinsBonus; i++)
        {
            GameObject clone = Instantiate(_coinPrefab,
                                           this.transform.position + Vector3.forward * _bonusDistanceFromSpawnPoint + Vector3.up * _bonusHeightOnTerrain,
                                           Quaternion.identity);
            clone.transform.RotateAround(this.transform.position, Vector3.up, 360 / (float)_numberOfCoinsBonus * i);
        }

        if (_numberOfBlueGemBonus > 0)
        {
            for (int i = 0; i < _numberOfBlueGemBonus; i++)
            {
                GameObject clone = Instantiate(_blueGemPrefab,
                                               this.transform.position + Vector3.forward * _bonusDistanceFromSpawnPoint + Vector3.up * _bonusHeightOnTerrain,
                                               Quaternion.identity);
                clone.transform.RotateAround(this.transform.position, Vector3.up, 360 / (float)_numberOfBlueGemBonus * i);
            }
        }

    }

    public GameObject CheckNewTarget()
    {
        GameObject nearstTargetFounded = null;
        float nearstDistance = _detectionRadius;

        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, nearstDistance, hitColliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider col = hitColliders[i];
            if (col == null) continue;

            GameObject obj = col.gameObject;

            if (!obj.CompareTag(Tags.Tower) && !obj.CompareTag(Tags.Player)) continue;
            if (_mainTarget != null && obj.transform == _mainTarget) continue;

            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance < nearstDistance)
            {
                nearstDistance = distance;
                nearstTargetFounded = obj;
            }
        }

        if (nearstTargetFounded != null)
        {
            return nearstTargetFounded;
        }

        return null;
    }

    public void DestroyGOEnemy()
    {
        SpawnBonus();

        if (DemonsPooling.Instance != null)
        {
            DemonsPooling.Instance.PutDemonPoolObj(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
        //Destroy(gameObject);
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
        AudioManager.Instance.PlaySFX("DeathSound");
        if (EnemiesManager.Instance != null) EnemiesManager.Instance.RemoveEnemy(this);
        StartDeathAnimation();
    }

    public void StartPlayAttackAnimation()
    {
        if (anim == null || IsAttacking || _deathStarted) return;

        if (CurrentTarget.gameObject.TryGetComponent<ILiveCheckable>(out var LiveCheckable))
        {
            if (!LiveCheckable.isAliveState()) return;
        }

        IsAttacking = true;

        //AudioManager.Instance.PlaySFX("EnemyRoar");
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
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

}