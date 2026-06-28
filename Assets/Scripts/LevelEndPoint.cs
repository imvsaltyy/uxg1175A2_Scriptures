using UnityEngine;

public class LevelEndPoint : MonoBehaviour
{
    [Header("Map")]
    public GameObject map;

    [Header("Current Level")]
    public GameObject currentLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (LevelNode.currentNode != null)
        {
            LevelNode.currentNode.ClearNode();
        }

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