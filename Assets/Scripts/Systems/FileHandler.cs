using System.IO;
using UnityEngine;

public static class FileHandler
{
    public static void SaveToJSON<T>(T toSave, string filename, bool isPersistant) // False leads to readonly "StreamingAssets"
    {
        string content = JsonUtility.ToJson(toSave, true);
        WriteFile(GetPath(filename, isPersistant), content);
    }

    public static T ReadFromJSON<T>(string filename, bool isPersistant)
    {
        string content = ReadFile(GetPath(filename, isPersistant));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return default(T);
        }

        T res = JsonUtility.FromJson<T>(content);
        return res;
    }

    private static string GetPath(string filename, bool isPersistant)
    {
        string folder = "";

#if UNITY_EDITOR
        if (isPersistant == true)
        {
            folder = "PersistentData";

            Debug.Log($"Loading Tutorial Data From: {Application.dataPath}/{folder}/{filename}");

            return System.IO.Path.Combine(Application.dataPath, folder, $"{filename}");
        }
        else
        {
            folder = "StreamingAssets";

            Debug.Log($"Loading Tutorial Data From: {Application.dataPath}/{folder}/{filename}");

            return System.IO.Path.Combine(Application.dataPath, folder, $"{filename}");
        }
#else
        if (persistant == true)
        {
            return System.IO.Path.Combine(Application.persistentDataPath, $"{filename}");
        }
        else
        {
            return System.IO.Path.Combine(Application.streamingAssetsPath, $"{filename}");
        }
#endif
    }

    private static void WriteFile(string path, string content)
    {
        string directory = Path.GetDirectoryName(path);

        if (Directory.Exists(directory) == false)
        {
            Directory.CreateDirectory(directory);
        }

        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}