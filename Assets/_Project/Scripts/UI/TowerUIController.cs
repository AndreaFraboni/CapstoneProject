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

            GameManager.Instance.OnTowersNotBuildable += DeactiveTowersUI;
            GameManager.Instance.OnTowersBuildable += ReactiveTowersUI;
        }

        UpdateCurrentCoins(GameManager.Instance.currentCoins);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnCoinsChanged -= UpdateCurrentCoins;

        GameManager.Instance.OnTowersNotBuildable -= DeactiveTowersUI;
        GameManager.Instance.OnTowersBuildable -= ReactiveTowersUI;
    }

    private void DeactiveTowersUI()
    {
        foreach (TowerUIButton towerButton in _towersButtonList)
        {
            if (towerButton == null) continue;
            towerButton._towerButton.interactable = false; // all Towers buttons deactivated why overcome max towers present in scene togheter !!!
        }
    }

    private void ReactiveTowersUI()
    {
        UpdateCurrentCoins(GameManager.Instance.currentCoins); // I use updatecurrent coins for reactive all Towers buttons
    }

    private void UpdateCurrentCoins(int coins)
    {
        if (_towersButtonList.Count > 0)
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
        else
        {
            Debug.LogError("Towers Button List is EMPTY !!!!!");
            return;
        }
    }


}
