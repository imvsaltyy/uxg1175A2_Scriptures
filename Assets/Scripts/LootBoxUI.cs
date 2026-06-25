using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxUI : MonoBehaviour
{
    public GameObject inventoryLootUI;
    public Transform gridParent;

    public static GameObject assignedLoot;
    private Image originalSlotImage;
    private Color originalColor;

    public GameObject lootBoxButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalSlotImage = gridParent.transform.GetChild(0).GetComponent<Image>();
        originalColor = gridParent.transform.GetChild(0).GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (assignedLoot != null)
        {
            Image slotImage = gridParent.transform.GetChild(0).GetComponent<Image>();
            Sprite toReplace = assignedLoot.GetComponent<Sprite>();

            Color replaceColor = assignedLoot.GetComponent<SpriteRenderer>().color;

            slotImage.sprite = toReplace;
            slotImage.color = replaceColor;

        }

        else
        {
            Debug.Log("Item not found");
        }
    }

    public void addIntoInventory()
    {
        Debug.Log("Added item into inventory");

        GameObject toBeAdded = Instantiate(assignedLoot, InventoryManager.Instance.transform);

        InventoryManager.Instance.AddItem(toBeAdded);

        //Image slotImage = gridParent.transform.GetChild(0).GetComponent<Image>();

        //slotImage = originalSlotImage;
        //slotImage.color = originalColor;

        this.gameObject.GetComponent<Button>().interactable = false;

    }


}
