using TMPro;
using UnityEngine;

public class GameOverUIMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _finalTimeText;
    [SerializeField] private TextMeshProUGUI _finalwavesText;

    private void OnEnable()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveChanged += UpdateWaveText;
            UpdateWaveText(WaveManager.Instance.CurrentWave);
        }

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeChanged += UpdateTimerText;
            UpdateTimerText(TimeManager.Instance.CurrentTimeElapsed);
        }
    }

    private void OnDisable()
    {
        if (WaveManager.Instance) WaveManager.Instance.OnWaveChanged -= UpdateWaveText;
        if (TimeManager.Instance) TimeManager.Instance.OnTimeChanged -= UpdateTimerText;
    }

    private void UpdateWaveText(int value)
    {
        if (_finalwavesText != null) _finalwavesText.text = $"{value}";
    }
    private void UpdateTimerText(float time)
    {
        if (_finalTimeText != null) _finalTimeText.text = $"{(int)time} s";
    }

}
