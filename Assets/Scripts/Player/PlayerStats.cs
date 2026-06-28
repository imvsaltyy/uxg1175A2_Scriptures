using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector] public string playerID;

    #region Base Stats
    [HideInInspector] public float baseHP;
    [HideInInspector] public float baseDamage;
    [HideInInspector] public float baseMoveSpeed;
    [HideInInspector] public float vision;
    [HideInInspector] public float fovAngle;
    [HideInInspector] public float rotationSpeed;
    [HideInInspector] public int inventorySize;
    [HideInInspector] public float baseCritRate;
    [HideInInspector] public float baseCritDamage;
    #endregion

    #region Upgrade Modifiers
    [HideInInspector] public float HPBoost;
    [HideInInspector] public float critRateBoost;
    [HideInInspector] public float damageMultiplier;
    [HideInInspector] public float damageReduction;
    [HideInInspector] public float moveSpeedMultiplier;
    #endregion

    #region Final Stats
    public float FinalHP => baseHP + HPBoost;
    public float FinalCritRate => baseCritRate + critRateBoost;
    public float FinalDamage => baseDamage * (1f + damageMultiplier);
    public float FinalMoveSpeed => baseMoveSpeed * (1f + moveSpeedMultiplier);
    #endregion

    private bool statsLoaded = false;

    private void Start()
    {
        if (!statsLoaded)
        {
            statsAssignment();
            ApplyPurchasedShopUpgrades();
        }
    }

    public void statsAssignment()
    {
        if (statsLoaded) return;

        if (GameManager.playerStats == null || GameManager.playerStats.Length == 0)
        {
            Debug.LogError("Player stats CSV not loaded!");
            return;
        }

        string[] columns = GameManager.playerStats[0].Split(',');

        playerID = columns[0].Trim();
        baseHP = float.Parse(columns[1].Trim());
        baseDamage = float.Parse(columns[2].Trim());
        baseMoveSpeed = float.Parse(columns[3].Trim());
        vision = float.Parse(columns[4].Trim());
        fovAngle = float.Parse(columns[5].Trim());
        rotationSpeed = float.Parse(columns[6].Trim());
        inventorySize = int.Parse(columns[7].Trim());
        baseCritRate = float.Parse(columns[8].Trim());
        baseCritDamage = float.Parse(columns[9].Trim());

        statsLoaded = true;
        Debug.Log(playerID + " base stats assigned.");
    }

    void ApplyPurchasedShopUpgrades()
    {
        // Sync list from PlayerManager
        if (PlayerManager.Instance != null)
            ShopUpgrades.purchasedUpgradeIDs = PlayerManager.Instance.ownedUpgradeIDs;

        for (int i = 0; i < ShopUpgrades.purchasedUpgradeIDs.Count; i++)
        {
            string purchasedID = ShopUpgrades.purchasedUpgradeIDs[i];

            for (int j = 0; j < GameManager.shopUpgrades.Length; j++)
            {
                string[] columns = GameManager.shopUpgrades[j].Split(',');
                if (columns[0].Trim() != purchasedID) continue;

                string statType = columns[3].Trim();
                float value = float.Parse(columns[4].Trim());

                switch (statType)
                {
                    case "DamageMultiplier": damageMultiplier += value; break;
                    case "CritRate": critRateBoost += value; break;
                    case "MaxHP": HPBoost += value; break;
                    case "DamageReduction": damageReduction += value; break;
                    case "MoveSpeedMultiplier": moveSpeedMultiplier += value; break;
                    default: Debug.LogWarning("Unknown stat type: " + statType); break;
                }

                Debug.Log("Applied upgrade: " + purchasedID);
                break;
            }
        }
    }

    // Call this when player takes damage — used by Bullet and Lazer
    public void TakeDamage(float dmg)
    {
        float reduced = dmg * (1f - damageReduction);
        baseHP -= reduced;
        Debug.Log("Player took " + reduced + " damage. HP remaining: " + baseHP);

        if (baseHP <= 0f)
            OnPlayerDeath();
    }

    void OnPlayerDeath()
    {
        Debug.Log("Player has died.");

        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnPlayerDeath();

        // Load level select after death
        SceneLoader sceneLoader = FindFirstObjectByType<SceneLoader>();
        if (sceneLoader != null)
            sceneLoader.GoToLevelSelectScene();
    }
}
