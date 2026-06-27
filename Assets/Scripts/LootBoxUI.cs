using UnityEngine;
using UnityEngine.UI;

public class LootBoxUI : MonoBehaviour
{
    public Transform gridParent;
    public inventoryUI inventoryUIPanel;

    public static GameObject assignedLoot;

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
            else
            {
                Debug.LogWarning("assignedLoot has no SpriteRenderer: " + assignedLoot.name);
            }
        }
        else
        {
            Debug.Log("No loot assigned to LootBoxUI.");
            slotImage.sprite = originalSprite;
            slotImage.color = originalColor;
        }
    }

    // Called by the "Take" button in the loot box UI
    public void addIntoInventory()
    {
        if (assignedLoot == null)
        {
            Debug.LogWarning("No loot to add.");
            return;
        }

        GameObject toBeAdded = Instantiate(assignedLoot, InventoryManager.Instance.transform);
        InventoryManager.Instance.AddItem(toBeAdded);

        Debug.Log("Added " + assignedLoot.name + " to inventory.");

        // Refresh inventory panel if open
        if (inventoryUIPanel != null)
            inventoryUIPanel.refreshInventory();

        // Disable the take button so player can't take twice
        Button btn = GetComponentInChildren<Button>();
        if (btn != null) btn.interactable = false;

        assignedLoot = null;
        gameObject.SetActive(false);
    }
}
