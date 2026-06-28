using UnityEngine;

public class LevelEndPoint : MonoBehaviour
{
    [Header("Scene Loader")]
    public SceneLoader sceneLoader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.CompleteCurrentLevel();
        }

        if (sceneLoader != null)
        {
            sceneLoader.GoToLevelSelectScene();
        }
        else
        {
            Debug.LogWarning("SceneLoader not assigned to endpoint.");
        }
    }
}