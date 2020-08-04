using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSettings : MenuAbs<MenuSettings>
{
    [SerializeField]
    private List<MenuComponent> _menuComponents;
    [SerializeField]
    private Image _bg;

    [SerializeField]
    private Switcher _audioSwitcher;
    private bool _isAudio
    {
        get => !AudioManager.muteMusic && !AudioManager.muteSFX;
        set { AudioManager.instance.MuteAll(!value); }
    }

    [SerializeField]
    private Switcher _vibrateSwitcher;
    private bool _isVibrate
    {
        get => VibrationMaster.isVibrate;
        set { VibrationMaster.isVibrate = value; }
    }

    private float _menuSpeed = 0.5f;
    private Coroutine _bgCorou;

    public override void Open()
    {
        _isOpened = true;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0.5f), _menuSpeed, onStart: () => _bg.gameObject.SetActive(true));
        _menuComponents.ForEach(mC => mC.Open(_menuSpeed));
        components.SetActive(true);
        RefreshSwitch();
    }

    public override void Close()
    {
        if (!_isOpened) return;
        Debug.Log("Close");
        _isOpened = false;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0f), _menuSpeed, onDone: () => _bg.gameObject.SetActive(false));
        _menuComponents.ForEach(mC => mC.Close(_menuSpeed));
        components.SetActive(false);
    }

    private void RefreshSwitch()
    {
        if (_audioSwitcher.curState != _isAudio)
            _audioSwitcher.TurnSwitch(_isAudio, 0.25f, () => { });
        if (_vibrateSwitcher.curState != _isVibrate)
            _vibrateSwitcher.TurnSwitch(_isVibrate, 0.25f, () => { });
    }

    public void ToggleAudio()
    {
        _isAudio = !_isAudio;
        _audioSwitcher.TurnSwitch(_isAudio, 0.25f, () => { });
    }

    public void ToggleVibration()
    {
        _isVibrate = !_isVibrate;
        _vibrateSwitcher.TurnSwitch(_isVibrate, 0.25f, () => { if (_isVibrate) VibrationMaster.Vibrate(100); });
    }
}
