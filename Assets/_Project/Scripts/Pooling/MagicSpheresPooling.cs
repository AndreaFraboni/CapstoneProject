using System.Collections.Generic;
using UnityEngine;

public class MagicSpheresPooling : MonoBehaviour
{
    public static MagicSpheresPooling Instance { get; private set; }

    [SerializeField] private GameObject[] _bulletPrefabs;

    //[SerializeField] private GameObject[] _bulletFrozenPrefabs;
    //[SerializeField] private GameObject[] _bulletFirePrefabs;
    //[SerializeField] private GameObject[] _bulletPoisonPrefabs;
    //[SerializeField] private GameObject[] _bulletMagicPrefabs;

    [SerializeField] private int _poolSize = 10;


    private Queue<GameObject> _bulletPool = new Queue<GameObject>();

    //private Queue<GameObject> frozenPool = new Queue<GameObject>();
    //private Queue<GameObject> firePool = new Queue<GameObject>();
    //private Queue<GameObject> poisonPool = new Queue<GameObject>();
    //private Queue<GameObject> magicPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //CreatePools(_poolSize);
    }


    //public void CreatePools(int num)
    //{
    //    CreateSinglePool(_bulletFrozenPrefabs, frozenPool, _poolSize);
    //    CreateSinglePool(_bulletFirePrefabs, firePool, _poolSize);
    //    CreateSinglePool(_bulletPoisonPrefabs, poisonPool, _poolSize);
    //    CreateSinglePool(_bulletMagicPrefabs, magicPool, _poolSize);
    //}

    //private void CreateSinglePool(GameObject[] prefabs, Queue<GameObject> targetPool, int num)
    //{
    //    for (int i = 0; i < num; i++)
    //    {
    //        GameObject prefab = prefabs[i % prefabs.Length];
    //        GameObject obj = Instantiate(prefab, transform);
    //        obj.SetActive(false);
    //        targetPool.Enqueue(obj);
    //    }
    //}

    //public GameObject GetPoolObj()
    //{
    //    //if (pool.Count == 0)
    //    //    CreatePool(1);
    //    return pool.Dequeue();
    //}

    //public void PutPoolObj(GameObject obj)
    //{
    //    //obj.SetActive(false);
    //    //pool.Enqueue(obj);
    //}


}
