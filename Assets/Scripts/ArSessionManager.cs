using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class ArSessionManager : MonoBehaviour
{
    private ARSession arSession;
    

    void Start()
    {
        arSession = GetComponent<ARSession>();     
    }

    // Call this method to reset the AR session
    public void ResetARSession()
    {
        if (arSession != null)
        {
            // Reset the AR session
            arSession.Reset();

        }
    }

    // Optional: Call this method to completely restart the AR session
    public void RestartARSession()
    {
        if (arSession != null)
        {
            StartCoroutine(RestartCoroutine());
        }
    }

    private IEnumerator RestartCoroutine()
    {
        arSession.enabled = false;

        yield return null;
        yield return null;

        arSession.enabled = true;
        Debug.Log("AR Session has been restarted.");
    }
}
