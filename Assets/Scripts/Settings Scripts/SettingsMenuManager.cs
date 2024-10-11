using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class SettingsMenuManager : MonoBehaviour
{   
    public Slider masterVol , musicVol , sfxVol;
    public AudioMixer mainAudioMixer;


    void Start()
    {
        LoadSettings();
    }

    public void ChangeMasterVolume()
    {
        float volume = masterVol.value;
        mainAudioMixer.SetFloat("MasterVol", volume);
        PlayerPrefs.SetFloat("MasterVol", volume);
    }

    public void ChangeMusicVolume()
    {
        float volume = musicVol.value;
        mainAudioMixer.SetFloat("MusicVol", volume);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void ChangeSfxVolume()
    {
        float volume = sfxVol.value;
        mainAudioMixer.SetFloat("SfxVol", volume);
        PlayerPrefs.SetFloat("SfxVol", volume);
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            float volume = PlayerPrefs.GetFloat("MasterVol");
            masterVol.value = volume;
            mainAudioMixer.SetFloat("MasterVol", volume);
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            float volume = PlayerPrefs.GetFloat("MusicVol");
            musicVol.value = volume;
            mainAudioMixer.SetFloat("MusicVol", volume);
        }

        if (PlayerPrefs.HasKey("SfxVol"))
        {
            float volume = PlayerPrefs.GetFloat("SfxVol");
            sfxVol.value = volume;
            mainAudioMixer.SetFloat("SfxVol", volume);
        }
     
    }
}
