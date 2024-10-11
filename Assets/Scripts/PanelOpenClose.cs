using UnityEngine;

public class PanelOpenClose : MonoBehaviour
{
    private bool state = false;
    public GameObject panel;

    public void SwitchState()
    {
        state = !state;
        panel.SetActive(state);
    }
}