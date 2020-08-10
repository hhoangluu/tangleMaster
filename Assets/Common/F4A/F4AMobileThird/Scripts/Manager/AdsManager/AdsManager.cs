using UnityEngine;
using System.Collections;

#if DEFINE_ADMOB
using GoogleMobileAds.Api;
#endif

#if DEFINE_CHARTBOOST
using ChartboostSDK;
#endif

using System;
using Newtonsoft.Json;
using System.IO;
using DG.Tweening;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace com.F4A.MobileThird
{
    #region Classes

    #region AdMob
    [System.Serializable]
    public class AdmobConfigData
    {
        [SerializeField]
        private string _version;
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        [SerializeField]
        private bool _isEnableAd = true;
        public bool IsEnableAd
        {
            get { return _isEnableAd; }
            set { _isEnableAd = value; }
        }

        [SerializeField]
        private DMCAdmobAppInfo[] _androidAppInfos = new DMCAdmobAppInfo[1];
        public DMCAdmobAppInfo[] AndroidAppInfos
        {
            get { return _androidAppInfos; }
            set { _androidAppInfos = value; }
        }

        [SerializeField]
        private DMCAdmobAppInfo[] _iosAppInfos = new DMCAdmobAppInfo[1];
        public DMCAdmobAppInfo[] IosAppInfos
        {
            get { return _iosAppInfos; }
            set { _iosAppInfos = value; }
        }

        [SerializeField]
        private bool _forChild;
        public bool ForChild
        {
            get { return _forChild; }
            set { _forChild = value; }
        }


        public DMCAdmobAppInfo GetAppInfo()
        {
#if UNITY_ANDROID
            if (AndroidAppInfos != null && AndroidAppInfos.Length > 0)
            {
                return AndroidAppInfos[Random.Range(0, AndroidAppInfos.Length)];
            }
#elif UNITY_IOS
            if (IosAppInfos != null && IosAppInfos.Length > 0)
            {
                return IosAppInfos[Random.Range(0, IosAppInfos.Length)];
            }
#endif
            return null;
        }

        public string GetAppId()
        {
            DMCAdmobAppInfo appInfo = GetAppInfo();
            if (appInfo != null)
            {
                return appInfo.AppId;
            }
            return "";
        }

        public string GetBannerId()
        {
            DMCAdmobAppInfo appInfo = GetAppInfo();
            if (appInfo != null)
            {
                return appInfo.BannerId;
            }
            return "";
        }

        public string GetInterstitialId()
        {
            DMCAdmobAppInfo appInfo = GetAppInfo();
            if (appInfo != null)
            {
                return appInfo.InterstitialId;
            }
            return "";
        }

        public string GetRewardId()
        {
            DMCAdmobAppInfo appInfo = GetAppInfo();
            if (appInfo != null)
            {
                return appInfo.RewardId;
            }
            return "";
        }

        public string GetNativeId()
        {
            DMCAdmobAppInfo appInfo = GetAppInfo();
            if (appInfo != null)
            {
                return appInfo.NativeAdvancedId;
            }
            return string.Empty;
        }
    }

    [System.Serializable]
    public class DMCAdmobAppInfo
    {
        [SerializeField]
        private string _appName = "_appName";
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        [SerializeField]
        private string _appId = "ca-app-pub-3940256099942544~3347511713";
        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        [SerializeField]
        private string _bannerName = "_bannerName";
        public string BannerName
        {
            get { return _bannerName; }
            set { _bannerName = value; }
        }

        [SerializeField]
        private string _bannerId = "ca-app-pub-3940256099942544/6300978111";
        public string BannerId
        {
            get { return _bannerId; }
            set { _bannerId = value; }
        }

        [SerializeField]
        private string _interstitialName = "_interstitialName";
        public string InterstitialName
        {
            get { return _interstitialName; }
            set { _interstitialName = value; }
        }

        [SerializeField]
        private string _interstitialId = "ca-app-pub-3940256099942544/1033173712";
        public string InterstitialId
        {
            get { return _interstitialId; }
            set { _interstitialId = value; }
        }

        [SerializeField]
        private string _interstitialVideoName = "_interstitialVideoName";
        public string InterstitialVideoName
        {
            get { return _interstitialVideoName; }
            set { _interstitialVideoName = value; }
        }
        [SerializeField]
        private string _interstitialVideoId = "ca-app-pub-3940256099942544/8691691433";
        public string InterstitialVideoId
        {
            get { return _interstitialVideoId; }
            set { _interstitialVideoId = value; }
        }

        [SerializeField]
        private string _rewardName = "_rewardName";
        public string RewardName
        {
            get { return _rewardName; }
            set { _rewardName = value; }
        }

        [SerializeField]
        private string _rewardId = "ca-app-pub-3940256099942544/5224354917";
        public string RewardId
        {
            get { return _rewardId; }
            set { _rewardId = value; }
        }

        [SerializeField]
        private string _nativeAdvancedName = "_nativeAdvancedName";
        public string NativeAdvancedName
        {
            get { return _nativeAdvancedName; }
            set { _nativeAdvancedName = value; }
        }
        [SerializeField]
        private string _nativeAdvancedId = "ca-app-pub-3940256099942544/2247696110";
        public string NativeAdvancedId
        {
            get { return _nativeAdvancedId; }
            set { _nativeAdvancedId = value; }
        }


        [SerializeField]
        private string _nativeAdvancedVideoName = "_nativeAdvancedVideoName";
        public string NativeAdvancedVideoName
        {
            get { return _nativeAdvancedVideoName; }
            set { _nativeAdvancedVideoName = value; }
        }
        [SerializeField]
        private string _nativeAdvancedVideoId = "ca-app-pub-3940256099942544/1044960115";
        public string NativeAdvancedVideoId
        {
            get { return _nativeAdvancedVideoId; }
            set { _nativeAdvancedVideoId = value; }
        }

        public DMCAdmobAppInfo()
        {
            _appId = "ca-app-pub-3940256099942544~3347511713";
            _bannerId = "ca-app-pub-3940256099942544/6300978111";
            _interstitialId = "ca-app-pub-3940256099942544/1033173712";
            _interstitialVideoId = "ca-app-pub-3940256099942544/8691691433";
            _rewardId = "ca-app-pub-3940256099942544/5224354917";
            _nativeAdvancedId = "ca-app-pub-3940256099942544/2247696110";
            _nativeAdvancedVideoId = "ca-app-pub-3940256099942544/1044960115";
        }
    }
    #endregion

    #region Unity Ads
    [System.Serializable]
    public class UnityAdConfigData
    {
        public bool enableAd = true;

        public bool testMode = false;

        [SerializeField]
        private DMCUnityAdsAppInfo[] _androidAppInfos = new DMCUnityAdsAppInfo[1];
        public DMCUnityAdsAppInfo[] AndroidAppInfos
        {
            get { return _androidAppInfos; }
            set { _androidAppInfos = value; }
        }

        [SerializeField]
        private DMCUnityAdsAppInfo[] _iosAppInfos = new DMCUnityAdsAppInfo[1];
        public DMCUnityAdsAppInfo[] IosAppInfos
        {
            get { return _iosAppInfos; }
            set { _iosAppInfos = value; }
        }

        public DMCUnityAdsAppInfo GetAppInfo()
        {
#if UNITY_ANDROID
            if (AndroidAppInfos != null && AndroidAppInfos.Length > 0)
            {
                return AndroidAppInfos[Random.Range(0, AndroidAppInfos.Length)];
            }
#elif UNITY_IOS
            if(IosAppInfos != null && IosAppInfos.Length > 0)
            {
                return IosAppInfos[Random.Range(0, IosAppInfos.Length)];
            }
#endif
            return null;

        }

        public string GetIdAd()
        {
            DMCUnityAdsAppInfo app = GetAppInfo();
            if (app != null)
            {
                return app.IdAd;
            }
            return "";
        }
    }

    [Serializable]
    public class DMCUnityAdsAppInfo
    {
        [SerializeField]
        private string _nameApp;
        public string NameApp
        {
            get { return _nameApp; }
            set { _nameApp = value; }
        }

        [SerializeField]
        private string _idApp;
        public string IdApp
        {
            get { return _idApp; }
            set { _idApp = value; }
        }

        [SerializeField]
        private string _idAd;
        public string IdAd
        {
            get { return _idAd; }
            set { _idAd = value; }
        }

        [SerializeField]
        private string _bannerPlacementId = "banner";
        public string BannerPlacementId
        {
            get { return _bannerPlacementId; }
            set { _bannerPlacementId = value; }
        }

        [SerializeField]
        private string _interstitialPlacementId = "interstitial";
        public string InterstitialPlacementId
        {
            get { return _interstitialPlacementId; }
            set { _interstitialPlacementId = value; }
        }

        [SerializeField]
        private string _videoPlacementId = "video";
        public string VideoPlacementId
        {
            get { return _videoPlacementId; }
            set { _videoPlacementId = value; }
        }

        [SerializeField]
        private string _rewardPlacementId = "rewardedVideo";
        public string RewardPlacementId
        {
            get { return _rewardPlacementId; }
            set { _rewardPlacementId = value; }
        }

    }
    #endregion

    #region Vungle
    [System.Serializable]
    public class VungleConfigData
    {
        public bool enableAd = false;

        [SerializeField]
        private VungleAppData[] androidApps = { };
        [SerializeField]
        private VungleAppData[] iosApps = { };
        [SerializeField]
        private VungleAppData[] winphoneApps = { };

        private VungleAppData _appCurrent = null;
        private VunglePlacementData _placementCurrent = null;

        private VungleAppData GetApp()
        {
            VungleAppData[] apps = null;
#if UNITY_IOS
            apps = iosApps;
#elif UNITY_ANDROID
            apps = androidApps;
#elif UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
            apps = winphoneApps;
#else
#endif
            _appCurrent = null;
            if (apps != null && apps.Length > 0)
            {
                _appCurrent = apps[Random.Range(0, apps.Length)];
            }
            return _appCurrent;
        }

        public string GetAppId()
        {
            string id = string.Empty;
            var app = GetApp();
            if(app != null)
            {
                id = app.appId;
            }
            return id;
        }

        public string GetPlacementId()
        {
            if (_placementCurrent != null) _placementCurrent.isLoad = false;
             var id = string.Empty;
            if(_appCurrent != null)
            {
                var placements = _appCurrent.placements;
                if(placements != null && placements.Length > 0)
                {
                    _placementCurrent = placements[Random.Range(0, placements.Length)];
                    if (_placementCurrent != null) id = _placementCurrent.id;
                }
            }
            return id;
        }

        public void SetLoadAd(string placementID, bool adPlayable)
        {
            if (_placementCurrent != null && _placementCurrent.id.Equals(placementID))
            {
                _placementCurrent.isLoad = adPlayable;
            }
        }

        public bool IsLoadAd()
        {
            if (_placementCurrent != null) return _placementCurrent.isLoad;
            return false;
        }

        public string GetPlacementIdCurrent()
        {
            if (_placementCurrent != null) return _placementCurrent.id;
            return string.Empty;
        }
    }

    [System.Serializable]
    public class VungleAppData
    {
        public string appId = string.Empty;
        public string appName = string.Empty;
        public VunglePlacementData[] placements;
    }

    [System.Serializable]
    public class VunglePlacementData
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public EAdType adType = EAdType.Rewarded;
        [NonSerialized][JsonIgnore]
        public bool isLoad = false;
    }
    #endregion


    #region Facebook Ads
    [System.Serializable]
    public class FacebookAdConfigData
    {
        public bool enableAd = true;

        public string[] androidBannerIds = { };
        public string[] androidInterstitialIds = { };
        public string[] androidRewardIds = { };

        public string[] iosBannerIds = { };
        public string[] iosInterstitialIds = { };
        public string[] iosRewardIds = { };

        public string GetBannerId()
        {
            string id = string.Empty;
            var ids = androidBannerIds;
#if UNITY_IOS
            ids = iosBannerIds;
#endif
            if (ids != null && ids.Length > 0) id = ids[Random.Range(0, ids.Length)];
            return id;
        }

        public string GetInterstitialId()
        {
            string id = string.Empty;
            var ids = androidInterstitialIds;
#if UNITY_IOS
            ids = iosInterstitialIds;
#endif
            if (ids != null && ids.Length > 0) id = ids[Random.Range(0, ids.Length)];
            return id;
        }

        public string GetRewardId()
        {
            string id = string.Empty;
            var ids = androidRewardIds;
#if UNITY_IOS
            ids = iosRewardIds;
#endif
            if (ids != null && ids.Length > 0) id = ids[Random.Range(0, ids.Length)];
            return id;
        }
    }
