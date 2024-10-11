using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonAnimation : MonoBehaviour
{
    public bool scaleChild = true;
    // Start is called before the first frame update
    void OnEnable()
    { 
        
        if (transform.childCount > 0 && scaleChild)
        {
            foreach (Transform go in transform)
            {
                go.localScale = Vector3.zero;
            }
        }
        transform.localScale = Vector3.zero;
        transform.DOScale(1, .5f).SetEase(Ease.OutBounce).OnComplete(() => { ChildAnimation(); });
    }
    
    void ChildAnimation()
    {
        if (transform.childCount > 0 && scaleChild)
        {
            foreach (Transform go in transform)
            {
                go.DOScale(1, .5f).SetEase(Ease.OutBounce);
            }
        }
    }

}
