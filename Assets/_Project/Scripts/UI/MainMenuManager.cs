using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioManager AudioManager;

    private void Awake()
    {
        AudioManager = Resources.Load<AudioManager>("AudioManager");
        Instantiate(AudioManager);
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Master");
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Music");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