#endregion

#region Chartboost
    [System.Serializable]
    public class DMCChartboostConfigData
    {
        [SerializeField]
        private bool _enableAd = true;
        public bool EnableAd
        {
            get { return _enableAd; }
            set { _enableAd = value; }
        }

        [SerializeField]
        private DMCChartboostAppInfo[] _androidApps = new DMCChartboostAppInfo[1];
        public DMCChartboostAppInfo[] AndroidApps
        {
            get { return _androidApps; }
            set { _androidApps = value; }
        }

        [SerializeField]
        private DMCChartboostAppInfo[] _iosApps = new DMCChartboostAppInfo[1];
        public DMCChartboostAppInfo[] IosApps
        {
            get { return _iosApps; }
            set { _iosApps = value; }
        }
    }

    [SerializeField]
    public class DMCChartboostAppInfo
    {
        [SerializeField]
        private string _appId;
        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        [SerializeField]
        private string _appSignature;
        public string AppSignature
        {
            get { return _appSignature; }
            set { _appSignature = value; }
        }

    }
#endregion


#region IronSource
	[System.Serializable]
    public class DMCIronSourceComponent
    {
        public bool enableAd = false;

        [SerializeField]
        private string[] _androidIds = { };
        [SerializeField]
        private string[] _iosIds = { };

        public string GetAppId()
        {
            string[] ids = _androidIds;
#if UNITY_IOS
            ids = _iosIds;
#endif
            string id = string.Empty;
            if (ids != null && ids.Length > 0)
            {
                id = ids[Random.Range(0, ids.Length)];
            }
            return id;
        }
    }
