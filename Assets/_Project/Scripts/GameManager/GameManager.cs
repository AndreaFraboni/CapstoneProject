using UnityEngine;
using UnityEngine.InputSystem;

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

        AudioManager.Instance.StopAllAudioSource();
        if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemePauseMenu");
        GameUIManager.Instance.ShowPause();
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;

        CurrentState = GameState.Playing;

        AudioManager.Instance.StopAllAudioSource();
        if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemeGame");
        GameUIManager.Instance.HidePause();
    }

    public void LoadMainMenu()
    {
        //AudioManager.Instance.StopAllAudioSource();
        GameUIManager.Instance.LoadMainMenu();
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver) return;

        CurrentState = GameState.GameOver;
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
