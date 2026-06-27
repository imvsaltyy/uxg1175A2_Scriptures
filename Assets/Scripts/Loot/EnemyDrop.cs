using UnityEngine;

public class EnemyDrop : iInventory
{
    public string toID;

    void Start()
    {
        statsAssignment();

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            sprite = sr.sprite;
    }

    void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.enemyDrop.Length; i++)
        {
            string[] columns = GameManager.enemyDrop[i].Split(',');
            if (columns.Length < 4) continue;

            if (columns[0].Trim() == toID)
            {
                lootID = toID;
                sellValue = float.Parse(columns[2].Trim());
                dropRate = float.Parse(columns[3].Trim());
                assigned = true;
                break;
            }
        }

        if (!assigned)
        {
            Debug.LogWarning("EnemyDrop: item not found in CSV for ID: " + toID);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log("Item picked up: " + lootID);

        // Move this GameObject into InventoryManager and deactivate it there
        transform.SetParent(InventoryManager.Instance.transform);
        InventoryManager.Instance.AddItem(gameObject);
    }
}
