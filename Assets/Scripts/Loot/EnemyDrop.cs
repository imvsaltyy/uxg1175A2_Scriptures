using UnityEngine;

public class EnemyDrop : iInventory
{
    public string toID;

    [HideInInspector] public GameObject toBeAddedItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statsAssignment();
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.enemyDrop.Length; i++)
        {
            string[] columns = GameManager.enemyDrop[i].Split(',');

            if (columns[0] == toID)
            {
                assigned = true;

                lootID = toID;
                sellValue = float.Parse(columns[2]);
                dropRate = float.Parse(columns[3]);
            }
        }

        if (!assigned)
        {
            Debug.Log("Dropped item not found!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Item is picked up by Payer");
            //InventoryManager.Instance.AddItem(gameObject);

            toBeAddedItem = Instantiate(gameObject, InventoryManager.Instance.transform);

            InventoryManager.Instance.AddItem(toBeAddedItem);   

            Destroy(gameObject);

        }
    }



}
