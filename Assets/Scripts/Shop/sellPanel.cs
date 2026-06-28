using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach this to the Sell Panel.
// It reads the live inventory and creates a sell button row for each item.
public class SellPanel : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform itemListParent;   // The vertical layout group that holds rows
    public GameObject sellRowPrefab;   // A prefab with: Image, TMP_Text (name), TMP_Text (price), Button

    private void OnEnable()
    {
        RefreshSellPanel();
    }

    public void RefreshSellPanel()
    {
        // Clear old rows
        foreach (Transform child in itemListParent)
            Destroy(child.gameObject);

        if (InventoryManager.Instance == null) return;

        var inventory = InventoryManager.Instance.inventory;

        if (inventory.Count == 0)
        {
            Debug.Log("Inventory is empty — nothing to sell.");
            return;
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject item = inventory[i];
            iInventory data = item.GetComponent<iInventory>();
            if (data == null) continue;

            // Instantiate a row
            GameObject row = Instantiate(sellRowPrefab, itemListParent);

            // Set item icon
            Image icon = row.transform.Find("Icon")?.GetComponent<Image>();
            if (icon != null)
            {
                SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
                if (sr != null) { icon.sprite = sr.sprite; icon.color = sr.color; }
            }

            // Set item name + rarity
            TMP_Text nameLabel = row.transform.Find("NameText")?.GetComponent<TMP_Text>();
            if (nameLabel != null)
                nameLabel.text = data.lootID + (string.IsNullOrEmpty(data.rarity) ? "" : " [" + data.rarity + "]");

            // Set sell price
            int sellAmt = Mathf.FloorToInt(data.FinalSellValue);
            TMP_Text priceLabel = row.transform.Find("PriceText")?.GetComponent<TMP_Text>();
            if (priceLabel != null)
                priceLabel.text = "$" + sellAmt;

            // Wire sell button — capture index for lambda
            int capturedIndex = i;
            Button sellBtn = row.GetComponentInChildren<Button>();
            if (sellBtn != null)
                sellBtn.onClick.AddListener(() => SellItem(capturedIndex));
        }
    }

    void SellItem(int index)
    {
        if (InventoryManager.Instance == null) return;
        if (index >= InventoryManager.Instance.inventory.Count) return;

        GameObject item = InventoryManager.Instance.inventory[index];
        iInventory data = item.GetComponent<iInventory>();
        if (data == null) return;

        int sellAmt = Mathf.FloorToInt(data.FinalSellValue);

        // Add currency via ShopInventory (which forwards to PlayerManager)
        if (ShopInventory.Instance != null)
            ShopInventory.Instance.AddCurrency(sellAmt);

        Debug.Log("Sold " + data.lootID + " for $" + sellAmt);

        InventoryManager.Instance.RemoveItem(item);
        RefreshSellPanel(); // rebuild list after selling
    }
}
