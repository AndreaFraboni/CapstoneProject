using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    private float _spawnrate = 20;
    private Coroutine Spawner;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Start()
    {
        Invoke("CallWave", 10); // wait some times before to start enemy wave !!!
    }

    private void CallWave()
    {
        Spawner = StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        int wave = 1;

        while (true)
        {
            SpawnEnemies(wave);

            float delay = Mathf.Clamp(_spawnrate - wave, 5f, _spawnrate);
            yield return new WaitForSeconds(delay);

            wave++;
        }
    }

    private void SpawnEnemies(int i)
    {
        Debug.Log("SPAWN ENEMY ......");
        AudioManager.Instance.PlaySFXAtPoint("StartWaveHorn", _cam.transform.position);




    }

    private void OnDisable()
    {
        if (Spawner != null)
        {
            StopCoroutine(Spawner);
            Spawner = null;
        }
    }



}
