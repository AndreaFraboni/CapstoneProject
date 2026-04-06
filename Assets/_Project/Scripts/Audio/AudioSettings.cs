using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AudioSetData
{
    public float masterVolValue;
    public float musicVolValue;
    public float sfxVolValue;
}

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void OnEnable()
    {
        LoadAudioSettings();
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

    public void LoadAudioSettings()
    {
        float masterVolume = 1f;
        float musicVolume = 1f;
        float sfxVolume = 1f;

        bool result = IOManager.Instance.LoadAudioSettings(ref masterVolume, ref musicVolume, ref sfxVolume);

        if (result)
        {
            SetMasterVolume(masterVolume);
            SetMusicVolume(musicVolume);
            SetSFXVolume(sfxVolume);

            AudioManager.Instance.SetSliderValue(_masterSlider, "Master");
            AudioManager.Instance.SetSliderValue(_musicSlider, "Music");
            AudioManager.Instance.SetSliderValue(_sfxSlider, "SFX");
        }
        else
        {
            Debug.Log("ERROR AUDIO SETTINGS NOT LOADED then I create new file now with current value !!!");
            SaveAudioSettings();
            AudioManager.Instance.SetSliderValue(_masterSlider, "Master");
            AudioManager.Instance.SetSliderValue(_musicSlider, "Music");
            AudioManager.Instance.SetSliderValue(_sfxSlider, "SFX");
        }
    }

    public void SaveAudioSettings()
    {
        float masterVolValue = _masterSlider.value;
        float musicVolValue = _musicSlider.value;
        float sfxVolValue = _sfxSlider.value;

        bool result = IOManager.Instance.SaveAudioSettings(masterVolValue, musicVolValue, sfxVolValue);
        if (result)
        {
            Debug.Log("AUDIO SETTINGS SAVED !!!");
        }
        else
        {
            Debug.Log("ERROR AUDIO SETTINGS NOT SAVED !!!");
        }
    }
}

