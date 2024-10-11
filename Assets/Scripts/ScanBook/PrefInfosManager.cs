using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrefInfosManager : MonoBehaviour
{
    public static void Initialize(GameObject newARObject, string content)
    {
        try
        {
            Transform canvasTransform = newARObject.transform.Find("CanvasInfoQuiz");
            if (canvasTransform == null)
            {
                Debug.LogError("CanvasInfoQuiz not found in prefab: " + newARObject.name);
                return;
            }

            Transform descriptionTransform = canvasTransform.Find("GameObjectDescription");
            if (descriptionTransform == null)
            {
                Debug.LogError("GameObjectDescription not found in CanvasInfoQuiz: " + newARObject.name);
                return;
            }

            TextMeshProUGUI textMeshPro = descriptionTransform.Find("TextDescription").GetComponent<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.LogError("TextDescription (TextMeshProUGUI) not found in GameObjectDescription: " + newARObject.name);
                return;
            }

            textMeshPro.text = content;

            Debug.Log($"Set content for TextMeshPro: {content}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error initializing AR object {newARObject.name}: {e.Message}");
        }
    }
}
