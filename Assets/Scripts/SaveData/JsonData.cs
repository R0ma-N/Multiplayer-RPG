using System.IO;
using UnityEngine;

public class JsonData<T>
{
    public void Save(T data, string path = null)
    {
        var str = JsonUtility.ToJson(data);
        File.WriteAllText(path, str);
    }

    public T Load(string path = null)
    {
        var str = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(str);
    }
}
