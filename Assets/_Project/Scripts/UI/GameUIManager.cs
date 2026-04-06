using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    // UI referements
    public GameObject pauseMenu;
    public GameObject gameOverBanner;
    public GameObject gameOverMenu;
    public GameObject winnerBanner;
    public GameObject winnerMenu;

    public bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemeGame");
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("MouseClickSound");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Restart()
    {
        AudioManager.Instance.StopAllAudioSource();
        gameOverBanner.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        AudioManager.Instance.StopAllAudioSource();
        gameOverBanner.SetActive(true);
        Invoke("ShowGameOverMenu", 1f);
    }

    public void ShowGameOverMenu()
    {
        AudioManager.Instance.PlayMusic("GameOverMusic");
        gameOverBanner.SetActive(false);
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
    }
    public void Winner()
    {
        AudioManager.Instance.StopAllAudioSource();
        winnerBanner.SetActive(true);
        Invoke("ShowWinnerMenu", 1f);
    }

    public void ShowWinnerMenu()
    {
        winnerBanner.SetActive(false);
        AudioManager.Instance.PlayMusic("WinnerMusic");
        Time.timeScale = 0;
        winnerMenu.SetActive(true);
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
