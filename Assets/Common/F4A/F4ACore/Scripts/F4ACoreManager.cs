/***************************************************
* This file was created by F4A
*****************************************************/
namespace com.F4A.MobileThird
{
    using System.Collections;

    using UnityEngine;
    using System.IO;
    using System;
    using Newtonsoft.Json;
	using UnityEngine.UI;
#if UNITY_2018_3_OR_NEWER
    using UnityEngine.Networking;
    using DG.Tweening;
#endif

    [Serializable]
	public class F4AConfigData 
    {
        public bool IsDebug = false;
        public int TargetFrameRate = 60;
        public bool IsShowTextVersion = false;
        public bool IsNeverSleep = true;

        public string idOrNameApp = "";
        public string urlAssetBundle = "";
        public int versionAssetBundle = 1;
        public string urlConfigAds = "";
        public int versionAds = 1;
        public string urlConfigWebView = "";
        public int versionWebView = 1;
        public string urlInAppPurchase = "";
        public int versionIAP = 1;
        public string urlSocial = "";
        public int versionSocial = 1;
        public string urlConfigSms = "";
    }

    [Serializable]
    public class DMCIpComponent
    {
        public string country = "None";
        public string countryCode = string.Empty;
        public string city = string.Empty;
        public string lat = string.Empty;
        public string lon = string.Empty;

        [JsonIgnore]
        public string deviceUniqueIdentifier = string.Empty;
        [JsonIgnore]
        public string deviceModel = string.Empty;
    }


    [AddComponentMenu("F4A/F4ACoreManager")]
    public class F4ACoreManager : SingletonMonoDontDestroy<F4ACoreManager>
    {
        #region Constants, Events, Delegates
        private const string KeyF4AConfig = "F4AConfig";

	    public static event Action<F4AConfigData, bool> OnDownloadF4AConfigCompleted = delegate { };
        #endregion

        #region Vairables Define
        /// <summary>
        /// ADS
        /// </summary>
        [Header("Define")]
        [SerializeField]
        private bool _defineAdColony;
        public bool DefineAdColony
        {
            get { return _defineAdColony; }
            set { _defineAdColony = value; }
        }

        [SerializeField]
        private bool _defineAdMob;
        public bool DefineAdMob
        {
            get { return _defineAdMob; }
            set { _defineAdMob = value; }
        }

        [SerializeField]
        private bool _defineAdNative;
        public bool DefineAdNative
        {
            get { return _defineAdNative; }
            set { _defineAdNative = value; }
        }

        [SerializeField]
        private bool _defineUnityAds;
        public bool DefineUnityAds
        {
            get { return _defineUnityAds; }
            set { _defineUnityAds = value; }
        }

        [SerializeField]
        private bool _defineCharstboost;
        public bool DefineCharstboost
        {
            get { return _defineCharstboost; }
            set { _defineCharstboost = value; }
        }

        [SerializeField]
        private bool _defineVungle;
        public bool DefineVungle
        {
            get { return _defineVungle; }
            set { _defineVungle = value; }
        }

        [SerializeField]
        private bool _defineIronSource;
        public bool DefineIronSource
        {
            get { return _defineIronSource; }
            set { _defineIronSource = value; }
        }

        [SerializeField]
        private bool _defineFacebookAds;
        public bool DefineFacebookAds
        {
            get { return _defineFacebookAds; }
            set { _defineFacebookAds = value; }
        }

        /// <summary>
        /// 
        /// </summary>

        [Space]
        [SerializeField]
        private bool _defineIAP;
        public bool DefineIAP
        {
            get { return _defineIAP; }
            set { _defineIAP = value; }
        }

        [SerializeField]
        private bool _defineFacebookSDK;
        public bool DefineFacebookSDK
        {
            get { return _defineFacebookSDK; }
            set { _defineFacebookSDK = value; }
        }

        [SerializeField]
        private bool _defineGameServices;
        public bool DefineGameServices
        {
            get { return _defineGameServices; }
            set { _defineGameServices = value; }
        }

        [SerializeField]
        private bool _defineUnityAnalytic;
        public bool DefineUnityAnalytic
        {
            get { return _defineUnityAnalytic; }
            set { _defineUnityAnalytic = value; }
        }

