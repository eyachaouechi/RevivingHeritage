using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using PaintIn3D;
using System;

public class CameraMover : MonoBehaviour
{
    // Public variables for positions A and B
    public Transform positionA;

    public Transform positionB;

    public Transform positionC;

    public Transform cam;

    public GameObject inGameUI;

    public GameObject selectUI;

    // Duration of the movement
    public float moveDuration = 2f;

    public List<GameObject> models;
    private int selectedModel =0;
    private int paintedModels = 0;
    // Start is called before the first frame update
    private void Start()
    {
        // Ensure the camera starts at position A
        cam.position = positionA.position;
    }

    public void ZoomIn()
    {
        MoveCamera(positionB,0);
    }

    public void ZoomOut()
    {
        models[selectedModel].GetComponentInChildren<P3dPaintable>().enabled = false;
        selectUI.transform.GetChild(0).gameObject.SetActive(false);
        Transform tr = models[selectedModel].transform;
        tr.GetChild(tr.childCount - 1).gameObject.SetActive(true);
        paintedModels++;
        if (paintedModels >= 2)
        {
            MoveModel(1);
            MoveCamera(positionC, 1);
            selectUI.SetActive(false);
        }
        else
        {
            MoveCamera(positionA, 1);

        }
      

       
    }

    private void MoveCamera(Transform destination,int value)
    {
        cam.DOMove(destination.position, moveDuration);
        if (value==0)//zoomin
        {
            selectUI.SetActive(false);
            // Tween the camera's position from A to B
            cam.DOMove(destination.position, moveDuration).OnComplete(() => { inGameUI.SetActive(true); }) ;
        }
        else//zoomout
        {
            inGameUI.SetActive(false);
            cam.DOMove(destination.position, moveDuration).OnComplete(() => { if (paintedModels < 2) selectUI.SetActive(true); });

        }

    }

    bool checkModel(int index)
    {
        return models[index].GetComponentInChildren<P3dPaintable>().enabled;
    }

    public void MoveModel(int direction)
    {
        foreach (var model in models)
        {
             Vector3 newPos=new Vector3(model.transform.position.x+(2*direction),model.transform.position.y,model.transform.position.z);
            model.transform.DOMove(newPos, 1);
           
        }
        selectedModel = direction > 0 ? 0 : 1;

        selectUI.transform.GetChild(0).gameObject.SetActive(checkModel(selectedModel));
    }
}