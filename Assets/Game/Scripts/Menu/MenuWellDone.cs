using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuWellDone : MenuAbs<MenuWellDone>
{
    [SerializeField]
    private List<MenuComponent> _menuComponents;
    [SerializeField]
    private Image _bg;

    [SerializeField]
    private Image _fillRewardProgress;
    [SerializeField]
    private Text _fillRewardProgressText;

    [SerializeField]
    private Text _rewardX3Amount;
    public Text rewardX3Amount => _rewardX3Amount;

    [SerializeField]
    private Text _rewardAmount;
    public Text rewardAmount => _rewardAmount;

    private float _menuSpeed = 0.5f;
    private Coroutine _bgCorou;

    public override void Open()
    {
        _isOpened = true;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0.5f), _menuSpeed, onStart: () => _bg.gameObject.SetActive(true));
        _menuComponents.ForEach(mC => mC.Open(_menuSpeed));
        components.SetActive(true);
    }

    public override void Close()
    {
        if (!_isOpened) return;
        _isOpened = false;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0f), _menuSpeed, onDone: () => _bg.gameObject.SetActive(false));
        _menuComponents.ForEach(mC => mC.Close(_menuSpeed));
        components.SetActive(false);
    }

    public void GetX3Reward()
    {
        TangleMasterGame.instance.LoadNextLevel();
        Close();
    }

    public void GetReward()
    {
        TangleMasterGame.instance.LoadNextLevel();
        Close();
    }
}
