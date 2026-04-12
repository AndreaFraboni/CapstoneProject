using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [SerializeField] int _damage = 10;
    [SerializeField] float _delayDamage = 1f; // secondi tra un danno e l'altro

    private float _timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<LifeController>(out LifeController _life))
        {
            _life.TakeDamage(_damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<LifeController>(out LifeController life)) return;

        _timer += Time.deltaTime;

        if (_timer >= _delayDamage)
        {
            life.TakeDamage(_damage);
            _timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _timer = 0;
    }
}
