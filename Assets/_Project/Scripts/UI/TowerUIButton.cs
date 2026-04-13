using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIButton : MonoBehaviour
{
    [SerializeField] public SO_TowerData _towerData;
    [SerializeField] public Button _towerButton;
    [SerializeField] public TextMeshProUGUI _towerPriceText;

    private void Awake()
    {
        if (_towerButton == null) _towerButton = GetComponent<Button>();

        SetButtonPriceText();
    }

    private void SetButtonPriceText()
    {
        if (_towerPriceText != null && _towerData != null)
        {
            _towerPriceText.text = _towerData.goldPrice.ToString();
        }
        else
        {
            Debug.LogError("Price text ref or tower data are not assigned !!!!");
        }
    }

}
