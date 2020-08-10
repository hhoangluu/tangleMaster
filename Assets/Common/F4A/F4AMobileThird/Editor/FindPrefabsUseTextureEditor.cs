using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FindPrefabsUseTextureEditor : EditorWindow
{
    public List<GameObject> Results = new List<GameObject>();
    public Object findObject;

    [MenuItem("F4A/FindPrefabsUseTexture")]
    static void ShowWindow()
    {
        var editor = GetWindow(typeof(FindPrefabsUseTextureEditor));
        editor.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        findObject = (Object)EditorGUILayout.ObjectField("", findObject, typeof(Object), true, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Find"))
            FindPrefabs();

        DisplayResult();
    }


    void FindPrefabs()
    {
        Results.Clear();
        var list = GetAllPrefabsAssets("Assets");
        Debug.Log(list.Count);
        CheckPrefabs(list);
    }

    void CheckPrefabs(List<GameObject> listPrefab)
    {
        if (findObject == null)
        {
            EditorUtility.DisplayDialog("Warning", "Please, input find Object", "Continue");
            return;
        }

        foreach (var prefab in listPrefab)
            if (CheckChild(prefab.transform))
                Results.Add(prefab);
    }

    bool CheckChild(Transform t)
    {
        if (t.GetComponent<SpriteRenderer>() != null
            && t.GetComponent< SpriteRenderer>().sprite == findObject)
                return true;
        foreach (Transform t2 in t)
            if (CheckChild(t2))
                return true;
        return false;
    }

    private Vector3 _scrollPos;
    void DisplayResult()
    {
        if (Results != null && Results.Count > 0)
        {
            GUILayout.Label("Count: " + Results.Count);
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            for (int i = 0; i < Results.Count; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField("", Results[i], typeof(GameObject), true);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
    }

    public static List<GameObject> GetAllPrefabsAssets(string path)
    {
        string[] paths = { path };
        var assets = AssetDatabase.FindAssets("t:prefab", paths);
        var assetsObj = assets.Select(s => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(s))).ToList();
        return assetsObj;
    }
}
