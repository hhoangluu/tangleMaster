using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Obi;
using UnityEngine.UI;

public class Level
{
    public List<LevelRod> _levelRod = new List<LevelRod>();
}

public class LevelRod
{
    public ActorState _actorState;
    public int _rodIndex;

    public int _pluggerIndex;

    public LevelRod(ActorState actorState, int rodIndex, int pluggerIndex)
    {
        _actorState = actorState;
        _rodIndex = rodIndex;
        _pluggerIndex = pluggerIndex;
    }
}

[Serializable]
public class ActorState
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> velocities = new List<Vector3>();
    public List<Quaternion> orientations = new List<Quaternion>();
    public List<Vector3> angularVelocities = new List<Vector3>();
}

public class LevelsManager : FiveSingleton<LevelsManager>
{
    [SerializeField]
    private Text _curLevelText;
    //public event Action CurLevelChanged = delegate { };
    private const string KEY_LEVEL_CURRENT = "KEY_LEVEL_CURRENT";
    public int curLevel
    {
        get => PlayerPrefs.GetInt(KEY_LEVEL_CURRENT, 0);
        private set { PlayerPrefs.SetInt(KEY_LEVEL_CURRENT, value); _curLevelText.text = "Level: " + curLevel.ToString(); }
    }

    private static string _saveLevelDirectory => "/Game/Levels/";
    private static string _levelExtension => ".actorState";

    private List<string> _levelDirectory = new List<string>();

    private LevelModel _curLevelModel;
    public LevelModel curLevelModel => _curLevelModel;

    private int _maxLevel;
    public int maxLevel => instance._maxLevel;

    protected override void Awake()
    {
        base.Awake();
        FiveDebug.LogError("LevelsManager-Awake!");
        _curLevelText.text = "Level: " + curLevel.ToString();
        Debug.Log("GetPath(_saveLevelDirectory): " + GetPath(_saveLevelDirectory));
        for (int i = 0; i < 100; i++)
        {
            string directory = GetPath(_saveLevelDirectory) + i.ToString() + "/";
            Debug.Log("directory: " + directory);
            if (Directory.Exists(directory))
            {
                _levelDirectory.Add(directory);
            }
            else
            {
                _maxLevel = _levelDirectory.Count();
                FiveDebug.LogError("LevelsManager-MaxLevel: " + maxLevel);
                return;
            }
        }
    }

    public void LoadLevel(int level)
    {
        int levelLoad = level < maxLevel ? level : maxLevel - 1;
        Level levelSave = LoadSaveLevel(level);
        for (int i = 0; i < TangleMasterGame.rodsManager.rods.Count; i++)
        {
            TangleMasterGame.rodsManager.rods[i].Reset();
            TangleMasterGame.rodsManager.rods[i].gameObject.SetActive(true);
            TangleMasterGame.rodsManager.rods[i].curPlugPlace = TangleMasterGame.plugPlacesManager.plugPlaces[levelSave._levelRod[i]._pluggerIndex];
            TangleMasterGame.plugPlacesManager.plugPlaces[levelSave._levelRod[i]._pluggerIndex].curRodPlugger = TangleMasterGame.rodsManager.rods[i].rodPlugger;
            TangleMasterGame.rodsManager.rods[i].SetPluggedAbs();
            LoadActorState(TangleMasterGame.rodsManager.rods[i].obiRod, levelSave._levelRod[i]._actorState);
        }
        if (levelLoad < LevelsDatabase.instance.levelModels.Count)
            _curLevelModel = LevelsDatabase.instance.levelModels[levelLoad];
        else
            _curLevelModel = LevelsDatabase.instance.levelModels.Last();
    }

