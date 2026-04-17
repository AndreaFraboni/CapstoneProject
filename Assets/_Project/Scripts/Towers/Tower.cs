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
    [SerializeField] private GameObject _projectilePrefab;

    private float _lastShoot = 0f;

    public SO_TowerData Data { get; private set; }

    public void Initialize(SO_TowerData data)
    {
        Data = data;

        _fireRate = data.fireRate;
        _shootForce = data.shootForce;
        _projectilePrefab = data.projectilePrefab;
        _fireRange = data.range;

        _isActivated = true;

        // Debug.Log($"The Tower named : {data.towerName} is ACTIVATED !!!");
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

        Vector3 targetPos = _target.gameObject.transform.position;
        Vector3 muzzlePos = _firePoint.position + _firePoint.forward * 0.5f;
        Vector3 direction = (targetPos - muzzlePos).normalized;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ShootFireBall");
        }

        Vector3 dir = (_target.position - _firePoint.position).normalized;
        Quaternion rotationdesired = Quaternion.LookRotation(dir);

        if (_projectilePrefab != null)
        {
            GameObject projectile = Instantiate(_projectilePrefab, muzzlePos, rotationdesired);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //                rb.AddForce(dir * _shootForce, ForceMode.Impulse);
                rb.velocity = dir * _shootForce;
            }
        }
        else
        {
            Debug.Log("Non ho riferimenti al Prefab del Bullet !!!");
            return;
        }


    }

}


