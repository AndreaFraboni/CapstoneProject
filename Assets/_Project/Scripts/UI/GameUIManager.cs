using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField] private string _mainMenuSceneName;

    // UI referements
    public GameObject pauseMenu;
    public GameObject gameOverBanner;
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

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Master");
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Music");
    }


    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("MouseClickSound");
    }

    public void ShowPause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
    }

    public void HidePause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
        //SceneManager.LoadScene(0);
        if (_mainMenuSceneName != null)
        {
            ScreenManager.Instance.LoadNextScene(_mainMenuSceneName);
        }
        else
        {
            Debug.LogError("Next Scene Asset is not assigned !!!!!");
        }
    }

    public void ShowGameOver()
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
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
    }

    public void ShowVictory()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
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
