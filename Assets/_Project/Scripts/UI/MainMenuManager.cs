using Unity.VisualScripting;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public AudioManager AudioManager;
    public IOManager IOManager;
    public ScreenFader Screenfader;

    [SerializeField] private string _LevelSceneAssetName;
    public float _splashDuration = 1f;

    private void Awake()
    {
        if (AudioManager.Instance == null)
        {
            AudioManager = Resources.Load<AudioManager>("AudioManager");
            Instantiate(AudioManager);
        }

        if (IOManager.Instance == null)
        {
            IOManager = Resources.Load<IOManager>("IOManager");
            Instantiate(IOManager);
        }

    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Master");
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Music");
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "SFX");
    }

    private void Start()
    {
        LoadAudioSettings();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllAudioSource();
            AudioManager.Instance.PlayMusic("ThemeMenu");
        }
    }

    private void LoadAudioSettings()
    {
        float masterVolume = 1f;
        float musicVolume = 1f;
        float sfxVolume = 1f;

        bool result = IOManager.Instance.LoadAudioSettings(ref masterVolume, ref musicVolume, ref sfxVolume);

        if (result)
        {
            AudioManager.Instance.SetMasterVolume(masterVolume);
            AudioManager.Instance.SetMusicVolume(musicVolume);
            AudioManager.Instance.SetSFXVolume(sfxVolume);
        }
        else
        {
            Debug.Log("ERROR AUDIO SETTINGS NOT LOADED !!!");
        }
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("MouseClickSound");
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (_LevelSceneAssetName != null)
        {
            ScreenManager.Instance.LoadNextScene(_LevelSceneAssetName);
        }
        else
        {
            Debug.LogError("Next Scene Asset is not assigned !!!!!");
        }
    }

    public void QuitGame()
    {
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

    }
}

