using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform _cannonBaseAim;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _target;

    [SerializeField] private bool _isActivated = false;

    [SerializeField] protected float _fireRate;
    [SerializeField] protected float _fireRange;
    [SerializeField] private float _shootForce;

    [SerializeField] private LifeController _lifeController;

    private float _lastShoot = 0f;

    public SO_TowerData TowerData { get; private set; }
    public SO_BulletData BulletData { get; private set; }

    private void Awake()
    {
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

    public void Initialize(SO_TowerData data, SO_BulletData bulletdata)
    {
        if (data == null || bulletdata == null)
        {
            Debug.LogError("BAD Initialize: null data !!!");
            return;
        }

        TowerData = data;
        BulletData = bulletdata;

        _fireRate = TowerData.fireRate;
        _shootForce = TowerData.shootForce;
        _fireRange = TowerData.range;

        _isActivated = true;
    }

    private void Update()
    {
        if (CheckNearstEnemy()) // Check if Enemy in range
        {
            if (_target != null)
            {
                if (_isActivated) // se torre attiva procedi .... segui il nemico ...
                {
                    _cannonBaseAim.LookAt(_target.transform.position + Vector3.up);
                    _cannonBaseAim.Rotate(0f, 180f, 0f, Space.Self);

                    if (CanShoot())
                    {
                        if (_target != null) Shoot();
                    }
                }
            }
        }
    }

    private bool CanShoot()
    {
        float shootTime = _lastShoot + _fireRate;
        if (Time.time >= shootTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Transform FindNearestEnemy()
    {
        GameObject NearstEnemyFounded = null;
        float nearstDistance = _fireRange;

        if (EnemiesManager.Instance == null)
        {
            Debug.LogError("EnemiesManager.Instance is NULL!");
            return null;
        }

        foreach (EnemyFSMController currentEnemy in EnemiesManager.Instance.listEnemies)
        {
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

    private bool CheckNearstEnemy()
    {
        _target = FindNearestEnemy();

        if (_target) return true;

        return false;
    }


    public void Shoot()
    {
        _lastShoot = Time.time;

        if (_target == null)
        {
            Debug.LogError("Tower Shoot: target NULL !!!");
            return;
        }

        if (BulletData == null)
        {
            Debug.LogError("Tower Shoot: BulletData NULL !!!");
            return;
        }

        if (MagicSpheresPooling.Instance == null)
        {
            Debug.LogError("MagicSpheresPooling.Instance is NULL !!!");
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ShootFireBall");
        }

        Bullet projectile = MagicSpheresPooling.Instance.GetPoolObj();
        if (projectile == null)
        {
            Debug.LogError("Il pool non ha restituito nessun Bullet !!!");
            return;
        }

        Vector3 muzzlePos = _firePoint.position + _firePoint.forward * 1.0f;
        Vector3 dir = (_target.position - _firePoint.position).normalized;
        Quaternion rotationdesired = Quaternion.LookRotation(dir);

        projectile.transform.position = muzzlePos;
        projectile.transform.rotation = rotationdesired;

        projectile.ResetBullet();

        projectile.SetBulletDamage(BulletData.damage);
        projectile.setBulletMatRenderer(BulletData.expMaterial);
        projectile.SetBulletMaterial(BulletData.expMaterial);
        projectile.SetDamageType(BulletData.damageTarget);

        projectile.gameObject.SetActive(true);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = dir * _shootForce;
        }
    }


    public void OnDefeated()
    {
        AudioManager.Instance.PlaySFX("MagicSpellExplode");
        Destroy(gameObject);
    }


}


