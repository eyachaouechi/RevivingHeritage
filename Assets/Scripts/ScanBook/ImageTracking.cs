using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
public class ImageTracking : MonoBehaviour
{
    public ARSession arSession;
    public Transform arCameraTransform;

    public JsonReader jsonReader;
    public TextAsset textJSONCarthageAr;
    public TextAsset textJSONCarthageEng;
    public TextAsset textJSONCarthageFr;
    public TextAsset textJSONMatmataAr;
    public TextAsset textJSONMatmataEng;
    public TextAsset textJSONMatmataFr;
    public TextAsset textJSONKSAAr;
    public TextAsset textJSONKSAEng;
    public TextAsset textJSONKSAFr;

    public Dictionary<string, GameObject> _arObjects = new Dictionary<string, GameObject>();
    public ARTrackedImageManager _arTrackedImageManager;


    public XRReferenceImageLibrary referenceLibraryCarthage;
    public XRReferenceImageLibrary referenceLibraryMatmata;
    public XRReferenceImageLibrary referenceLibraryKSA;

    [SerializeField] private GameObject[] prefabsToSpawnCarthage;
    [SerializeField] private GameObject[] prefabsToSpawnMatmata;
    [SerializeField] private GameObject[] prefabsToSpawnKSA;


    // Variables for pinch gesture
    private Vector2 initialTouchPosition = Vector2.zero;
    private Vector2 previousTouchPosition = Vector2.zero;
    private float initialScale = 0f;


    private void Awake()
    {
        //_arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        //jsonReader = GetComponent<JsonReader>();
    }

