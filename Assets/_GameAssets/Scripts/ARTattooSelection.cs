using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTattooSelection : MonoBehaviour
{
    public Texture[] sprites;
    public Material material;

    public GameObject prefab;

    public void ChangeFilter(int filter)
    {
        material.SetTexture("_MainTex", sprites[filter]);
        prefab.GetComponent<Renderer>().material = material;
    }
}
