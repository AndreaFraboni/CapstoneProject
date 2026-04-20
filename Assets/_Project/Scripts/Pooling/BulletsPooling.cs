using System.Collections.Generic;
using UnityEngine;

public class BulletsPooling : MonoBehaviour
{
    public static BulletsPooling Instance { get; private set; }

    [SerializeField] private Bullet _bulletPrefab;

    [SerializeField] private int _poolSize = 10;

    private Queue<Bullet> _bulletPool = new Queue<Bullet>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CreatePool(_poolSize);
    }

    public void CreatePool(int num)
    {
        if (_bulletPrefab == null)
        {
            Debug.LogError("MagicSpheresPooling: _bulletPrefab is NULL !!!");
            return;
        }

        for (int i = 0; i < num; i++)
        {
            Bullet obj = Instantiate(_bulletPrefab, transform);
            obj.gameObject.SetActive(false);
            _bulletPool.Enqueue(obj);
        }
    }

    public Bullet GetPoolObj()
    {
        if (_bulletPool.Count == 0) CreatePool(1);
        if (_bulletPool.Count == 0) return null;
        Bullet obj = _bulletPool.Dequeue();
        return obj;
    }

    public void PutPoolObj(Bullet obj)
    {
        obj.gameObject.SetActive(false);
        _bulletPool.Enqueue(obj);
    }


}
