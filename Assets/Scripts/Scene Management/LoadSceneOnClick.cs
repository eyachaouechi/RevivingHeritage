using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.XR;
using System;

public class LoadSceneOnClick : MonoBehaviour
{
    public int sceneIndex;
    public GameObject imageSaved;

    Canvas canvas;
    public void OnClickScreenCaptureButton()
    {
        StartCoroutine(CaptureScreen());
    }

    public IEnumerator CaptureScreen()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;
        canvas.enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        //storage/emulated/0/DCIM/screenshot_638005695640000000.png
        string fileName = GetAndroidExternalStoragePath() + "/screenshot_" + DateTime.Now.Ticks + ".png";
        Debug.Log("[Ario] " + fileName);
        // Take screenshot
        var tex = ScreenCapture.CaptureScreenshotAsTexture();
        var bytes = tex.EncodeToPNG();
        File.WriteAllBytes(fileName, bytes);
        // ScreenCapture.CaptureScreenshot(fileName);

        // Show UI after we're done
        canvas.enabled = true;
    }
    private string GetAndroidExternalStoragePath()
    {
        if (Application.platform != RuntimePlatform.Android)
            return Application.persistentDataPath;

        var jc = new AndroidJavaClass("android.os.Environment");
        var path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory",
            jc.GetStatic<string>("DIRECTORY_DCIM"))
            .Call<string>("getAbsolutePath");
        return path;
    }
    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    // RequestPermissionAsync methods prior to calling NativeGallery functions
    private async void RequestPermissionAsynchronously(NativeGallery.PermissionType permissionType, NativeGallery.MediaType mediaTypes)
    {
        NativeGallery.Permission permission = await NativeGallery.RequestPermissionAsync(permissionType, mediaTypes);
        Debug.Log("Permission result: " + permission);
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "DCX", "Dcx.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.Log("Permission result: " + permission);

        // To avoid memory leaks
        Destroy(ss);
    }



    public void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSceneInd(int index)
    {
        SceneManager.LoadScene(index);


    }

    public void CheckMenu()
    {
       // SceneCheck.instance.checkSceneMenu = true;

    }

    public void ScreenShot()
    {
        {
            StartCoroutine(CaptureScreen());
            StartCoroutine(SaveScreenshot(2));

        }
    }

    IEnumerator SaveScreenshot(int seconds)
    {
        imageSaved.SetActive(true);
        yield return new WaitForSeconds(seconds);
        imageSaved.SetActive(false);

    }

 

    public void ToggleAudio()
    {
        // Find all AudioSource components in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            float vol = audioSource.volume;

            if (audioSource.isPlaying)
            {
                // Stop the audio if it is playing
                audioSource.volume = 0;

                audioSource.Stop();
            }
            else
            {
                // Play the audio from the beginning if it is not playing
                audioSource.volume = 1;
                audioSource.Play();
            }
        }

        Debug.Log("Toggled all audio sources.");
    }

}
