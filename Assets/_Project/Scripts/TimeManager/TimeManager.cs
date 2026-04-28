using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private float _currentTime = 0;

    public Action<float> OnTimeChanged;

    public int time_elapsed;

    public int CurrentTimeElapsed => (int)_currentTime;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        OnTimeChanged?.Invoke(_currentTime);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        var state = GameManager.Instance.CurrentState;

        if (state == GameState.GameOver || state == GameState.Victory)
        {
            TimeUpdate();
            return;
        }

        if (state == GameState.Paused) return;

        _currentTime += Time.deltaTime;
        TimeUpdate();
    }

    private void TimeUpdate()
    {
        int secondiTrascorsi = (int)_currentTime;
        time_elapsed = (int)_currentTime;
        OnTimeChanged?.Invoke(_currentTime);
    }

    public void AddTime(float value)
    {
        _currentTime += value;
    }
}



