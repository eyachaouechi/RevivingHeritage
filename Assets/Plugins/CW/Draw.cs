
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PaintIn3D;
using UnityEngine.SceneManagement;
public class Draw : MonoBehaviour
    {
        [SerializeField] Slider slider;
        public Color col = Color.black;
    public P3dPaintSphere lifeChanger;



    public void ChangeSize()
    {


        // lifeChanger.MultiplyScale(slider.value+0.4f);
        lifeChanger.Radius = slider.value;


    }
    public void ChoseColor()
        {
            col = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color;
        lifeChanger.Color = col;
        }

   


    }//class end
