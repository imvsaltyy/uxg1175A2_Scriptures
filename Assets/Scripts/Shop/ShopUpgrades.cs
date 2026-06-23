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

    // TEMPORARY HARDCODED CURRENCY
    public static int playerCurrency = 100;
    public static List<string> purchasedUpgradeIDs = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        statsAssignment();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.shopUpgrades.Length; i++)
        {
            string[] columns = GameManager.shopUpgrades[i].Split(',');

            if (columns[0] == shopUpgradeID)
            {
                shopUpgradeID = columns[0];
                upgradeName = columns[1];
                description = columns[2];
                statType = columns[3];

                value = float.Parse(columns[4]);
                cost = int.Parse(columns[5]);
                isOneTime = bool.Parse(columns[6]);

                assigned = true;

                Debug.Log(upgradeName + " shop upgrade assigned");
                break;
            }
        }

        if (!assigned)
        {
            Debug.LogWarning("Shop upgrade ID not found: " + shopUpgradeID);
        }
    }

    public void BuyUpgrade()
    {
        if (isOneTime && purchasedUpgradeIDs.Contains(shopUpgradeID))
        {
            Debug.Log("Already purchased: " + upgradeName);
            return;
        }

        if (playerCurrency < cost)
        {
            Debug.Log("Not enough currency. Current currency: " + playerCurrency);
            return;
        }

        playerCurrency -= cost;
        purchasedUpgradeIDs.Add(shopUpgradeID);

        Debug.Log("Purchased: " + upgradeName);
        Debug.Log("Remaining currency: " + playerCurrency);
    }
}
