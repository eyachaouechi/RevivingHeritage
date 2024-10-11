using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateImageTracking : MonoBehaviour
{
    public ImageTracking iT;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Activate),2);
    }

    void Activate()
    {
        iT.enabled = true;
    }
}
