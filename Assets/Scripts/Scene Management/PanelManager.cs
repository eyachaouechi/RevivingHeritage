using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
    public UIAnimation anim;
    public float scaleValue;
    public List<GameObject> panels = new List<GameObject>();
    void Awake()
    {
        // Désactiver tous les panels sauf le premier
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == 0);
        }
    }

    public void ActivatePanel(int index)
    {
        if(index==3)
        {
            panels[3].SetActive(true);
        }
        
        // Vérifier si l'index est valide
        else  if (index >= 0 && index < panels.Count)
        {
            // Parcourir tous les panels
            for (int i = 0; i < panels.Count; i++)
            {
                // Activer le panel correspondant à l'index et désactiver les autres
                panels[i].SetActive(i == index);
                if (index == 2)
                {
                    anim.LogoScaler(scaleValue,1);
                }
                else
                {
                    anim.LogoScaler(1,1);
                }
            }
        }
        else
        {
            Debug.LogWarning("Index de panel invalide.");
        }
    }
}
