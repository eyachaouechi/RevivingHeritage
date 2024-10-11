using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParts : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Transform objectTransform;
    private Collider objectCollider;
    private Vector3 originalPosition;
    public LayerMask layerMask;
    private bool isOverlapping = false;
    private float lerpSpeed = 2f; // Control the speed of interpolation

    private void Start()
    {
        objectTransform = transform;
        objectCollider = GetComponent<Collider>();
        originalPosition = objectTransform.localPosition; // Store the original position
    }

    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(objectTransform.position);
        offset = objectTransform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        // Adjust the position relative to the parent's rotation
        Vector3 localPosition = transform.parent.InverseTransformPoint(curPosition);
        localPosition.z = originalPosition.z; // Maintain the original z position
        objectTransform.localPosition = localPosition;

        CheckOverlap();
    }

    private void OnMouseUp()
    {
        if (!isOverlapping)
        {
            objectTransform.localPosition = originalPosition; // Return to original position if not overlapping
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = screenPoint.z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void CheckOverlap()
    {
        Collider[] hitColliders = Physics.OverlapBox(objectCollider.bounds.center, objectCollider.bounds.extents, objectTransform.rotation, layerMask);
        isOverlapping = false;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.gameObject.name == gameObject.name)
            {
                Renderer renderer = hitCollider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    if (material.HasProperty("_CutoffHeight"))
                    {
                        GetComponent<Renderer>().enabled = false;
                        GetComponent<MeshCollider>().enabled = false;
                        StartCoroutine(LerpCutoffHeight(material, 1.5f)); // Start the interpolation coroutine
                        isOverlapping = true;
                    }
                }
            }
        }
    }

    private IEnumerator LerpCutoffHeight(Material material, float targetValue)
    {
        float startValue = material.GetFloat("_CutoffHeight");
        float elapsedTime = 0f;

        while (elapsedTime < lerpSpeed)
        {
            elapsedTime += Time.deltaTime * 1;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime);
            material.SetFloat("_CutoffHeight", newValue);
            yield return null;
        }

        material.SetFloat("_CutoffHeight", targetValue); // Ensure it ends at the target value
        transform.GetComponentInParent<JarMiniGame>().PartsCount++;
        Destroy(gameObject);
    }
}
