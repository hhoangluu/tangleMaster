using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class PoolData
{
    public string key;
    public int poolAmount;
    public GameObject source;
}

public class FivelionsPool : ScriptableSingleton<FivelionsPool>
{
#if UNITY_EDITOR
    [MenuItem("Fivelions/Pool")]
    public static void PoolSetting() { FiveScriptable.CreateAsset<FivelionsPool>("Assets/#Fivelions/Pool/Resources/"); }
#endif

    public PoolData[] InitData;

    private Dictionary<string, int> PoolHolder = new Dictionary<string, int>();
    private List<GameObject[]> PoolList = new List<GameObject[]>();

    static GameObject inst;
    public static bool initialized { get; set; }

    public void Init()
    {
        if (initialized)
            return;

        initialized = true;
        for (int i = 0; i < InitData.Length; i++)
        {
            PoolHolder.Add(InitData[i].key, i);
            PoolList.Add(new GameObject[InitData[i].poolAmount]);

            for (int init = 0; init < InitData[i].poolAmount; init++)
            {
                inst = Instantiate(InitData[i].source);
                inst.gameObject.SetActive(false);
                PoolList[i][init] = inst;
                DontDestroyOnLoad(inst);
            }
        }
    }

    public T GetPool<T>(string key = "typeof(T).Name", bool actived = false)
    {
        key = key == "typeof(T).Name" ? typeof(T).Name : key;
        int index = PoolHolder[key];
        for (int i = 0; i < PoolList[index].Length; i++)
        {
            if (!PoolList[index][i].activeSelf)
            {
                PoolList[index][i].SetActive(actived);
                return PoolList[index][i].GetComponent<T>();
            }
        }
        return default(T);
    }

    public void PoolOnDisable()
    {
        PoolHolder.Clear();
        PoolList.Clear();
        initialized = false;
    }
}