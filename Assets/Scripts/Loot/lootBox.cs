using UnityEngine;

public class lootBox : iInventory
{
    private bool lootAssigned = false;
    public GameObject lootBoxUI;
    public GameObject inventoryUIObject;

    public GameObject[] lootBoxPrefabs;
    [HideInInspector] public GameObject assignedLoot;

    void Start()
    {
        AssignLootBoxDrop();
    }

    void AssignLootBoxDrop()
    {
        if (GameManager.lootBoxDrop == null || GameManager.lootBoxDrop.Length == 0)
        {
            Debug.LogWarning("LootBox drop table not loaded.");
            return;
        }

        int random = Random.Range(1, 101);

        for (int i = 0; i < GameManager.lootBoxDrop.Length; i++)
        {
            string[] columns = GameManager.lootBoxDrop[i].Split(',');
            if (columns.Length < 4) continue;

            int dropThreshold = int.Parse(columns[3].Trim());

            if (random <= dropThreshold)
            {
                lootAssigned = true;
                lootID = columns[0].Trim();
                sellValue = float.Parse(columns[2].Trim());
                dropRate = float.Parse(columns[3].Trim());
                break;
            }
        }

        if (!lootAssigned)
        {
            Debug.Log("No loot assigned to loot box (rolled above all thresholds).");
            return;
        }

        // Find the prefab that matches the rolled loot ID
        for (int i = 0; i < lootBoxPrefabs.Length; i++)
        {
            if (lootBoxPrefabs[i].name == lootID)
            {
                assignedLoot = lootBoxPrefabs[i];
                Debug.Log("LootBox assigned: " + assignedLoot.name);
                return;
            }
        }

        Debug.LogWarning("LootBox prefab not found for lootID: " + lootID);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        LootBoxUI.assignedLoot = assignedLoot;

        if (lootBoxUI != null)
            lootBoxUI.SetActive(true);

        if (inventoryUIObject != null)
            inventoryUIObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (lootBoxUI != null)
            lootBoxUI.SetActive(false);

        if (inventoryUIObject != null)
            inventoryUIObject.SetActive(false);
    }
}
