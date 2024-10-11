using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefMovement : MonoBehaviour
{
    private Vector2 initialTouchPosition = Vector2.zero;
    private float initialScale = 0f;


    public AudioClip audioArabic;
    public AudioClip audioEnglish;
    public AudioClip audioFrench;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // Get the language localization setting
        int localizationID = PlayerPrefs.GetInt("Localekey", 0);
        string localization = GetLocalizationString(localizationID);

        // Play the appropriate audio based on localization
        PlayAudio(localization);
    }

    void Update()
    {
        HandlePinchGesture();
    }

 

    private string GetLocalizationString(int localizationID)
    {
        switch (localizationID)
        {
            case 0:
                return "AR"; // Arabic
            case 1:
                return "ENG"; // English
            case 2:
                return "FR"; // French
            default:
                return "ENG"; // Default to English
        }
    }

    private void PlayAudio(string language)
    {
        switch (language)
        {
            case "AR":
                if (audioArabic != null)
                {
                    audioSource.clip = audioArabic;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning("Arabic audio not available for this prefab.");
                }
                break;
            case "ENG":
                if (audioEnglish != null)
                {
                    audioSource.clip = audioEnglish;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning("English audio not available for this prefab.");
                }
                break;
            case "FR":
                if (audioFrench != null)
                {
                    audioSource.clip = audioFrench;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning("French audio not available for this prefab.");
                }
                break;
            default:
                Debug.LogWarning("Unknown language specified.");
                break;
        }
    }



    private void HandlePinchGesture()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                initialTouchPosition = (touch0.position + touch1.position) / 2f;
                initialScale = Vector2.Distance(touch0.position, touch1.position);
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPosition = (touch0.position + touch1.position) / 2f;
                float currentScale = Vector2.Distance(touch0.position, touch1.position);

                float deltaScale = currentScale / initialScale;

                transform.localScale *= deltaScale;

                initialScale = currentScale;
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;

                if (Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y))
                {
                    float rotationSpeed = 1f;
                    float rotationAmount = touchDeltaPosition.x * rotationSpeed;

                    transform.Rotate(Vector3.up, rotationAmount, Space.World);
                }
                else
                {
                    float rotationSpeed = 1f;
                    float rotationAmountX = touchDeltaPosition.y * rotationSpeed;
                    float rotationAmountZ = -touchDeltaPosition.x * rotationSpeed;

                    transform.Rotate(Vector3.right, rotationAmountX, Space.World);
                    transform.Rotate(Vector3.forward, rotationAmountZ, Space.World);
                }
            }
        }
    }
}