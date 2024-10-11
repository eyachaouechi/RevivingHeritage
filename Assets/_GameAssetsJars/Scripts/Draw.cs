
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CW.Common;
public class Draw : MonoBehaviour
    {
        [SerializeField] Slider slider;
        public Color col = Color.black;
        


        //public void ChangeSize()
        //{
        //    P3dPaintSphere.MultiplyScale = slider.value;



        //}
        public void ChoseColor()
        {
            col = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color;
        }




    }//class end
