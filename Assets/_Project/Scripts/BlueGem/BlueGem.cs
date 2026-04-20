using UnityEngine;

public class BlueGem : MonoBehaviour, IPickable
{
    [Header("Blue Gem BONUS parameters")]
    [SerializeField] private float _rotSpeed = 100f;
    [SerializeField] private int _value = 10;

    void Update()
    {
        transform.Rotate(_rotSpeed * Time.deltaTime, 0, 0);
    }

    public void PickUp(Picker collector)
    {
        collector.AddBlueGem(_value);
        Destroy(gameObject);
    }
}
