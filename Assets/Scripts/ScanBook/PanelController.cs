using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    
    [SerializeField] private float delay = 2f;
    event Action action;
    int counter = 0;
    void Start()
    {
        Excute();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    void Excute()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i == counter)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            transform.GetChild(i).gameObject.SetActive(false);

        }
        counter++;
        if (counter > transform.childCount-1)
            counter = 0;
        Invoke(nameof(Excute), delay);
    }
}