    private void Start()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        jsonReader = GetComponent<JsonReader>();

        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);
        int localizationID = PlayerPrefs.GetInt("Localekey", 0);
        string localization = GetLocalizationString(localizationID);
        XRReferenceImageLibrary selectedLibrary = null;
        TextAsset selectedJSON = null;


        switch (selectedLevel)
        {
            case 0:
                selectedLibrary = referenceLibraryCarthage;
                _arTrackedImageManager.referenceLibrary = referenceLibraryCarthage;
                selectedJSON = JsonFormat(selectedLevel, localization);
                break;
            case 1:
                selectedLibrary = referenceLibraryMatmata;
                _arTrackedImageManager.referenceLibrary = referenceLibraryMatmata;
                selectedJSON = JsonFormat(selectedLevel, localization);
                break;
            case 2:
                selectedLibrary = referenceLibraryKSA;
                _arTrackedImageManager.referenceLibrary = referenceLibraryKSA;
                selectedJSON = JsonFormat(selectedLevel, localization);
                break;
            default:
                selectedLibrary = referenceLibraryCarthage;
                _arTrackedImageManager.referenceLibrary = referenceLibraryCarthage;
                selectedJSON = JsonFormat(selectedLevel, localization);
                break;
        }



        _arTrackedImageManager.referenceLibrary = selectedLibrary;

        // Ensure JSON is loaded
        if (selectedJSON != null)
        {
            jsonReader.LoadJson(selectedJSON);
            Debug.Log("json loaded");
        }
        else
        {
            Debug.LogError("Selected JSON file is null.");
        }
        InstantiatePrefabsForLevel(selectedLevel);


        _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private TextAsset JsonFormat(int level, string localizationID)
    {
        if (level == 0)
        {
            if (localizationID == "AR")
            {
                return textJSONCarthageAr;
            }
            if (localizationID == "ENG")
            {
                return textJSONCarthageEng;
            }
            else
            {
                return textJSONCarthageFr;
            }
        }
        if (level == 1)
        {
            if (localizationID == "AR")
            {
                return textJSONMatmataAr;
            }
            if (localizationID == "ENG")
            {
                return textJSONMatmataEng;
            }
            else
            {
                return textJSONMatmataFr;
            }
        }
        else
        {
            if (localizationID == "AR")
            {
                return textJSONKSAAr;
            }
            if (localizationID == "ENG")
            {
                return textJSONKSAEng;
            }
            else
            {
                return textJSONKSAFr;
            }
        }
    }

    private void InstantiatePrefabsForLevel(int selectedLevel)
    {
        switch (selectedLevel)
        {
            case 0:
                InstantiatePrefabs(prefabsToSpawnCarthage);
                break;
            case 1:
                InstantiatePrefabs(prefabsToSpawnMatmata);
                break;
            case 2:
                InstantiatePrefabs(prefabsToSpawnKSA);
                break;
            default:
                InstantiatePrefabs(prefabsToSpawnCarthage);
                break;
        }
    }


    private void InstantiatePrefabs(GameObject[] prefabsToSpawn)
    {
        if (prefabsToSpawn == null)
        {
            return;
        }
        int infoCount = jsonReader.myInfoList.infos.Length;

        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            try
            {
                if (i >= infoCount)
                {
                    continue;
                }

                GameObject prefab = prefabsToSpawn[i];
                if (prefab == null)
                {
                    continue;
                }

                GameObject newARObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                if (newARObject == null)
                {
                    continue;
                }

                newARObject.name = prefab.name;
                newARObject.SetActive(false);
                InitializeARObject(newARObject, newARObject.name, i);
                LookAtCamera(newARObject, arCameraTransform);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error instantiating prefab at index {i}: {e.Message}");
            }
        }
    }
    private void LookAtCamera(GameObject obj, Transform cameraTransform)
    {
        // Calculate the direction from the object to the camera
        Vector3 directionToCamera = cameraTransform.position - obj.transform.position;
        directionToCamera.y = 0; // Keep the rotation only in the horizontal plane

        // Calculate the rotation needed to look at the camera
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);

        // Adjust the rotation to match the desired facing direction (e.g., X or Y axis)
        obj.transform.rotation = lookRotation;

       
        obj.transform.Rotate(0, 180, 0); 
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
                return "ENG"; // Default to English if unknown
        }
    }

        void OnEnable()
        {
       // _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        void OnDisable()
        {
        _arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }


        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (eventArgs != null)
        {
            foreach (var trackedImage in eventArgs.added)
            {

                UpdateTrackedImage(trackedImage);
            }

            foreach (var trackedImage in eventArgs.updated)
            {
                UpdateTrackedImage(trackedImage);
            }

            foreach (var trackedImage in eventArgs.removed)
            {
                if (trackedImage.referenceImage != null && _arObjects.ContainsKey(trackedImage.referenceImage.name))
                {
                    _arObjects[trackedImage.referenceImage.name].SetActive(false);
                }
            }
        }
        
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            if (_arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            }
            return;
        }

        if (_arObjects.ContainsKey(trackedImage.referenceImage.name))
        {
            _arObjects[trackedImage.referenceImage.name].SetActive(true);
            _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        }
        else
        {
            int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);
            GameObject[] prefabsToSpawn = null;

            switch (selectedLevel)
            {
                case 0:
                    prefabsToSpawn = prefabsToSpawnCarthage;
                    break;
                case 1:
                    prefabsToSpawn = prefabsToSpawnMatmata;
                    break;
                case 2:
                    prefabsToSpawn = prefabsToSpawnKSA;
                    break;
            }

            if (prefabsToSpawn != null)
            {
                try
                {
                    GameObject newARObject = Instantiate(prefabsToSpawn[0], trackedImage.transform.position, Quaternion.identity);
                    newARObject.name = trackedImage.referenceImage.name;
                    _arObjects.Add(trackedImage.referenceImage.name, newARObject);
                    newARObject.SetActive(true); 
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error instantiating prefab for tracked image {trackedImage.referenceImage.name}: {e.Message}");
                }
            }
        }

        HandlePinchGesture(trackedImage);
    }

    private void InitializeARObject(GameObject newARObject, string imageName, int i)
    {

        try
        {

            Transform canvasTransform = newARObject.transform.Find("CanvasInfoQuiz");
            if (canvasTransform == null)
            {
                return;
            }

            Transform descriptionTransform = canvasTransform.Find("GameObjectDescription");
            if (descriptionTransform == null)
            {
                return;
            }

            TextMeshProUGUI textMeshPro = descriptionTransform.Find("TextDescription").GetComponent<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                return;
            }

            textMeshPro.text = jsonReader.myInfoList.infos[i].content;
            _arObjects.Add(newARObject.name, newARObject);

        }
        catch (Exception e)
        {
            Debug.LogError($"Error initializing AR object {newARObject.name}: {e.Message}");
        }
    }

    private TextMeshProUGUI FindTextMeshProByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                TextMeshProUGUI textMesh = child.GetComponent<TextMeshProUGUI>();
                if (textMesh != null)
                {
                    return textMesh;
                }
            }
            TextMeshProUGUI result = FindTextMeshProByName(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private void HandlePinchGesture(ARTrackedImage trackedImage)
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

                _arObjects[trackedImage.referenceImage.name].transform.localScale *= deltaScale;

                initialScale = currentScale;
                previousTouchPosition = currentTouchPosition;
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

                    _arObjects[trackedImage.referenceImage.name].transform.Rotate(Vector3.up, rotationAmount, Space.World);
                }
                else
                {
                    float rotationSpeed = 1f;
                    float rotationAmountX = touchDeltaPosition.y * rotationSpeed;
                    float rotationAmountZ = -touchDeltaPosition.x * rotationSpeed;

                    _arObjects[trackedImage.referenceImage.name].transform.Rotate(Vector3.right, rotationAmountX, Space.World);
                    _arObjects[trackedImage.referenceImage.name].transform.Rotate(Vector3.forward, rotationAmountZ, Space.World);
                }
            }
        }
    }
}
