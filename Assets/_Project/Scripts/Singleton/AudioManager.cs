using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioManager : GenericSingleton<AudioManager>
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, sfxFootStepsSource;

    public void SetSliderValue(Slider slider, string group)
    {
        if (_mixer.GetFloat(group, out float decibel))
        {
            float percentage = Mathf.Pow(10, decibel / 20);
            slider.value = percentage;
        }
    }

    public void SetVolume(float value, string group)
    {
        if (value > 0.01f)
        {
            float volume = Mathf.Log10(value) * 20;
            _mixer.SetFloat(group, volume);
        }
        else
        {
            _mixer.SetFloat(group, -80f);
        }
    }
    public void SetMasterVolume(float value)
    {
        SetVolume(value, "Master");
    }

    public void SetMusicVolume(float value)
    {
        SetVolume(value, "Music");
    }

    public void SetSFXVolume(float value)
    {
        SetVolume(value, "SFX");
    }

    public void PlayMusic(string name)
    {
        foreach (Sound _sound in musicSounds)
        {
            if (_sound.name == name)
            {
                if (musicSource.isPlaying) musicSource.Stop();
                musicSource.clip = _sound.clip;
                musicSource.Play();
                return;
            }
        }

        Debug.LogWarning($"Music Not Found in my list: {name}");
    }

    public void PlaySFX(string name)
    {
        foreach (Sound sound in sfxSounds)
        {
            if (sound.name == name)
            {
                if (sound.clip == null)
                {
                    Debug.LogError($"Clip nulla per SFX: {name}");
                    return;
                }

                if (sfxSource == null)
                {
                    Debug.LogError("SFX Source non assegnato!");
                    return;
                }
                sfxSource.PlayOneShot(sound.clip);
                return;
            }
        }

        Debug.LogError($"SFX sound Not Found in my list: {name}");
    }

    public void PlayFootsteps(string name)
    {
        foreach (Sound sound in sfxSounds)
        {
            if (sound.name == name)
            {
                sfxFootStepsSource.clip = sound.clip;
                if (!sfxFootStepsSource.isPlaying) sfxFootStepsSource.Play();
                return;
            }
        }
    }

    public void PlaySFXAtPoint(string name, Vector3 position)
    {
        foreach (Sound sound in sfxSounds)
        {
            if (sound.name == name)
            {
                if (sound.clip == null)
                {
                    Debug.LogError($"Clip nulla per SFX: {name}");
                    return;
                }
                //AudioSource.PlayClipAtPoint(sound.clip, position);
                GameObject tempGO = new GameObject("TempAudio"); // create the temp object
                tempGO.transform.position = position; // set its position
                AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
                aSource.clip = sound.clip; // define the clip

                aSource.outputAudioMixerGroup = sfxMixerGroup;

                aSource.Play(); // start the sound
                Destroy(tempGO, sound.clip.length); // destroy object after clip duration
                return;
            }
        }
        Debug.LogError($"SFX sound Not Found in my list: {name}");
    }

    // Stop All Audio Source !!!!!
    public void StopAllAudioSource()
    {
        if (musicSource.isPlaying) musicSource.Stop();
        if (sfxSource.isPlaying) sfxSource.Stop();
    }
}