    private Level LoadSaveLevel(int level)
    {
        FiveDebug.LogError("LevelsManager-LoadSaveLevel: " + (level + 1) + "/" + maxLevel);
        if (level >= maxLevel) return null;

        Level levelResult = new Level();

        string levelPath = _levelDirectory[level];
        //Debug.Log("LoadLevel levelPath: " + levelPath);
        foreach (var n in Directory.GetFiles(levelPath))
        {
            //Debug.Log("n: " + n);
            if (Path.GetExtension(n) == _levelExtension)
            {
                string fileName = Path.GetFileName(n);

                string rodN = "";
                for (int lR = fileName.IndexOf("R") + 1; lR < fileName.IndexOf("-"); lR++)
                {
                    rodN = rodN + fileName[lR];
                }
                int rodNumber;
                int.TryParse(rodN, out rodNumber);

                string pluggerN = "";
                for (int pN = fileName.IndexOf("P") + 1; pN < fileName.IndexOf("."); pN++)
                {
                    pluggerN = pluggerN + fileName[pN];
                }
                int pluggerNumber;
                int.TryParse(pluggerN, out pluggerNumber);

                string json = File.ReadAllText(n);
                ActorState actorState = JsonUtility.FromJson<ActorState>(json);

                levelResult._levelRod.Add(new LevelRod(actorState, rodNumber, pluggerNumber));
            }
        }
        levelResult._levelRod = levelResult._levelRod.OrderBy(r => r._rodIndex).ToList();

        return levelResult;
    }

    public void SaveCurMeshState(int level)
    {
        string directory = GetPath(_saveLevelDirectory) + level.ToString() + "/";

        FiveDebug.LogError("LevelsManager-SaveCurMeshState: " + directory);

        if (Directory.Exists(directory))
            Directory.Delete(directory, true);
        Directory.CreateDirectory(directory);

        for (int r = 0; r < TangleMasterGame.rodsManager.rods.Count; r++)
        {
            int indexRod = TangleMasterGame.plugPlacesManager.IndexOfPlugPlace(TangleMasterGame.rodsManager.rods[r].curPlugPlace);
            string savePath = Path.Combine(directory, string.Format("R{0}-P{1}{2}", r.ToString(), indexRod, _levelExtension));

            ActorState actorState = SaveActorState(TangleMasterGame.rodsManager.rods[r].obiRod);
            string data = JsonUtility.ToJson(actorState);
            File.WriteAllText(savePath, data);
        }
    }

    private void LoadActorState(ObiActor actor, ActorState actorState)
    {
        for (int i = 0; i < actor.particleCount; ++i)
        {
            int solverIndex = actor.solverIndices[i];

            actor.solver.positions[solverIndex] = actorState.positions[i];
            actor.solver.velocities[solverIndex] = actorState.velocities[i];

            if (actor.usesOrientedParticles)
            {
                actor.solver.orientations[solverIndex] = actorState.orientations[i];
                actor.solver.angularVelocities[solverIndex] = actorState.angularVelocities[i];
            }
        }
    }

    private ActorState SaveActorState(ObiActor actor)
    {
        ActorState actorState = new ActorState();

        for (int i = 0; i < actor.particleCount; ++i)
        {
            int solverIndex = actor.solverIndices[i];

            actorState.positions.Add(actor.solver.positions[solverIndex]);
            actorState.velocities.Add(actor.solver.velocities[solverIndex]);

            if (actor.usesOrientedParticles)
            {
                actorState.orientations.Add(actor.solver.orientations[solverIndex]);
                actorState.angularVelocities.Add(actor.solver.angularVelocities[solverIndex]);
            }
        }
        return actorState;
    }

    private static string GetPath(string directory)
    {
#if !UNITY_EDITOR
                return Application.persistentDataPath + directory;
#else
        if (!Directory.Exists(Application.dataPath + directory))
            Directory.CreateDirectory(Application.dataPath + directory);
        return Application.dataPath + directory;
#endif
    }
}

//void Reset(ObiActor actor)
//{
//    if (actor.isLoaded)
//    {
//        Matrix4x4 l2sTransform = actor.actorLocalToSolverMatrix;
//        Quaternion l2sRotation = l2sTransform.rotation;

//        for (int i = 0; i < actor.particleCount; ++i)
//        {
//            int solverIndex = actor.solverIndices[i];

//            actor.solver.positions[solverIndex] = l2sTransform.MultiplyPoint3x4(actor.blueprint.positions[i]);
//            actor.solver.velocities[solverIndex] = l2sTransform.MultiplyVector(actor.blueprint.velocities[i]);

//            if (actor.usesOrientedParticles)
//            {
//                actor.solver.orientations[solverIndex] = l2sRotation * actor.blueprint.orientations[i];
//                actor.solver.angularVelocities[solverIndex] = l2sTransform.MultiplyVector(actor.blueprint.angularVelocities[i]);
//            }
//        }
//    }
//}