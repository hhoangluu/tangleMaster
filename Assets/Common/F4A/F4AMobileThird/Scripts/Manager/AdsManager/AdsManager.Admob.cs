using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
#if DEFINE_ADMOB
using GoogleMobileAds.Api;
#endif

namespace com.F4A.MobileThird
{
    public partial class AdsManager
    {
#if DEFINE_ADMOB
        AdRequest adRequestAdmob = null;

        private InterstitialAd interstitialAdmob;
        private BannerView bannerViewAdmob;
        private RewardedAd rewardedAd;
#endif
        private bool _isRewardAdmobCompleted = false;

        private bool IsEnableAdmobAd()
        {
            return adConfigData != null && adConfigData.admobConfig != null && adConfigData.admobConfig.IsEnableAd;
        }

        private void InitializationAdmob()
        {
            try
            {
#if DEFINE_ADMOB
                if (IsEnableAdmobAd())
                {
                    //MobileAds.SetiOSAppPauseOnBackground(true);

                    string appId = "";
                    if (adConfigData != null && adConfigData.admobConfig != null)
                    {
                        appId = adConfigData.admobConfig.GetAppId();
                    }
                    if (!string.IsNullOrEmpty(appId))
                    {
                        // Initialize the Google Mobile Ads SDK.
                        MobileAds.Initialize(appId);
                    }

                    AdRequest.Builder builder = new AdRequest.Builder();
                    for (int i = 0; i < adConfigData.IdDeviceTests.Length; i++)
                    {
                        builder.TagForChildDirectedTreatment(adConfigData.admobConfig.ForChild)
                            .AddTestDevice(adConfigData.IdDeviceTests[i]);
                    }
                    adRequestAdmob = builder.Build();

                    RequestBannerAdmob();
                    RequestInterstitialAdmob();
                    RequestRewardAdmob();
                }
#endif
            }
            catch (Exception ex)
            {
                AnalyticManager.Instance.CustomeEvent("InitializationAdmob Failed!" + ex.Message);
            }
        }

        #region Interstitial

        private void DestroyInterstitialAdmob()
        {
#if DEFINE_ADMOB
            if (interstitialAdmob != null)
            {
                interstitialAdmob.OnAdClosed -= InterstitialAdmob_HandleAdClosed;
                interstitialAdmob.OnAdFailedToLoad -= InterstitialAdmob_HandleAdFailedToLoad;
                interstitialAdmob.OnAdLeavingApplication -= InterstitialAdmob_HandleAdLeavingApplication;
                interstitialAdmob.OnAdLoaded -= InterstitialAdmob_HandleAdLoaded;
                interstitialAdmob.OnAdOpening -= InterstitialAdmob_HandleAdOpening;

                interstitialAdmob.Destroy();
                interstitialAdmob = null;
            }
#endif
        }

        private void RequestInterstitialAdmob()
        {
            _tweenInterstitialAdmob.Reset();

            DestroyInterstitialAdmob();
#if DEFINE_ADMOB
            if (IsEnableAdmobAd())
            {
                string id = "";
                id = adConfigData.admobConfig.GetInterstitialId();
                Debug.Log("@LOG_F4A AdsManager.Admob.RequestInterstitialAdmob id:" + id);
                if (!string.IsNullOrEmpty(id))
                {
                    interstitialAdmob = new InterstitialAd(id);
                    interstitialAdmob.LoadAd(adRequestAdmob);
                }

                if (interstitialAdmob != null)
                {
                    interstitialAdmob.OnAdClosed += InterstitialAdmob_HandleAdClosed;
                    interstitialAdmob.OnAdFailedToLoad += InterstitialAdmob_HandleAdFailedToLoad;
                    interstitialAdmob.OnAdLeavingApplication += InterstitialAdmob_HandleAdLeavingApplication;
                    interstitialAdmob.OnAdLoaded += InterstitialAdmob_HandleAdLoaded;
                    interstitialAdmob.OnAdOpening += InterstitialAdmob_HandleAdOpening;
                }
            }
#endif
        }

