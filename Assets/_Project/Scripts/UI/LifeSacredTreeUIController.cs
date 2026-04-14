using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeSacredTreeUIController : MonoBehaviour
{
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private TextMeshProUGUI _currenLifeText;
    [SerializeField] private Image _barlifeFillable;

    private void OnEnable()
    {
        if (_lifeController != null)
        {
            _lifeController.OnHealthChanged += UpdateSacredTreeLifeText;
            UpdateSacredTreeLifeText(_lifeController.GetHp(), _lifeController.GetMaxHp());
        }
        else
        {
            Debug.LogError("LifeUIController: LifeController non trovato!");

        }
    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnHealthChanged -= UpdateSacredTreeLifeText;
    }

    private void UpdateSacredTreeLifeText(int lifeNum, int maxhealth)
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.GameOver) return;
        if (maxhealth <= 0) return;

        if (_currenLifeText != null) _currenLifeText.text = lifeNum + "/" + maxhealth;
        if (_barlifeFillable != null) _barlifeFillable.fillAmount = (float)lifeNum / maxhealth;
    }


}