        [Space]
        [SerializeField]
        private bool _defineFirebaseAnalytic;
        public bool DefineFirebaseAnalytic
        {
            get { return _defineFirebaseAnalytic; }
            set { _defineFirebaseAnalytic = value; }
        }

        [SerializeField]
        private bool _defineFirebaseCrashlytic;
        public bool DefineFirebaseCrashlytic
        {
            get { return _defineFirebaseCrashlytic; }
            set { _defineFirebaseCrashlytic = value; }
        }

        [SerializeField]
        private bool _defineFirebaseMessaging;
        public bool DefineFirebaseMessaging
        {
            get { return _defineFirebaseMessaging; }
            set { _defineFirebaseMessaging = value; }
        }

        [SerializeField]
        private bool _defineFirebaseAuth;
        public bool DefineFirebaseAuth
        {
            get { return _defineFirebaseAuth; }
            set { _defineFirebaseAuth = value; }
        }

        [SerializeField]
        private bool _defineFirebaseRemote;
        public bool DefineFirebaseRemote
        {
            get { return _defineFirebaseRemote; }
            set { _defineFirebaseRemote = value; }
        }


        [Space]
        [SerializeField]
        private bool _defineAppflyer;
        public bool DefineAppflyer
        {
            get { return _defineAppflyer; }
            set { _defineAppflyer = value; }
        }

        [SerializeField]
        private bool _defineMixPanel;
        public bool DefineMixPanel
        {
            get { return _defineMixPanel; }
            set { _defineMixPanel = value; }
        }

        #endregion

        #region Variables
        [Header("Core")]
        public bool IsGetConfigOnline = true;
        [SerializeField]
        private bool _isRunInBackground = true;

        [SerializeField]
        private string urlConfig = "";
        [SerializeField]
        private TextAsset textConfigDefault = null;
		
        [SerializeField]
        private F4AConfigData configData;
	    public F4AConfigData ConfigData
        {
	        get { return configData; }
	        set { configData = value; }
        }

        [SerializeField]
        private F4AEResolutionDevice resolutionDevice = F4AEResolutionDevice.None;
        public F4AEResolutionDevice ResolutionDevice
        {
            get { return resolutionDevice; }
        }

        [Header("UI")]
	    [SerializeField]
	    private Text versionText = null;

        [SerializeField]
        private GameObject toastPopup = null;
        [SerializeField]
	    private Text toastText = null;
	    [SerializeField]
	    private RateUsPanel rateUsPanel = null;

        public GameObject AdsBannerShape;

        public DMCIpComponent ipComponent = new DMCIpComponent();
        #endregion

        #region Unity Methods
        private void Start()
		{
            GetResolutionDevice();

            if (toastPopup) toastPopup.SetActive(false);
			if(rateUsPanel) rateUsPanel.HidePanel();
            Application.runInBackground = _isRunInBackground;

            StartCoroutine(AsyncDownloadF4AConfig());

            if (AdsBannerShape) AdsBannerShape.SetActive(false);

            Debug.Log("@LOG F4A id Device:" + SystemInfo.deviceUniqueIdentifier);
		}

#if UNITY_EDITOR
        private void Update()
        {
            // Capture screen
            if (Input.GetKeyDown(KeyCode.C))
            {
                CaptureScreen();
            }
        }
#endif

        private void OnDestroy()
        {
        }
        #endregion

        #region Methods
        protected override void Initialization()
        {
            StartCoroutine(IERequestToIpAPI());
        }

        private IEnumerator IERequestToIpAPI()
        {
#if UNITY_2018_3_OR_NEWER
            UnityWebRequest www = UnityWebRequest.Get(@"http://ip-api.com/json");
            www.SendWebRequest();
#else
            WWW www = new WWW(worldClockUrl);
#endif

            while (!www.isDone)
            {
                yield return null;
            }

            if (!string.IsNullOrEmpty(www.error))
            {
            }
            else
            {
#if UNITY_2018_3_OR_NEWER
                var info = www.downloadHandler.text;
#else
                var info = www.text;
#endif
                Debug.Log("@LOG IERequestToIpAPI info:" + info);
                ipComponent = JsonConvert.DeserializeObject<DMCIpComponent>(info);

                ipComponent.deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
                ipComponent.deviceModel = SystemInfo.deviceModel;
            }

            yield return null;
        }

