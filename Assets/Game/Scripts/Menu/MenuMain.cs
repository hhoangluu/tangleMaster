using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuMain : MenuAbs<MenuMain>
{
    [SerializeField]
    private Text _currentLevelText;

    [SerializeField]
    private List<MenuComponent> _menuComponents;

    [SerializeField]
    private MenuComponent _btnEditor;

    [SerializeField]
    private Image _bg;

    private Coroutine _bgCorou;

    protected override void Awake()
    {
        base.Awake();
        _menuComponents.ForEach(mC => mC.SetAsClosed());
#if UNITY_EDITOR
        _btnEditor.gameObject.SetActive(true);
#else
        _btnEditor.gameObject.SetActive(false);
#endif
    }

    private void Start()
    {
        DMCGameUtilities.OnChangeCurrentLevel += DMCGameUtilities_OnChangeCurrentLevel;
        DMCGameUtilities_OnChangeCurrentLevel(DMCGameUtilities.LevelCurrent);
        Open();
    }

    private void OnDestroy()
    {
        DMCGameUtilities.OnChangeCurrentLevel -= DMCGameUtilities_OnChangeCurrentLevel;
    }

    private void DMCGameUtilities_OnChangeCurrentLevel(int level)
    {
        _currentLevelText.text = "Level " + (level + 1);
    }

    public override void Open()
    {
        _isOpened = true;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0f), 0.5f, onStart: () => _bg.gameObject.SetActive(true));
        _menuComponents.ForEach(mC => mC.Open(0.5f));
    }

    public override void Close()
    {
        _isOpened = false;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0f), 0.5f, onDone: () => _bg.gameObject.SetActive(false));
        _menuComponents.ForEach(mC => mC.Close(0.5f));
    }

    public void OpenSettings()
    {
        //Close();
        MenuSettings.instance.Open();
    }

    public void OpenTheme()
    {
        MenuTheme.instance.Open();
    }

    public void OpenChallenge()
    {

    }

    public void OpenEditor()
    {
        Close();
        MenuEditor.instance.Open();
    }

    public void ResetGame()
    {
        TangleMasterGame.instance.ResetGame();
    }

    public void PlayGame()
    {
        _isOpened = false;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0f), 0.5f,
            onDone: () =>
            {
                _bg.gameObject.SetActive(false); TangleMasterGame.instance.StartGame();
            });
        _menuComponents.ForEach(mC => mC.Close(0.5f));
        //TangleMasterGame.instance.StartGame();
    }
}
