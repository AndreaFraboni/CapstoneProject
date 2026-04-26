using TMPro;
using UnityEngine;

public class GameOverUIMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _finalTimeText;
    [SerializeField] private TextMeshProUGUI _finalwavesText;

    private void OnEnable()
    {
        // Save Player STATS in LeaderBoard ....
        IOManager.Instance.SetPlayerName("LocalPlayer");
        IOManager.Instance.SetPlayerTime(TimeManager.Instance.time_elapsed);
    }

    private void Start()
    {
        if (WaveManager.Instance != null && TimeManager.Instance != null)
        {
            WaveManager.Instance.OnWaveChanged += UpdateWaveText;
            TimeManager.Instance.OnTimeChanged += UpdateTimerText;
        }
        else
        {
            Debug.LogError("Wave and Time Manager: are NULL ?????");
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
