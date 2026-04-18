using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 15;
    [SerializeField] private GameObject _explosionWavePrefab;
    [SerializeField] private Material _explosionMaterial;

    private Renderer _bulletRenderer;

    private bool _isExploded = false;

    public DamageTarget _damageTarget = DamageTarget.Enemy;

    private void Awake()
    {
        _bulletRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isExploded) return;
        _isExploded = true;

        Vector3 hitPoint = transform.position;

        AudioManager.Instance.PlaySFXAtPoint("MagicSpellExplode", hitPoint);

        SpawnExplosionWave(hitPoint);

        Destroy(gameObject);
    }

    public void setBulletMatRenderer(Material mat)
    {
        if (_bulletRenderer != null)
        {
            _bulletRenderer.material = mat;
        }
    }

    public void SetBulletDamage(int value)
    {
        _damage = value;
    }

    public void SetBulletMaterial(Material mat)
    {
        _explosionMaterial = mat;
    }

    public void SetDamageType(DamageTarget dtarget)
    {
        _damageTarget = dtarget;
    }

    private void SpawnExplosionWave(Vector3 position)
    {
        if (_explosionWavePrefab == null)
        {
            Debug.LogError("EXPLOSION WAVE PREFAB is NULL !!");
            return;
        }

        GameObject wave = Instantiate(_explosionWavePrefab, position, Quaternion.identity);
        ExplosionWave explosionWave = wave.GetComponent<ExplosionWave>();
        if (explosionWave != null)
        {
            explosionWave.SetMaterialWave(_explosionMaterial);
            explosionWave.SetDamageWave(_damage);
            explosionWave.GetComponent<ExplosionWave>().SetDamageTarget(_damageTarget);
        }
    }

}
