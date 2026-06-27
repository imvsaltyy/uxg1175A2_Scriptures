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
        Image slot = gridParent.GetChild(0).GetComponent<Image>();
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

    void RefreshUI()
    {
        Image slotImage = gridParent.GetChild(0).GetComponent<Image>();
        if (slotImage == null) return;

        if (assignedLoot != null)
        {
            SpriteRenderer sr = assignedLoot.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                slotImage.sprite = sr.sprite;
                slotImage.color = sr.color;
            }
        }
        else
        {
            slotImage.sprite = originalSprite;
            slotImage.color = originalColor;
        }

        // Update optional rarity and sell value labels
        if (rarityText != null)
            rarityText.text = assignedRarity;

        if (sellValueText != null)
            sellValueText.text = "Sell: $" + assignedSellValue.ToString("F0");
    }

    // Wired to "Take" button in Inspector
    public void AddIntoInventory()
    {
        if (assignedLoot == null)
        {
            Debug.LogWarning("LootBoxUI: No loot to add.");
            return;
        }

        GameObject toBeAdded = Instantiate(assignedLoot, InventoryManager.Instance.transform);

        // Carry rarity and final sell value onto the inventory item
        iInventory inv = toBeAdded.GetComponent<iInventory>();
        if (inv != null)
        {
            inv.rarity = assignedRarity;
            inv.sellValue = assignedSellValue; // already has multiplier applied
        }

        InventoryManager.Instance.AddItem(toBeAdded);
        Debug.Log("Added " + assignedLoot.name + " [" + assignedRarity + "] to inventory.");

        if (inventoryUIPanel != null)
            inventoryUIPanel.refreshInventory();

        // Disable take button so player can't take twice from same box
        Button btn = GetComponentInChildren<Button>();
        if (btn != null) btn.interactable = false;

        assignedLoot = null;
        gameObject.SetActive(false);
    }
}
