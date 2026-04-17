using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeEnemyUIController : MonoBehaviour
{
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private Image _barlifeFillable;

    private void OnEnable()
    {
        if (_lifeController != null)
        {
            _lifeController.OnHealthChanged += UpdateLifeText;
            UpdateLifeText(_lifeController.GetHp(), _lifeController.GetMaxHp());
        }
        else
        {
            Debug.LogError("LifeEnemyUIController: LifeController non trovato!");

        }
    }

    private void OnDisable()
    {
        if (_lifeController != null) _lifeController.OnHealthChanged -= UpdateLifeText;
    }

    private void UpdateLifeText(int lifeNum, int maxhealth)
    {
        if (_barlifeFillable != null) _barlifeFillable.fillAmount = (float)lifeNum / maxhealth;
    }
}
