using UnityEngine;

public class EnemyDrop : iInventory
{
    // Set this in the prefab Inspector to match the lootID in EnemyLootDropTrial.csv
    public string toID;

    void Start()
    {
        statsAssignment();

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sprite = sr.sprite;
    }

    void statsAssignment()
    {
        if (GameManager.enemyDrop == null || GameManager.enemyDrop.Length == 0)
        {
            Debug.LogError("EnemyDrop: enemyDrop CSV not loaded!");
            return;
        }

        bool assigned = false;

        for (int i = 0; i < GameManager.enemyDrop.Length; i++)
        {
            string[] cols = GameManager.enemyDrop[i].Split(',');
            if (cols.Length < 4) continue;

            if (cols[0].Trim() == toID.Trim())
            {
                lootID = cols[0].Trim();
                // col[1] = Category (EnemyDrop) — not needed at runtime
                sellValue = float.Parse(cols[2].Trim());
                dropRate = float.Parse(cols[3].Trim());
                rarity = "Common"; // enemy drops are always Common rarity
                assigned = true;
                Debug.Log("EnemyDrop stats assigned: " + lootID);
                break;
            }
        }

        if (!assigned)
        {
            Debug.LogWarning("EnemyDrop: ID not found in CSV: " + toID);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log("Item picked up: " + lootID);

        // Re-parent into InventoryManager and deactivate (AddItem handles SetActive(false))
        transform.SetParent(InventoryManager.Instance.transform);
        InventoryManager.Instance.AddItem(gameObject);
    }
}
