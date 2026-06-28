using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Progress")]
    public int currency = 100;

    [Header("Selected Weapon")]
    public string selectedWeaponID = "nerf_gun";

    [Header("Selected Level")]
    public string selectedLevelID = "NORMAL_01";

    [Header("Owned Shop Upgrades")]
    public List<string> ownedUpgradeIDs = new List<string>();

    [Header("Level Progress")]
    public List<string> completedLevelIDs = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SelectWeapon(string weaponID)
    {
        selectedWeaponID = weaponID;
        Debug.Log("Selected Weapon: " + selectedWeaponID);
    }

    public void SelectLevel(string levelID)
    {
        selectedLevelID = levelID;
        Debug.Log("Selected Level: " + selectedLevelID);
    }

    public bool HasUpgrade(string upgradeID)
    {
        return ownedUpgradeIDs.Contains(upgradeID);
    }

    public bool BuyUpgrade(string upgradeID, int cost)
    {
        if (ownedUpgradeIDs.Contains(upgradeID))
        {
            Debug.Log("Already bought: " + upgradeID);
            return false;
        }

        if (currency < cost)
        {
            Debug.Log("Not enough currency. Have: " + currency + " Need: " + cost);
            return false;
        }

        currency -= cost;
        ownedUpgradeIDs.Add(upgradeID);

        Debug.Log("Bought upgrade: " + upgradeID);
        Debug.Log("Remaining currency: " + currency);
        return true;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        Debug.Log("Currency added: " + amount + " | Total: " + currency);
    }

    public void CompleteLevel(string levelID)
    {
        if (!completedLevelIDs.Contains(levelID))
        {
            completedLevelIDs.Add(levelID);
            Debug.Log("Level completed: " + levelID);
        }
    }

    // Called when the player dies — clears run loot but keeps currency and upgrades
    public void OnPlayerDeath()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ClearInventory();

        Debug.Log("Player died. Inventory cleared. Currency and upgrades retained.");
    }
}
