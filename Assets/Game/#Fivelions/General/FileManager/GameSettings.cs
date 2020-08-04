using UnityEngine;
using UnityEditor;

public class GameSettings : ScriptableSingleton<GameSettings>
{
#if UNITY_EDITOR
    [MenuItem("Fivelions/Game/GameSettings")]
    public static void OpenGameSettings()
    {
        FiveScriptable.CreateAsset<GameSettings>("Assets/Game/Resources/");
    }
#endif

    [SerializeField]
    private string _gameID;
    public static string gameID => instance._gameID;

    [SerializeField]
    private bool _isVibrate;
    public static bool isVibrate => instance._isVibrate;

    [SerializeField]
    private bool _isDebug;
    public static bool isDebug => instance._isDebug;
}
