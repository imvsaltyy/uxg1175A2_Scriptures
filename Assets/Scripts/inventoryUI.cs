using UnityEngine;
using UnityEngine.UI;

public class inventoryUI : MonoBehaviour
{
    public Transform gridParent;

    private void OnEnable()
    {
        refreshInventory();
    }

    public void displayInventory()
    {
        bool isCurrentlyActive = gameObject.activeSelf;
        gameObject.SetActive(!isCurrentlyActive);

        if (gameObject.activeSelf)
            refreshInventory();
    }

    public void refreshInventory()
    {
        if (InventoryManager.Instance == null) return;

        int itemCount = InventoryManager.Instance.inventory.Count;
        int slotCount = gridParent.childCount;

        for (int i = 0; i < slotCount; i++)
        {
            Transform slot = gridParent.GetChild(i);
            Image slotImage = slot.GetComponent<Image>();
            if (slotImage == null) continue;

            if (i < itemCount)
            {
                GameObject item = InventoryManager.Instance.inventory[i];
                SpriteRenderer sr = item.GetComponentInChildren<SpriteRenderer>(true);

                if (sr != null)
                {
                    slotImage.sprite = sr.sprite;
                    slotImage.color = sr.color;
                }

                // Show sell value on the slot if there's a Text child
                Text sellLabel = slot.GetComponentInChildren<Text>();
                if (sellLabel != null)
                {
                    iInventory inv = item.GetComponent<iInventory>();
                    if (inv != null)
                        sellLabel.text = "$" + inv.FinalSellValue.ToString("F0");
                }
            }
            else
            {
                // Empty slot — clear it
                slotImage.sprite = null;
                slotImage.color = new Color(1f, 1f, 1f, 0.2f);

                Text sellLabel = slot.GetComponentInChildren<Text>();
                if (sellLabel != null) sellLabel.text = "";
            }
        }
    }

    // Wire this to a Sell button on each inventory slot.
    // Pass the slot index via the Button's OnClick event in the Inspector.
    public void SellItem(int slotIndex)
    {
        if (InventoryManager.Instance == null) return;
        if (slotIndex >= InventoryManager.Instance.inventory.Count) return;

        GameObject item = InventoryManager.Instance.inventory[slotIndex];
        iInventory itemData = item.GetComponent<iInventory>();
        if (itemData == null) return;

        int sellAmount = Mathf.FloorToInt(itemData.FinalSellValue);

        if (PlayerManager.Instance != null)
            PlayerManager.Instance.AddCurrency(sellAmount);

        Debug.Log("Sold " + itemData.lootID + " [" + itemData.rarity + "] for $" + sellAmount);

        InventoryManager.Instance.RemoveItem(item);
        refreshInventory();
    }
}
