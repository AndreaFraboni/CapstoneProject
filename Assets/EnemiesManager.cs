using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance { get; private set; }

    //public List<EnemyController> listEnemies = new List<EnemyController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }




    //public void RegistEnemy(EnemyController enemy)
    //{
    //    if (enemy == null) return;
    //    if (!listEnemies.Contains(enemy))
    //    {
    //        listEnemies.Add(enemy);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("l'enemy non verrà registrato in quanto è già registrato !!!!");
    //    }
    //}

    //public void RemoveEnemy(EnemyController enemy)
    //{
    //}
}
