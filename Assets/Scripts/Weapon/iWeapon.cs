using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class iWeapon : iInventory
{
    [HideInInspector] public string weaponTypeName;

    //Shared Weapon Stats
    [HideInInspector] public string ID;
    [HideInInspector] public float dmgMultiplier;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float damageSpeed;
    [HideInInspector] public float raySize;
    [HideInInspector] public float increaseRate;


    protected virtual void Start()
    {
        statsAssignment();
    }

    void statsAssignment()
    {
        bool assigned = false;

        for (int i = 0; i < GameManager.weaponTypes.Length; i++)
        {
            string[] columns = GameManager.weaponTypes[i].Split(',');

            if (columns[0] == weaponTypeName)
            {
                ID = columns[0];
                dmgMultiplier = float.Parse(columns[1]);
                fireRate = float.Parse(columns[2]);
                damageSpeed = float.Parse(columns[3]);
                raySize = float.Parse(columns[4]);
                increaseRate = float.Parse(columns[5]);

                assigned = true;
                Debug.Log(ID + " stats assigned");
            }
        }

        if (!assigned)
        {
            Debug.Log("Weapon Type is not found!");
            Destroy(gameObject);
        }
    }

    private void Fire()
    {

    }
}
