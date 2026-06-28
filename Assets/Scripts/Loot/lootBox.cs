using UnityEngine;

public class lootBox : iInventory
{
    [Header("UI")]
    public GameObject lootBoxUIObject;
    public GameObject inventoryUIObject;

    [Header("Loot Pool")]
    public GameObject[] lootBoxPrefabs;

    [Header("CSV Settings")]
    public bool useCSVData = true;

    [Header("Trigger Settings")]
    public bool openOnTriggerEnter = true;
    public string playerTag = "Player";

    [Header("Rolled Loot")]
    [HideInInspector] public GameObject assignedLoot;

    private string rolledRarity = "Common";
    private float rolledSellValue = 0f;

    private bool hasRolledLoot = false;
    private bool isCurrentlyOpen = false;

    void Start()
    {
        FindUIObjects();
        RollLootBoxContents();
    }

    void FindUIObjects()
    {
        GameObject inventoryLoot = GameObject.Find("InventoryLoot");

        if (inventoryLoot == null)
        {
            Debug.LogWarning("lootBox: InventoryLoot not found in scene.");
            return;
        }

        if (lootBoxUIObject == null)
        {
            Transform lootBoxUI = inventoryLoot.transform.Find("LootBoxUI");

            if (lootBoxUI != null)
            {
                lootBoxUIObject = lootBoxUI.gameObject;
            }
            else
            {
                Debug.LogWarning("lootBox: LootBoxUI not found under InventoryLoot.");
            }
        }

        if (inventoryUIObject == null)
        {
            Transform inventoryUI = inventoryLoot.transform.Find("InventoryUI");

            if (inventoryUI != null)
            {
                inventoryUIObject = inventoryUI.gameObject;
            }
            else
            {
                Debug.LogWarning("lootBox: InventoryUI not found under InventoryLoot.");
            }
        }
    }

    void RollLootBoxContents()
    {
        if (hasRolledLoot) return;

        if (useCSVData)
        {
            RollLootFromCSV();
        }
        else
        {
            RollLootFromPrefabArrayOnly();
        }

        hasRolledLoot = true;

        if (assignedLoot != null)
        {
            Debug.Log(gameObject.name + " rolled loot: " + assignedLoot.name +
                " | Rarity: " + rolledRarity +
                " | Sell Value: " + rolledSellValue);
        }
    }

    void RollLootFromCSV()
    {
        if (GameManager.lootBoxDrop == null || GameManager.lootBoxDrop.Length == 0)
        {
            Debug.LogWarning("lootBox: GameManager.lootBoxDropTrial is empty. Falling back to prefab random.");
            RollLootFromPrefabArrayOnly();
            return;
        }

        if (lootBoxPrefabs == null || lootBoxPrefabs.Length == 0)
        {
            Debug.LogWarning("lootBox: No loot prefabs assigned.");
            return;
        }

        float totalDropRate = 0f;

        // Start from 1 if row 0 is your header
        for (int i = 1; i < GameManager.lootBoxDrop.Length; i++)
        {
            string row = GameManager.lootBoxDrop[i];

            if (string.IsNullOrWhiteSpace(row)) continue;

            string[] columns = row.Split(',');

            if (columns.Length < 4)
            {
                Debug.LogWarning("lootBox: CSV row does not have enough columns: " + row);
                continue;
            }

            float dropRate;

            if (float.TryParse(columns[3], out dropRate))
            {
                totalDropRate += dropRate;
            }
            else
            {
                Debug.LogWarning("lootBox: Invalid drop rate in row: " + row);
            }
        }

        if (totalDropRate <= 0)
        {
            Debug.LogWarning("lootBox: Total drop rate is 0. Falling back to prefab random.");
            RollLootFromPrefabArrayOnly();
            return;
        }

        float randomValue = Random.Range(0f, totalDropRate);
        float currentRate = 0f;

        for (int i = 1; i < GameManager.lootBoxDrop.Length; i++)
        {
            string row = GameManager.lootBoxDrop[i];

            if (string.IsNullOrWhiteSpace(row)) continue;

            string[] columns = row.Split(',');

            if (columns.Length < 4) continue;

            string lootName = columns[0].Trim();
            string rarity = columns[1].Trim();

            float sellValue;
            float dropRate;

            if (!float.TryParse(columns[2], out sellValue))
            {
                Debug.LogWarning("lootBox: Invalid sell value for " + lootName);
                sellValue = 0f;
            }

            if (!float.TryParse(columns[3], out dropRate))
            {
                Debug.LogWarning("lootBox: Invalid drop rate for " + lootName);
                continue;
            }

            currentRate += dropRate;

            if (randomValue <= currentRate)
            {
                GameObject matchedPrefab = FindLootPrefabByName(lootName);

                if (matchedPrefab == null)
                {
                    Debug.LogWarning("lootBox: Could not find prefab matching CSV loot name: " + lootName);
                    RollLootFromPrefabArrayOnly();
                    return;
                }

                assignedLoot = matchedPrefab;
                rolledRarity = rarity;
                rolledSellValue = sellValue;

                Debug.Log("CSV rolled loot: " + lootName +
                    " | Rarity: " + rarity +
                    " | Sell Value: " + sellValue +
                    " | Drop Rate: " + dropRate);

                return;
            }
        }

        Debug.LogWarning("lootBox: CSV roll failed. Falling back to prefab random.");
        RollLootFromPrefabArrayOnly();
    }

