using UnityEngine;

public class Coin : MonoBehaviour, IPickable
{
    [Header("COIN object 3D Parameters")]
    [SerializeField] private float _rotSpeed = 100f;
    [SerializeField] private int _coinValue = 1;

    void Update()
    {
        transform.Rotate(_rotSpeed * Time.deltaTime, 0, 0);
    }

    public void PickUp(Picker collector)
    {
        collector.AddCoins(_coinValue);
        Destroy(gameObject);
    }
}
