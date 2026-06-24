using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //Static String[] to for objects to pull data directly from file
    [HideInInspector]
    public static string[] enemyType;
    [HideInInspector]
    public static string[] enemyVariant;
    [HideInInspector]
    public static string[] weaponTypes;
    [HideInInspector]
    public static string[] playerStats;
    [HideInInspector]
    public static string[] shopUpgrades;

    [HideInInspector] public static string[] enemyDrop;
    [HideInInspector] public static string[] lootBoxDrop;

    public static Canvas gameUI;

    //Game Management Area
    private GameObject[] activeEnemies;

    void Awake()
    {
        //Load data from the CSV Files in Resources and assign into String[]
        enemyType = LoadExcelData("EnemyStatsTrial");
        enemyVariant = LoadExcelData("EnemyVariantTrial");
        weaponTypes = LoadExcelData("WeaponStatsTrial");
        playerStats = LoadExcelData("PlayerStatsTrial");
        shopUpgrades = LoadExcelData("ShopUpgradesTrial");

        enemyDrop = LoadExcelData("EnemyLootDropTrial");
        lootBoxDrop = LoadExcelData("LootBoxDropTrial");
    }

    void Start()
    {
        //For Debugging

        //for (int i = 0; i < weaponTypes.Length; i++)
        //{
        //    Debug.Log(weaponTypes[i]);
        //}

        //for (int i = 0; i < activeEnemies.Length; i++)
        //{
        //    Debug.Log("Enemy Found: " + activeEnemies[i].GetComponent<spawnEnemy>().id);
        //}

        //InventoryManager.Instance.FindAll();

    }

    //private void OnApplicationQuit()
    //{
    //    activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    for (int i = 0; i < activeEnemies.Length; i++)
    //    {
    //        activeEnemies[i].gameObject.GetComponent<spawnEnemy>();
    //    }

    //}

    //Getting Data from CSV File
    public string[] LoadExcelData(string fileName)
    {
        //bool assigned = false;
        List<string> list = new List<string>();

        //Load CSV file from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        if (csvFile == null)
        {
            //Debug.LogError("CSV file not found.");
            return null;
        }

        string[] lines = csvFile.text.Split("\n");

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            //Split line by commas
            //string[] columns = lines[i].Split(',');

            list.Add(lines[i]);

        }

        return list.ToArray();

    }
}