    GameObject FindLootPrefabByName(string lootName)
    {
        for (int i = 0; i < lootBoxPrefabs.Length; i++)
        {
            if (lootBoxPrefabs[i] == null) continue;

            string prefabName = lootBoxPrefabs[i].name.Trim();

            // Exact match
            if (prefabName == lootName)
            {
                return lootBoxPrefabs[i];
            }

            // Safer match in case prefab has "(Clone)" or spacing issue
            if (prefabName.ToLower() == lootName.ToLower())
            {
                return lootBoxPrefabs[i];
            }
        }

        return null;
    }

    void RollLootFromPrefabArrayOnly()
    {
        if (lootBoxPrefabs == null || lootBoxPrefabs.Length == 0)
        {
            Debug.LogWarning("lootBox: No loot prefabs assigned.");
            return;
        }

        int randomIndex = Random.Range(0, lootBoxPrefabs.Length);
        assignedLoot = lootBoxPrefabs[randomIndex];

        if (assignedLoot == null)
        {
            Debug.LogWarning("lootBox: Rolled loot is null.");
            return;
        }

        iInventory inv = assignedLoot.GetComponent<iInventory>();

        if (inv == null)
        {
            inv = assignedLoot.GetComponentInChildren<iInventory>(true);
        }

        if (inv != null)
        {
            rolledRarity = inv.rarity;
            rolledSellValue = inv.sellValue;
        }
        else
        {
            Debug.LogWarning(
                "lootBox: Assigned loot '" + assignedLoot.name +
                "' has no iInventory script on itself or its children. Using default rarity and sell value."
            );

            rolledRarity = "Common";
            rolledSellValue = 0f;
        }
    }

    public void OpenLootBox()
    {
        if (isCurrentlyOpen)
        {
            Debug.Log("lootBox: This lootbox is already open.");
            return;
        }

        if (lootBoxUIObject == null || inventoryUIObject == null)
        {
            FindUIObjects();
        }

        if (lootBoxUIObject == null)
        {
            Debug.LogError("lootBox: Cannot open because lootBoxUIObject is missing.");
            return;
        }

        if (assignedLoot == null)
        {
            RollLootBoxContents();
        }

        if (assignedLoot == null)
        {
            Debug.LogWarning("lootBox: This lootbox has no loot.");
            return;
        }

        isCurrentlyOpen = true;

        LootBoxUI.assignedLoot = assignedLoot;
        LootBoxUI.assignedRarity = rolledRarity;
        LootBoxUI.assignedSellValue = rolledSellValue;
        LootBoxUI.currentLootBox = this;

        lootBoxUIObject.SetActive(true);

        if (inventoryUIObject != null)
        {
            inventoryUIObject.SetActive(true);
        }

        LootBoxUI ui = lootBoxUIObject.GetComponent<LootBoxUI>();

        if (ui != null)
        {
            ui.RefreshUI();
        }

        Debug.Log("Opened lootbox: " + gameObject.name +
            " | Showing loot: " + assignedLoot.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Lootbox trigger entered by: " + collision.name + " | Tag: " + collision.tag);

        if (!openOnTriggerEnter) return;

        if (collision.CompareTag(playerTag))
        {
            OpenLootBox();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Lootbox trigger exited by: " + collision.name + " | Tag: " + collision.tag);

        if (collision.CompareTag(playerTag))
        {
            if (LootBoxUI.currentLootBox == this)
            {
                LootBoxUI ui = lootBoxUIObject.GetComponent<LootBoxUI>();

                if (ui != null)
                {
                    ui.CloseLootBoxUI();
                }
                else
                {
                    lootBoxUIObject.SetActive(false);
                }

                if (inventoryUIObject != null)
                {
                    inventoryUIObject.SetActive(false);
                }

                isCurrentlyOpen = false;

                Debug.Log("Player left lootbox area. Lootbox UI and Inventory UI closed.");
            }
        }
    }
}