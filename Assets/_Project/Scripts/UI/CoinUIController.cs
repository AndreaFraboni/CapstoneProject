using TMPro;
using UnityEngine;

public class CoinUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentCoinsText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged += UpdateCoinsText;
            UpdateCoinsText(GameManager.Instance.currentCoins);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnCoinsChanged -= UpdateCoinsText;
    }

    private void UpdateCoinsText(int coins)
    {
        if (_currentCoinsText != null) _currentCoinsText.text = coins.ToString();
    }
}
