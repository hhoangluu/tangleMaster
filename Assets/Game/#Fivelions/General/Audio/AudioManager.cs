using System;
using AudioSettingsDatabase;
using UnityEngine;

public class AudioManager : FiveSingleton<AudioManager>
{
    private MusicSubscriber _musicSubscriber;
    public static MusicSubscriber musicSubscriber => instance._musicSubscriber;

    public event Action UpdateMusicVolumn = delegate { AudioSettingData.Save(); };

    public static bool muteMusic
    {
        get { return AudioSettingData.instance.musicSettings.mute; }
        set { AudioSettingData.instance.musicSettings.mute = value; instance.UpdateMusicVolumn(); }
    }

    public static float musicVol
    {
        get { return AudioSettingData.instance.musicSettings.volumn; }
        set { AudioSettingData.instance.musicSettings.volumn = value; instance.UpdateMusicVolumn(); }
    }

    private SfxSubscriber _sfxSubscriber;
    public static SfxSubscriber sfxSubscriber => instance._sfxSubscriber;

    public event Action UpdateSfxVolumn = delegate { AudioSettingData.Save(); };

    public static bool muteSFX
    {
        get { return AudioSettingData.instance.sfxSettings.mute; }
        set { AudioSettingData.instance.sfxSettings.mute = value; instance.UpdateSfxVolumn(); }
    }

    public static float sfxVol
    {
        get { return AudioSettingData.instance.sfxSettings.volumn; }
        set { AudioSettingData.instance.sfxSettings.volumn = value; instance.UpdateSfxVolumn(); }
    }

    protected override void Awake()
    {
        base.Awake();
        GameObject music = new GameObject();
        _musicSubscriber = music.AddComponent<MusicSubscriber>();
        _musicSubscriber.Init(this.transform);

        GameObject sfx = new GameObject();
        _sfxSubscriber = sfx.AddComponent<SfxSubscriber>();
        _sfxSubscriber.Init(this.transform);
    }

    private void Start()
    {
        UpdateMusicVolumn();
        UpdateSfxVolumn();
    }

    public void MuteAll(bool value)
    {
        AudioSettingData.instance.musicSettings.mute = value;
        UpdateMusicVolumn();
        AudioSettingData.instance.sfxSettings.mute = value;
        UpdateSfxVolumn();
        AudioSettingData.Save();
    }
}
