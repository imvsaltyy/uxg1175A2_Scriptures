using UnityEngine;
using UnityEngine.UI;

public class LootBoxUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform gridParent;
    public inventoryUI inventoryUIPanel;
    public Text rarityText;
    public Text sellValueText;

    [Header("Current Opened LootBox Data")]
    public static GameObject assignedLoot;
    public static string assignedRarity = "Common";
    public static float assignedSellValue = 0f;

    // This stores which physical lootbox is currently opened
    public static lootBox currentLootBox;

    private Sprite originalSprite;
    private Color originalColor;

    void Start()
    {
        Image slot = GetLootSlotImage();

        if (slot != null)
        {
            originalSprite = slot.sprite;
            originalColor = slot.color;
        }

        RefreshUI();
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    Image GetLootSlotImage()
    {
        if (gridParent == null)
        {
            Debug.LogWarning("LootBoxUI: gridParent is not assigned.");
            return null;
        }

        if (gridParent.childCount == 0)
        {
            Debug.LogWarning("LootBoxUI: gridParent has no slots.");
            return null;
        }

        Transform firstSlot = gridParent.GetChild(0);

        // This allows it to find the Image even if it is on a child object
        Image slotImage = firstSlot.GetComponentInChildren<Image>(true);

        if (slotImage == null)
        {
            Debug.LogWarning("LootBoxUI: first slot has no Image component.");
            return null;
        }

        return slotImage;
    }

    public void RefreshUI()
    {
        Image slotImage = GetLootSlotImage();
        if (slotImage == null) return;

        // Make sure the slot/image is visible
        slotImage.gameObject.SetActive(true);

        if (assignedLoot != null)
        {
            SpriteRenderer sr = assignedLoot.GetComponentInChildren<SpriteRenderer>(true);

            if (sr != null)
            {
                slotImage.sprite = sr.sprite;
                slotImage.color = Color.white;
            }
            else
            {
                Debug.LogWarning("LootBoxUI: assignedLoot has no SpriteRenderer.");
            }
        }
        else
        {
            slotImage.sprite = originalSprite;
            slotImage.color = originalColor;
        }

        if (rarityText != null)
            rarityText.text = assignedRarity;

        if (sellValueText != null)
            sellValueText.text = "Sell: $" + assignedSellValue.ToString("F0");

        Debug.Log("LootBoxUI refreshed. Loot: " +
            (assignedLoot != null ? assignedLoot.name : "NULL"));
    }

    // Put this on the Take button OnClick()
    public void AddIntoInventory()
    {
        Debug.Log("Take button clicked.");

        if (assignedLoot == null)
        {
            Debug.LogWarning("LootBoxUI: No loot to add.");
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("No InventoryManager found in scene.");
            return;
        }

        // Make a copy of the loot prefab for inventory
        GameObject toBeAdded = Instantiate(assignedLoot, InventoryManager.Instance.transform);

        // Copy rarity and sell value into the inventory item
        iInventory inv = toBeAdded.GetComponent<iInventory>();
        if (inv != null)
        {
            inv.rarity = assignedRarity;
            inv.sellValue = assignedSellValue;
        }

        InventoryManager.Instance.AddItem(toBeAdded);

        Debug.Log("Added loot into inventory: " + toBeAdded.name);
        Debug.Log("Inventory count is now: " + InventoryManager.Instance.inventory.Count);

        if (inventoryUIPanel != null)
        {
            inventoryUIPanel.refreshInventory();
            Debug.Log("Inventory UI refreshed.");
        }
        else
        {
            Debug.LogWarning("inventoryUIPanel is not assigned in LootBoxUI Inspector.");
        }

        // Destroy the actual lootbox that was opened
        if (currentLootBox != null)
        {
            Destroy(currentLootBox.gameObject);
            currentLootBox = null;
        }

        // Clear shared UI data
        assignedLoot = null;
        assignedRarity = "Common";
        assignedSellValue = 0f;

        gameObject.SetActive(false);
    }

    public void CloseLootBoxUI()
    {
        assignedLoot = null;
        assignedRarity = "Common";
        assignedSellValue = 0f;
        currentLootBox = null;

        gameObject.SetActive(false);
    }
}