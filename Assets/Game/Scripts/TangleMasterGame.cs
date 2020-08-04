using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class TangleMasterGame : FiveSingleton<TangleMasterGame>
{
    public static RodsManager rodsManager => RodsManager.instance;

    public static PlugPlacesManager plugPlacesManager => PlugPlacesManager.instance;

    public static LevelsManager levelsManager => LevelsManager.instance;

    public static GameController gameController => GameController.instance;

    public int curLevel => levelsManager.curLevel;

    private LevelModel curLevelModel => levelsManager.curLevelModel;

    private RodPlugger _activeRodPlugger;

    private Coroutine _corou;

    private bool _isPlayable;
    public bool isPlayable => _isPlayable;

    private int _totalFree;

    private int _totalFreeDone;

    private void Start()
    {
        FiveDebug.LogError("TangleMasterGame-Start-LoadLevel");
        levelsManager.LoadLevel(curLevel);
    }

    private IEnumerator DelayDo(float delay, Action toDo)
    {
        yield return new WaitForSeconds(delay);
        toDo?.Invoke();
    }

    private void SwapPlugPlace(RodPlugger ropePlugger, PlugPlace plugPlace)
    {
        FiveDebug.Log("Swap: " + ropePlugger + " -> " + plugPlace);
        ropePlugger.rodHost.curPlugPlace.curRodPlugger = null;
        ropePlugger.rodHost.curPlugPlace = plugPlace;
        plugPlace.curRodPlugger = ropePlugger;
        ropePlugger.rodHost.SetPlugged();
    }

    public void OnPlugSlotClicked(PlugPlace plugPlace)
    {
        FiveDebug.Log("OnPlugSlotClicked: " + plugPlace.name);
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
        FiveDebug.Log("OnRodPluggerClicked: " + rodPlugger.name);
        switch (rodPlugger.rodHost.curRodState)
        {
            case RodState.plugged:
                if (_activeRodPlugger == null)
                {
                    if (!rodPlugger.rodHost.isFree)
                    {
                        rodPlugger.rodHost.SetUnPlugged();
                        _activeRodPlugger = rodPlugger;
                    }
                }
                break;
            case RodState.unplugged:
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

    public void IsFree(Rod rod)
    {
        _totalFree += 1;
        FiveDebug.LogError("IsFree: " + _totalFree + "/" + rodsManager.rods.Count);
        rod.SetFree(onDone: () => _totalFreeDone += 1);

        if (_totalFree == rodsManager.rods.Count)
        {
            gameController.isControllable = false;
            if (_corou != null) StopCoroutine(_corou);
            _corou = StartCoroutine(ShowWellDone());
            //if (curLevel + 1 < LevelsManager.instance.maxLevel)
            //{
            //    gameController.isControllable = false;
            //    if (_corou != null) StopCoroutine(_corou);
            //    _corou = StartCoroutine(ShowWellDone());
            //}
        }
    }

    private IEnumerator ShowWellDone()
    {
        FiveDebug.LogError("ShowWellDone");
        while (_totalFreeDone < rodsManager.rods.Count)
            yield return GameManager.WaitForEndOfFrame;
        yield return new WaitForSeconds(0.5f);

        MenuWellDone.instance.rewardX3Amount.text = "123";
        MenuWellDone.instance.rewardAmount.text = "12";
        MenuWellDone.instance.Open();
    }

    public void LoadNextLevel()
    {
        _totalFree = 0;
        _totalFreeDone = 0;
        _isPlayable = false;
        LevelsManager.instance.LoadLevel(curLevel + 1);
        MenuMain.instance.Open();
    }

    //private IEnumerator LoadNextLevel()
    //{
    //    FiveDebug.LogError("LoadNextLevel");
    //    while (_totalFree < LevelsManager.instance.maxLevel)
    //        yield return GameManager.WaitForEndOfFrame;
    //    yield return new WaitForSeconds(1f);
    //    FiveDebug.LogError("LoadNextLevel-WaitDone");
    //    //curLevel += 1;
    //    _totalFree = 0;
    //    _totalFreeDone = 0;
    //    _isPlayable = false;
    //    LevelsManager.instance.LoadLevel(curLevel + 1);
    //    MenuMain.instance.Open();
    //}

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    //public void StopGame()
    //{
    //    _isPlayable = false;

    //}        

    public void StartGame()
    {
        FiveDebug.LogError("StartGame!");
        _isPlayable = true;
        gameController.isControllable = true;
        rodsManager.rods.ForEach(r => r.rodChecker.isCheckFree = true);
        //StartCoroutine(DelayDo(1f, () =>
        //{
        //    _gameController.isControllable = true;
        //    rodsManager.rods.ForEach(r => r.rodChecker.isCheckFree = true);
        //}));
    }
}
