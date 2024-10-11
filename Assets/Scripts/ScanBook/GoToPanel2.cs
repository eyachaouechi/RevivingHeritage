using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPanel2 : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public float delayTime = 4f;

    private bool switching = false;

    void Start()
    {
        // Activer le panel 1 et désactiver le panel 2 au démarrage
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    void Update()
    {
        if (switching)
            return;

        // Activer le panel 2 et désactiver le panel 1 après le délai spécifié
        Invoke("SwitchPanels", delayTime);

        // Activer le panel 2 si l'utilisateur appuie sur un bouton de balayage
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            SwitchPanels();
        }
    }

    void SwitchPanels()
    {
        // Activer le panel 2 et désactiver le panel 1
        panel2.SetActive(true);
        panel1.SetActive(false);
        switching = true;
    }
}
