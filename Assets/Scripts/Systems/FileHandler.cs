using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public static class FileHandler
{
    public static void SaveToJSON<T>(T toSave, string filename)
    {
        string content = JsonUtility.ToJson(toSave, true);
        WriteFile(GetPath(filename), content);
    }

    public static T ReadFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return default(T);
        }

        T res = JsonUtility.FromJson<T>(content);
        return res;
    }

    private static string GetPath(string filename)
    {
#if UNITY_EDITOR
        return System.IO.Path.Combine(Application.dataPath, "JSON", $"{filename}");
#else
        return System.IO.Path.Combine(Application.persistentDataPath, "JSON", $"{filename}");
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