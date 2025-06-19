using UnityEngine;

public static class JsonHelper
{
    public static string ToJson(int[] array)
    {
        return JsonUtility.ToJson(new Wrapper { Items = array });
    }

    [System.Serializable]
    private class Wrapper
    {
        public int[] Items;
    }
}
