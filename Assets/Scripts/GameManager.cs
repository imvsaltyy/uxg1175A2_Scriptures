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
    [HideInInspector] public static string[] lootBoxSize;
    [HideInInspector] public static string[] lootBoxRarity;
    [HideInInspector] public static string[] lootBoxMultiplier;
    [HideInInspector] public static string[] rarityMultiplier;

    void Awake()
    {
        enemyType = LoadCSV("EnemyStatsTrial");
        weaponTypes = LoadCSV("WeaponStatsTrial");
        playerStats = LoadCSV("PlayerStatsTrial");
        shopUpgrades = LoadCSV("ShopUpgradesTrial");
        enemyDrop = LoadCSV("EnemyLootDropTrial");
        lootBoxDrop = LoadCSV("LootBoxDropTrial");
        levelSelections = LoadCSV("LevelSelectionsTrial");
        lootBoxSize = LoadCSV("LootBoxSizeTrial");
        lootBoxRarity = LoadCSV("LootBoxRarityTrial");
        lootBoxMultiplier = LoadCSV("LootBoxMultiplierTrial");
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

        for (int i = 1; i < lines.Length; i++) // skip header row
        {
            string line = lines[i].Trim();
            if (!string.IsNullOrWhiteSpace(line))
                rows.Add(line);
        }

        Debug.Log(fileName + " loaded: " + rows.Count + " rows.");
        return rows.ToArray();
    }

    // -------------------------------------------------------
    // Static helpers so any script can look up data cleanly
    // -------------------------------------------------------

    /// Returns the sell value multiplier for a given rarity name (e.g. "Common", "Legendary")
    public static float GetRarityMultiplier(string rarityName)
    {
        if (rarityMultiplier == null) return 1f;

        foreach (string row in rarityMultiplier)
        {
            string[] cols = row.Split(',');
            if (cols.Length < 2) continue;

            if (cols[0].Trim().Equals(rarityName, System.StringComparison.OrdinalIgnoreCase))
            {
                // Values stored as "*1", "*2" etc — strip the asterisk
                string raw = cols[1].Trim().Replace("*", "");
                if (float.TryParse(raw, out float mult))
                    return mult;
            }
        }

        Debug.LogWarning("Rarity multiplier not found for: " + rarityName);
        return 1f;
    }

    /// Returns the lootbox multiplier row for a given level type (e.g. "normal", "chest", "swarm")
    /// Columns: [0]=id, [1]=Small, [2]=Medium, [3]=Large, [4]=Common, [5]=Uncommon, [6]=Rare, [7]=Epic, [8]=Legendary
    public static float[] GetLootBoxMultipliers(string multiplierID)
    {
        if (lootBoxMultiplier == null) return null;

        foreach (string row in lootBoxMultiplier)
        {
            string[] cols = row.Split(',');
            if (cols.Length < 9) continue;

            if (cols[0].Trim().Equals(multiplierID.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                float[] mults = new float[8];
                for (int i = 0; i < 8; i++)
                    float.TryParse(cols[i + 1].Trim(), out mults[i]);
                return mults;
            }
        }

        Debug.LogWarning("LootBox multiplier not found for: " + multiplierID);
        return null;
    }
}
