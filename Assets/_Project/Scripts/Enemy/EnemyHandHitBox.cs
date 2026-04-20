using UnityEngine;

public class EnemyHandHitBox : MonoBehaviour
{
    public int physicalDamage = 0;

    [SerializeField] private Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();
        if (_col != null) _col.enabled = false;  // Hand hitbox start disabled !!!
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<LifeController>(out LifeController _life))
        {
            _life.TakeDamage(physicalDamage);
        }
    }

    public void EnableHitbox()
    {
        if (_col != null) _col.enabled = true; // activated in beginning of attack animation
    }

    public void DisableHitbox()
    {
        if (_col != null) _col.enabled = false; // deactivated in the end of attack animation
    }

}
