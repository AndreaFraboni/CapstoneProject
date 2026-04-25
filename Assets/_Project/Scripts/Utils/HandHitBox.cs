using UnityEngine;

public class HandHitBox : MonoBehaviour
{
    public int physicalDamage = 0;
    [SerializeField] private Collider _col;

    private bool isActive = false;

    private void Awake()
    {
        _col = GetComponent<BoxCollider>();
        if (_col == null)
        {
            Debug.LogError("NESSUN BOX COLLIDER TROVATO SU " + gameObject.name);
            return;
        }

        isActive = false;

        _col.enabled = false;
    }

    public void EnableHitbox()
    {
        isActive = true;
        if (_col != null) _col.enabled = true;
    }

    public void DisableHitbox()
    {
        isActive = false;
        if (_col != null) _col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (other.TryGetComponent<LifeController>(out LifeController life))
        {
            life.TakeDamage(physicalDamage);
        }
    }




}
