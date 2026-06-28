using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Existing data arrays
    [HideInInspector] public static string[] enemyType;
    [HideInInspector] public static string[] weaponTypes;
    [HideInInspector] public static string[] playerStats;
    [HideInInspector] public static string[] shopUpgrades;
    [HideInInspector] public static string[] enemyDrop;
    [HideInInspector] public static string[] lootBoxDrop;
    [HideInInspector] public static string[] levelSelections;

    // New data arrays
    [HideInInspector] public static string[] rarityMultiplier;

    // NEW
    [HideInInspector] public static string[] sellValues;

    void Awake()
    {
        enemyType = LoadCSV("EnemyStatsTrial");
        weaponTypes = LoadCSV("WeaponStatsTrial");
        playerStats = LoadCSV("PlayerStatsTrial");
        shopUpgrades = LoadCSV("ShopUpgradesTrial");
        enemyDrop = LoadCSV("EnemyLootDropTrial");
        lootBoxDrop = LoadCSV("LootBoxDropTrial");
        levelSelections = LoadCSV("LevelSelectionsTrial");
        rarityMultiplier = LoadCSV("RarityMultiplierTrial");
    }

    public string[] LoadCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        if (csvFile == null)
        {
            Debug.LogWarning("CSV not found in Resources: " + fileName);
            return new string[0];
        }

        string[] lines = csvFile.text.Split('\n');
        List<string> rows = new List<string>();

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (!string.IsNullOrWhiteSpace(line))
                rows.Add(line);
        }

        Debug.Log(fileName + " loaded: " + rows.Count + " rows.");
        return rows.ToArray();
    }

    //-------------------------------------------------------
    // Rarity Multiplier
    //-------------------------------------------------------

    public static float GetRarityMultiplier(string rarityName)
    {
        if (rarityMultiplier == null)
            return 1f;

        foreach (string row in rarityMultiplier)
        {
            string[] cols = row.Split(',');

            if (cols.Length < 2)
                continue;

            if (cols[0].Trim().Equals(rarityName, System.StringComparison.OrdinalIgnoreCase))
            {
                string raw = cols[1].Trim().Replace("*", "");

                if (float.TryParse(raw, out float mult))
                    return mult;
            }
        }

        Debug.LogWarning("Rarity multiplier not found for: " + rarityName);
        return 1f;
    }

    //-------------------------------------------------------
    // Sell Value Lookup
    //-------------------------------------------------------

    public static int GetSellValue(string lootID)
    {
        if (sellValues == null)
            return 0;

        foreach (string row in sellValues)
        {
            string[] cols = row.Split(',');

            if (cols.Length < 3)
                continue;

            if (cols[0].Trim().Equals(lootID.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(cols[2].Trim(), out int value))
                    return value;
            }
        }

        Debug.LogWarning("Sell value not found for: " + lootID);
        return 0;
    }
}