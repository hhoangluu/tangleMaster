using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using Five.String;

public class TangleMasterGame : FiveSingleton<TangleMasterGame>
{
    public static RopeManager ropeManager => RopeManager.instance;

    public static PlugPlacesManager plugPlacesManager => PlugPlacesManager.instance;

    public static LevelsManager levelsManager => LevelsManager.instance;

    public static GameController gameController => GameController.instance;

    private LevelModel curLevelModel => levelsManager.curLevelModel;

    private RodPlugger _activeRodPlugger;

    private Coroutine _corou;

    private bool _isPlayable;
    public bool isPlayable => _isPlayable;

    private int _totalFree;

    private int _totalFreeDone;

    private void Start()
    {
        //for (int i = 0; i < ropeManager.ropes.Count; i++)
        //{
        //    ropeManager.ropes[i].curPlugPlace = plugPlacesManager.plugPlaces[i];
        //    plugPlacesManager.plugPlaces[i].curRodPlugger = ropeManager.ropes[i].rodPlugger;

        //    ropeManager.ropes[i].SetPlugged();
        //}

         LevelsManager.instance.LoadLevel(DMCGameUtilities.LevelCurrent);
    }

    public void ResetOriginRopes()
    {
        for (int i = 0; i < ropeManager.ropes.Count; i++)
        {
            ropeManager.ropes[i].curPlugPlace = plugPlacesManager.plugPlaces[i];
            plugPlacesManager.plugPlaces[i].curRodPlugger = ropeManager.ropes[i].rodPlugger;

            ropeManager.ropes[i].SetPlugged();
        }
    }
    
    private IEnumerator DelayDo(float delay, Action toDo)
    {
        yield return new WaitForSeconds(delay);
        toDo?.Invoke();
    }

    private void SwapPlugPlace(RodPlugger ropePlugger, PlugPlace plugPlace)
    {
   //     FiveDebug.Log("Swap: " + ropePlugger + " -> " + plugPlace);
        ropePlugger.rodHost.curPlugPlace.curRodPlugger = null;
        ropePlugger.rodHost.curPlugPlace = plugPlace;
        plugPlace.curRodPlugger = ropePlugger;
        ropePlugger.rodHost.SetPlugged();
    }

    public void OnPlugSlotClicked(PlugPlace plugPlace)
    {
     //   FiveDebug.Log("OnPlugSlotClicked: " + plugPlace.name);
        if (_activeRodPlugger)
        {
            if (_activeRodPlugger.rodHost.curPlugPlace == plugPlace)
            {
                if (!_activeRodPlugger.rodHost.isFree)
                {
                    _activeRodPlugger.rodHost.SetPlugged();
                }
                _activeRodPlugger = null;
            }
            else if (plugPlace.curRodPlugger == null)
            {
                //swap
                if (!_activeRodPlugger.rodHost.isFree)
                {
                    SwapPlugPlace(_activeRodPlugger, plugPlace);
                }
                _activeRodPlugger = null;
            }
        }
    }

    public void OnRodPluggerClicked(RodPlugger rodPlugger)
    {
        Debug.Log("OnRodPluggerClicked: " + rodPlugger.name);
        switch (rodPlugger.rodHost.curRodState)
        {
            case RopeState.plugged:
                if (_activeRodPlugger == null)
                {
                    if (!rodPlugger.rodHost.isFree)
                    {
                        rodPlugger.rodHost.SetUnPlugged();
                        _activeRodPlugger = rodPlugger;
                    }
                }
                break;
            case RopeState.unplugged:
                if (rodPlugger == _activeRodPlugger)
                {
                    if (!rodPlugger.rodHost.isFree)
                    {
                        rodPlugger.rodHost.SetPlugged();
                    }
                    _activeRodPlugger = null;
                }
                break;
        }
    }

   

    public void IsFree(Rope rod)
    {
        _totalFree += 1;
        Debug.Log("@LOG IsFree: " + _totalFree + "/" + ropeManager.ropes.Count);
        rod.SetFree(onDone: () => _totalFreeDone += 1);
        Handheld.Vibrate();
        if (_totalFree == ropeManager.ropes.Count)
        {
            Debug.Log("s");
            if (_corou != null) StopCoroutine(_corou);
            _corou = StartCoroutine(ShowWellDone());
        }
    }

    private IEnumerator ShowWellDone()
    {
        Debug.Log("@LOG LoadNextLevel");
   
        yield return new WaitWhile(() => _totalFreeDone < ropeManager.ropes.Count);
        yield return new WaitForSeconds(1f);

        MenuWellDone.instance.Open();
    }

    public void LoadNextLevel()
    {

        Debug.Log("@LOG LoadNextLevel");
        _totalFree = 0;
        _totalFreeDone = 0;
        _isPlayable = false;
        foreach (var rope in ropeManager.ropes)
        {
            rope.OnNextLevel();
        }
        if (DMCGameUtilities.LevelCurrent >= 28) DMCGameUtilities.LevelCurrent = 0;
        else DMCGameUtilities.LevelCurrent++;

        LevelsManager.instance.LoadLevel(DMCGameUtilities.LevelCurrent);
        MenuMain.instance.Open();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }     

    public void EditorMode()
    {
        _isPlayable = true;
        gameController.isControllable = true;
    }

    public void EditorModeExit()
    {
        _isPlayable = false;
        gameController.isControllable = false;
    }

    public void StartGame()
    {
        _isPlayable = true;
        gameController.isControllable = true;
        ropeManager.ropes.ForEach(r => r.rodChecker.isCheckFree = true);
        Application.targetFrameRate = 60;
    }
}
