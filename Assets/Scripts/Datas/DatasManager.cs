using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class DatasManager : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    void Awake()
    {
    }


    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            float volume = PlayerPrefs.GetFloat("MasterVol");
            mainAudioMixer.SetFloat("MasterVol", volume);
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            float volume = PlayerPrefs.GetFloat("MusicVol");
            mainAudioMixer.SetFloat("MusicVol", volume);
        }

        if (PlayerPrefs.HasKey("SfxVol"))
        {
            float volume = PlayerPrefs.GetFloat("SfxVol");
            mainAudioMixer.SetFloat("SfxVol", volume);
        }
    }



    // index 1 carthage, index 2 matmata, index 3 ksa
    public void OnLevelSelected(int levelIndex)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        PlayerPrefs.Save();

    }

    public void OnARSceneButtonClicked()
    {
        // Load the AR scene
        SceneManager.LoadScene("ImageTarget");
    }

   
}