#endregion

#region AdColony
    [System.Serializable]
    public class DMCAdColonyConfigData
    {
        [SerializeField]
        private bool _enableAd = true;
        public bool EnableAd
        {
            get { return _enableAd; }
            set { _enableAd = value; }
        }


        [SerializeField]
        private DMCAdColonyAppInfo[] _androidInfos;
        public DMCAdColonyAppInfo[] AndroidInfos
        {
            get { return _androidInfos; }
            set { _androidInfos = value; }
        }

        [SerializeField]
        private DMCAdColonyAppInfo[] _iosInfos;
        public DMCAdColonyAppInfo[] IosInfos
        {
            get { return _iosInfos; }
            set { _iosInfos = value; }
        }

        public DMCAdColonyAppInfo GetAppInfo()
        {
            DMCAdColonyAppInfo app = null;
            var infos = _androidInfos;
#if UNITY_IOS
            infos = _iosInfos;
#endif
            if (infos != null && infos.Length > 0)
            {
                app = infos[Random.Range(0, infos.Length)];
            }
            return app;
        }

        public List<string> GetConfigApp()
        {
            List<string> config = new List<string>();
            var app = GetAppInfo();
            if (app != null)
            {
                config.Add(app.AppId);

                foreach (var zone in app.Zones)
                {
                    config.Add(zone.ZoneId);
                }
            }
            return config;
        }

        public Dictionary<string, string[]> GetDictionConfigApp()
        {
            Dictionary<string, string[]> config = null;
            var app = GetAppInfo();
            if (app != null && app.Zones.Length > 0)
            {
                config = new Dictionary<string, string[]>();
                string[] zones = new string[app.Zones.Length];
                for (int counter = 0; counter < app.Zones.Length; counter++)
                {
                    zones[counter] = app.Zones[counter].ZoneId;
                }

                config[app.AppId] = zones;
            }
            return config;
        }
    }

    [System.Serializable]
    public class DMCAdColonyAppInfo
    {
        [SerializeField]
        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        [SerializeField]
        private string _appId;
        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        [SerializeField]
        private DMCAdColonyZoneInfo[] _zones;
        public DMCAdColonyZoneInfo[] Zones
        {
            get { return _zones; }
            set { _zones = value; }
        }
    }

    [System.Serializable]
    public class DMCAdColonyZoneInfo
    {
        [SerializeField]
        private string _zoneName;
        public string ZoneName
        {
            get { return _zoneName; }
            set { _zoneName = value; }
        }

        [SerializeField]
        private string _zoneId;
        public string ZoneId
        {
            get { return _zoneId; }
            set { _zoneId = value; }
        }

    }
#endregion

#region MobPub
    [System.Serializable]
    public class DMCMopubAdComponent
    {
        [SerializeField]
        private string _appName = "_appName";
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        [SerializeField]
        private string _appId = "_appId";
        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        [SerializeField]
        private string _bannerName = "_bannerName";
        public string BannerName
        {
            get { return _bannerName; }
            set { _bannerName = value; }
        }

        [SerializeField]
        private string _bannerId = "_bannerId";
        public string BannerId
        {
            get { return _bannerId; }
            set { _bannerId = value; }
        }

        [SerializeField]
        private string _interstitialName = "_interstitialName";
        public string InterstitialName
        {
            get { return _interstitialName; }
            set { _interstitialName = value; }
        }

        [SerializeField]
        private string _interstitialId = "_interstitialId";
        public string InterstitialId
        {
            get { return _interstitialId; }
            set { _interstitialId = value; }
        }

        [SerializeField]
        private string _rewardName = "_rewardName";
        public string RewardName
        {
            get { return _rewardName; }
            set { _rewardName = value; }
        }

        [SerializeField]
        private string _rewardId = "_rewardId";
        public string RewardId
        {
            get { return _rewardId; }
            set { _rewardId = value; }
        }

        [SerializeField]
        private string _nativeAdvancedName = "_nativeAdvancedName";
        public string NativeAdvancedName
        {
            get { return _nativeAdvancedName; }
            set { _nativeAdvancedName = value; }
        }
        [SerializeField]
        private string _nativeAdvancedId = "_nativeAdvancedId";
        public string NativeAdvancedId
        {
            get { return _nativeAdvancedId; }
            set { _nativeAdvancedId = value; }
        }
    }

    [System.Serializable]
    public class DMCMopubConfigComponent
    {
        [SerializeField]
        private DMCMopubAdComponent _androidComponent = new DMCMopubAdComponent();
        public DMCMopubAdComponent AndroidComponent
        {
            get { return _androidComponent; }
            set { _androidComponent = value; }
        }
        [SerializeField]
        private DMCMopubAdComponent _iosComponent = new DMCMopubAdComponent();
        public DMCMopubAdComponent IosComponent
        {
            get { return _iosComponent; }
            set { _iosComponent = value; }
        }
    }
