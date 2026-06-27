using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<GameObject> inventory = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(GameObject item)
    {
        inventory.Add(item);
        item.SetActive(false);
        Debug.Log("Added to inventory: " + item.name);
    }

    public void RemoveItem(GameObject item)
    {
        inventory.Remove(item);
        Destroy(item);
        Debug.Log("Removed from inventory: " + item.name);
    }

    // Call this when the player dies — wipe everything
    public void ClearInventory()
    {
        for (int i = inventory.Count - 1; i >= 0; i--)
        {
            Destroy(inventory[i]);
        }
        inventory.Clear();
        Debug.Log("Inventory cleared (player died).");
    }

    public int GetCount()
    {
        return inventory.Count;
    }
}
