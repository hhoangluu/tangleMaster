using System.IO;
using UnityEngine;
using UnityEditor;

public class F4AEditor
{
    /// <summary>
    /// Careful when using
    /// Delete all empty folders in project (In folder Assets)
    /// </summary>
    /// <author>sinh.nguyen</author>
    [MenuItem("F4A/Unity/Delete Empty Directories In Project")]
    public static void DeleteEmptyDirsInProject()
    {
        string dir = Application.dataPath;

        foreach (var directory in Directory.GetDirectories(dir))
        {
            DeleteEmptyDirs(directory);
            if (Directory.GetFiles(directory).Length == 0 &&
                Directory.GetDirectories(directory).Length == 0)
            {
                Directory.Delete(directory, false);
            }
        }

        UnityEditor.AssetDatabase.Refresh();
    }

    private static void DeleteEmptyDirs(string dir)
    {
        if (string.IsNullOrEmpty(dir))
            return;

        foreach (var directory in Directory.GetDirectories(dir))
        {
            DeleteEmptyDirs(directory);
            if (Directory.GetFiles(directory).Length == 0 &&
                Directory.GetDirectories(directory).Length == 0)
            {
                Directory.Delete(directory, false);

#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
        }
    }

    [MenuItem("F4A/Unity/PlayerPres/Delete All")]
    public static void DeletePlayerPres()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("F4A/Unity/Editor/Delete All Pref")]
    static void DeleteAllEditorPrefs()
    {
        EditorPrefs.DeleteAll();
    }

    [MenuItem("F4A/Unity/Caching/Clear Cache")]
    static void ClearCache()
    {
        Caching.ClearCache();
    }
}