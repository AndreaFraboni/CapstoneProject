using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _spawnMiniBossPoint;
    [SerializeField] private Transform _targetForEnemy;

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

            if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) StopWaveManager();


            //if (wave % 1 == 0)
            //{
                SpawnMiniBossEnemy();
            //}

            SpawnEnemies();

            float delay = Mathf.Clamp(_spawnrate - wave, 5f, _spawnrate);
            yield return new WaitForSeconds(delay);

            wave++;
        }
    }

    private void StopWaveManager()
    {
        if (Spawner != null)
        {
            StopCoroutine(Spawner);
            Spawner = null;
        }
    }


    private void SpawnEnemies()
    {
        AudioManager.Instance.PlaySFXAtPoint("StartWaveHorn", _cam.transform.position);

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            EnemyFSMController enemycloneprefab = DemonsPooling.Instance.GetDemonPoolObj();
            if (enemycloneprefab == null)
            {
                Debug.LogError("NO Enemy Prefab from DemonsPool !!!");
                return;
            }

            enemycloneprefab.transform.position = _spawnPoints[i].position;

            enemycloneprefab.gameObject.SetActive(true);
            enemycloneprefab.ResetEnemy(_targetForEnemy);
        }
    }

    private void SpawnMiniBossEnemy()
    {
        EnemyFSMController miniBossEnemyClonePrefab = DemonsPooling.Instance.GetMiniBossDemonPoolObj();
        if (miniBossEnemyClonePrefab == null)
        {
            Debug.LogError("NO mini Boss Enemy Prefab from DemonsPool !!!");
            return;
        }

        miniBossEnemyClonePrefab.transform.position = _spawnMiniBossPoint.position;

        miniBossEnemyClonePrefab.gameObject.SetActive(true);
        miniBossEnemyClonePrefab.ResetEnemy(_targetForEnemy);
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
