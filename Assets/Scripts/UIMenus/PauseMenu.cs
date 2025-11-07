using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}
