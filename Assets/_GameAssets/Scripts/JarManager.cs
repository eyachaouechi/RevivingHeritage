using System.Collections.Generic;
using UnityEngine;

public class JarManager : MonoBehaviour
{
    public List<GameObject> jarParts;
    public GameObject miniGame;



    private void OnMouseDown()
    {
        if(miniGame != null)
        {
            miniGame.SetActive(true);

            RandomObjectPlacer.Instance.ColliderActive(false);
            foreach (GameObject jar in RandomObjectPlacer.Instance.spawnedObjects)
            {
                if (jar != transform.parent.gameObject)
                {
                    jar.SetActive(false);
                }
            }

            gameObject.SetActive(false);
        }
       
    }

    private void OnEnable()
    {
        foreach (GameObject jar in RandomObjectPlacer.Instance.spawnedObjects)
        {
            jar.SetActive(true);

        }
        RandomObjectPlacer.Instance.ColliderActive(true);
        JarMiniGame mg = miniGame.GetComponent<JarMiniGame>();
        if (mg.PartsCount == mg.maxParts)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            foreach (GameObject parts in jarParts)
            {
                parts.gameObject.SetActive(false);
               
            }
        }
    }
}
