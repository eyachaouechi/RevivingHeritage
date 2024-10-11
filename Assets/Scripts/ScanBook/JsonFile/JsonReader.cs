using System;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
  //  public TextAsset textJSON;

    [System.Serializable]
    public class Infos
    {
        public int id;
        public String content;
    }

    [System.Serializable]
    public class InfoList
    {
        public Infos[] infos;
    }

    public InfoList myInfoList = new InfoList();

    public void LoadJson(TextAsset jsonFile)
    {
        if (jsonFile != null)
        {
          //  Debug.Log("Loading JSON file: " + jsonFile.name);
            try
            {
                myInfoList = JsonUtility.FromJson<InfoList>(jsonFile.text);
              //  Debug.Log("Loaded JSON: " + jsonFile.text);

                // Log the loaded info to check if parsing is correct
                foreach (var info in myInfoList.infos)
                {
                    // Debug.Log($"Loaded Info: ID={info.id}, Content={info.content}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error parsing JSON: " + e.Message);
                Debug.LogError("JSON content: " + jsonFile.text);
            }
        }
        else
        {
            Debug.LogError("JSON file is null.");
        }
    }

    public string GetContent(int id)
    {


        if (id >= 0 && id < myInfoList.infos.Length)
        {
           // Debug.Log("info " + id);
            return myInfoList.infos[id].content;
        }
        else
        {
           // Debug.LogError("Invalid index or no content available for index: " + id);
            return null;
        }
    }
}
