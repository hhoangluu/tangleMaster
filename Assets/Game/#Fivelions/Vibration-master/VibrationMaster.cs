using UnityEngine;
using VibrationSettingsDatabase;

public static class VibrationMaster
{
    public static bool isVibrate
    {
        get { return VibrationSettingData.instance.isVibrate; }
        set { VibrationSettingData.instance.isVibrate = value; VibrationSettingData.Save(); }
    }

    public static void Vibrate(long milliseconds)
    {
#if UNITY_EDITOR
        Debug.Log("Bzzzt! Cool Vibrate: " + milliseconds);
        return;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        if (isVibrate)
            Vibration.Vibrate(milliseconds);
        return;
#endif

#if UNITY_IPHONE
        if (isVibrate)
            Vibration.Vibrate();
        return;
#endif
    }
}
