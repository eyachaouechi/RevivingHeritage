using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance = null;


    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and mark it to not be destroyed on load
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Ensure the AudioSource plays music if it's not already playing
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true; // Loop the music
            audioSource.Play();
        }
    }
}
