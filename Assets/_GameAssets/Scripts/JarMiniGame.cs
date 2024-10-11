using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarMiniGame : MonoBehaviour
{
    public float zPadding = 5f; // Padding on the z-axis
    public float yPadding = 2f; // Padding on the y-axis
    public float specifiedTime = 1f; // Time in seconds to complete the scaling
    private int partsCount = 0;
    public int maxParts = 4;
    public GameObject jar;
    private Camera cam;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public int PartsCount
    {
        get { return partsCount; }
        set
        {
            partsCount = value;
            if (partsCount == maxParts)
            {
                jar.SetActive(true);
                RandomObjectPlacer.Instance.JarsDone++;
                StartCoroutine(LerpScale(Vector3.one, Vector3.zero, specifiedTime));
            }
        }
    }

    private void OnEnable()
    {
        cam = Camera.main;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        StartCoroutine(CameraFollow());
        StartCoroutine(LerpScale(Vector3.zero, Vector3.one, specifiedTime));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator CameraFollow()
    {
        while (true)
        {
            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraUp = cam.transform.up;
            Vector3 targetPosition = cam.transform.position + cameraForward * zPadding + cameraUp * yPadding;
            transform.position = targetPosition;

            Quaternion cameraRotation = cam.transform.rotation;
            Quaternion additionalRotation = Quaternion.Euler(-30f, 0f, 0f); // +30 degrees on the x-axis
            transform.rotation = cameraRotation * additionalRotation;

            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator LerpScale(Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale; // Ensure the final scale is set to endScale
        if (endScale == Vector3.zero)
        {
            Destroy(gameObject);
        }
    }
}
