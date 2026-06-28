using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndPoint : MonoBehaviour
{
    [Header("Map")]
    public GameObject map;

    [Header("Current Level")]
    public GameObject currentLevel;

    [Header("Final Level")]
    public string finalLevelID = "SWARM_02";

    [Header("Scene")]
    private string startSceneName = "Shop";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        // Tell LevelManager this level is completed
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.CompleteCurrentLevel();
        }

        // Clear/unlock the map node if using LevelNode system
        if (LevelNode.currentNode != null)
        {
            LevelNode.currentNode.ClearNode();
        }

        string currentLevelID = "";

        if (currentLevel != null)
        {
            currentLevelID = currentLevel.name;
        }
        else if (LevelManager.Instance != null)
        {
            currentLevelID = LevelManager.Instance.levelID;
        }

        // If player completed SWARM_02, go back to start scene
        if (currentLevelID == finalLevelID)
        {
            SceneManager.LoadScene(startSceneName);
            return;
        }

        // Otherwise, return to map
        if (currentLevel != null)
        {
            currentLevel.SetActive(false);
        }

        if (map != null)
        {
            map.SetActive(true);
        }
    }
}