using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Current Level Data")]
    public string levelID;
    public string levelType;

    public float enemySpawnInterval;
    public int totalEnemies;

    public float enemyHPMultiplier;
    public float enemySpeedMultiplier;
    public float enemyDmgMultiplier;

    public int maxLootBox;
    public string lootboxRatesMultiplierID;

    [Header("Level Objects")]
    public GameObject[] levelObjects;

    private GameObject activeLevelObject;

    [Header("Enemy Prefabs")]
    public GameObject enemyWalkerPrefab;
    public GameObject enemyShooterPrefab;

    private List<Transform> enemySpawnPoints = new List<Transform>();

    [Header("Enemy Progress")]
    public int totalEnemiesSpawned;
    public int currentAliveEnemies;
    public int enemiesKilled;

    private Coroutine enemySpawnCoroutine;

    [Header("LootBox Prefab")]
    public GameObject lootBoxPrefab;

    private List<Transform> lootBoxSpawnPoints = new List<Transform>();

    [Header("Endpoint")]
    private GameObject endpointObject;
    private bool levelCleared;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //LoadSelectedLevel();
    }

    public void SetupLevelAfterNodeClick(GameObject newActiveLevel)
    {
        if (newActiveLevel == null)
        {
            Debug.LogWarning("SetupLevelAfterNodeClick failed: newActiveLevel is null.");
            return;
        }

        if (enemySpawnCoroutine != null)
        {
            StopCoroutine(enemySpawnCoroutine);
            enemySpawnCoroutine = null;
        }

        ResetLevelProgress();

        activeLevelObject = newActiveLevel;
        levelID = activeLevelObject.name;

        AssignLevelData(levelID);

        FindEnemySpawnPoints();
        FindLootBoxSpawnPoints();
        FindEndpoint();

        SpawnLootBoxes();

        if (CanSpawnEnemies())
        {
            enemySpawnCoroutine = StartCoroutine(SpawnEnemiesRoutine());
        }
        else
        {
            Debug.Log("This level does not spawn enemies.");
            ActivateEndpoint();
        }

        Debug.Log("Level setup after node click: " + levelID);
    }

    void LoadSelectedLevel()
    {
        ResetLevelProgress();

        if (PlayerManager.Instance == null)
        {
            Debug.LogWarning("PlayerManager not found. Defaulting to NORMAL_01.");
            levelID = "NORMAL_01";
        }
        else
        {
            levelID = PlayerManager.Instance.selectedLevelID;
        }

        if (string.IsNullOrEmpty(levelID))
        {
            levelID = "NORMAL_01";
        }

        AssignLevelData(levelID);
        ActivateLevelObject(levelID);

        FindEnemySpawnPoints();
        FindLootBoxSpawnPoints();
        FindEndpoint();

        SpawnLootBoxes();

        if (CanSpawnEnemies())
        {
            enemySpawnCoroutine = StartCoroutine(SpawnEnemiesRoutine());
        }
        else
        {
            Debug.Log("This level does not spawn enemies.");

            // For chest-only levels, allow endpoint immediately.
            ActivateEndpoint();
        }
    }

    void ResetLevelProgress()
    {
        totalEnemiesSpawned = 0;
        currentAliveEnemies = 0;
        enemiesKilled = 0;
        levelCleared = false;
    }

    void AssignLevelData(string selectedLevelID)
    {
        if (GameManager.levelSelections == null || GameManager.levelSelections.Length == 0)
        {
            Debug.LogError("Level data CSV not loaded!");
            return;
        }

        bool assigned = false;

        for (int i = 0; i < GameManager.levelSelections.Length; i++)
        {
            string[] columns = GameManager.levelSelections[i].Split(',');

            if (columns.Length < 9)
            {
                Debug.LogWarning("Invalid level data row: " + GameManager.levelSelections[i]);
                continue;
            }

            if (columns[0].Trim() == selectedLevelID)
            {
                levelID = columns[0].Trim();
                levelType = columns[1].Trim();

                enemySpawnInterval = float.Parse(columns[2].Trim());

                totalEnemies = Mathf.RoundToInt(float.Parse(columns[3].Trim()));

                enemyHPMultiplier = float.Parse(columns[4].Trim());
                enemySpeedMultiplier = float.Parse(columns[5].Trim());
                enemyDmgMultiplier = float.Parse(columns[6].Trim());

                maxLootBox = Mathf.RoundToInt(float.Parse(columns[7].Trim()));
                lootboxRatesMultiplierID = columns[8].Trim();

                assigned = true;

                Debug.Log("===== LEVEL DATA ASSIGNED =====");
                Debug.Log("Level ID: " + levelID);
                Debug.Log("Level Type: " + levelType);
                Debug.Log("Enemy Spawn Interval: " + enemySpawnInterval);
                Debug.Log("Total Enemies To Spawn: " + totalEnemies);
                Debug.Log("Enemy HP Multiplier: " + enemyHPMultiplier);
                Debug.Log("Enemy Speed Multiplier: " + enemySpeedMultiplier);
                Debug.Log("Enemy Damage Multiplier: " + enemyDmgMultiplier);
                Debug.Log("Max LootBox: " + maxLootBox);
                Debug.Log("Lootbox Rate Type: " + lootboxRatesMultiplierID);

                break;
            }
        }

        if (!assigned)
        {
            Debug.LogError("Level ID not found in CSV: " + selectedLevelID);
        }
    }

    void ActivateLevelObject(string selectedLevelID)
    {
        activeLevelObject = null;

        for (int i = 0; i < levelObjects.Length; i++)
        {
            levelObjects[i].SetActive(false);
        }

        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (levelObjects[i].name == selectedLevelID)
            {
                activeLevelObject = levelObjects[i];
                break;
            }
        }

        if (activeLevelObject != null)
        {
            activeLevelObject.SetActive(true);
            Debug.Log("Activated level object: " + activeLevelObject.name);
        }
        else
        {
            Debug.LogError("No level object found with name: " + selectedLevelID);
        }
    }

    void FindEnemySpawnPoints()
    {
        enemySpawnPoints.Clear();

        if (activeLevelObject == null)
        {
            Debug.LogWarning("No active level object found.");
            return;
        }

        Transform spawnParent = activeLevelObject.transform.Find("EnemySpawnPoints");

        if (spawnParent == null)
        {
            Debug.LogWarning("No EnemySpawnPoints object found in " + activeLevelObject.name);
            return;
        }

        for (int i = 0; i < spawnParent.childCount; i++)
        {
            enemySpawnPoints.Add(spawnParent.GetChild(i));
        }

        Debug.Log("Enemy spawn points found: " + enemySpawnPoints.Count);
    }

    void FindLootBoxSpawnPoints()
    {
        lootBoxSpawnPoints.Clear();

        if (activeLevelObject == null)
        {
            Debug.LogWarning("No active level object found.");
            return;
        }

        Transform spawnParent = activeLevelObject.transform.Find("LootBoxSpawnPoints");

        if (spawnParent == null)
        {
            Debug.LogWarning("No LootBoxSpawnPoints object found in " + activeLevelObject.name);
            return;
        }

        for (int i = 0; i < spawnParent.childCount; i++)
        {
            lootBoxSpawnPoints.Add(spawnParent.GetChild(i));
        }

        Debug.Log("LootBox spawn points found: " + lootBoxSpawnPoints.Count);
    }

    void FindEndpoint()
    {
        endpointObject = null;

        if (activeLevelObject == null)
        {
            return;
        }

        Transform endpointTransform = activeLevelObject.transform.Find("EndPoint");

        if (endpointTransform != null)
        {
            endpointObject = endpointTransform.gameObject;
            endpointObject.SetActive(false);

            Debug.Log("Endpoint found and disabled.");
        }
        else
        {
            Debug.LogWarning("No EndPoint object found in " + activeLevelObject.name);
        }
    }

    void SpawnLootBoxes()
    {
        if (maxLootBox <= 0)
        {
            Debug.Log("No lootboxes for this level.");
            return;
        }

        if (lootBoxPrefab == null)
        {
            Debug.LogWarning("LootBox prefab not assigned in LevelManager.");
            return;
        }

        if (lootBoxSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No lootbox spawn points available.");
            return;
        }

        int amountToSpawn = Mathf.Min(maxLootBox, lootBoxSpawnPoints.Count);

        for (int i = 0; i < amountToSpawn; i++)
        {
            Transform spawnPoint = lootBoxSpawnPoints[i];

            GameObject lootBox = Instantiate(
                lootBoxPrefab,
                spawnPoint.position,
                Quaternion.identity,
                activeLevelObject.transform
            );

            Debug.Log("Spawned lootbox: " + lootBox.name);
            Debug.Log("Lootbox rate type for this level: " + lootboxRatesMultiplierID);
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (totalEnemiesSpawned < totalEnemies)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(enemySpawnInterval);
        }

        Debug.Log("All enemies for this level have been spawned.");
    }

    void SpawnEnemy()
    {
        if (enemySpawnPoints.Count == 0)
        {
            Debug.LogWarning("No enemy spawn points available.");
            return;
        }

        GameObject enemyPrefab = GetRandomEnemyPrefab();

        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is missing.");
            return;
        }

        Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)];

        GameObject spawnedEnemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity,
            activeLevelObject.transform
        );

        totalEnemiesSpawned++;
        currentAliveEnemies++;

        Debug.Log("Spawned enemy: " + spawnedEnemy.name);
        Debug.Log("Total spawned: " + totalEnemiesSpawned + " / " + totalEnemies);
        Debug.Log("Current alive enemies: " + currentAliveEnemies);
    }

    GameObject GetRandomEnemyPrefab()
    {
        int randomEnemy = Random.Range(0, 2);

        if (randomEnemy == 0)
        {
            return enemyWalkerPrefab;
        }
        else
        {
            return enemyShooterPrefab;
        }
    }

    public void EnemyDied()
    {
        enemiesKilled++;
        currentAliveEnemies--;

        if (currentAliveEnemies < 0)
        {
            currentAliveEnemies = 0;
        }

        Debug.Log("Enemy died.");
        Debug.Log("Enemies killed: " + enemiesKilled + " / " + totalEnemies);
        Debug.Log("Current alive enemies: " + currentAliveEnemies);

        CheckLevelClearCondition();
    }

    void CheckLevelClearCondition()
    {
        if (levelCleared)
        {
            return;
        }

        if (totalEnemies <= 0)
        {
            ActivateEndpoint();
            return;
        }

        if (enemiesKilled >= totalEnemies)
        {
            ActivateEndpoint();
        }
    }

    void ActivateEndpoint()
    {
        if (levelCleared)
        {
            return;
        }

        levelCleared = true;

        if (endpointObject != null)
        {
            endpointObject.SetActive(true);
            Debug.Log("Level cleared. Endpoint activated.");
        }
        else
        {
            Debug.LogWarning("Level cleared, but endpoint object is missing.");
        }
    }

    public bool CanSpawnEnemies()
    {
        if (levelType == "chest")
        {
            return false;
        }

        if (totalEnemies <= 0)
        {
            return false;
        }

        if (enemySpawnInterval <= 0)
        {
            return false;
        }

        return true;
    }

    public bool CanSpawnLootBoxes()
    {
        return maxLootBox > 0;
    }

    public GameObject GetActiveLevelObject()
    {
        return activeLevelObject;
    }

    public string GetLootboxRatesMultiplierID()
    {
        return lootboxRatesMultiplierID;
    }

    public void CompleteCurrentLevel()
    {
        if (!levelCleared)
        {
            Debug.Log("Cannot complete level yet. Enemies are not all killed.");
            return;
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.CompleteLevel(levelID);
        }
    }
}