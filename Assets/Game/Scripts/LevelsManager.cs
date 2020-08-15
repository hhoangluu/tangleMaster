using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Obi;

[Serializable]
public class Level
{
    public List<LevelRod> _levelRod = new List<LevelRod>();
}

[Serializable]
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
    private static string _saveLevelDirectory => "/Game/Resources/Levels/";
    private static string _levelExtension => ".json";

    //private List<string> _levelDirectory = new List<string>();

    private LevelModel _curLevelModel;
    public LevelModel curLevelModel => _curLevelModel;

    [SerializeField]
    private int _maxLevel;
    public int maxLevel => instance._maxLevel;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        string directory = GetPath(_saveLevelDirectory) + i.ToString() + "/";
    //        //Debug.Log("directory: " + directory);
    //        if (Directory.Exists(directory))
    //        {
    //            _levelDirectory.Add(directory);
    //        }
    //        else
    //        {
    //            _maxLevel = _levelDirectory.Count();
    //            Debug.Log("@LOG LevelsManager-MaxLevel: " + maxLevel);
    //            return;
    //        }
    //    }
    //}

    public void LoadLevel(int level)
    {
        int levelLoad = level < maxLevel ? level : maxLevel - 1;
        Level levelSave = LoadSaveLevel(level);
        for (int i = 0; i < TangleMasterGame.ropeManager.ropes.Count; i++)
        {
            TangleMasterGame.ropeManager.ropes[i].Reset();
            TangleMasterGame.ropeManager.ropes[i].gameObject.SetActive(true);
            TangleMasterGame.ropeManager.ropes[i].curPlugPlace = TangleMasterGame.plugPlacesManager.plugPlaces[levelSave._levelRod[i]._pluggerIndex];
            TangleMasterGame.plugPlacesManager.plugPlaces[levelSave._levelRod[i]._pluggerIndex].curRodPlugger = TangleMasterGame.ropeManager.ropes[i].rodPlugger;
            TangleMasterGame.ropeManager.ropes[i].SetPluggedAbs();
            LoadActorState(TangleMasterGame.ropeManager.ropes[i].obiRope, levelSave._levelRod[i]._actorState);
        }
        if (levelLoad < LevelsDatabase.instance.levelModels.Count)
            _curLevelModel = LevelsDatabase.instance.levelModels[levelLoad];
        else
            _curLevelModel = LevelsDatabase.instance.levelModels.Last();
    }

    private Level LoadSaveLevel(int level)
    {
        Debug.Log("@LOG LevelsManager-LoadSaveLevel: " + (level + 1) + "/" + maxLevel);
        if (level >= maxLevel) return null;

        string path = "Levels/" + "Level" + level.ToString() + _levelExtension;
        Debug.Log("LoadSaveLevel: " + path);
        string jsonFilePath = path.Replace(".json", "");
        TextAsset levelSaveTextAsset = Resources.Load<TextAsset>(jsonFilePath);
        Debug.Log("levelSaveJson: " + levelSaveTextAsset.text);
        Level levelSave = JsonUtility.FromJson<Level>(levelSaveTextAsset.text);
        return levelSave;
    }

    public void SaveCurMeshState(int level)
    {
        string directory = Application.dataPath + _saveLevelDirectory;

        Debug.Log("@LOG LevelsManager-SaveCurMeshState: " + directory);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        Level levelSave = new Level();

        for (int r = 0; r < TangleMasterGame.ropeManager.ropes.Count; r++)
        {
            int indexRod = TangleMasterGame.plugPlacesManager.IndexOfPlugPlace(TangleMasterGame.ropeManager.ropes[r].curPlugPlace);

            ActorState actorState = SaveActorState(TangleMasterGame.ropeManager.ropes[r].obiRope);
            LevelRod levelRod = new LevelRod(actorState, r, indexRod);
            levelSave._levelRod.Add(levelRod);
        }
        string data = JsonUtility.ToJson(levelSave);
        string path = directory + "Level" + level.ToString() + _levelExtension;
        Debug.Log("@LOG LevelsManager-SaveCurMeshState path: " + path);
        File.WriteAllText(path, data);
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
            Debug.Log("Load actor state");
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

//    private static string GetPath(string directory)
//    {
//#if UNITY_EDITOR
//        return Application.dataPath + directory;
//#else
//        if (!Directory.Exists(Application.dataPath + directory))
//            Directory.CreateDirectory(Application.dataPath + directory);
//        return Application.dataPath + directory;
//#endif
//    }
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