using UnityEngine;
using System.Collections.Generic;

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

    [Header("Purchase State")]
    public bool isPurchased;

    // Kept for PlayerStats to read from — populated by PlayerManager
    public static List<string> purchasedUpgradeIDs = new List<string>();

    private void Start()
    {
        statsAssignment();

        // Sync static list from PlayerManager on scene load
        if (PlayerManager.Instance != null)
            purchasedUpgradeIDs = PlayerManager.Instance.ownedUpgradeIDs;

        // Reflect already-purchased state in the UI
        isPurchased = purchasedUpgradeIDs.Contains(shopUpgradeID);
    }

    private void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.shopUpgrades.Length; i++)
        {
            string[] columns = GameManager.shopUpgrades[i].Split(',');

            if (columns[0].Trim() == shopUpgradeID)
            {
                shopUpgradeID = columns[0].Trim();
                upgradeName = columns[1].Trim();
                description = columns[2].Trim();
                statType = columns[3].Trim();
                value = float.Parse(columns[4].Trim());
                cost = int.Parse(columns[5].Trim());
                isOneTime = bool.Parse(columns[6].Trim());

                assigned = true;
                Debug.Log(upgradeName + " shop upgrade assigned");
                break;
            }
        }

        if (!assigned)
            Debug.LogWarning("Shop upgrade ID not found: " + shopUpgradeID);
    }

    public void BuyUpgrade()
    {
        if (PlayerManager.Instance == null)
        {
            Debug.LogError("PlayerManager not found!");
            return;
        }

        if (isOneTime && PlayerManager.Instance.ownedUpgradeIDs.Contains(shopUpgradeID))
        {
            Debug.Log("Already purchased: " + upgradeName);
            return;
        }

        bool success = PlayerManager.Instance.BuyUpgrade(shopUpgradeID, cost);

        if (success)
        {
            isPurchased = true;
            // Keep static list in sync so PlayerStats can read it
            purchasedUpgradeIDs = PlayerManager.Instance.ownedUpgradeIDs;
            Debug.Log("Purchased: " + upgradeName);
        }
        else
        {
            Debug.Log("Not enough currency to buy: " + upgradeName);
        }
    }
}
