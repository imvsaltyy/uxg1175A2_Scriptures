using UnityEngine;
using UnityEngine.UI;

public class LootBoxUI : MonoBehaviour
{
    public Transform gridParent;
    public inventoryUI inventoryUIPanel;
    public Text rarityText;    // assign in Inspector to show rarity label
    public Text sellValueText; // assign in Inspector to show sell value

    // Set by lootBox.cs before this panel activates
    public static GameObject assignedLoot;
    public static string assignedRarity = "Common";
    public static float assignedSellValue = 0f;

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
        Image slotImage = firstSlot.GetComponent<Image>();

        if (slotImage == null)
        {
            Debug.LogWarning("LootBoxUI: first slot has no Image component.");
            return null;
        }

        return slotImage;
    }

    void RefreshUI()
    {
        Image slotImage = GetLootSlotImage();
        if (slotImage == null) return;

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
    }

    // Wired to "Take" button in Inspector
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

        GameObject toBeAdded = Instantiate(assignedLoot, InventoryManager.Instance.transform);

        iInventory inv = toBeAdded.GetComponent<iInventory>();
        if (inv != null)
        {
            inv.rarity = assignedRarity;
            inv.sellValue = assignedSellValue;
        }

        InventoryManager.Instance.AddItem(toBeAdded);

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

        assignedLoot = null;
        gameObject.SetActive(false);
    }
}
