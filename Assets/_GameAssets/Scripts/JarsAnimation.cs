using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarsAnimation : MonoBehaviour
{

    public float lerpSpeed = 2f;


    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                if (material.HasProperty("_CutoffHeight"))
                {
                    StartCoroutine(LerpCutoffHeight(child,material, lerpSpeed, 1.5f)); // Start the interpolation coroutine
                    if (child.childCount > 0)
                    {
                        Renderer childRenderer = child.GetChild(0).GetComponent<Renderer>();
                        Material childMaterial = childRenderer.material;
                        StartCoroutine(LerpCutoffHeight(child.GetChild(0), childMaterial, lerpSpeed / 2, 1.5f));
                    }
                }
            }
        }
    }


    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                if (material.HasProperty("_CutoffHeight"))
                {
                    material.SetFloat("_CutoffHeight", -1);
                    if (child.childCount > 0)
                    {
                        Renderer childRenderer = child.GetChild(0).GetComponent<Renderer>();
                        Material childMaterial = childRenderer.material;
                        childMaterial.SetFloat("_CutoffHeight", -1);
                    }
                }
            }
        }
    }
    private IEnumerator LerpCutoffHeight(Transform child, Material material, float lerpTime, float targetValue=1.5f)
    {
        float startValue = material.GetFloat("_CutoffHeight");
        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime * (1/lerpSpeed);
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime);

            material.SetFloat("_CutoffHeight", newValue);
            yield return null;
        }

        material.SetFloat("_CutoffHeight", targetValue); // Ensure it ends at the target value
       
    }
}
