using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Victory,
        TowerPlacing
    }

    public GameState CurrentState { get; private set; } = GameState.Playing;

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
        AudioManager.Instance.StopAllAudioSource();
        if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemeGame");
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (CurrentState == GameState.Playing)
            PauseGame();
        else if (CurrentState == GameState.Paused)
            ResumeGame();
    }

    public bool IsPlaying()
    {
        return CurrentState == GameState.Playing;
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Paused;

        if (AudioManager.Instance != null) AudioManager.Instance.StopAllAudioSource();
        if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemePauseMenu");
        GameUIManager.Instance.ShowPause();
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;
        if (AudioManager.Instance != null) AudioManager.Instance.StopAllAudioSource();
        if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemeGame");
        StartCoroutine(ResumeInNextFrame());
    }

    private IEnumerator ResumeInNextFrame()
    {
        yield return null;
        GameUIManager.Instance.HidePause();
        CurrentState = GameState.Playing;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        GameUIManager.Instance.LoadMainMenu();
    }

    public void SetGameOverState()
    {
        if (CurrentState == GameState.GameOver) return;
        CurrentState = GameState.GameOver;
    }

    public void GameOver()
    {
        GameUIManager.Instance.ShowGameOver();
    }

    public void Victory()
    {
        if (CurrentState == GameState.Victory) return;

        CurrentState = GameState.Victory;
        GameUIManager.Instance.ShowVictory();
    }

    public void QuitGame()
    {
        GameUIManager.Instance.QuitGame();
    }

}
