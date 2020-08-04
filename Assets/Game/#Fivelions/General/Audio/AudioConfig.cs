using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioConfig : ScriptableSingleton<AudioConfig>
{
#if UNITY_EDITOR
    [MenuItem("Fivelions/Audio Config")]
    public static void Settings() { FiveScriptable.CreateAsset<AudioConfig>("Assets/Game/Resources/"); }
#endif

    [SerializeField]
    [Range(0f, 1f)]
    private float _musicVol = 1f;
    public static float musicVol => instance._musicVol;

    [SerializeField]
    [Range(0f, 1f)]
    private float _sfxVol = 1f;
    public static float sfxVol => instance._sfxVol;

    //public AudioClip btnPlay;
    //public AudioClip btnSub;
    //public AudioClip scannerDetected;
}
