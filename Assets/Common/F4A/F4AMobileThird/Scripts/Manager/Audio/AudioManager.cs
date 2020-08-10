using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

namespace com.F4A.MobileThird
{
    [System.Serializable]
    public class AudioInfo
    {
        public string KeyAudio = "";
        public AudioClip Source;
    }

    [Serializable]
    public class AudioReachedEventArgs{
        public Sprite spriteOn, spriteOff;
    }

    [System.Serializable]
    public class DMCListAudioClipInfo
    {
        [SerializeField]
        private string _nameListAudio;
        public string NameListAudio
        {
            get { return _nameListAudio; }
            set { _nameListAudio = value; }
        }
        [SerializeField]
        private List<AudioClip> _listAudioClip = new List<AudioClip>();
        public List<AudioClip> ListAudioClip
        {
            get { return _listAudioClip; }
        }
    }

    [AddComponentMenu("F4A/AudioManager")]
    public class AudioManager : SingletonMonoDontDestroy<AudioManager>
    {
        // AudioReachedEventArgs -> args, bool -> isMute
        public static event Action<AudioReachedEventArgs> OnChangeSfxValue = delegate { };
        // AudioReachedEventArgs -> args, bool -> isMute
        public static event Action<AudioReachedEventArgs> OnChangeMusicValue = delegate { };
        public static event Action OnChangeVibrate = delegate { };

        [Header("--Action: OnChangeSfxValue(AudioReachedEventArgs,bool isMute)")]
        [Header("--Action: OnChangeMusicValue(AudioReachedEventArgs,bool isMute)")]

        [SerializeField]
        private List<AudioClip> listSfx = new List<AudioClip>();
        [SerializeField]
        private List<DMCListAudioClipInfo> _listAudioSoundInfo = new List<DMCListAudioClipInfo>();

        [SerializeField]
        private List<AudioClip> listMusic = new List<AudioClip>();
        [SerializeField]
        private AudioSource sfxSource = null;
        [SerializeField]
        private AudioSource musicSource = null;

        [SerializeField]
        private AudioReachedEventArgs sfxReachedEventArgs = null;
        [SerializeField]
        private AudioReachedEventArgs musicReachedEventArgs = null;

        [SerializeField]
        private bool isOneButtonSelect = false;
        [SerializeField]
        private bool isEnableSfx = true;
        public bool IsEnableSfx
        {
            get
            {
                //isEnableSfx = CPlayerPrefs.GetBool(F4AUtils.KeySfx, true);
                return isEnableSfx;
            }
            set
            {
                isEnableSfx = value;
                sfxSource.mute = !isEnableSfx;
                //PlayerPrefs.SetInt(F4AUtils.KeySfx, isEnableSfx ? 1 : 0);
                CPlayerPrefs.SetBool(DMCMobileUtils.KeySfx, isEnableSfx);
                OnChangeSfxValue?.Invoke(sfxReachedEventArgs);
                //SaveLoadDataManager.Instance.DataController.CoreDataInfo.IsEnableSound = value;

                if (isOneButtonSelect) IsEnableMusic = value;
            }
        }

        [SerializeField]
        private bool isEnableMusic = true;
        public bool IsEnableMusic
        {
            get
            {
                //isEnableMusic = CPlayerPrefs.GetBool(F4AUtils.KeyMusic, true);
                return isEnableMusic;
            }
            set
            {
                isEnableMusic = value;
                musicSource.mute = !isEnableMusic;
                //PlayerPrefs.SetInt(F4AUtils.KeyMusic, isEnableMusic ? 1 : 0);
                CPlayerPrefs.SetBool(DMCMobileUtils.KeyMusic, isEnableMusic);
                OnChangeMusicValue?.Invoke(musicReachedEventArgs);
                //SaveLoadDataManager.Instance.DataController.CoreDataInfo.IsEnableMusic = value;
            }
        }

        [SerializeField]
        private bool _isEnableVibrate = true;
        public bool IsEnableVibrate
        {
            get { return _isEnableVibrate; }
            set
            {
                _isEnableVibrate = value;
                CPlayerPrefs.SetBool(DMCMobileUtils.KeyVibrate, value);
                if (OnChangeVibrate != null) OnChangeVibrate();
                //SaveLoadDataManager.Instance.DataController.CoreDataInfo.IsEnableVibrate = value;
            }
        }

        protected override void Initialization()
        {
            RefreshAudio();
        }

        public void RefreshAudio()
        {
            IsEnableSfx = CPlayerPrefs.GetBool(DMCMobileUtils.KeySfx, true);
            IsEnableMusic = CPlayerPrefs.GetBool(DMCMobileUtils.KeyMusic, true);
            IsEnableVibrate = CPlayerPrefs.GetBool(DMCMobileUtils.KeyVibrate, true);
        }

