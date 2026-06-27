using UnityEngine;

public class iInventory : MonoBehaviour
{
    [HideInInspector] public string lootID;
    [HideInInspector] public string rarity;   // "Common", "Uncommon", "Rare", "Epic", "Legendary"
    [HideInInspector] public float sellValue; // base sell value from CSV
    [HideInInspector] public float dropRate;
    [HideInInspector] public Sprite sprite;

    /// Final sell value after applying rarity multiplier
    public float FinalSellValue
    {
        get
        {
            float mult = GameManager.GetRarityMultiplier(rarity);
            return sellValue * mult;
        }
    }
}
