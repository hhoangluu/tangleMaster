using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

[Serializable]
public class LevelModel
{
    [SerializeField]
    private int _rewardCoin;
    public int rewardCoin => _rewardCoin;

    public int rewardX3Coin => _rewardCoin * 3;
}

public class LevelsDatabase : ScriptableSingleton<LevelsDatabase>
{
#if UNITY_EDITOR
    [MenuItem("Fivelions/Game/LevelSettings")]
    public static void OpenLevelSettings()
    {
        FiveScriptable.CreateAsset<LevelsDatabase>("Assets/Game/Resources/");
    }
#endif

    [SerializeField]
    private List<LevelModel> _levelModels = new List<LevelModel>();
    public List<LevelModel> levelModels => _levelModels;
}
