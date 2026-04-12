using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 15;

    [SerializeField] private GameObject _explosionWavePrefab;
    [SerializeField] private Material _explosionMaterial;

    private bool _isExploded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("MagicSpellExplode");

        Vector3 hitPoint = transform.position;

        SpawnExplosionWave(hitPoint);
        Explode();
    }

    private void SpawnExplosionWave(Vector3 position)
    {
        if (_explosionWavePrefab == null) return;
        GameObject Wave = Instantiate(_explosionWavePrefab, position, Quaternion.identity);
        Wave.GetComponent<ExplosionWave>().SetMaterialWave(_explosionMaterial);
        Wave.GetComponent<ExplosionWave>().SetDamageWave(_damage);
    }

    private void Explode()
    {
        if (_isExploded) return;
        _isExploded = true;
        Destroy(gameObject);
    }
}
