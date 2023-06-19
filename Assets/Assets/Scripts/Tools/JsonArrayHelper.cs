using UnityEngine;

public static class JsonArrayHelper
{
    public static void FixJson(ref string json)
    {
        json = "{\"Items\":" + json + "}";
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }
}

[System.Serializable]
public class Wrapper<T>
{
    public T[] Items;
}