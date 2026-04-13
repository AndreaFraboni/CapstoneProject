using System.Collections.Generic;
using UnityEngine;

public class TowerUIController : MonoBehaviour
{
    [SerializeField] private List<TowerUIButton> _towersButtonList = new List<TowerUIButton>();

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged += UpdateCurrentCoins;
        }

        UpdateCurrentCoins(GameManager.Instance.currentCoins);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnCoinsChanged -= UpdateCurrentCoins;
    }

    private void UpdateCurrentCoins(int coins)
    {
        foreach (TowerUIButton towerButton in _towersButtonList)
        {
            if (towerButton == null) continue;

            if (coins >= towerButton._towerData.goldPrice)
                towerButton._towerButton.interactable = true;
            else
                towerButton._towerButton.interactable = false;
        }
    }


}