        public F4AEResolutionDevice GetResolutionDevice()
        {
            int wScreen = Screen.width;
            int hScreen = Screen.height;
            int rateResolution = 1000;
            int ratio = rateResolution * wScreen / hScreen;
            Debug.Log("ration: " + ratio + "/wScreen:" + wScreen + "/hScreen: " + hScreen);
            if (ratio == rateResolution * 480 / 800)
            {
                resolutionDevice = F4AEResolutionDevice.Resolution_480_800;
            }
            else if (ratio == rateResolution * 640 / 1136)
            {
                resolutionDevice = F4AEResolutionDevice.Resolution_640_1136;
            }
            else if (ratio == rateResolution * 800 / 1280)
            {
                resolutionDevice = F4AEResolutionDevice.Resolution_800_1280;
            }
            //else if (ratio == rateResolution * 750 / 1334)
            //{
            //    resolutionDevice = F4AEResolutionDevice.Resolution_750_1334;
            //}
            //else if (ratio == rateResolution * 1080 / 1920)
            //{
            //    resolutionDevice = F4AEResolutionDevice.Resolution_1080_1920;
            //}
            else if (ratio == rateResolution * 1125 / 2436)
            {
                resolutionDevice = F4AEResolutionDevice.iPhoneX_1125_2436;
            }
            else if (ratio == rateResolution * 828 / 1792)
            {
                resolutionDevice = F4AEResolutionDevice.iPhoneXR_828_1792;
            }
            else if (ratio == rateResolution * 1242 / 2688)
            {
                resolutionDevice = F4AEResolutionDevice.iPhoneXSMax_1242_2688;
            }
            else if (ratio == rateResolution * 1242 / 2208)
            {
                resolutionDevice = F4AEResolutionDevice.iPhone7_Plus_5_5_1242_2208;
            }
            else if (ratio == rateResolution * 2048 / 2732)
            {
                resolutionDevice = F4AEResolutionDevice.iPadPro_12_9_2048_2732;
            }
            else
            {
                resolutionDevice = F4AEResolutionDevice.None;
            }
			return resolutionDevice;
        }
        