        public void PlaySfx(string sfxName, float volume, bool isOneShot = true)
        {
            AudioClip clip = listSfx.Where(sfx => sfx.name.Equals(sfxName)).FirstOrDefault();
            if (!clip)
            {
                foreach (DMCListAudioClipInfo info in _listAudioSoundInfo)
                {
                    clip = info.ListAudioClip.Where(sfx => sfx.name.Equals(sfxName)).FirstOrDefault();
                    if (clip)
                    {
                        break;
                    }
                }
            }
            if (clip) PlaySfx(clip, volume, isOneShot);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfxName"></param>
        /// <param name="isOneShot"></param>
        public void PlaySfx(string sfxName, bool isOneShot = true)
        {
            PlaySfx(sfxName, 1, isOneShot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="isOneShot"></param>
        public void PlaySfx(AudioClip clip, float volume = 1, bool isOneShot = true)
        {
            if (sfxSource)
            {
                sfxSource.volume = volume;

                if (isOneShot)
                {
                    sfxSource.loop = false;
                    sfxSource.PlayOneShot(clip);
                }
                else
                {
                    sfxSource.clip = clip;
                    sfxSource.loop = true;
                    sfxSource.Play();
                }
            }
        }

        public AudioClip GetAudioMusic(string musicName)
        {
            var clip = listMusic.Where(sfx => sfx.name.Equals(musicName)).FirstOrDefault();
            return clip;
        }

        public bool IsReadyMusic(string musicName)
        {
            return GetAudioMusic(musicName) != null;
        }

        public float GetLenghtMusic(string musicName)
        {
            var clip = GetAudioMusic(musicName);
            if (clip) return clip.length;
            else return -1;
        }

        public void PlayMusic(string musicName, bool isLoop = true, bool isContinue = true)
        {
            if(!isContinue || !musicSource.clip || (isContinue && !musicSource.clip.name.Equals(musicName)))
            {
                musicSource.Stop();
                var clip = listMusic.Where(sfx => sfx.name.Equals(musicName)).FirstOrDefault();
                if (clip)
                {
                    musicSource.clip = clip;
                    musicSource.loop = isLoop;
                    musicSource.Play();
                }
            }
        }


        public void PauseMusic()
        {
            if (musicSource) musicSource.Pause();
        }

        public void StopMusic()
        {
            if (musicSource) musicSource.Stop();
        }

        public void SetPitchMusic(float pitch)
        {
            if(musicSource)
            {
                //Debug.Log("SetPitchMusic pitch:" + musicSource.pitch);
                musicSource.pitch = pitch;
            }
        }

        public void SetVolumeMusic(float volume)
        {
            if (musicSource) musicSource.volume = volume;
        }

        public float GetVolumeMusic()
        {
            if (musicSource) return musicSource.volume;
            else return 0;
        }

        public bool IsPlayMusic()
		{
            return musicSource.isPlaying;
		}

        public float GetTimeMusicCurrent()
        {
            return musicSource.time;
        }

        public void AddAudioClipMusic(AudioClip clip)
        {
            listMusic.Add(clip);
        }

        public void SwitchMusicVolume()
        {
            this.IsEnableMusic = !this.IsEnableMusic;
            //SetMusicSource(null);
        }

        public void SwitchMusicVolume(params Image[] images)
        {
            this.IsEnableMusic = !this.IsEnableMusic;
            if (images != null && images.Length > 0)
            {
                foreach (var img in images)
                {
                    img.sprite = this.IsEnableMusic ? musicReachedEventArgs.spriteOn : musicReachedEventArgs.spriteOff;
                }
            }
        }

        public void SwitchSfxVolume()
        {
            this.IsEnableSfx = !this.IsEnableSfx;
            //SetSfxSource(null);
        }

        public void SwitchSfxVolume(params Image[] images)
        {
            this.IsEnableSfx = !this.IsEnableSfx;
            if (images != null && images.Length > 0)
            {
                foreach (var img in images)
                {
                    img.sprite = this.IsEnableSfx ? sfxReachedEventArgs.spriteOn : sfxReachedEventArgs.spriteOff;
                }
            }
        }

        public void PlayVibrate()
        {
            if (IsEnableVibrate) Handheld.Vibrate();
        }

        [ContextMenu("ReloadAudio")]
        public void ReloadAudio()
        {
            foreach(var v in listSfx)
            {
            }
        }

        public void SetupAudioSource()
        {
            var childs = transform.GetComponentsInChildren<AudioSource>();
            foreach(var child in childs)
            {
                DestroyImmediate(child.gameObject);
            }

            GameObject sfx = new GameObject("sfx");
            sfx.transform.parent = transform;
            sfxSource = sfx.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;

            GameObject music = new GameObject("music");
            music.transform.parent = transform;
            musicSource = music.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
        }
    }
}