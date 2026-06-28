using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainGameScene()
    {
        //SceneManager.LoadScene("Main");
        SceneManager.LoadScene("caijie testing");
    }

    public void GoToLevelSelectScene()
    {
        SceneManager.LoadScene("LevelDesign");
    }

    public void GoToShopScene()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoToStartScene()
    {
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
