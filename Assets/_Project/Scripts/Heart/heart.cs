using UnityEngine;

public class Heart : MonoBehaviour, IPickable
{
    [Header("HEART BONUS parameters")]
    [SerializeField] private float _rotSpeed = 100f;
    [SerializeField] private int _value = 10;

    void Update()
    {
        transform.Rotate(_rotSpeed * Time.deltaTime, 0, 0);
    }

    public void PickUp(Picker collector)
    {
        collector.AddHealth(_value);
        Destroy(gameObject);
    }
}
