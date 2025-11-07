using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
