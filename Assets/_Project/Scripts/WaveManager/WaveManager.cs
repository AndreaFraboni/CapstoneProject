using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] private Transform[] _spawnPoints;

    private float _spawnrate = 20;
    private Coroutine Spawner;
    private Camera _cam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _cam = Camera.main;
    }

    private void Start()
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
