using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class inventoryUI : MonoBehaviour
{
    private bool display = false;
    public GameObject inventoryLootUI;
    public Transform gridParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        refreshInventory();
    }

    private void OnEnable()
    {
        refreshInventory();
        
    }

    public void displayInventory()
    {
        gameObject.SetActive(!display);
        display = !display;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        else
        {
            refreshInventory();

            gameObject.SetActive(true);
        }

    }

    public void refreshInventory()
    {
        if (InventoryManager.Instance.inventory.Count > 0)
        {
            for (int i = 0; i < InventoryManager.Instance.inventory.Count; i++)
            {
                Image slotImage = gridParent.transform.GetChild(i).GetComponent<Image>();

                Sprite toReplace = InventoryManager.Instance.inventory[i].GetComponent<SpriteRenderer>().sprite;

                Color replaceColor = InventoryManager.Instance.inventory[i].GetComponent<SpriteRenderer>().color;

                slotImage.sprite = toReplace;
                slotImage.color = replaceColor;
            }
        }
    }
}
