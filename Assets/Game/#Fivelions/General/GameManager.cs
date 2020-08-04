using UnityEngine;

public class GameManager : DontDestroyOnLoadSingleton<GameManager>
{
    public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    private void Start()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