        protected IEnumerator AsyncDownloadF4AConfig()
	    {
            yield return new WaitForSeconds(1);

		    Debug.Log("AsyncDownloadF4AConfig url: " + urlConfig);

            if (versionText != null && BuildManager.Instance)
            {
                versionText.text = BuildManager.Instance.GetVersion();
            }

            if(IsGetConfigOnline)
            {
                StartCoroutine(DMCMobileUtils.AsyncGetDataFromUrl(urlConfig, textConfigDefault, (string data) =>
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        var config = JsonConvert.DeserializeObject<F4AConfigData>(data);
                        ConfigData = config;
                        OnDownloadF4AConfigCompleted?.Invoke(ConfigData, true);
                    }
                    else
                    {
                        OnDownloadF4AConfigCompleted?.Invoke(ConfigData, false);
                    }
                    SetupCore();
                }));
            }
            else
            {
                OnDownloadF4AConfigCompleted?.Invoke(ConfigData, false);
                SetupCore();
            }
        }

        private void SetupCore()
        {
#if UNITY_2018_1_OR_NEWER
            Debug.unityLogger.logEnabled = configData.IsDebug;
#endif
            if (versionText) versionText.gameObject.SetActive(ConfigData.IsShowTextVersion);
            Application.targetFrameRate = ConfigData.TargetFrameRate;
            if (ConfigData.IsNeverSleep) Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void Toast(string message)
        {
            Toast(message, true);
        }

        private Coroutine coroutine = null;
        public void Toast(string message, bool shortDuration)
        {
#if UNITY_ANDROID
            AndroidNativeFunctions.ShowToast(message, shortDuration);
#elif UNITY_IOS

#endif
        }

        IEnumerator IEShowToast(string message, bool shortDuration){
	    	toastPopup.SetActive(true);
		    toastText.text = message;
            yield return new WaitForSeconds(shortDuration ? 2 : 4);
		    toastPopup.SetActive(false);
	    }
	    
	    public void ShowRateUsPanel(){
	    	if(rateUsPanel){
	    		rateUsPanel.ShowPanel();
	    	}
	    }

        private void CaptureScreen()
        {
#if UNITY_EDITOR
            string path = Application.dataPath;
            int index = path.LastIndexOf("/", StringComparison.Ordinal);
            path = path.Substring(0, index);
            path += "/Captures";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            System.DateTime now = System.DateTime.Now;
            string strFolderOfNow = string.Format("{0:0000}{1:00}{2:00}", now.Year, now.Month, now.Day);
            path += "/" + strFolderOfNow;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += "/ScreenShot_" + string.Format("{0:00}{1:00}{2:00}", now.TimeOfDay.Hours, now.TimeOfDay.Minutes, now.TimeOfDay.Seconds) + ".png";

#if UNITY_5_6
            Application.CaptureScreenshot(path);
#elif UNITY_2017 || UNITY_2018
            ScreenCapture.CaptureScreenshot(path);
#else
            ScreenCapture.CaptureScreenshot(path);
#endif

#endif
        }
#endregion

#region Editor Methods
        public void AddAdsManager()
        {
            AdsManager manager = transform.GetComponentInChildren<AdsManager>();
            if (manager == null)
            {
                GameObject component = new GameObject();
                component.transform.SetParent(transform);
                component.AddComponent<AdsManager>();
                component.name = "AdsManager";
            }
        }

        public void AddSocialManager()
        {
            var adsManager = transform.GetComponentInChildren<SocialManager>();
            if (adsManager == null)
            {
                GameObject component = new GameObject();
                component.transform.SetParent(transform);
                component.AddComponent<SocialManager>();
                component.name = "SocialManager";
            }
        }

        public void AddIAPManager()
        {
            var manager = transform.GetComponentInChildren<IAPManager>();
            if (manager == null)
            {
                GameObject component = new GameObject();
                component.transform.SetParent(transform);
                component.AddComponent<IAPManager>();
                component.name = "IAPManager";
            }
        }

        public void AddBuildManager()
        {
            var manager = transform.GetComponentInChildren<BuildManager>();
            if (manager == null)
            {
                GameObject component = new GameObject();
                component.transform.SetParent(transform);
                component.AddComponent<BuildManager>();
                component.name = "BuildManager";
            }
        }

        public void AddShephertzManager()
        {
            var manager = transform.GetComponentInChildren<ShephertzManager>();
            if (manager == null)
            {
                GameObject component = new GameObject();
                component.transform.SetParent(transform);
                component.AddComponent<ShephertzManager>();
                component.name = "ShephertzManager";
            }
        }

        public void AddPlayFabManager()
        {
            var manager = transform.GetComponentInChildren<PlayFabManager>();
            if (manager == null)
            {
                GameObject component = new GameObject();
                component.transform.SetParent(transform);
                component.AddComponent<PlayFabManager>();
                component.name = "PlayFabManager";
            }
        }

        //		public void AddAudioManager(){
        //			var adsManager = transform.GetComponentInChildren<AudioManager> ();
        //			if (adsManager == null) {
        //				GameObject component = new GameObject ();
        //				component.transform.SetParent (transform);
        //				component.AddComponent<AudioManager> ();
        //				component.name = "AudioManager";
        //			}
        //		}

        public void AddAllMobileThird()
        {
            AddAdsManager();
            AddSocialManager();
            AddIAPManager();
            AddBuildManager();
            AddShephertzManager();
        }

#if UNITY_EDITOR
        [ContextMenu("Save Info")]
        public void SaveInfo()
        {
            var str = JsonConvert.SerializeObject(ConfigData);
            string pathDirectory = Path.Combine(Application.dataPath, @"Common/Data");
            DMCMobileUtils.CreateDirectory(pathDirectory);
            string path = Path.Combine(pathDirectory, @"CoreInfo.txt");
            StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(str);
            file.Close();

            UnityEditor.AssetDatabase.Refresh();
        }

        [ContextMenu("Load Info")]
        public void LoadInfo()
        {
            string path = Application.dataPath + "/Common/Data/CoreInfo.txt";

            string text = System.IO.File.ReadAllText(path);
            ConfigData = JsonConvert.DeserializeObject<F4AConfigData>(text);

            UnityEditor.AssetDatabase.Refresh();
        }
#endif

#endregion
    }
}