#endregion

    [System.Serializable]
    public class AdDataComponent
    {
        [JsonProperty("name")]
        public string Name;//{get;set;}
        [JsonProperty("priority")]
        public int Priority;//{get;set;}
        [JsonProperty("enable")]
        public bool Enable;
    }

    [System.Serializable]
    public class AdConfigData
    {
        [SerializeField]
        private bool isEnableAds = true;
        public bool IsEnableAds
        {
            get { return isEnableAds; }
            set { isEnableAds = value; }
        }

        [SerializeField]
        private string[] _idDeviceTests = { "id01", "id02" };
        public string[] IdDeviceTests
        {
            get { return _idDeviceTests; }
            set { _idDeviceTests = value; }
        }

        [SerializeField]
        private EAdPosition adPosition = EAdPosition.Bottom;
        public EAdPosition AdPosition
        {
            get { return adPosition; }
            set { adPosition = value; }
        }

        [Header("Ad Network")]
        public ERewardedAdNetwork[] rewardAdOrder = { ERewardedAdNetwork.AdMob, ERewardedAdNetwork.UnityAds, ERewardedAdNetwork.Vungle };
        public EInterstitialAdNetwork[] interstitialAdsOrder = { EInterstitialAdNetwork.AdMob, EInterstitialAdNetwork.UnityAds };
        public EVideoAdNetwork[] videoAdsOrder = { EVideoAdNetwork.UnityAds };
        public EBannerAdNetwork[] bannerAdsOrder = { EBannerAdNetwork.AdMob};

        [Header("Ads")]
        public DMCAdColonyConfigData adColonyConfigData;
        public AdmobConfigData admobConfig;
        public UnityAdConfigData unityAdConfig;
        public VungleConfigData vungleConfig;
        public FacebookAdConfigData facebookAdConfig;
        public DMCChartboostConfigData chartboostConfigData;
        public DMCMopubConfigComponent mopubConfigComponent;
        public DMCIronSourceComponent ironSourceComp;

        [Header("Config")]
        public int timesEventShowInterstitial = 1;
        [Tooltip("Seconds delay show Interstitial")]
        public float timeDalyShowInterstitial = 15;
        public int timeIntervalInterstitial = 15;
        public bool isShowBannerAd = false;
    }


    public class DMCTrackAdData
    {
        private EAdType _adType;
        public EAdType AdType
        {
            get { return _adType; }
            set { _adType = value; }
        }

        private Dictionary<int, DMCTrackAdNetworkData> _adNetwordDatas = new Dictionary<int, DMCTrackAdNetworkData>();
        public Dictionary<int, DMCTrackAdNetworkData> AdNetwordDatas
        {
            get { return _adNetwordDatas; }
            set { _adNetwordDatas = value; }
        }

    }

    public class DMCTrackAdNetworkData
    {
        private EAdNetwork _adNetwork;
        public EAdNetwork AdNetwork
        {
            get { return _adNetwork; }
            set { _adNetwork = value; }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
    }
#endregion

    /// <summary>
    /// A class is manager all ads in project
    /// AdMob, UnityAds, Vungle, Chartboost, StartApp
    /// </summary>
    [AddComponentMenu("F4A/AdsManager")]
    public partial class AdsManager : SingletonMono<AdsManager>
    {
        private const string NameFileAdsInfo = @"AdsInfo.txt";
        private const string KeyVersionAdsConfig = "VERSION_ADS_CONFIG";

#pragma warning disable 414
        public static event Action<bool> OnRemoveAds = delegate { };

        /// <summary>
        /// ERewardedAdNetwork, Type, Amount
        /// </summary>
        public static event Action<ERewardedAdNetwork, string, double> OnRewardedAdCompleted = delegate { };
        public static event Action<ERewardedAdNetwork> OnRewardedAdSkiped = delegate { };
        public static event Action<ERewardedAdNetwork> OnRewardedAdFailed = delegate { };

        public static event Action<EVideoAdNetwork> OnVideodAdCompleted = delegate { };
        public static event Action<EVideoAdNetwork> OnVideodAdFailed = delegate { };

        public static event Action<EInterstitialAdNetwork> OnInterstitialAdClosed = delegate { };
        public static event Action<EInterstitialAdNetwork> OnInterstitialAdFailed = delegate { };

        public static event Action<EBannerAdNetwork> OnBannerAdLoad = delegate { };
        public static event Action<EBannerAdNetwork> OnBannerShow = delegate { };

#region Variables
        [Header("ADS MANAGER")]
        [SerializeField]
        private string urlFileDataSetting = null;
        [SerializeField]
        private TextAsset fileAdConfigDefault = null;
        [SerializeField]
        private AdConfigData adConfigData;

        private int countEventShowInterstitial = 0;
        private float lastTimeShowInterstital = -10000;

        private Dictionary<EAdType, DMCTrackAdData> _dicTrackAdData = new Dictionary<EAdType, DMCTrackAdData>();

        private Vector2 sizeAdsShape = new Vector2(320, 50);

        private float _timeScale = 1;
#endregion

#region Unity Method

        private void OnEnable()
        {
            IAPManager.OnBuyPurchaseSuccessed += IAPManager_OnBuyPurchaseSuccessed;
        }

        private void OnDisable()
        {
            IAPManager.OnBuyPurchaseSuccessed -= IAPManager_OnBuyPurchaseSuccessed;
        }

        // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        private void Start()
        {
#if DEFINE_UNITY_ADS
            StartUnityAds();
#endif

            F4ACoreManager.OnDownloadF4AConfigCompleted += F4ACoreManager_OnDownloadF4AConfigCompleted;

            OnBannerAdLoad += AdsManager_OnBannerAdLoad;
            OnBannerShow += AdsManager_OnBannerShow;

            OnInterstitialAdClosed += AdsManager_OnInterstitialAdClosed;
            OnRewardedAdCompleted += AdsManager_OnRewardedAdCompleted;
            OnRewardedAdSkiped += AdsManager_OnRewardedAdSkiped;
            OnRewardedAdFailed += AdsManager_OnRewardedAdFailed;

            if (CPlayerPrefs.HasKey(DMCMobileUtils.KeyRemoveAdsOld))
            {
                CPlayerPrefs.SetBool(DMCMobileUtils.KeyRemoveAds, CPlayerPrefs.GetBool(DMCMobileUtils.KeyRemoveAdsOld, false));
            }
        }

        private void Update()
        {
            if (F4ACoreManager.Instance.AdsBannerShape && F4ACoreManager.Instance.AdsBannerShape.activeSelf)
            {
                var shape = F4ACoreManager.Instance.AdsBannerShape.GetComponent<RectTransform>();
                shape.sizeDelta = sizeAdsShape;
            }
        }

        protected void OnDestroy()
        {
            F4ACoreManager.OnDownloadF4AConfigCompleted -= F4ACoreManager_OnDownloadF4AConfigCompleted;
            
            OnBannerAdLoad -= AdsManager_OnBannerAdLoad;
            OnBannerShow -= AdsManager_OnBannerShow;

            OnInterstitialAdClosed -= AdsManager_OnInterstitialAdClosed;
            OnRewardedAdCompleted -= AdsManager_OnRewardedAdCompleted;
            OnRewardedAdSkiped -= AdsManager_OnRewardedAdSkiped;
            OnRewardedAdFailed -= AdsManager_OnRewardedAdFailed;
        }
        #endregion

        #region Events
        private void AdsManager_OnBannerAdLoad(EBannerAdNetwork adNetwork)
        {
            if (adConfigData.isShowBannerAd)
            {
                ShowBannerAds();
                FixSizeShapeBanner();
            }
        }

        private void AdsManager_OnBannerShow(EBannerAdNetwork adNetwork)
        {
        }

        private void FixSizeShapeBanner()
        {
            if (F4ACoreManager.Instance.AdsBannerShape)
            {
                StartCoroutine(IEWaitForSeconds(0, () =>
                {
                    F4ACoreManager.Instance.AdsBannerShape.SetActive(true);
                    var shape = F4ACoreManager.Instance.AdsBannerShape.GetComponent<RectTransform>();
                    sizeAdsShape = new Vector2(320, 50);
                    if (Screen.height > 720)
                    {
                        sizeAdsShape = new Vector2(728, 100);
                    }
                    shape.sizeDelta = sizeAdsShape;
                    Debug.Log("@LOG AdsManager size:" + sizeAdsShape.x + "," + sizeAdsShape.y);
                }));
            }
        }

        private void AdsManager_OnInterstitialAdClosed(EInterstitialAdNetwork adNetwork)
        {
            EventsManager.Instance.LogEvent("ads_interstitial_close", "ads_network", adNetwork.ToString());
        }

        private void AdsManager_OnRewardedAdFailed(ERewardedAdNetwork adNetwork)
        {
            EventsManager.Instance.LogEvent("ads_reward_end", new Dictionary<string, string>()
            {{"ads_network", adNetwork.ToString() },{"status","failed" } });
        }

        private void AdsManager_OnRewardedAdSkiped(ERewardedAdNetwork adNetwork)
        {
            EventsManager.Instance.LogEvent("ads_reward_end", new Dictionary<string, string>()
            {{"ads_network", adNetwork.ToString() },{"status","skipped" } });
        }

        private void AdsManager_OnRewardedAdCompleted(ERewardedAdNetwork adNetwork, string nameAd, double value)
        {
            EventsManager.Instance.LogEvent("ads_reward_end", new Dictionary<string, string>()
            {{"ads_network", adNetwork.ToString() },{"status","completed" } });
        }

        private void IAPManager_OnBuyPurchaseSuccessed(string id, bool modeTest, string receipt)
        {
            var product = IAPManager.Instance.GetProductInfoById(id);
            if (product != null && product.IsTypeRemoveAds())
            {
                RemoveAds(true);
            }
        }

        #endregion

        private void F4ACoreManager_OnDownloadF4AConfigCompleted(F4AConfigData configData, bool success)
        {
            if (configData != null)
            {
                if (!string.IsNullOrEmpty(configData.urlConfigAds))
                {
                    urlFileDataSetting = configData.urlConfigAds;
                }

#if DEFINE_WEBVIEW
                if (!string.IsNullOrEmpty(configData.urlConfigWebView))
                {
                    urlWebViewConfig = configData.urlConfigWebView;
                }
#endif
            }

#if DEFINE_WEBVIEW
            InitWebViewScroll();
#endif

            if (F4ACoreManager.Instance.IsGetConfigOnline)
            {
                var dataLocal = CPlayerPrefs.GetString(NameFileAdsInfo, "");
                if (configData.versionAds == CPlayerPrefs.GetInt(KeyVersionAdsConfig, 0)
                    //&& DMCFileUtilities.IsFileExist(NameFileAdsInfo)
                    && !string.IsNullOrEmpty(dataLocal)
                    )
                {
                    if (!string.IsNullOrEmpty(dataLocal))
                    {
                        adConfigData = JsonConvert.DeserializeObject<AdConfigData>(dataLocal);
                    }
                    InitAds();
                }
                else
                {
                    StartCoroutine(DMCMobileUtils.AsyncGetDataFromUrl(urlFileDataSetting, fileAdConfigDefault, (string data) =>
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            adConfigData = JsonConvert.DeserializeObject<AdConfigData>(data);
                            CPlayerPrefs.SetInt(KeyVersionAdsConfig, configData.versionAds);
                            CPlayerPrefs.SetString(NameFileAdsInfo, data);
#if UNITY_EDITOR
                            DMCFileUtilities.SaveFile(data, NameFileAdsInfo);
#endif
                        }
                        else if(!string.IsNullOrEmpty(dataLocal))
                        {
                            adConfigData = JsonConvert.DeserializeObject<AdConfigData>(dataLocal);
                        }
                        InitAds();
                    }));
                }
            }
            else
            {
                InitAds();
            }
        }

        private void InitAds()
        {
#if DEFINE_ADMOB
            InitializationAdmob();
#endif
#if DEFINE_UNITY_ADS
            InitializationUnityAds();
#endif
#if DEFINE_VUNGLE
            InitVungle();
#endif
#if DEFINE_CHARTBOOST
            InitChartboost();
#endif
#if DEFINE_ADCOLONY
            InitAdColony();
#endif
#if DEFINE_IRONSOURCE
            InitIronSource();
#endif
#if DEFINE_WEBVIEW
            LoadWebViewScroll();
#endif
#if DEFINE_FACEBOOK_ADS
            InitializationFacebookAd();
#endif
        }

        public bool IsEnalbleAds()
        {
            return adConfigData != null && adConfigData.IsEnableAds;
        }

