using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TattooDescriptions : MonoBehaviour
{
    public string _arDescriptiontext;
    public string _enDescriptiontext;
    public string _frDescriptiontext;
    private string _descriptiontext;
    public GameObject description;

    private void Start()
    {
        int localizationID = PlayerPrefs.GetInt("Localekey", 1);
        switch (localizationID)
        {
            case 0:
                _descriptiontext = _arDescriptiontext;
                break;
            case 1:
                _descriptiontext = _enDescriptiontext;
                break;
            case 2:
                _descriptiontext = _frDescriptiontext;
                break;

        }
        ButtonPressed();
    }
    public void ButtonPressed()
    {
        Image descriptionImage = description.transform.GetChild(1).GetComponentInChildren<Image>();
        descriptionImage.sprite= GetComponent<Image>().sprite;
        TextMeshProUGUI descriptionText = description.GetComponentInChildren<TextMeshProUGUI>();
        descriptionText.text = _descriptiontext;
    }
}
