using System.Collections.Generic;
using UnityEngine;

public class EntUIController : MonoBehaviour
{
    [SerializeField] private List<EntsUIButton> _EntsButtonList = new List<EntsUIButton>();

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged += OnCoinsChanged;
            GameManager.Instance.OnBlueGemsChanged += OnBlueGemsChanged;

            UpdateButtons();
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged -= OnCoinsChanged;
            GameManager.Instance.OnBlueGemsChanged -= OnBlueGemsChanged;
        }
    }
    private void OnCoinsChanged(int coins)
    {
        UpdateButtons();
    }

    private void OnBlueGemsChanged(int blueGems)
    {
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (GameManager.Instance == null) return;

        foreach (EntsUIButton entButton in _EntsButtonList)
        {
            if (entButton == null) continue;
            if (entButton._EntData == null) continue;
            if (entButton._EntButton == null) continue;

            bool hasGold = GameManager.Instance.currentCoins >= entButton._EntData.goldPrice;
            bool hasBlueGems = GameManager.Instance.currentBlueGems >= entButton._EntData.blueGemsPrice;

            if (hasGold && hasBlueGems)
            {
                entButton._EntButton.interactable = true;
            }
            else
            {
                entButton._EntButton.interactable = false;
            }
        }
    }

}
