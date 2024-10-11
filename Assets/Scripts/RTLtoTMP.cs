using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RTLtoTMP : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    public TextMeshProUGUI m_RTL;

    private void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        m_RTL.text = m_TextMeshProUGUI.text;
    }
}