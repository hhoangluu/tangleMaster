using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class FiveScriptable
{
#if UNITY_EDITOR
    public static void CreateAsset<T>(string dir, string name = null, bool multipler = false, int offset = 0) where T : ScriptableObject
    {
        string mult = multipler ? (Resources.LoadAll<T>("").Length + offset).ToString() : "";
        string assetPathAndName = dir + "/" + (name == null ? typeof(T).Name : name) + mult + ".asset";

        if (!System.IO.File.Exists(assetPathAndName))
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            //Selection.activeObject = asset;
            Debug.Log("Asset created: " + assetPathAndName);
        }
        else
            Debug.LogError("File Exists: " + assetPathAndName);
        Selection.activeObject = Resources.Load<T>(typeof(T).Name);
        //EditorUtility.FocusProjectWindow();
    }
#endif
}