#region Show Ad Methods

#region Interstitial Ads
        public void SetOrderInterstitialAds(EInterstitialAdNetwork[] adNetworks)
        {
            if(adNetworks != null && adNetworks.Length > 0) adConfigData.interstitialAdsOrder = adNetworks;
        }

        public bool IsInterstitialAdsReady()
        {
            try
            {
                if (IsRemoveAds() || !IsEnalbleAds())
                    return false;
                if (Time.realtimeSinceStartup - lastTimeShowInterstital < adConfigData.timeDalyShowInterstitial
                    || countEventShowInterstitial + 1 < adConfigData.timesEventShowInterstitial)
                    return false;
                int count = adConfigData.interstitialAdsOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    if (IsInterstitialAdReady(adConfigData.interstitialAdsOrder[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("IsInterstitialAdsReady ex:" + ex.Message);
                return false;
            }
        }

        protected bool IsInterstitialAdReady(EInterstitialAdNetwork adsType)
        {
            try
            {
                switch(adsType)
                {
                    case EInterstitialAdNetwork.AdMob:
#if DEFINE_ADMOB
                        return IsInterstitialAdmobReady();
#endif
                        break;
                    case EInterstitialAdNetwork.Chartboost:
#if DEFINE_CHARTBOOST
					    return Chartboost.hasInterstitial (CBLocation.Default);
#endif
                        break;
                    case EInterstitialAdNetwork.UnityAds:
#if DEFINE_UNITY_ADS
                        return IsInterstitialUnityAdReady();
#endif
                        break;
                    case EInterstitialAdNetwork.IronSource:
#if DEFINE_IRONSOURCE
                        return IsIronSourceInterstitialReady();
#endif
                        break;
                }

#if DEFINE_FACEBOOK_ADS
                if (adsType == EInterstitialAdNetwork.FacebookAudience)
                {
                    return IsInterstitialFBReady();
                }
#endif
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("@LOG IsInterstitialAdReady ex:" + ex.Message);
                return false;
            }
        }

        private Action _onInterstitialClose = null;
        public bool ShowInterstitialAds(int adsInterval = -1, Action onInterstitialClose = null, string locationId = "", EInterstitialAdNetwork[] adNetworks = null)
        {
            SetOrderInterstitialAds(adNetworks);

            _onInterstitialClose = onInterstitialClose;

            if(adsInterval > 0)
            {
                adConfigData.timeDalyShowInterstitial = adsInterval;
            }

            try
            {
                if (IsRemoveAds())
                {
                    Debug.Log("@LOG_F4A ShowInterstitialAds Is Remove Ads");
                    return false;
                }
                Debug.Log("@LOG_F4A AdsManager.ShowInterstitialAds Start");
                if (IsInterstitialAdsReady())
                {
                    countEventShowInterstitial++;
                    int count = adConfigData.interstitialAdsOrder.Length;
                    for (int i = 0; i < count; i++)
                    {
                        var adNetwork = adConfigData.interstitialAdsOrder[i];
                        if (ShowInterstitialAd(adNetwork))
                        {
                            Debug.Log("@LOG_F4A AdsManager.ShowInterstitialAds AdNetwork:" + adNetwork);

                            EventsManager.Instance.LogEvent("ads_interstitial_show", "ads_network", adNetwork.ToString());

                            countEventShowInterstitial = 0;
                            lastTimeShowInterstital = Time.realtimeSinceStartup;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("ShowInterstitialAds ex:" + ex.Message);
                return false;
            }
        }

        protected bool ShowInterstitialAd(EInterstitialAdNetwork adsType)
        {
            try
            {
                Debug.Log("<color=green>@LOG ShowInterstitialAd adsType: " + adsType + "</color>");
                if (adsType == EInterstitialAdNetwork.AdMob)
                {
#if DEFINE_ADMOB
                    return ShowInterstitialAdmob();
#endif
                }
                else if (adsType == EInterstitialAdNetwork.Chartboost)
                {
#if DEFINE_CHARTBOOST
			    	if (Chartboost.hasInterstitial (CBLocation.Default)) {
			    		Chartboost.showInterstitial (CBLocation.Default);
			    		Chartboost.cacheInterstitial (CBLocation.Default);
	                	InitChartboost ();	
	                	return true;
			    	}
			    	InitChartboost ();
#endif
                }
                else if (adsType == EInterstitialAdNetwork.UnityAds)
                {
#if DEFINE_UNITY_ADS
                    return ShowInterstitialUnityAd();
#endif
                }
                else if (adsType == EInterstitialAdNetwork.WebView)
                {
#if DEFINE_WEBVIEW
                    if (webViewRequestData.webViewConfigData.activeAds)
                    {
                        return ShowInterstitialWebView();
                    }
#endif
                }
                else if(adsType == EInterstitialAdNetwork.IronSource)
                {
#if DEFINE_IRONSOURCE
                    return ShowIronSourceInterstitial();
#endif
                }
                else if(adsType == EInterstitialAdNetwork.FacebookAudience)
                {
#if DEFINE_FACEBOOK_ADS
                    return ShowInterstitialFB();
#endif
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("ShowInterstitialAd ex:" + ex.Message);
                return false;
            }
        }
#endregion

#region REWARD ADS
        public void SetOrderRewardAds(ERewardedAdNetwork[] rewardedAdNetworks)
        {
            if(rewardedAdNetworks != null && rewardedAdNetworks.Length > 0) adConfigData.rewardAdOrder = rewardedAdNetworks;
        }

        /// <summary>
        /// Determines whether this instance is reward video ready.
        /// </summary>
        /// <returns><c>true</c> if this instance is reward video ready; otherwise, <c>false</c>.</returns>
        public bool IsRewardAdsReady()
        {
            try
            {
                int count = adConfigData.rewardAdOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    if (IsRewardAdsReady(adConfigData.rewardAdOrder[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("IsRewardAdsReady ex:" + ex.Message);
                return false;
            }
        }

        public bool IsRewardAdsReady(ERewardedAdNetwork adsType)
        {
            try
            {
#if DEFINE_UNITY_ADS
                if (adsType == ERewardedAdNetwork.UnityAds)
                {
                    return IsRewardedUnityAdReady();
                }
#endif

#if DEFINE_ADMOB
                if (adsType == ERewardedAdNetwork.AdMob)
                {
                    return IsRewardAdMobReady();
                }
#endif

#if DEFINE_VUNGLE
                if (adsType == ERewardedAdNetwork.Vungle)
                {
                    return IsRewardVungleReady();
                }
#endif

#if DEFINE_CHARTBOOST
	            if (adsType == ERewardedAdNetwork.Chartboost){
                    return IsRewardChartboostReady();
	        	}
#endif

#if DEFINE_ADCOLONY
                if(adsType == ERewardedAdNetwork.AdColony)
                {
                    return IsAdColonyReady();
                }
#endif

#if DEFINE_IRONSOURCE
                if(adsType == ERewardedAdNetwork.IronSource)
                {
                    return IsIronSourceRewardReady();
                }
#endif
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("IsRewardAdsReady ex:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// This method is show video and reward when watch complete video
        /// </summary>
        public bool ShowRewardAds()
        {
//#if UNITY_EDITOR
//            OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.None, string.Empty, DMCF4AConst.AmountAdsDefault);
//            return true;
//#endif

            try
            {
                int count = adConfigData.rewardAdOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    if (ShowRewardAd(adConfigData.rewardAdOrder[i]))
                    {
                        EventsManager.Instance.LogEvent("ads_reward_show", "ads_network", adConfigData.rewardAdOrder[i].ToString());
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("AdsManager.ShowRewardAds ex:" + ex.Message);
                return false;
            }
        }

        private bool ShowRewardAd(ERewardedAdNetwork adsType)
        {
            try
            {
#if DEFINE_UNITY_ADS
                if (adsType == ERewardedAdNetwork.UnityAds)
                {
                    return ShowRewardUnityAd();
                }
#endif

#if DEFINE_ADMOB
                if (adsType == ERewardedAdNetwork.AdMob)
                {
                    return ShowRewardAdmobAd();
                }
#endif

#if DEFINE_VUNGLE
                if (adsType == ERewardedAdNetwork.Vungle)
                {
                    return ShowRewardVungleAd();
                }
#endif

#if DEFINE_CHARTBOOST
                if (adsType == ERewardedAdNetwork.Chartboost)
                {
                    return ShowRewardCharstboost();
                }
#endif

#if DEFINE_ADCOLONY
                if (adsType == ERewardedAdNetwork.AdColony)
                {
                    return ShowAdColony();
                }
#endif

#if DEFINE_IRONSOURCE
                if (adsType == ERewardedAdNetwork.IronSource)
                {
                    return ShowIronSourceReward();
                }
#endif
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("AdsManager.ShowRewardAd ex:" + ex.Message);
                return false;
            }
        }
#endregion

#region Video Ads
        public bool IsVideoAdsReady()
        {
            return false;
        }

        private bool IsVideoAdReady(EVideoAdNetwork adType)
        {
            return false;
        }

        /// <summary>
        /// This method is only show video and don't reward when watch video
        /// </summary>
        public bool ShowVideoAds()
        {
            try
            {
                // can't show because it remove ads.
                if (IsRemoveAds())
                {
                    Debug.Log("@LOG_F4A ShowVideoAds Is Remove Ads");
                    return false;
                }
                int count = adConfigData.videoAdsOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    if (ShowVideoAd(adConfigData.videoAdsOrder[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("AdsManager.ShowVideoAds ex:" + ex.Message);
                return false;
            }
        }

        protected bool ShowVideoAd(EVideoAdNetwork adType)
        {
            try
            {
                // can't show because it remove ads.
                if (IsRemoveAds())
                    return false;
#if DEFINE_UNITY_ADS
                if (adType == EVideoAdNetwork.UnityAds)
                {
                    return ShowVideoUnityAd();
                }
#endif
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("AdsManager.ShowVideoAd ex:" + ex.Message);
                return false;
            }
        }
        #endregion

        #region Banner
        public void SetOrderBannerAds(EBannerAdNetwork[] adNetworks)
        {
            if (adNetworks != null && adNetworks.Length > 0) adConfigData.bannerAdsOrder = adNetworks;
        }

        public bool IsBannerAdsReady()
        {
            try
            {
                // can't show because it remove ads.
                if (IsRemoveAds())
                    return false;
                int count = adConfigData.bannerAdsOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    if (ShowBannerAd(adConfigData.bannerAdsOrder[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("AdsManager.ShowBannerAds ex:" + ex.Message);
                return false;
            }
        }

        private bool IsBannerAdReady(EBannerAdNetwork type)
        {
            try
            {
                // can't show because it remove ads.
                if (IsRemoveAds())
                    return false;
                if (type == EBannerAdNetwork.WebView)
                {
#if DEFINE_WEBVIEW
                    if (webViewRequestData.webViewConfigData.activeAds)
                    {
                        return IsBannerWebViewReady();
                    }
#endif
                }
                else if (type == EBannerAdNetwork.AdMob)
                {
#if DEFINE_ADMOB
                    return IsBannerAdAdmobReady();
#endif
                }
                else if (type == EBannerAdNetwork.UnityAds)
                {
#if DEFINE_UNITY_ADS
                    return IsBannerUnityAdReady();
#endif
                }
                else if (type == EBannerAdNetwork.IronSource)
                {
#if DEFINE_IRONSOURCE
                    return IsIronSourceBannerReady();
#endif
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("ShowVideoAd ex:" + ex.Message);
                return false;
            }
        }

        public bool ShowBannerAds(EBannerAdNetwork[] adNetworks = null)
        {
            try
            {
                if(adNetworks != null && adNetworks.Length > 0)
                {
                    adConfigData.bannerAdsOrder = adNetworks;
                }

                // can't show because it remove ads.
                if (IsRemoveAds())
                {
                    Debug.Log("@LOG_F4A ShowBannerAds Is Remove Ads");
                    return false;
                }
                Debug.Log("<color=green>ShowBannerAds Start</color>");
                int count = adConfigData.bannerAdsOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    if (ShowBannerAd(adConfigData.bannerAdsOrder[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("ShowBannerAds ex:" + ex.Message);
                return false;
            }
        }

        protected bool ShowBannerAd(EBannerAdNetwork type)
        {
            try
            {
                // can't show because it remove ads.
                if (IsRemoveAds()) return false;

                Debug.Log("<color=green>AdsManager.ShowBannerAd type:" + type + "</color>");

                if (type == EBannerAdNetwork.WebView)
                {
#if DEFINE_WEBVIEW
                    return ShowBannerWebView();
#endif
                }
                else if (type == EBannerAdNetwork.AdMob)
                {
#if DEFINE_ADMOB
                    return ShowBannerAdAdmob();
#endif
                }
                else if (type == EBannerAdNetwork.UnityAds)
                {
#if DEFINE_UNITY_ADS
                    return ShowBannerUnityAd();
#endif
                }
                else if(type == EBannerAdNetwork.IronSource)
                {
#if DEFINE_IRONSOURCE
                    return ShowIronSourceBanner();
#endif
                }
                else if(type == EBannerAdNetwork.FacebookAudience)
                {
#if DEFINE_FACEBOOK_ADS
                    return ShowBannerFB();
#endif
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("AdsManager.ShowVideoAd ex:" + ex.Message);
                return false;
            }
        }

        public bool HideBannerAds()
        {
            try
            {
                Debug.Log("<color=red>HideBannerAds Start</color>");
                int count = adConfigData.bannerAdsOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    HideBannerAd(adConfigData.bannerAdsOrder[i]);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log("HideBannerAds ex:" + ex.Message);
                return false;
            }
        }

        private bool HideBannerAd(EBannerAdNetwork type)
        {
            try
            {
                if (type == EBannerAdNetwork.WebView)
                {
#if DEFINE_WEBVIEW
                    if (webViewRequestData.webViewConfigData.activeAds)
                    {
                        return HideBannerWebView();
                    }
#endif
                }
                else if (type == EBannerAdNetwork.AdMob)
                {
#if DEFINE_ADMOB
                    return HideBannerAdAdmob();
#endif
                }
                else if(type == EBannerAdNetwork.IronSource)
                {
#if DEFINE_IRONSOURCE
                    return HideIronSourceBanner();
#endif
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("HideBannerAd ex:" + ex.Message);
                return false;
            }
        }

        public bool DestroyBannerAds()
        {
            try
            {
                int count = adConfigData.bannerAdsOrder.Length;
                for (int i = 0; i < count; i++)
                {
                    DestroyBannerAd(adConfigData.bannerAdsOrder[i]);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log("DestroyBannerAds ex:" + ex.Message);
                return false;
            }
        }

        private bool DestroyBannerAd(EBannerAdNetwork type)
        {
            try
            {
                if (type == EBannerAdNetwork.WebView)
                {
#if DEFINE_WEBVIEW
                    if (webViewRequestData.webViewConfigData.activeAds)
                    {
                        return HideBannerWebView();
                    }
#endif
                }
                else if (type == EBannerAdNetwork.AdMob)
                {
#if DEFINE_ADMOB
                    DestroyBannerAdAdmob();
#endif
                }
                else if(type == EBannerAdNetwork.IronSource)
                {
#if DEFINE_IRONSOURCE
                    HideIronSourceBanner();
                    DestroyIronSourceBanner();
#endif
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log("HideBannerAd ex:" + ex.Message);
                return false;
            }
        }
#endregion
        /// <summary>
        /// Removes the ads.
        /// </summary>
        public void RemoveAds(bool value)
        {
            SetRemoveAds(value);
        }

        /// <summary>
        /// Sets the remove ads.
        /// </summary>
        /// <param name="isRemoveAd">If set to <c>true</c> is remove.</param>
	    private void SetRemoveAds(bool isRemoveAd)
        {
            //PlayerPrefs.SetInt(F4AUtils.KeyRemoveAds, isRemoveAd ? F4AUtils.AD_REMOVE : F4AUtils.AD_DONT_REMOVE);
            CPlayerPrefs.SetBool(DMCMobileUtils.KeyRemoveAds, isRemoveAd);
            if (isRemoveAd)
            {
                DestroyBannerAds();
                if (F4ACoreManager.Instance.AdsBannerShape) F4ACoreManager.Instance.AdsBannerShape.SetActive(false);
            }

            if (OnRemoveAds != null)
            {
                Debug.Log("@LOG F4A SetRemoveAds isRemoveAd: " + isRemoveAd);
                OnRemoveAds(isRemoveAd);
            }
        }

        public bool IsRemoveAds()
        {
            //return PlayerPrefs.GetInt(F4AUtils.KeyRemoveAds, F4AUtils.AD_DONT_REMOVE) == F4AUtils.AD_REMOVE;
            return CPlayerPrefs.GetBool(DMCMobileUtils.KeyRemoveAds, false);
        }

#endregion //GENERAL

        IEnumerator IEWaitForSeconds(float delay, System.Action callBack)
        {
            if(delay <= 0)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
            callBack?.Invoke();
        }


#if UNITY_EDITOR
        public void SaveInfo()
        {
            string str = JsonConvert.SerializeObject(adConfigData);
            string pathDirectory = Path.Combine(Application.dataPath, DMCMobileUtils.PathFolderData);
            DMCMobileUtils.CreateDirectory(pathDirectory);

            string path = Path.Combine(pathDirectory, NameFileAdsInfo);
            StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(str);
            file.Close();

            UnityEditor.AssetDatabase.Refresh();
        }

        public void LoadInfo()
        {
            string path = Path.Combine(Application.dataPath, DMCMobileUtils.PathFolderData);
            path = Path.Combine(path, NameFileAdsInfo);
            string text = System.IO.File.ReadAllText(path);
            adConfigData = JsonConvert.DeserializeObject<AdConfigData>(text);
        }
#endif
    }
}