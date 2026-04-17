using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance { get; private set; }

    public List<EnemyFSMController> listEnemies = new List<EnemyFSMController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegistEnemy(EnemyFSMController enemy)
    {
        if (enemy == null) return;

        if (!listEnemies.Contains(enemy))
        {
            listEnemies.Add(enemy);
        }
        else
        {
            Debug.LogWarning("l'enemy non verrà registrato in quanto è già registrato !!!!");
        }
    }

    public void RemoveEnemy(EnemyFSMController enemy)
    {
        if (enemy == null) return;

        if (listEnemies.Remove(enemy))
        {
            //Debug.Log("Ho rimosso un enemy!!!");
        }
        else
        {
            Debug.LogError("Stò provando a rimuovere un enemy che non è nella lista !!!");
        }
    }
}
