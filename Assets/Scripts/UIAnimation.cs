using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
public class UIAnimation : MonoBehaviour
{
    public RectTransform logo;
    private float logoBounce=5f;
  
    // Start is called before the first frame update
    void Start()
    {
        LogoBouncer();
        
    }

    private void LogoBouncer()
    {
        logoBounce=Screen.height * 0.05f;
        logo.localScale = Vector3.zero;
        LogoScaler(1,1.5f,true);
        logo.DOAnchorPosY(logo.anchoredPosition.y+logoBounce, 1.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void LogoScaler(float scaleValue,float duration=0, bool bounce=false)
    {
        if (bounce) logo.DOScale(scaleValue, duration).SetEase(Ease.OutBounce);
        else logo.DOScale(scaleValue, duration);
    }


    public void ScaleObject(Transform go)
    {
        
    
            go.DOScale(1, .5f).SetEase(Ease.OutBounce);
     
    }

}
