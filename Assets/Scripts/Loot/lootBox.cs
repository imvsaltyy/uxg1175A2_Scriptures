using Unity.VisualScripting;
using UnityEngine;

public class lootBox : iInventory
{
    private bool lootAssigned = false;
    public GameObject lootBoxUI;
    public GameObject inventoryUI;

    public GameObject[] lootBoxPrefabs;
    [HideInInspector] public GameObject assignedLoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lootBoxDrop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void lootBoxDrop()
    {
        int random = UnityEngine.Random.Range(1, 100);

        if (!lootAssigned)
        {
            for (int i = 0; i < GameManager.lootBoxDrop.Length; i++)
            {
                string[] columns = GameManager.lootBoxDrop[i].Split(',');

                if (random > int.Parse(columns[3]))
                {

                    lootAssigned = true;
                    //Debug.Log("Random: " + random);
                    //Debug.Log("Assigned Loot: " + columns[0]);

                    lootID = columns[0];
                    sellValue = float.Parse(columns[2]);
                    dropRate = float.Parse(columns[3]);

                    break;
                }

                else
                {
                    continue;
                }
            }
        }

        if (!lootAssigned)
        {
            Debug.Log("Loot Box Drop not found");
        }

        if (lootID == null)
        {
            Debug.Log("Item if not assigned!");
        }

        for (int i = 0; i < lootBoxPrefabs.Length; i++)
        {

            if (lootBoxPrefabs[i].name == lootID)
            {
                assignedLoot = lootBoxPrefabs[i];
                Debug.Log("Loot Assigned: " + assignedLoot);
            }
        }

        if (assignedLoot == null)
        {
            Debug.Log("Assigned Prefab not found!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            LootBoxUI.assignedLoot = assignedLoot;
            lootBoxUI.gameObject.SetActive(true);
            inventoryUI.gameObject.SetActive(true);
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            lootBoxUI.gameObject.SetActive(false);
            inventoryUI.gameObject.SetActive(false);
        }
    }


}
