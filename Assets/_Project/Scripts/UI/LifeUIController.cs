using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeUIController : MonoBehaviour
{
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private TextMeshProUGUI _currenLifeText;
    [SerializeField] private Image _bar_lifeBarFillable;

    private void OnEnable()
    {
        if (_lifeController != null)
        {
            _lifeController.OnHealthChanged += UpdateLifeText;
            UpdateLifeText(_lifeController.GetHp(), _lifeController.GetMaxHp());
        }
        else
        {
            Debug.LogError("LifeUIController: LifeController non trovato!");

        }
    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnHealthChanged -= UpdateLifeText;
    }

    private void UpdateLifeText(int lifeNum, int maxhealth)
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.GameOver) return;

        if (maxhealth <= 0) return;
        if (_currenLifeText != null) _currenLifeText.text = lifeNum.ToString() + "/" + maxhealth;
        if (_bar_lifeBarFillable != null) _bar_lifeBarFillable.fillAmount = (float)lifeNum / maxhealth;
    }
}
