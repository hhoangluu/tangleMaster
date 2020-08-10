namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum F4AEResolutionDevice
    {
        None,
        iPhoneX_1125_2436,
        iPhoneXR_828_1792,
        iPhoneXSMax_1242_2688,
        iPhone7_Plus_5_5_1242_2208, // 720x1280, 1080x1920, 750x1334
        iPadPro_12_9_2048_2732,
        Resolution_480_800,
        Resolution_640_1136,
        Resolution_800_1280,
        //Resolution_750x1334,
        //Resolution_1080_1920,
    }

    public enum F4ATypeScene{
        Portrait,
        Landscape,
    }

    #region Enum Ads
    public enum EAdPosition
    {
        Top = 0,
        Bottom = 1,
        TopLeft = 2,
        TopRight = 3,
        BottomLeft = 4,
        BottomRight = 5,
        Center = 6
    }

    // List of all supported ad networks
    public enum EAdNetwork
    {
        None,
        AdColony,
        AdMob,
        Chartboost,
        Heyzap,
        UnityAds,
        Vungle,
        StartApp,
        WebView, // custom
        ImageFromServer,
        Applovin,
        Tapjoy,
        Fyber,
        Mopub,
        FacebookAudience,
        Receptiv,
        IronSource,
        Appnext
    }

    public enum EAdType
    {
        Banner,
        Interstitial,
        Rewarded,
        Video,
        Native,
    }

    public enum EBannerAdNetwork
    {
        None = EAdNetwork.None,
        AdMob = EAdNetwork.AdMob,
        Heyzap = EAdNetwork.Heyzap,
        WebView = EAdNetwork.WebView,
        UnityAds = EAdNetwork.UnityAds,
        IronSource = EAdNetwork.IronSource,
        FacebookAudience = EAdNetwork.FacebookAudience,
    }

    public enum EInterstitialAdNetwork
    {
        None = EAdNetwork.None,
        AdColony = EAdNetwork.AdColony,
        AdMob = EAdNetwork.AdMob,
        Chartboost = EAdNetwork.Chartboost,
        Heyzap = EAdNetwork.Heyzap,
        UnityAds = EAdNetwork.UnityAds,
        WebView = EAdNetwork.WebView,
        IronSource = EAdNetwork.IronSource,
        Appnext = EAdNetwork.Appnext,
        FacebookAudience = EAdNetwork.FacebookAudience,
    }

    public enum EVideoAdNetwork
    {
        None = EAdNetwork.None,
        UnityAds = EAdNetwork.UnityAds,
        AdMob = EAdNetwork.AdMob,
        AdColony = EAdNetwork.AdColony,
        Appnext = EAdNetwork.Appnext,
    }

    public enum ERewardedAdNetwork
    {
        None = EAdNetwork.None,
        AdColony = EAdNetwork.AdColony,
        AdMob = EAdNetwork.AdMob,
        Chartboost = EAdNetwork.Chartboost,
        Heyzap = EAdNetwork.Heyzap,
        UnityAds = EAdNetwork.UnityAds,
        Vungle = EAdNetwork.Vungle,
        IronSource = EAdNetwork.IronSource,
        Mopub = EAdNetwork.Mopub,
        Appnext = EAdNetwork.Appnext,
        FacebookAudience = EAdNetwork.FacebookAudience,
    }
    #endregion


    public enum EStorePublishType
    {
        AppleStore = 0,
        GooglePlayStore,
        SamsungStore,
        AmazonStore,
    }

    public enum EListenType
    {
        ////
        //DOWNLOAD_CONFIG_FILE_COMPLETED,
        //DOWNLOAD_CONFIG_FILE_FAILED,

        //// AssetBundle
        //DOWNLOAD_ALL_ASSET_START,
        //DOWNLOAD_ALL_ASSET_COMPLETED,
        //DOWNLOAD_ALL_ASSET_FAILED,

        //DOWNLOAD_ASSET_START,
        //DOWNLOAD_ASSET_COMPLETED,
        //DOWNLOAD_ASSET_FAILED,

        //DOWNLOAD_ASSET_PROGRESS,

        //// In App Purchase
        //IAP_BUY_SUCCEEDED,
        //IAP_BUY_FAILED,

        //// Ads
        //ADS_REWARD_COMPLETED,
        //ADS_REWARD_SKIPED,
        //ADS_REWARD_FAILED,

        //REMOVE_ADS,

        //// Audio Manager
        //CHANGE_SOUND,
        //CHANGE_MUSIC,
    }

    public enum ELoadFileMethod
    {
        None,
        FileFromLocal,
        FileFromServer,
    }


    public enum EDownloadAssetBundleMethod
    {
        LoadFromMemoryAsync,
        LoadFromFile,
        LoadFromCacheOrDownload,
        UnityWebRequest
    }

    public enum EGetDataMethod // HTTP
    {
        POST,
        GET,
    }

    public enum EAudioMethod
    {
        Toggle,
        Slider,
    }

    public enum ELanguages
    {
        Arabic = 0,
        Chinese = 1,
        Dutch = 2,
        English = 3,
        Filipino = 4,
        French = 5,
        German = 6,
        Hindi = 7,
        Indonesian = 8,
        Italian = 9,
        Japanese = 10,
        Portuguese = 11,
        Russian = 12,
        Spanish = 13,
        Turkish = 14,
        Urdu = 15
    }

    public enum EFirebaseEventAnalytic
    {
        EventPostScore,
        EventLogin,
        EventLevelUp,
        EventJoinGroup,
        ParameterAffiliation,
        EventEcommercePurchase,
        EventPresentOffer,
        EventEarnVirtualCurrency,
        EventCampaignDetails,
        EventBeginCheckout,
        EventAppOpen,
        EventAddToWishlist,
        EventAddToCart,
        EventAddPaymentInfo,
        EventCheckoutProgress,
        EventPurchaseRefund,
        EventGenerateLead,
        EventSearch,
        EventRemoveFromCart,
        EventLevelEnd,
        EventLevelStart,
        EventViewSearchResults,
        EventViewItemList,
        EventViewItem,
        EventUnlockAchievement,
        ParameterAchievementId,
        EventTutorialBegin,
        EventSpendVirtualCurrency,
        EventSignUp,
        EventShare,
        EventSetCheckoutOption,
        EventSelectContent,
        EventTutorialComplete,
    }
}