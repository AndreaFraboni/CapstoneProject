using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("TIMER SETTINGS")]
    [SerializeField] private float _currentTime;

    public Action<float> OnTimeChanged;

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
        _currentTime += Time.deltaTime;

        TimeUpdate();
    }

    private void TimeUpdate()
    {
        int secondiTrascorsi = (int)_currentTime;
        OnTimeChanged?.Invoke(_currentTime);
    }

    public void AddTime(float value)
    {
        _currentTime += value;
    }
}



