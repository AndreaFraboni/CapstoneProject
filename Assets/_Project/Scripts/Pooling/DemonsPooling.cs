using System.Collections.Generic;
using UnityEngine;

public class DemonsPooling : MonoBehaviour
{
    public static DemonsPooling Instance { get; private set; }

    [SerializeField] private EnemyFSMController _EnemyRef;
    [SerializeField] private int _poolSize = 10;

    private Queue<EnemyFSMController> _EnemiesPool = new Queue<EnemyFSMController>();

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
        if (_EnemyRef == null)
        {
            Debug.LogError("EnemiesPooling: _EnemyPrefab is NULL !!!");
            return;
        }

        for (int i = 0; i < num; i++)
        {
            EnemyFSMController obj = Instantiate(_EnemyRef, transform);
            obj.gameObject.SetActive(false);
            _EnemiesPool.Enqueue(obj);
        }
    }

    public EnemyFSMController GetPoolObj()
    {
        if (_EnemiesPool.Count == 0) CreatePool(1);
        if (_EnemiesPool.Count == 0) return null;
        EnemyFSMController obj = _EnemiesPool.Dequeue();
        return obj;
    }

    public void PutPoolObj(EnemyFSMController obj)
    {
        obj.gameObject.SetActive(false);
        _EnemiesPool.Enqueue(obj);
    }


}
