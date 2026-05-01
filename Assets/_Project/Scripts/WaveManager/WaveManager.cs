using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _spawnMiniBossPoint;
    [SerializeField] private Transform _targetForEnemy;

    private float _spawnrate = 50;
    private Coroutine Spawner;
    private Camera _cam;

    private int _currentWave = 0;

    public Action<int> OnWaveChanged;

    public int CurrentWave => _currentWave;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _cam = Camera.main;

        OnWaveChanged?.Invoke(_currentWave);
    }

    private void Start()
    {
        Invoke("CallWave", 20); // wait some times before to start enemy wave !!!
    }

    private void CallWave()
    {
        Spawner = StartCoroutine(StartWaves());
    }

    private void Update()
    {
        OnWaveChanged?.Invoke(_currentWave);
    }

    private IEnumerator StartWaves()
    {
        _currentWave = 1;

        while (true)
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) StopWaveManager();

            if (_currentWave % 10 == 0) // spawn mini boss after 10 waves ....
            {
                SpawnMiniBossEnemy();
            }

            SpawnEnemies();

            float delay = Mathf.Clamp(_spawnrate - _currentWave, 5f, _spawnrate);
            yield return new WaitForSeconds(delay);

            _currentWave++;
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

        AudioManager.Instance.PlaySFXAtPoint("EnemyRoar", -_spawnMiniBossPoint.position);

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
