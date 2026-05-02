using TMPro;
using UnityEngine;

public class BlueGemUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentBlueGemText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBlueGemsChanged += UpdateBlueGemsText;
            UpdateBlueGemsText(GameManager.Instance.currentBlueGems);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnBlueGemsChanged -= UpdateBlueGemsText;
    }

    private void UpdateBlueGemsText(int coins)
    {
        if (_currentBlueGemText != null) _currentBlueGemText.text = coins.ToString();
    }
}