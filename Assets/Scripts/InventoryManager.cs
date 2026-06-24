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
        //Debug.Log(item.GetComponent<iInventory>().lootID + " added");

        Debug.Log(item);
    }

    public void RemoveItem(GameObject item)
    {
        inventory.Remove(item);
        Destroy(item);
    }

    public void FindAll()
    {
        Debug.Log("Inventory Count: " + inventory.Count);
    }
}
