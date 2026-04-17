using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 15;
    [SerializeField] private GameObject _explosionWavePrefab;
    [SerializeField] private Material _explosionMaterial;

    private bool _isExploded = false;

    public DamageTarget _damageTarget = DamageTarget.Enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (_isExploded) return;

        _isExploded = true;

        Vector3 hitPoint = transform.position;

        AudioManager.Instance.PlaySFXAtPoint("MagicSpellExplode", hitPoint);

        SpawnExplosionWave(hitPoint);

        Destroy(gameObject);
    }

    private void SpawnExplosionWave(Vector3 position)
    {
        if (_explosionWavePrefab == null) return;

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
