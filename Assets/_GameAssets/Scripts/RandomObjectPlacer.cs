using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class RandomObjectPlacer : MonoBehaviour
{
    public static RandomObjectPlacer Instance;
    public GameObject[] objectsToPlace; // Array to store the objects
    public float minSpacing = 1f; // Minimum spacing between objects
    public float maxSpacing = 2f; // Maximumspacing between objects
    public List<GameObject> spawnedObjects = new List<GameObject>();
    private ARPlaneManager arPlaneManager;
    public Button doneButton;
    public int jarsDone = 0;

    public GameObject[] win;
    private Vector3 firstPlaneCenter=Vector3.zero;

    void Start()
    {
        Instance = this;
        arPlaneManager = GetComponent<ARPlaneManager>();
        // Initialize the button to be inactive at the start
        doneButton.interactable = false;

        // Add the listener for the button click event
  

        foreach (GameObject go in win)
        {

            go.SetActive(false);
        }
    }
    public int JarsDone
    {
        get { return jarsDone; }
        set
        {
            jarsDone = value;
            if (jarsDone == 3)
            {
                foreach(GameObject go in win)
                {
                   
                    go.SetActive(true);

                }

                foreach (GameObject go in spawnedObjects)
                {
                    Destroy(go);
                }
                win[0].transform.position = firstPlaneCenter;
            }
        }
    }

    public void StopScanning()
    {
        arPlaneManager.enabled = false;
        StartTheGame();
    }

    private void Update()
    {
        TrackableCollection<ARPlane> arPlanes = arPlaneManager.trackables;
        if (arPlanes.count > 1)
        {
            doneButton.interactable = true;

        }
    }

    public void ColliderActive(bool value)
    {
        foreach (var plane in arPlaneManager.trackables)
        {

            plane.gameObject.SetActive(value);

        }
    }
    void StartTheGame()
    {
        if (objectsToPlace.Length == 0)
        {
            Debug.LogWarning("No objects to place.");
            return;
        }

        // Collect all plane boundaries
        List<Vector3> planeCenters = new List<Vector3>();
        foreach (var plane in arPlaneManager.trackables)
        {
            planeCenters.Add(plane.center);
            if (firstPlaneCenter == Vector3.zero)
            {
                firstPlaneCenter = plane.center;
            }
            //plane.gameObject.SetActive(false);
            //plane.enabled = false;
        }

        // If no planes are detected
        if (planeCenters.Count == 0)
        {
            Debug.LogWarning("No planes detected.");
            return;
        }

        // Find a suitable position for each object
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < objectsToPlace.Length; i++)
        {
            Vector3 randomPosition;
            bool validPosition = false;

            do
            {
                // Choose a random plane center
                Vector3 planeCenter = planeCenters[Random.Range(0, planeCenters.Count)];

                // Generate a random position around the plane center
                randomPosition = planeCenter + new Vector3(
                    Random.Range(-1f, 1f),
                    0,
                    Random.Range(-1f, 1f)
                );

                validPosition = true;

                // Check if the random position is spaced enough from all other positions
                foreach (Vector3 pos in positions)
                {
                    if (Vector3.Distance(randomPosition, pos) < minSpacing)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            positions.Add(randomPosition);
            GameObject go = Instantiate(objectsToPlace[i], positions[i], Quaternion.identity);
            // Place the objects
            spawnedObjects.Add(go);
        }


    }

}