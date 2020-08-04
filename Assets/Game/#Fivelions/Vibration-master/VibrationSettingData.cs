namespace VibrationSettingsDatabase
{
    public class VibrationSettingData : Serializable<VibrationSettingData>
    {
        public bool isVibrate = GameSettings.isVibrate;
    }
}
