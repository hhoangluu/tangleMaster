using UnityEngine;
using System.IO;
using System.Threading;
using Five.String;

public static class FileManager
{
    public static T Open<T>() where T : ISerializable, new()
    {
        T @out = new T();
        string path = Preferences.GetSaveFilePath<T>();

        Thread fileResult = new Thread(() =>
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                @out = JsonUtility.FromJson<T>(json);
            }
            else
            {
                File.WriteAllText(path, JsonUtility.ToJson(@out));
            }
            //@out.path = path;
        });

        fileResult.Start();
        fileResult.Join();
        //Debug.Log("Opened Path: ".Color(Color.cyan) + path);
        Debug.Log(string.Format("{0}: {1}", "Opened Path: ".Color(Color.cyan), path));
        return @out;
    }

    public static void Save<T>(this T file) where T : ISerializable
    {
        string path = Preferences.GetSaveFilePath<T>();

        if (File.Exists(path))
            File.Delete(path);

        Thread fileSave = new Thread(() =>
        {
            string data = JsonUtility.ToJson(file);
            File.WriteAllText(path, data);
        });
        fileSave.Start();
        fileSave.Join();
        //Debug.Log("Saved Path: ".Color(Color.green) + path);
        Debug.Log(string.Format("{0}: {1}", "Saved Path: ".Color(Color.green), path));
    }

    //private static void Delete(this string path)
    //{
    //    System.IO.File.Delete(path);
    //}
}
