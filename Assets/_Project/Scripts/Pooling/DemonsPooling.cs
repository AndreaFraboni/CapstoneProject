using System.Collections.Generic;
using UnityEngine;

public class DemonsPooling : MonoBehaviour
{
    public static DemonsPooling Instance { get; private set; }

    [SerializeField] private EnemyFSMController _enemyRef;
    [SerializeField] private EnemyFSMController _miniBossEnemyRef;
    [SerializeField] private int _demonPoolSize = 10;
    [SerializeField] private int _miniBossDemonPoolSize = 5;

    private Queue<EnemyFSMController> _enemiesDemonPool = new Queue<EnemyFSMController>();
    private Queue<EnemyFSMController> _miniBossEnemiesPool = new Queue<EnemyFSMController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CreateDemonPool(_demonPoolSize);
        CreateMiniBossDemonPool(_miniBossDemonPoolSize);
    }

    public void CreateDemonPool(int num)
    {
        if (_enemyRef == null)
        {
            Debug.LogError("Demon Enemies Pooling: _enemyRef is NULL !!!");
            return;
        }

        for (int i = 0; i < num; i++)
        {
            EnemyFSMController obj = Instantiate(_enemyRef, transform);
            obj.gameObject.SetActive(false);
            _enemiesDemonPool.Enqueue(obj);
        }
    }

    public void CreateMiniBossDemonPool(int num)
    {
        if (_miniBossEnemyRef == null)
        {
            Debug.LogError("Demon Enemies Pooling: _enemyRef is NULL !!!");
            return;
        }

        for (int i = 0; i < num; i++)
        {
            EnemyFSMController obj = Instantiate(_miniBossEnemyRef, transform);
            obj.gameObject.SetActive(false);
            _miniBossEnemiesPool.Enqueue(obj);
        }
    }


    public EnemyFSMController GetDemonPoolObj()
    {
        if (_enemiesDemonPool.Count == 0) CreateDemonPool(1);
        if (_enemiesDemonPool.Count == 0) return null;
        EnemyFSMController obj = _enemiesDemonPool.Dequeue();
        return obj;
    }

    public void PutDemonPoolObj(EnemyFSMController obj)
    {
        obj.gameObject.SetActive(false);
        _enemiesDemonPool.Enqueue(obj);
    }

    public EnemyFSMController GetMiniBossDemonPoolObj()
    {
        if (_miniBossEnemiesPool.Count == 0) CreateDemonPool(1);
        if (_miniBossEnemiesPool.Count == 0) return null;
        EnemyFSMController obj = _miniBossEnemiesPool.Dequeue();
        return obj;
    }

    public void PutMiniBossDemonPoolObj(EnemyFSMController obj)
    {
        obj.gameObject.SetActive(false);
        _miniBossEnemiesPool.Enqueue(obj);
    }



}
