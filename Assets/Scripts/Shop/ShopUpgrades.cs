using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ShopUpgrades : MonoBehaviour
{
    [Header("Shop Upgrade ID")]
    public string shopUpgradeID;

    [Header("Assigned From CSV")]
    [HideInInspector] public string upgradeName;
    [HideInInspector] public string description;
    [HideInInspector] public string statType;
    [HideInInspector] public float value;
    [HideInInspector] public int cost;
    [HideInInspector] public bool isOneTime;

    [Header("UI (assign per upgrade button)")]
    public TMP_Text nameText;
    public TMP_Text descText;
    public TMP_Text costText;
    public Button buyButton;

    [Header("Purchase State")]
    public bool isPurchased;

    public static List<string> purchasedUpgradeIDs = new List<string>();

    // One shared currency text for the whole shop scene — assign on ANY one upgrade, all will find it
    public static TMP_Text sharedCurrencyText;
    public TMP_Text currencyTextToRegister; // drag your currency TMP_Text here on ONE upgrade object

    private void Start()
    {
        // Register the shared currency text if this object has one assigned
        if (currencyTextToRegister != null)
            sharedCurrencyText = currencyTextToRegister;

        statsAssignment();

        if (PlayerManager.Instance != null)
            purchasedUpgradeIDs = PlayerManager.Instance.ownedUpgradeIDs;

        isPurchased = purchasedUpgradeIDs.Contains(shopUpgradeID);
        RefreshUI();
        UpdateSharedCurrencyText();
    }

    private void statsAssignment()
    {
        if (GameManager.shopUpgrades == null || GameManager.shopUpgrades.Length == 0)
        {
            Debug.LogError("ShopUpgrades CSV not loaded!");
            return;
        }

        bool assigned = false;
        for (int i = 0; i < GameManager.shopUpgrades.Length; i++)
        {
            string[] col = GameManager.shopUpgrades[i].Split(',');
            if (col.Length < 7) continue;
            if (col[0].Trim() != shopUpgradeID) continue;

            upgradeName = col[1].Trim();
            description = col[2].Trim();
            statType = col[3].Trim();
            value = float.Parse(col[4].Trim());
            cost = Mathf.RoundToInt(float.Parse(col[5].Trim()));
            isOneTime = bool.Parse(col[6].Trim());
            assigned = true;
            Debug.Log(upgradeName + " assigned, cost=$" + cost);
            break;
        }

        if (!assigned)
            Debug.LogWarning("Shop upgrade ID not found: " + shopUpgradeID);
    }

    void RefreshUI()
    {
        if (nameText != null) nameText.text = upgradeName;
        if (descText != null) descText.text = description;
        if (costText != null) costText.text = "$" + cost;
        if (buyButton != null) buyButton.interactable = !isPurchased;
    }

    static void UpdateSharedCurrencyText()
    {
        if (sharedCurrencyText != null && PlayerManager.Instance != null)
            sharedCurrencyText.text = "$" + PlayerManager.Instance.currency;
    }

    // Wire this to every Buy button's OnClick in Inspector
    public void BuyUpgrade()
    {
        if (PlayerManager.Instance == null) { Debug.LogError("No PlayerManager!"); return; }

        if (isOneTime && isPurchased)
        {
            Debug.Log("Already purchased: " + upgradeName);
            return;
        }

        bool success = PlayerManager.Instance.BuyUpgrade(shopUpgradeID, cost);

        if (success)
        {
            isPurchased = true;
            purchasedUpgradeIDs = PlayerManager.Instance.ownedUpgradeIDs;
            RefreshUI();
            UpdateSharedCurrencyText(); // update currency display immediately
            Debug.Log("Purchased: " + upgradeName + " | Remaining: $" + PlayerManager.Instance.currency);
        }
        else
        {
            Debug.Log("Not enough currency. Have: $" + PlayerManager.Instance.currency + " Need: $" + cost);
        }
    }
}