        private bool IsInterstitialAdmobReady()
        {
#if DEFINE_ADMOB
            return !Application.isEditor && IsEnableAdmobAd() && interstitialAdmob != null && interstitialAdmob.IsLoaded();
#endif
            return false;
        }
        private bool ShowInterstitialAdmob()
        {
#if DEFINE_ADMOB
            if (SystemLanguage.Chinese == Application.systemLanguage
                        || SystemLanguage.ChineseSimplified == Application.systemLanguage
                        || SystemLanguage.ChineseTraditional == Application.systemLanguage)
            {
                return false;
            }

            if (IsInterstitialAdmobReady())
            {
                Debug.Log("@LOG_F4A ShowInterstitialAdmob");
                interstitialAdmob.Show();
                return true;
            }
            else
            {
                RequestInterstitialAdmob();
                return false;
            }
#endif
            return false;
        }

        public Action InterstitialAdmob_OnleAdLeavingApplication;
        public Action InterstitialAdmob_OnleAdLoaded;
        public Action InterstitialAdmob_OnleAdOpening;

        private Tween _tweenInterstitialAdmob = null;
#if DEFINE_ADMOB
        private void InterstitialAdmob_HandleAdClosed(object sender, EventArgs e)
        {
            if (_timeScale > 0) Time.timeScale = _timeScale;
            else Time.timeScale = 1;

            OnInterstitialAdClosed?.Invoke(EInterstitialAdNetwork.AdMob);
            RequestInterstitialAdmob();
        }

        private void InterstitialAdmob_HandleAdOpening(object sender, EventArgs e)
        {
            InterstitialAdmob_OnleAdOpening?.Invoke();

            _timeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void InterstitialAdmob_HandleAdLoaded(object sender, EventArgs e)
        {
            if (InterstitialAdmob_OnleAdLoaded != null)
                InterstitialAdmob_OnleAdLoaded();
        }

        private void InterstitialAdmob_HandleAdLeavingApplication(object sender, EventArgs e)
        {
            if (InterstitialAdmob_OnleAdLeavingApplication != null)
                InterstitialAdmob_OnleAdLeavingApplication();
        }

        private void InterstitialAdmob_HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            _tweenInterstitialAdmob = DOVirtual.DelayedCall(3, () => { RequestInterstitialAdmob(); });
            OnInterstitialAdFailed?.Invoke(EInterstitialAdNetwork.AdMob);
        }
#endif
        #endregion

        #region Reward

