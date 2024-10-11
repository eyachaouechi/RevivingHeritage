using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalSelectors : MonoBehaviour
{
    private bool active = false;
    private static LocalSelectors instance;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);     
    }

    private void Start()
    {
        SavedLanguage();
    }

    public void ChangeLocale(int localeID)
    {
        if (active)
            return;
        StartCoroutine(SetLocale(localeID));
    }

    private IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;

        // Check if the locale ID is valid
        if (localeID >= 0 && localeID < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            PlayerPrefs.SetInt("Localekey", localeID);
            Debug.Log($"Locale set to: {LocalizationSettings.AvailableLocales.Locales[localeID].Identifier.Code}");
        }
        else
        {
            Debug.LogError($"Invalid locale ID: {localeID}");
        }

        active = false;
    }

    public void SavedLanguage()
    {
        int savedLocaleID = PlayerPrefs.GetInt("Localekey", 0);
        StartCoroutine(SetLocale(savedLocaleID));
    }
}
