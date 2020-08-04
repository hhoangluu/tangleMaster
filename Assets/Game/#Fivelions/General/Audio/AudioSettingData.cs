using System;

namespace AudioSettingsDatabase
{
    [Serializable]
    public class AudioSetting
    {
        public bool mute = false;
        public float volumn = 1f;

        public AudioSetting(bool mute, float volumn)
        {
            this.mute = mute;
            this.volumn = volumn;
        }
    }

    public class AudioSettingData : Serializable<AudioSettingData>
    {
        public AudioSetting musicSettings = new AudioSetting(false, AudioConfig.musicVol);
        public AudioSetting sfxSettings = new AudioSetting(false, AudioConfig.sfxVol);
    }
}