        private Tween _tweenRewardAdmob = null;
        private void RequestRewardAdmob()
        {
            _tweenRewardAdmob.Reset();
#if DEFINE_ADMOB
            try
            {
                if (rewardedAd != null)
                {
                    // Called when an ad request has successfully loaded.
                    this.rewardedAd.OnAdLoaded -= RewardedAd_OnAdLoaded;
                    // Called when an ad request failed to load.
                    this.rewardedAd.OnAdFailedToLoad -= RewardedAd_OnAdFailedToLoad;
                    // Called when an ad is shown.
                    this.rewardedAd.OnAdOpening -= RewardedAd_OnAdOpening;
                    // Called when an ad request failed to show.
                    this.rewardedAd.OnAdFailedToShow -= RewardedAd_OnAdFailedToShow;
                    // Called when the user should be rewarded for interacting with the ad.
                    this.rewardedAd.OnUserEarnedReward -= RewardedAd_OnUserEarnedReward;
                    // Called when the ad is closed.
                    this.rewardedAd.OnAdClosed -= RewardedAd_OnAdClosed;

                    rewardedAd = null;
                }

                if (IsEnableAdmobAd())
                {
                    string id = "";
                    if (adConfigData != null && adConfigData.admobConfig != null)
                    {
                        id = adConfigData.admobConfig.GetRewardId();
                    }
                    if (!string.IsNullOrEmpty(id))
                    {
                        rewardedAd = new RewardedAd(id);
                        if (rewardedAd != null)
                        {
                            rewardedAd.LoadAd(adRequestAdmob);

                            // Called when an ad request has successfully loaded.
                            this.rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded;
                            // Called when an ad request failed to load.
                            this.rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;
                            // Called when an ad is shown.
                            this.rewardedAd.OnAdOpening += RewardedAd_OnAdOpening;
                            // Called when an ad request failed to show.
                            this.rewardedAd.OnAdFailedToShow += RewardedAd_OnAdFailedToShow;
                            // Called when the user should be rewarded for interacting with the ad.
                            this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;
                            // Called when the ad is closed.
                            this.rewardedAd.OnAdClosed += RewardedAd_OnAdClosed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("AdsManager.AdMob.RequestRewardAdmob ex:" + ex.Message);
            }
#endif
        }

        protected bool IsRewardAdMobReady()
        {
#if DEFINE_ADMOB
            try
            {
                var isReady = !Application.isEditor && IsEnableAdmobAd() && rewardedAd != null && rewardedAd.IsLoaded();
                return isReady;
            }
            catch (Exception ex)
            {
                Debug.LogError("AdsManager.AdMob.IsRewardAdMobReady ex:" + ex.Message);
            }
#endif
            return false;
        }

        protected bool ShowRewardAdmobAd()
        {
#if DEFINE_ADMOB
            try
            {
                if (IsRewardAdMobReady())
                {
                    Debug.Log("@LOG_F4A ShowRewardAdmobAd");
                    _isRewardAdmobCompleted = false;
                    rewardedAd.Show();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("@LOG_F4A AdsManager.AdMob.ShowRewardAdmobAd ex:" + ex.Message);
            }
#endif
            return false;
        }
#if DEFINE_ADMOB

        private void RewardedAd_OnAdClosed(object sender, EventArgs e)
        {
            if (_isRewardAdmobCompleted)
            {
                OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.AdMob, "adAdmob", 0.02f);
            }
            else
            {
                OnRewardedAdSkiped?.Invoke(ERewardedAdNetwork.AdMob);
            }
            RequestRewardAdmob();

            if (_timeScale > 0) Time.timeScale = _timeScale;
            else Time.timeScale = 1;
        }

        private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
        {
            _isRewardAdmobCompleted = true;
        }

        private void RewardedAd_OnAdFailedToShow(object sender, AdErrorEventArgs e)
        {
            Debug.LogError("@LOG_F4A AdsManager.AdMob.RewardedAd_OnAdFailedToShow e:" + e.Message);
            OnRewardedAdFailed?.Invoke(ERewardedAdNetwork.AdMob);
            _tweenRewardAdmob = DOVirtual.DelayedCall(3, () => { RequestRewardAdmob(); });
        }

        private void RewardedAd_OnAdOpening(object sender, EventArgs e)
        {
            _timeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void RewardedAd_OnAdFailedToLoad(object sender, AdErrorEventArgs e)
        {
            Debug.LogError("@LOG_F4A AdsManager.AdMob.RewardedAd_OnAdFailedToLoad e:" + e.Message);
            _tweenRewardAdmob = DOVirtual.DelayedCall(3, () => { RequestRewardAdmob(); });
        }

        private void RewardedAd_OnAdLoaded(object sender, EventArgs e)
        {
        }
#endif
        #endregion

        #region Banner Ad

        private Tween _tweenBannerAdmob = null;
        private void RequestBannerAdmob()
        {
#if DEFINE_ADMOB
            _tweenBannerAdmob.Reset();

            if (bannerViewAdmob != null)
            {
                bannerViewAdmob.OnAdLoaded -= BannerViewAdmob_OnAdLoaded;
                bannerViewAdmob.OnAdFailedToLoad -= BannerViewAdmob_OnAdFailedToLoad;
            }

            string id = "";

            if (adConfigData != null && adConfigData.admobConfig != null)
            {
                id = adConfigData.admobConfig.GetBannerId();
            }
            if (!string.IsNullOrEmpty(id))
            {
                Debug.Log("@LOG AdsManager.Admob.RequestBannerAdmob id: " + id);
                AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
                //bannerViewAdmob = new BannerView(id, AdSize.Banner, (AdPosition)adConfigData.AdPosition);
                bannerViewAdmob = new BannerView(id, adaptiveSize, (AdPosition)adConfigData.AdPosition);
            }
            if (bannerViewAdmob != null)
            {
                bannerViewAdmob.LoadAd(adRequestAdmob);
                bannerViewAdmob.OnAdLoaded += BannerViewAdmob_OnAdLoaded;
                bannerViewAdmob.OnAdFailedToLoad += BannerViewAdmob_OnAdFailedToLoad;
            }
#endif
        }

#if DEFINE_ADMOB
        private void BannerViewAdmob_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            _tweenBannerAdmob = DOVirtual.DelayedCall(3, () => { RequestBannerAdmob(); });
        }
#endif

        private void BannerViewAdmob_OnAdLoaded(object sender, EventArgs e)
        {
            OnBannerAdLoad?.Invoke(EBannerAdNetwork.AdMob);
        }

        protected bool IsBannerAdAdmobReady()
        {
#if DEFINE_ADMOB
            if (IsEnableAdmobAd() && bannerViewAdmob != null)
            {
                Debug.Log("@LOG AdsManager.Admob.IsBannerAdAdmobReady");
                return true;
            }
#endif
            return false;
        }

        protected bool ShowBannerAdAdmob()
        {
#if DEFINE_ADMOB
            if (IsEnableAdmobAd())
            {
                if (bannerViewAdmob != null)
                {
                    Debug.Log("@LOG AdsManager.Admob.ShowBannerAdAdmob");
                    bannerViewAdmob.Show();
                    return true;
                }
                else
                {
                    RequestBannerAdmob();
                }
            }
#endif
            return false;
        }

        protected bool HideBannerAdAdmob()
        {
#if DEFINE_ADMOB
            if (IsEnableAdmobAd())
            {
                if (bannerViewAdmob != null)
                {
                    bannerViewAdmob.Hide();
                    return true;
                }
            }
#endif
            return false;
        }

        protected bool DestroyBannerAdAdmob()
        {
#if DEFINE_ADMOB
            if (IsEnableAdmobAd() && bannerViewAdmob != null)
            {
                bannerViewAdmob.Hide();
                bannerViewAdmob.Destroy();
                return true;
            }
#endif
            return false;
        }
        #endregion

        #region Native Ad
#if DEFINE_AD_NATIVE
        private UnifiedNativeAd _unifiedNativeAd;
        public UnifiedNativeAd UnifiedNativeAd
        {
            get { return _unifiedNativeAd; }
        }
#endif
        protected void InitNativeAd()
        {
#if DEFINE_AD_NATIVE
            string id = string.Empty;
            id = adConfigData.admobConfig.GetNativeId();
            Debug.Log("@LOG_F4A AdsManager.InitNativeAd id:" + id);
            if (!string.IsNullOrEmpty(id))
            {
                AdLoader adLoader = new AdLoader.Builder(id).ForUnifiedNativeAd().Build();
                adLoader.LoadAd(new AdRequest.Builder().Build());

                adLoader.OnUnifiedNativeAdLoaded += AdLoader_OnUnifiedNativeAdLoaded;
                adLoader.OnAdFailedToLoad += AdLoader_OnAdFailedToLoad;
                adLoader.OnNativeAdClicked += AdLoader_OnNativeAdClicked;
            }
#endif
        }

        private GameObject _registerIconObject;
        private GameObject _registerMainImgObject;
        private GameObject _registerCallActionObject;
        private bool _unifiedNativeAdLoaded = false;
        private Action<DMCDataNativeAd> _callBack;
        private bool _isShowNative = false;

        private RawImage imageIcon;

#if DEFINE_AD_NATIVE
        private void Update()
        {
            if (_isShowNative && _unifiedNativeAdLoaded && _unifiedNativeAd != null)
            {
                _unifiedNativeAdLoaded = false;
                _isShowNative = false;
                var data = GetDataNativeAd();
                _callBack?.Invoke(data);

                //if (_registerIconObject)
                //{
                //    if (!_unifiedNativeAd.RegisterIconImageGameObject(_registerIconObject))
                //    {
                //    }
                //    else
                //    {
                //    }
                //}

                //if (_registerMainImgObject)
                //{
                //    if (!this._unifiedNativeAd.RegisterCallToActionGameObject(_registerMainImgObject))
                //    {
                //    }
                //    else
                //    {
                //    }
                //}

                if (_registerCallActionObject)
                {
                    if (!_unifiedNativeAd.RegisterCallToActionGameObject(_registerCallActionObject))
                    {
                        Debug.Log("@LOG AdsManager.Update RegisterCallToActionGameObject " + _registerCallActionObject.name + " failed");
                    }
                    else
                    {
                        Debug.Log("@LOG AdsManager.Update RegisterCallToActionGameObject " + _registerCallActionObject.name + " success");
                    }
                }
            }
    }
#endif

        public void ShowNative(GameObject registerIconObject, GameObject registerMainImgObject, GameObject registerCallActionObject,
            Action<DMCDataNativeAd> callBack)
        {
#if DEFINE_AD_NATIVE
            InitNativeAd();
            _registerIconObject = registerIconObject;
            _registerMainImgObject = registerMainImgObject;
            _registerCallActionObject = registerCallActionObject;
            _callBack = callBack;
            _isShowNative = true;
#endif
        }

#if DEFINE_AD_NATIVE
        private void AdLoader_OnNativeAdClicked(object sender, EventArgs e)
        {
            Debug.Log("@LOG AdsManager.AdLoader_OnNativeAdClicked");
        }

        private void AdLoader_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Debug.Log("@LOG AdsManager.AdLoader_OnAdFailedToLoad e:" + e.Message);
        }

        private DMCDataNativeAd dataNativeAd = null;
        private void AdLoader_OnUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs e)
        {
            Debug.Log("@LOG AdsManager.AdLoader_OnUnifiedNativeAdLoaded");
            _unifiedNativeAd = e.nativeAd;
            dataNativeAd = GetDataNativeAd();
            _unifiedNativeAdLoaded = true;
        }
#endif

        public DMCDataNativeAd GetDataNativeAd()
        {
            DMCDataNativeAd data = null;
#if DEFINE_AD_NATIVE
            if (_unifiedNativeAd != null)
            {
                Debug.Log("@LOG AdsManager.GetDataNativeAd GetStore:" + _unifiedNativeAd.GetStore());
                List<Texture2D> lstImg = _unifiedNativeAd.GetImageTextures();
                data = new DMCDataNativeAd();
                data.title = _unifiedNativeAd.GetHeadlineText();
                data.body = _unifiedNativeAd.GetBodyText();
                data.advertiserName = _unifiedNativeAd.GetAdvertiserText();
                if (lstImg != null && lstImg.Count > 0) data.mainImg = lstImg[0];
                data.iconAds = _unifiedNativeAd.GetIconTexture();
                data.price = _unifiedNativeAd.GetPrice();
                data.star = _unifiedNativeAd.GetStarRating();
                data.storeName = _unifiedNativeAd.GetStore();
                data.callToActionText = _unifiedNativeAd.GetCallToActionText();
                data.adChoice = _unifiedNativeAd.GetAdChoicesLogoTexture();
            }
#endif
            return data;
        }
        #endregion
    }

    [Serializable]
    public class DMCDataNativeAd
    {
        public string advertiserName;
        public string socialContext;
        public string body;
        public string title;
        public string sponsored;
        public string status;
        public double star;
        public string price;
        public string storeName;
        public Texture2D callToActionButton;
        public string callToActionText;
        public Texture2D mainImg;
        public Texture2D iconAds;
        public Texture2D adChoice;
    }
}