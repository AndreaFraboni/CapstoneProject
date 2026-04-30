using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDropManager : MonoBehaviour
{
    public static BonusDropManager Instance { get; private set; }

    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _blueGemPrefab;
    [SerializeField] private GameObject _heartPrefab;

    [SerializeField] private int _maxBonusPerWave = 5;

    private int _currentWaveNum = 0;

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

    public void TrySpawnBonus(Vector3 spawnPosition, int coins, int blueGems, int hearts)
    {
        for (int i = 0; i < coins; i++)
        {
            SpawnCoin(spawnPosition);
        }

        for (int i = 0; i < blueGems; i++)
        {
            SpawnBlueGem(spawnPosition);
        }

        for (int i = 0; i < hearts; i++)
        {
            SpawnHeart(spawnPosition);
        }
    }

    private void SpawnCoin(Vector3 spawnposition)
    {

    }
    private void SpawnBlueGem(Vector3 spawnposition)
    {

    }
    private void SpawnHeart(Vector3 spawnposition)
    {

    }

    //if (_numberOfCoinsBonus > 0)
    //{
    //    for (int i = 0; i < _numberOfCoinsBonus; i++)
    //    {
    //        GameObject clone = Instantiate(_coinPrefab,
    //                                       this.transform.position + Vector3.forward * _bonusDistanceFromSpawnPoint + Vector3.up * _bonusHeightOnTerrain,
    //                                       _coinPrefab.transform.rotation);

    //        float angleStep = 360f / (float)_numberOfCoinsBonus;
    //        float angle = angleStep * i;
    //        clone.transform.RotateAround(transform.position, Vector3.up, angle);
    //    }
    //}

    //if (_numberOfBlueGemBonus > 0)
    //{
    //    for (int i = 0; i < _numberOfBlueGemBonus; i++)
    //    {
    //        GameObject clone = Instantiate(_blueGemPrefab,
    //                                       this.transform.position + Vector3.forward * _bonusDistanceFromSpawnPoint + Vector3.up * _bonusHeightOnTerrain,
    //                                       _blueGemPrefab.transform.rotation);
    //        float angleStep = 360f / (float)_numberOfBlueGemBonus;
    //        float angle = angleStep * i;
    //        clone.transform.RotateAround(transform.position, Vector3.up, angle);
    //    }
    //}
    //if (_numberOfHearts > 0)
    //{
    //    for (int i = 0; i < _numberOfHearts; i++)
    //    {
    //        GameObject clone = Instantiate(_heartPrefab,
    //                                       this.transform.position + Vector3.forward * _bonusDistanceFromSpawnPoint + Vector3.up * _bonusHeightOnTerrain,
    //                                       _heartPrefab.transform.rotation);
    //        float angleStep = 360f / (float)_numberOfHearts;
    //        float angle = angleStep * i;
    //        clone.transform.RotateAround(transform.position, Vector3.up, angle);
    //    }


}
