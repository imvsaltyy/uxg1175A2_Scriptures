using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [HideInInspector] public string playerID;

    #region Player Stats
    [HideInInspector] public float HP;
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public float vision;
    [HideInInspector] public float fovAngle;
    [HideInInspector] public float rotationSpeed;

    [HideInInspector] public int inventorySize;

    [HideInInspector] public float critRate;
    [HideInInspector] public float critDamage;
    #endregion

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        statsAssignment();
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

        HP = float.Parse(columns[1]);
        damage = float.Parse(columns[2]);
        speed = float.Parse(columns[3]);
        vision = float.Parse(columns[4]);
        fovAngle = float.Parse(columns[5]);
        rotationSpeed = float.Parse(columns[6]);

        inventorySize = int.Parse(columns[7]);

        critRate = float.Parse(columns[8]);
        critDamage = float.Parse(columns[9]);

        Debug.Log(playerID + " stats assigned");
        Debug.Log("HP: " + HP);
        Debug.Log("Damage: " + damage);
        Debug.Log("Speed: " + speed);
        Debug.Log("Vision: " + vision);
        Debug.Log("FOV Angle: " + fovAngle);
        Debug.Log("RotationSpeed: " + rotationSpeed);
        Debug.Log("Inventory Size: " + inventorySize);
        Debug.Log("Crit Rate: " +  critRate);
        Debug.Log("Crit Damage: " + critDamage);
        
    }
}
