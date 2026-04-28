using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTowerUIController : MonoBehaviour
{
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private Image _barlifeFillable;

    private void OnEnable()
    {
        if (_lifeController != null)
        {
            _lifeController.OnHealthChanged += UpdateTowerLifeText;
            UpdateTowerLifeText(_lifeController.GetHp(), _lifeController.GetMaxHp());
        }
        else
        {
            Debug.LogError("LifeUIController: LifeController non trovato!");

        }
    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnHealthChanged -= UpdateTowerLifeText;
    }

    private void UpdateTowerLifeText(int lifeNum, int maxhealth)
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.GameOver) return;
        if (maxhealth <= 0) return;

        if (_barlifeFillable != null) _barlifeFillable.fillAmount = (float)lifeNum / maxhealth;
    }
}
