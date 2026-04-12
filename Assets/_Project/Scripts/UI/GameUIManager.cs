using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    // UI referements
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject winnerMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowPause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void HidePause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
    }

    public void ShowVictory()
    {
        Time.timeScale = 0;
        winnerMenu.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		     Application.Quit();
#endif
        }
    }
}
