using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text totalSellValueText;
    public TMP_Text currencyText;
    public inventoryUI inventoryUIPanel;

    private void OnEnable()
    {
        RefreshSellScreen();
    }

    public void RefreshSellScreen()
    {
        UpdateTotalSellValueText();
        UpdateCurrencyText();

        if (inventoryUIPanel != null)
        {
            inventoryUIPanel.refreshInventory();
        }
    }

    public int CalculateTotalSellValue()
    {
        if (InventoryManager.Instance == null)
        {
            return 0;
        }

        int total = 0;

        for (int i = 0; i < InventoryManager.Instance.inventory.Count; i++)
        {
            GameObject item = InventoryManager.Instance.inventory[i];

            if (item == null) continue;

            iInventory itemData = item.GetComponent<iInventory>();

            if (itemData == null)
            {
                itemData = item.GetComponentInChildren<iInventory>(true);
            }

            if (itemData != null)
            {
                total += Mathf.FloorToInt(itemData.FinalSellValue);
            }
        }

        return total;
    }

    public void SellAllLoot()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("ShopSellUI: No InventoryManager found.");
            return;
        }

        if (PlayerManager.Instance == null)
        {
            Debug.LogWarning("ShopSellUI: No PlayerManager found.");
            return;
        }

        int totalSellValue = CalculateTotalSellValue();

        if (totalSellValue <= 0)
        {
            Debug.Log("No loot to sell.");
            RefreshSellScreen();
            return;
        }

        PlayerManager.Instance.AddCurrency(totalSellValue);

        // Clear all inventory loot after selling
        for (int i = InventoryManager.Instance.inventory.Count - 1; i >= 0; i--)
        {
            GameObject item = InventoryManager.Instance.inventory[i];

            if (item != null)
            {
                Destroy(item);
            }
        }

        InventoryManager.Instance.inventory.Clear();

        Debug.Log("Sold all loot for $" + totalSellValue);

        RefreshSellScreen();
    }

    void UpdateTotalSellValueText()
    {
        if (totalSellValueText != null)
        {
            totalSellValueText.text = "Total Sell Value: $" + CalculateTotalSellValue();
        }
    }

    void UpdateCurrencyText()
    {
        if (currencyText != null && PlayerManager.Instance != null)
        {
            currencyText.text = "$" + PlayerManager.Instance.currency;
        }
    }
}