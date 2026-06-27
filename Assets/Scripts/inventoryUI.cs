using UnityEngine;
using UnityEngine.UI;

public class inventoryUI : MonoBehaviour
{
    public GameObject inventoryLootUI;
    public Transform gridParent;

    private void OnEnable()
    {
        refreshInventory();
    }

    // Called by a button to show/hide this panel
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
            Image slotImage = gridParent.GetChild(i).GetComponent<Image>();
            if (slotImage == null) continue;

            if (i < itemCount)
            {
                GameObject item = InventoryManager.Instance.inventory[i];
                SpriteRenderer sr = item.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    slotImage.sprite = sr.sprite;
                    slotImage.color = sr.color;
                }
            }
            else
            {
                // Clear empty slots
                slotImage.sprite = null;
                slotImage.color = new Color(1f, 1f, 1f, 0.2f);
            }
        }
    }

    // Called by a UI button on each inventory slot to sell that item
    public void SellItem(int slotIndex)
    {
        if (InventoryManager.Instance == null) return;
        if (slotIndex >= InventoryManager.Instance.inventory.Count) return;

        GameObject item = InventoryManager.Instance.inventory[slotIndex];
        iInventory itemData = item.GetComponent<iInventory>();

        if (itemData == null) return;

        int sellAmount = Mathf.FloorToInt(itemData.sellValue);

        if (PlayerManager.Instance != null)
            PlayerManager.Instance.AddCurrency(sellAmount);

        Debug.Log("Sold " + itemData.lootID + " for " + sellAmount + " currency.");

        InventoryManager.Instance.RemoveItem(item);
        refreshInventory();
    }
}
