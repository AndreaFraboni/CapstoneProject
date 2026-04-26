using System;
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
        TowerPlacing,
        EntPlacing
    }

    public GameState CurrentState { get; private set; } = GameState.Playing;

    [SerializeField] private int startingCoins = 100;
    public int currentCoins { get; private set; }

    [SerializeField] private int startingBlueGems = 0;
    public int currentBlueGems { get; private set; }

    [SerializeField] private int _maxTowersInScene = 4;
    public int currentTowersSpawned = 0;

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnBlueGemsChanged;

    public event Action OnTowersNotBuildable;
    public event Action OnTowersBuildable;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentCoins = startingCoins;
        currentBlueGems = startingBlueGems;
    }

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllAudioSource();
            if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemeGame");
        }

        OnCoinsChanged?.Invoke(currentCoins);
        OnBlueGemsChanged?.Invoke(currentBlueGems);
    }

    //***************************** manage towers ...... ************************************************//
    public bool CanAddOtherTower(int amount)
    {
        if (currentTowersSpawned + amount > _maxTowersInScene)
        {
            OnTowersNotBuildable?.Invoke();
            ExitTowerPlacing();
            return false;
        }

        currentTowersSpawned += amount;

        if (currentTowersSpawned >= _maxTowersInScene)
        {
            OnTowersNotBuildable?.Invoke();
            ExitTowerPlacing();
        }

        return true;
    }

    public void RemoveTower()
    {
        currentTowersSpawned--;
        if (currentTowersSpawned < 0) currentTowersSpawned = 0;
        if (currentTowersSpawned < _maxTowersInScene)
        {
            OnTowersBuildable?.Invoke();
        }
    }

    //****************************** manage coins and bluegems ..... ***********************************//
    public void AddBlueGems(int amount)
    {
        if (amount <= 0) return;
        currentBlueGems = currentBlueGems + amount;
        OnBlueGemsChanged?.Invoke(currentBlueGems);
    }

    public bool SpendBlueGems(int amount)
    {
        if (amount <= 0) return false;
        if (currentBlueGems < amount) return false;
        currentBlueGems = currentBlueGems - amount;
        if (currentBlueGems <= 0) currentBlueGems = 0;
        OnBlueGemsChanged?.Invoke(currentBlueGems);
        return true;
    }

    public bool CanSpendBlueGems(int amount)
    {
        if (currentBlueGems > 0 && currentBlueGems >= amount)
            return true;
        else
            return false;
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        currentCoins = currentCoins + amount;
        OnCoinsChanged?.Invoke(currentCoins);
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;
        if (currentCoins < amount) return false;
        currentCoins = currentCoins - amount;
        if (currentCoins <= 0) currentCoins = 0;
        OnCoinsChanged?.Invoke(currentCoins);
        return true;
    }

    public bool CanSpendCoins(int amount)
    {
        if (currentCoins > 0 && currentCoins >= amount)
            return true;
        else
            return false;
    }

    //**************** manage pause&resume game and all phase of game : gameover restart loadmenu quit game .. ********************************//
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
        if (CurrentState == GameState.Playing)
        {
            return true;
        }

        if (CurrentState == GameState.TowerPlacing)
        {
            return true;
        }

        if (CurrentState == GameState.EntPlacing)
        {
            return true;
        }

        return false;
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Paused;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllAudioSource();
            if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemePauseMenu");
        }

        GameUIManager.Instance.ShowPause();
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllAudioSource();
            if (!AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.PlayMusic("ThemeGame");
        }

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
        if (GameUIManager.Instance != null) GameUIManager.Instance.ShowGameOver();
    }

    public void Victory()
    {
        if (CurrentState == GameState.Victory) return;

        CurrentState = GameState.Victory;

        if (GameUIManager.Instance != null) GameUIManager.Instance.ShowVictory();
    }

    public void QuitGame()
    {
        if (GameUIManager.Instance != null) GameUIManager.Instance.QuitGame();
    }

    //************************** manage Tower & Ents Game State Enter and Exit *******************//
    public void EnterTowerPlacing()
    {
        CurrentState = GameState.TowerPlacing;
    }

    public void ExitTowerPlacing()
    {
        CurrentState = GameState.Playing;
    }

    public void EnterEntPlacing()
    {
        CurrentState = GameState.EntPlacing;
    }

    public void ExitEntPlacing()
    {
        CurrentState = GameState.Playing;
    }


}
