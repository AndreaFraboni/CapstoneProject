using TMPro;
using UnityEngine;

public class WavesUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currenWaveText;

    private void Start()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveChanged += UpdateWaveText;
        }
        else
        {
            Debug.LogError("WaveManager: is NULL ?????");
        }
    }

    private void OnDisable()
    {
        if (WaveManager.Instance) WaveManager.Instance.OnWaveChanged -= UpdateWaveText;
    }

    private void UpdateWaveText(int value)
    {
        if (WaveManager.Instance != null)
        {
            if (_currenWaveText != null) _currenWaveText.text = $"{value}";
        }
    }
}
