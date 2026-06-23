using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [HideInInspector] public string playerID;

    #region Player Stats
    [HideInInspector] public float baseHP;
    [HideInInspector] public float baseDamage;
    [HideInInspector] public float baseMoveSpeed;
    [HideInInspector] public float vision;
    [HideInInspector] public float fovAngle;
    [HideInInspector] public float rotationSpeed;

    [HideInInspector] public int inventorySize;

    [HideInInspector] public float baseCritRate;
    [HideInInspector] public float baseCritDamage;

    [HideInInspector] public float HPBoost;
    [HideInInspector] public float critRateBoost;
    [HideInInspector] public float damageMultiplier;
    [HideInInspector] public float damageReduction;
    [HideInInspector] public float moveSpeedMultiplier;
    #endregion

    public float FinalHP
    {
        get { return baseHP + HPBoost; }
    }

    public float FinalCritRate
    {
        get { return baseCritRate + critRateBoost; }
    }

    public float FinalDamage
    {
        get { return baseDamage * (1 + damageMultiplier); }
    }

    public float FinalMoveSpeed
    {
        get { return baseMoveSpeed * (1 + moveSpeedMultiplier); }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        statsAssignment();
        Debug.Log("===== BEFORE SHOP UPGRADES =====");
        DebugCurrentStats();
        ApplyPurchasedShopUpgrades();
        Debug.Log("===== AFTER SHOP UPGRADES =====");
        DebugCurrentStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void statsAssignment()
    {
        if (GameManager.playerStats == null || GameManager.playerStats.Length == 0)
        {
            Debug.LogError("Player stats CSV not loaded!");
            return;
        }

        string[] columns = GameManager.playerStats[0].Split(',');

        playerID = columns[0];

        baseHP = float.Parse(columns[1]);
        baseDamage = float.Parse(columns[2]);
        baseMoveSpeed = float.Parse(columns[3]);
        vision = float.Parse(columns[4]);
        fovAngle = float.Parse(columns[5]);
        rotationSpeed = float.Parse(columns[6]);

        inventorySize = int.Parse(columns[7]);

        baseCritRate = float.Parse(columns[8]);
        baseCritDamage = float.Parse(columns[9]);

        Debug.Log(playerID + " stats assigned");
        Debug.Log("HP: " + baseHP);
        Debug.Log("Damage: " + baseDamage);
        Debug.Log("Speed: " + baseMoveSpeed);
        Debug.Log("Vision: " + vision);
        Debug.Log("FOV Angle: " + fovAngle);
        Debug.Log("RotationSpeed: " + rotationSpeed);
        Debug.Log("Inventory Size: " + inventorySize);
        Debug.Log("Crit Rate: " +  baseCritRate);
        Debug.Log("Crit Damage: " + baseCritDamage);
        
    }

    void ApplyPurchasedShopUpgrades()
    {
        for (int i = 0; i < ShopUpgrades.purchasedUpgradeIDs.Count; i++)
        {
            string purchasedID = ShopUpgrades.purchasedUpgradeIDs[i];

            for (int j = 0; j < GameManager.shopUpgrades.Length; j++)
            {
                string[] columns = GameManager.shopUpgrades[j].Split(',');

                if (columns[0] == purchasedID)
                {
                    string statType = columns[3];
                    float value = float.Parse(columns[4]);

                    switch (statType)
                    {
                        case "DamageMultiplier":
                            damageMultiplier += value;
                            break;

                        case "CritRate":
                            critRateBoost += value;
                            break;

                        case "MaxHP":
                            HPBoost += value;
                            break;

                        case "DamageReduction":
                            damageReduction += value;
                            break;

                        case "MoveSpeedMultiplier":
                            moveSpeedMultiplier += value;
                            break;

                        default:
                            Debug.LogWarning("Unknown stat type: " + statType);
                            break;
                    }

                    Debug.Log("Applied upgrade to player: " + purchasedID);
                    break;
                }
            }
        }
    }

    void DebugCurrentStats()
    {
        Debug.Log("Final HP: " + FinalHP);
        Debug.Log("Final Damage: " + FinalDamage);
        Debug.Log("Final Crit Rate: " + FinalCritRate);
        Debug.Log("Damage Reduction: " + damageReduction);
        Debug.Log("Final Move Speed: " + FinalMoveSpeed);
    }
}
