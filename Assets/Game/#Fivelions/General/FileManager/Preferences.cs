using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
#pragma warning disable

public class Preferences : ScriptableSingleton<Preferences>
{
#if UNITY_EDITOR
    [MenuItem("Fivelions/Preferences")]
    public static void FilePreferences()
    {
        FiveScriptable.CreateAsset<Preferences>("Assets/Game/Resources/");
    }
#endif

    [SerializeField]
    private string _savefileDirectory = "/Game/Saves";
    public static string savefileDirectory => instance._savefileDirectory;

    [SerializeField]
    private string _savefileExtensions = ".five";
    public static string savefileExtensions => instance._savefileExtensions;

    [SerializeField]
    private bool _isEncrypted;
    public static bool isEncrypted => instance._isEncrypted;

    [SerializeField]
    private string _encryptedPassword;
    public static string encryptedPassword => instance._encryptedPassword;

    public static string GetSaveFilePath<T>() where T : ISerializable
    {
        return (Path.Combine(GetPath(savefileDirectory), "(" + typeof(T).Name + ")" + savefileExtensions));
    }

    private static string GetPath(string directory)
    {
#if !UNITY_EDITOR
                return Application.persistentDataPath;
#else
        if (!Directory.Exists(Application.dataPath + directory))
            Directory.CreateDirectory(Application.dataPath + directory);
        return Application.dataPath + directory;
#endif
    }
}
