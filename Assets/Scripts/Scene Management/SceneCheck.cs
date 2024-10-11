using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCheck : MonoBehaviour
{
    public static SceneCheck instance;
    public int sceneID;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

   

    private void OnLevelWasLoaded(int level)
    {

        if (level == 0)
        {
            var levelsPanelParent = FindObjectOfType<Canvas>().GetComponent<PanelManager>();
            levelsPanelParent.ActivatePanel(7);

        }
    }
}
