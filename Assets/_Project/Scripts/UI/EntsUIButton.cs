using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntsUIButton : MonoBehaviour
{
    [SerializeField] public SO_EntData _EntData;
    [SerializeField] public Button _EntButton;
    [SerializeField] public TextMeshProUGUI _EntGoldPriceText;
    [SerializeField] public TextMeshProUGUI _EntBlueGemsPriceText;

    private void Awake()
    {
        if (_EntButton == null) _EntButton = GetComponent<Button>();

        SetButtonPriceText();
    }

    private void SetButtonPriceText()
    {
        if (_EntGoldPriceText != null && _EntData != null && _EntBlueGemsPriceText != null)
        {
            _EntGoldPriceText.text = _EntData.goldPrice.ToString();
            _EntBlueGemsPriceText.text = _EntData.blueGemsPrice.ToString();
        }
        else
        {
            Debug.LogError("Price Gold/BlueGems text ref or Ent data are not assigned !!!!");
        }
    }

}
