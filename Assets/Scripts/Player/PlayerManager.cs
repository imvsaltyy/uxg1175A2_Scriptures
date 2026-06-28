using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Currency UI")]
    [SerializeField] private TMP_Text currencyText;

    AudioManager audioManager;

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
        {
            currencyText.text = "$" + currency.ToString();
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        UpdateCurrencyUI();
    }

    //=========================
    // WEAPON & LEVEL
    //=========================

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

    //=========================
    // CURRENCY FUNCTIONS
    //=========================

    // Add money
    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyUI();

        Debug.Log("Added $" + amount + " | Current Currency: $" + currency);
    }

    // Spend money
    public bool SpendCurrency(int amount)
    {
        if (currency < amount)
        {
            Debug.Log("Not enough currency!");
            return false;
        }

        currency -= amount;
        UpdateCurrencyUI();

        Debug.Log("Spent $" + amount + " | Current Currency: $" + currency);
        return true;
    }

    // Returns current amount of currency
    public int GetCurrency()
    {
        return currency;
    }

    // Check if player has enough money
    public bool CanAfford(int amount)
    {
        return currency >= amount;
    }

    //=========================
    // SHOP UPGRADES
    //=========================

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

        if (!SpendCurrency(cost))
            return false;

        ownedUpgradeIDs.Add(upgradeID);

        Debug.Log("Bought upgrade: " + upgradeID);
        return true;
    }

    //=========================
    // LEVELS
    //=========================

    public void CompleteLevel(string levelID)
    {
        if (!completedLevelIDs.Contains(levelID))
        {
            completedLevelIDs.Add(levelID);
            Debug.Log("Level completed: " + levelID);
        }
    }

    //=========================
    // PLAYER DEATH
    //=========================

    public void OnPlayerDeath()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ClearInventory();

        audioManager.PlaySFX(audioManager.characterdeath);

        Debug.Log("Player died. Inventory cleared. Currency and upgrades retained.");
    }
}