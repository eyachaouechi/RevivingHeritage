using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

public class ARTemplateMenuManager : MonoBehaviour
{
    public ARSession arSession;

    [SerializeField]
    private Button m_CreateButton;

    public GameObject contentCarthage;
    public GameObject contentMatmata;
    public GameObject contentKSA;
    private int selectedLevel;

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

    [SerializeField] private GameObject[] prefabsToSpawnCarthage;
    [SerializeField] private GameObject[] prefabsToSpawnMatmata;
    [SerializeField] private GameObject[] prefabsToSpawnKSA;

    [SerializeField]
    private Button m_DeleteButton;

    [SerializeField]
    private GameObject m_ObjectMenu;

    [SerializeField]
    private Button m_CancelButton;

    [SerializeField]
    private GameObject createBtn;

    public ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARPlaneManager arPlaneManager;

    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    public Transform arCameraTransform;

    private void Start()
    {
        arCameraTransform = Camera.main.transform;

        arPlaneManager = GetComponent<ARPlaneManager>();

        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0); // Default to Carthage if not set
        int localizationID = PlayerPrefs.GetInt("Localekey", 0);

        // Debugging the selected level and localization
        Debug.Log($"Selected Level: {selectedLevel}, Localization ID: {localizationID}");

        // Set content visibility based on selected level
        contentCarthage.SetActive(selectedLevel == 0);
        contentMatmata.SetActive(selectedLevel == 1);
        contentKSA.SetActive(selectedLevel == 2);

        arRaycastManager = GetComponent<ARRaycastManager>();
        if (arRaycastManager == null)
        {
            arRaycastManager = FindObjectOfType<ARRaycastManager>();
        }

        jsonReader = GetComponent<JsonReader>();

        // Load the appropriate JSON file based on the selected level and localization
        LoadJsonForLevelAndLanguage(selectedLevel, localizationID);
        m_ObjectMenu.SetActive(false);
        if (m_DeleteButton != null)
        {
            m_DeleteButton.onClick.AddListener(DeleteAllSpawnedPrefabs);
        }
    }

    private TextAsset JsonFormat(int level, int localizationID)
    {
        if (level == 0)
        {
            if (localizationID == 0)
            {
                return textJSONCarthageAr;
            }
            if (localizationID == 1)
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
            if (localizationID == 0)
            {
                return textJSONMatmataAr;
            }
            if (localizationID == 1)
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
            if (localizationID == 0)
            {
                return textJSONKSAAr;
            }
            if (localizationID == 1)
            {
                return textJSONKSAEng;
            }
            else
            {
                return textJSONKSAFr;
            }
        }
    }

    private void LoadJsonForLevelAndLanguage(int level, int localizationID)
    {
        TextAsset jsonFile = null;
        jsonFile = JsonFormat(level, localizationID);

        if (jsonFile != null)
        {
            jsonReader.LoadJson(jsonFile);
        }
        else
        {
            Debug.LogError("JSON file not found for the selected level and language.");
        }
    }

    public void SetObjectToSpawn(int prefabIndex)
    {
        DeleteAllSpawnedPrefabs();

        if (arRaycastManager == null)
        {
            return;
        }

        if (jsonReader == null)
        {
            return;
        }

        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            GameObject[] prefabsToSpawn = GetPrefabsForLevel(selectedLevel);
            if (prefabsToSpawn == null || prefabIndex >= prefabsToSpawn.Length)
            {
                return;
            }

            GameObject objectPrefab = prefabsToSpawn[prefabIndex];
            GameObject newARObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
            LookAtCamera(newARObject, arCameraTransform);

            // Add the new object to the list of spawned prefabs
            spawnedPrefabs.Add(newARObject);

            // Retrieve content for the description using the prefab ID
            string content = jsonReader.GetContent(prefabIndex);

            // Initialize the AR object with the description
            PrefInfosManager.Initialize(newARObject, content);

            Debug.Log($"Spawned and initialized object: {newARObject.name} with content: {content}");
        }
        else
        {
            Debug.LogWarning("No plane detected at the screen center.");
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

        // For example, if the front face is aligned with the object's negative Z axis:
        obj.transform.Rotate(0, 0, 0); // Adjust the rotation to face the correct direction
    }

    private GameObject[] GetPrefabsForLevel(int level)
    {
        switch (level)
        {
            case 0:
                return prefabsToSpawnCarthage;

            case 1:
                return prefabsToSpawnMatmata;

            case 2:
                return prefabsToSpawnKSA;

            default:
                return null;
        }
    }

    public void DeleteAllSpawnedPrefabs()
    {
        foreach (GameObject spawnedPrefab in spawnedPrefabs)
        {
            if (spawnedPrefab != null)
            {
                Destroy(spawnedPrefab);
            }
        }

        // Clear the list after deletion
        spawnedPrefabs.Clear();
    }

    public void create()
    {
        arPlaneManager.enabled = false;
    }
}