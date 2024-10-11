using System;

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
