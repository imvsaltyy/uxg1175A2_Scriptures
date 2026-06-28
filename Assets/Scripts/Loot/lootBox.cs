using UnityEngine;

public class lootBox : iInventory
{
    public GameObject lootBoxUIObject;
    public GameObject inventoryUIObject;
    public GameObject[] lootBoxPrefabs;

    [HideInInspector] public GameObject assignedLoot;

    // Rolled rarity name for this box (e.g. "Common", "Legendary")
    private string rolledRarity = "Common";

    void Start()
    {
        FindUIObjects();
        RollLootBoxContents();
    }

    void FindUIObjects()
    {
        GameObject InventoryLoot = GameObject.Find("InventoryLoot");

        if (InventoryLoot == null)
        {
            Debug.LogWarning("InventoryLoot not found in scene!");
            return;
        }

        Transform lootBoxUI = InventoryLoot.transform.Find("LootBoxUI");
        Transform inventoryUI = InventoryLoot.transform.Find("InventoryUI");

        if (lootBoxUI != null)
        {
            lootBoxUIObject = lootBoxUI.gameObject;
        }

        if (inventoryUI != null)
        {
            inventoryUIObject = inventoryUI.gameObject;
        }
    }

    void RollLootBoxContents()
    {
        // --- Step 1: Roll rarity using LootBoxRarityTrial.csv ---
        // Columns: [0]=rarityID, [1]=rarityName, [2]=spawnRate%
        // spawnRate is a percentage out of 100, so we use a 0-100 roll
        rolledRarity = RollRarity();

        // --- Step 2: Pick a loot item from LootBoxDropTrial.csv ---
        // Columns: [0]=lootID, [1]=category, [2]=sellValue, [3]=dropRate (out of 100)
        // dropRate is stored as a percentage (25, 15, 10 etc.) — roll 0-100
        string pickedLootID = RollLootItem(rolledRarity);

        if (pickedLootID == null)
        {
            Debug.LogWarning("LootBox: No loot item was rolled.");
            return;
        }

        lootID = pickedLootID;
        rarity = rolledRarity;

        // Find base sell value from the loot table
        for (int i = 0; i < GameManager.lootBoxDrop.Length; i++)
        {
            string[] cols = GameManager.lootBoxDrop[i].Split(',');
            if (cols.Length < 4) continue;
            if (cols[0].Trim() == lootID)
            {
                sellValue = float.Parse(cols[2].Trim());
                break;
            }
        }

        // --- Step 3: Match to a prefab ---
        for (int i = 0; i < lootBoxPrefabs.Length; i++)
        {
            if (lootBoxPrefabs[i].name == lootID)
            {
                assignedLoot = lootBoxPrefabs[i];
                Debug.Log("LootBox assigned: " + lootID + " [" + rolledRarity + "] SellValue=" + FinalSellValue);
                return;
            }
        }

        Debug.LogWarning("LootBox: prefab not found for lootID: " + lootID);
    }

    string RollRarity()
    {
        if (GameManager.lootBoxRarity == null || GameManager.lootBoxRarity.Length == 0)
            return "Common";

        // Get multipliers for this level's type if LevelManager exists
        float[] mults = null;
        if (LevelManager.Instance != null)
            mults = GameManager.GetLootBoxMultipliers(LevelManager.Instance.lootboxRatesMultiplierID);

        // Build weighted table
        // Rarity order in multiplier CSV columns [4..8]: Common, Uncommon, Rare, Epic, Legendary
        string[] rarityNames = { "Common", "Uncommon", "Rare", "Epic", "Legendary" };
        float total = 0f;
        float[] weights = new float[GameManager.lootBoxRarity.Length];

        for (int i = 0; i < GameManager.lootBoxRarity.Length; i++)
        {
            string[] cols = GameManager.lootBoxRarity[i].Split(',');
            if (cols.Length < 3) continue;

            float baseRate = float.Parse(cols[2].Trim());
            float mult = (mults != null && i < mults.Length) ? mults[4 + i] : 1f; // cols [4..8] = rarity mults
            weights[i] = baseRate * mult;
            total += weights[i];
        }

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        for (int i = 0; i < GameManager.lootBoxRarity.Length; i++)
        {
            string[] cols = GameManager.lootBoxRarity[i].Split(',');
            if (cols.Length < 3) continue;

            cumulative += weights[i];
            if (roll <= cumulative)
            {
                string name = cols[1].Trim(); // "Common", "Uncommon", etc.
                Debug.Log("Rolled rarity: " + name + " (roll=" + roll + "/" + total + ")");
                return name;
            }
        }

        return "Common";
    }

    string RollLootItem(string rarityName)
    {
        if (GameManager.lootBoxDrop == null || GameManager.lootBoxDrop.Length == 0)
            return null;

        // dropRate column is out of 100 — build weighted table
        float total = 0f;
        float[] weights = new float[GameManager.lootBoxDrop.Length];

        for (int i = 0; i < GameManager.lootBoxDrop.Length; i++)
        {
            string[] cols = GameManager.lootBoxDrop[i].Split(',');
            if (cols.Length < 4) continue;
            float.TryParse(cols[3].Trim(), out weights[i]);
            total += weights[i];
        }

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        for (int i = 0; i < GameManager.lootBoxDrop.Length; i++)
        {
            string[] cols = GameManager.lootBoxDrop[i].Split(',');
            if (cols.Length < 4) continue;

            cumulative += weights[i];
            if (roll <= cumulative)
                return cols[0].Trim();
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        LootBoxUI.assignedLoot = assignedLoot;
        LootBoxUI.assignedRarity = rolledRarity;
        LootBoxUI.assignedSellValue = FinalSellValue;

        if (lootBoxUIObject != null) lootBoxUIObject.SetActive(true);
        if (inventoryUIObject != null) inventoryUIObject.SetActive(true);

        Debug.Log(lootBoxUIObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (lootBoxUIObject != null) lootBoxUIObject.SetActive(false);
        if (inventoryUIObject != null) inventoryUIObject.SetActive(false);
    }
}
