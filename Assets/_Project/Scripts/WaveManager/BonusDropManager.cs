using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDropManager : MonoBehaviour
{
    public static BonusDropManager Instance { get; private set; }

    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _blueGemPrefab;
    [SerializeField] private GameObject _heartPrefab;

    [SerializeField] private float _bonusDistanceFromSpawnPoint = 1f;
    [SerializeField] private float _bonusHeightOnTerrain = 0.25f;

    [SerializeField] private int _maxBonusPerWave = 50;

    [SerializeField] private int _currentWaveNum = 0;

    private int _bonusSpawned = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (WaveManager.Instance != null) WaveManager.Instance.OnWaveChanged += ResetBonusWave;
    }

    private void OnDisable()
    {
        if (WaveManager.Instance != null) WaveManager.Instance.OnWaveChanged -= ResetBonusWave;
    }

    private void ResetBonusWave(int currentWave)
    {
        _currentWaveNum = currentWave;
        _bonusSpawned = 0;
    }

    public void TrySpawnBonus(Vector3 spawnPosition, int numcoins, int numblueGems, int numhearts)
    {
        if (_bonusSpawned >= _maxBonusPerWave) return; // già oltre il limite di bonus spawnabili

        int coinsToSpawn = numcoins;
        int gemsToSpawn = numblueGems;
        int heartsToSpawn = numhearts;

        if (_currentWaveNum <= 2)  
        {
            gemsToSpawn = 1;
            heartsToSpawn = 0;
        }
        else if (_currentWaveNum >2 && _currentWaveNum <= 5)
        {
            gemsToSpawn = 2;
            heartsToSpawn = 1;
            coinsToSpawn += 2;
        }
        else if (_currentWaveNum > 5 && _currentWaveNum < 10)
        {
            gemsToSpawn = 2;
            heartsToSpawn = 1;
            coinsToSpawn += 5;
        }
        else if (_currentWaveNum >=10) 
        {
            coinsToSpawn += 5;
            gemsToSpawn += 1;
            heartsToSpawn = 1;
        }

        SpawnBonus(spawnPosition, _heartPrefab, heartsToSpawn);
        SpawnBonus(spawnPosition, _blueGemPrefab, gemsToSpawn);
        SpawnBonus(spawnPosition, _coinPrefab, coinsToSpawn);

    }

    private void SpawnBonus(Vector3 spawnpoint, GameObject prefab, int amount)
    {
        if (prefab == null) return;
        if (amount <= 0) return;

        for (int i = 0; i < amount; i++)
        {
            if (_bonusSpawned >= _maxBonusPerWave) return;

            GameObject clone = Instantiate(prefab,
                                           spawnpoint + Vector3.forward * _bonusDistanceFromSpawnPoint + Vector3.up * _bonusHeightOnTerrain,
                                           prefab.transform.rotation);

            float angleStep = 360f / (float)amount;
            float angle = angleStep * i;
            clone.transform.RotateAround(spawnpoint, Vector3.up, angle);

            _bonusSpawned++;
        }
    }
}
