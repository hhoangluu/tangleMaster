namespace com.F4A.MobileThird
{
    using DG.Tweening;
#if DEFINE_FACEBOOK_ADS
    using AudienceNetwork;
#endif
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class AdsManager
    {
#if DEFINE_FACEBOOK_ADS
        private AdView adView;
        private InterstitialAd interstitialAd;
#endif
        private bool _isLoadInterstitialFbAds = false;
        private bool _didCloseInterstitialFbAds = false;

#if DEFINE_FACEBOOK_ADS
        private RewardedVideoAd rewardedVideoAd;
#endif
        private bool _isLoadRewardFbAds = false;
        private bool _didCloseRewardFbAds = false;

        private bool IsEnableFacebookAd()
        {
            return adConfigData != null && adConfigData.facebookAdConfig != null && adConfigData.facebookAdConfig.enableAd;
        }

        private void InitializationFacebookAd()
        {
            if (!IsRemoveAds())
            {
#if DEFINE_FACEBOOK_ADS
                for (int i = 0; i < adConfigData.IdDeviceTests.Length; i++)
                {
                    AdSettings.AddTestDevice(adConfigData.IdDeviceTests[i]);
                }
#endif
                LoadBannerFB();
                LoadInterstitialFB();
                LoadRewardedFB();
            }
        }

        Tween _tweenLoadBannerFB = null;
        protected void LoadBannerFB()
        {
#if DEFINE_FACEBOOK_ADS
            if(_tweenLoadBannerFB != null)
            {
                _tweenLoadBannerFB.Kill();
                _tweenLoadBannerFB = null;
            }
            if (this.adView)
            {
                adView.AdViewDidFailWithError -= AdView_AdViewDidFailWithError;
                adView.AdViewWillLogImpression -= AdView_AdViewWillLogImpression;
                adView.AdViewDidClick -= AdView_AdViewDidClick;

                this.adView.Dispose();
                this.adView = null;
            }

            if (!IsEnableFacebookAd()) return;
            var id = adConfigData.facebookAdConfig.GetBannerId();
            if(string.IsNullOrEmpty(id))
            {
                Debug.Log("@LOG LoadBannerFB can't find id banner");
                return;
            }
            if (Screen.width <= 720)
            {
                this.adView = new AdView(id, AdSize.BANNER_HEIGHT_50);
            }
            else
            {
                this.adView = new AdView(id, AdSize.BANNER_HEIGHT_90);
            }
            this.adView.Register(this.gameObject);
            Debug.Log("@LOG LoadBannerFB id:" + id);

            // Set delegates to get notified on changes or when the user interacts with the ad.
            this.adView.AdViewDidLoad = (delegate ()
            {
                Debug.Log("@LOG LoadBannerFB");
                this.adView.Show(100);
                ShowBannerFB();
            });

            adView.AdViewDidFailWithError += AdView_AdViewDidFailWithError;
            adView.AdViewWillLogImpression += AdView_AdViewWillLogImpression;
            adView.AdViewDidClick += AdView_AdViewDidClick;

            // Initiate a request to load an ad.
            adView.LoadAd();
#endif
        }

        private void AdView_AdViewDidFailWithError(string error)
        {
            Debug.Log("@LOG Facebook Ads Banner failed to load with error: " + error);
            _tweenLoadBannerFB = DOVirtual.DelayedCall(3, () => { LoadBannerFB(); });
        }

        private void AdView_AdViewWillLogImpression()
        {
            Debug.Log("@LOG Facebook Ads Banner logged impression.");
        }

        private void AdView_AdViewDidClick()
        {
            Debug.Log("@LOG Facebook Ads Banner clicked.");
        }

        protected bool ShowBannerFB()
        {
#if DEFINE_FACEBOOK_ADS
            if (this.adView)
            {
                this.adView.Show(adConfigData.AdPosition == EAdPosition.Bottom ? AdPosition.BOTTOM : AdPosition.TOP);
                return true;
            }
#endif
            return false;
        }

        protected void DestroyBannerFB()
        {
            // Dispose of banner ad when the scene is destroyed
#if DEFINE_FACEBOOK_ADS
            if (this.adView)
            {
                adView.AdViewDidFailWithError -= AdView_AdViewDidFailWithError;
                adView.AdViewWillLogImpression -= AdView_AdViewWillLogImpression;
                adView.AdViewDidClick -= AdView_AdViewDidClick;

                this.adView.Dispose();
                this.adView = null;
            }
#endif
        }

        Tween _tweenLoadInterstitialFB = null;
        protected void LoadInterstitialFB()
        {
#if DEFINE_FACEBOOK_ADS
            if (_tweenLoadInterstitialFB != null)
            {
                _tweenLoadInterstitialFB.Kill();
                _tweenLoadInterstitialFB = null;
            }
            if (this.interstitialAd != null)
            {
                this.interstitialAd.Dispose();
                this.interstitialAd = null;
            }

            if (!IsEnableFacebookAd()) return;

            var id = adConfigData.facebookAdConfig.GetInterstitialId();
            if (string.IsNullOrEmpty(id))
            {
                Debug.Log("@LOG LoadInterstitialFB can't find id interstitial");
                return;
            }
            Debug.Log("@LOG LoadInterstitialFB id:" + id);
            this.interstitialAd = new InterstitialAd(id);
            this.interstitialAd.Register(this.gameObject);

            // Set delegates to get notified on changes or when the user interacts with the ad.
            this.interstitialAd.InterstitialAdDidLoad = (delegate ()
            {
                Debug.Log("@LOG Facebook Ads Interstitial ad loaded.");
                this._isLoadInterstitialFbAds = true;
            });
            interstitialAd.InterstitialAdDidFailWithError = (delegate (string error)
            {
                Debug.Log("@LOG Facebook Ads Interstitial ad failed to load with error: " + error);
                _tweenLoadInterstitialFB = DOVirtual.DelayedCall(1, () => { LoadInterstitialFB(); });
            });
            interstitialAd.InterstitialAdWillLogImpression = (delegate ()
            {
                Debug.Log("@LOG Facebook Ads Interstitial ad logged impression.");
            });
            interstitialAd.InterstitialAdDidClick = (delegate ()
            {
                Debug.Log("@LOG Facebook Ads Interstitial ad clicked.");
            });

            this.interstitialAd.interstitialAdDidClose = (delegate ()
            {
                Debug.Log("@LOG Facebook Ads Interstitial ad did close.");
                OnInterstitialAdClosed?.Invoke(EInterstitialAdNetwork.FacebookAudience);
                this._didCloseInterstitialFbAds = true;
                if (this.interstitialAd != null)
                {
                    this.interstitialAd.Dispose();
                    this.interstitialAd = null;
                }
                LoadInterstitialFB();
            });

#if UNITY_ANDROID
            /*
             * Only relevant to Android.
             * This callback will only be triggered if the Interstitial activity has
             * been destroyed without being properly closed. This can happen if an
             * app with launchMode:singleTask (such as a Unity game) goes to
             * background and is then relaunched by tapping the icon.
             */
            this.interstitialAd.interstitialAdActivityDestroyed = (delegate ()
            {
                if (!this._didCloseInterstitialFbAds)
                {
                    Debug.Log("@LOG Facebook Ads Interstitial activity destroyed without being closed first.");
                    Debug.Log("@LOG Facebook Ads Game should resume.");
                }
            });
#endif

            // Initiate the request to load the ad.
            this.interstitialAd.LoadAd();
#endif
        }

        protected bool IsInterstitialFBReady()
        {
#if DEFINE_FACEBOOK_ADS
            if (this._isLoadInterstitialFbAds)
            {
                return true;
            }
#endif
            return false;
        }

        protected bool ShowInterstitialFB()
        {
#if DEFINE_FACEBOOK_ADS
            if (this._isLoadInterstitialFbAds)
            {
                Debug.Log("@LOG Facebook Ads Interstitial ShowInterstitialFB!");
                this.interstitialAd.Show();
                this._isLoadInterstitialFbAds = false;
                return true;
            }
            else
            {
                Debug.Log("@LOG Facebook Ads Interstitial Ad not loaded!");
                LoadInterstitialFB();
                return false;
            }
#endif
            return false;
        }

        Tween _tweenLoadRewardedFB = null;
        protected void LoadRewardedFB()
        {
#if DEFINE_FACEBOOK_ADS
            if(_tweenLoadRewardedFB != null)
            {
                _tweenLoadRewardedFB.Kill();
                _tweenLoadRewardedFB = null;
            }
            if (this.rewardedVideoAd != null)
            {
                this.rewardedVideoAd.Dispose();
                this.rewardedVideoAd = null;
            }

            if (!IsEnableFacebookAd()) return;

            var id = adConfigData.facebookAdConfig.GetRewardId();
            if (string.IsNullOrEmpty(id))
            {
                Debug.Log("@LOG LoadRewardedFB can't find id reward");
                return;
            }
            Debug.Log("@LOG LoadRewardedFB id:" + id);
            // Create the rewarded video unit with a placement ID (generate your own on the Facebook app settings).
            // Use different ID for each ad placement in your app.
            this.rewardedVideoAd = new RewardedVideoAd(id);

            this.rewardedVideoAd.Register(this.gameObject);

            // Set delegates to get notified on changes or when the user interacts with the ad.
            this.rewardedVideoAd.RewardedVideoAdDidLoad = (delegate ()
            {
                Debug.Log("@LOG Facebook Ads RewardedVideo ad loaded.");
                this._isLoadRewardFbAds = true;
            });
            this.rewardedVideoAd.RewardedVideoAdDidFailWithError = (delegate (string error)
            {
                Debug.Log("@LOG Facebook Ads RewardedVideo ad failed to load with error: " + error);
                _tweenLoadRewardedFB = DOVirtual.DelayedCall(3, () => { LoadRewardedFB(); });
            });
            this.rewardedVideoAd.RewardedVideoAdWillLogImpression = (delegate ()
            {
                Debug.Log("@LOG Facebook Ads RewardedVideo ad logged impression.");
            });
            this.rewardedVideoAd.RewardedVideoAdDidClick = (delegate ()
            {
                Debug.Log("@LOG Facebook Ads RewardedVideo ad clicked.");
            });

            this.rewardedVideoAd.rewardedVideoAdDidClose = (delegate ()
            {
                OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.FacebookAudience, "Facebook", 0.02);

                Debug.Log("@LOG Facebook Ads Rewarded video ad did close.");
                this._didCloseRewardFbAds = true;
                if (this.rewardedVideoAd != null)
                {
                    this.rewardedVideoAd.Dispose();
                    this.rewardedVideoAd = null;
                }
                LoadRewardedFB();
            });

#if UNITY_ANDROID
            /*
             * Only relevant to Android.
             * This callback will only be triggered if the Rewarded Video activity
             * has been destroyed without being properly closed. This can happen if
             * an app with launchMode:singleTask (such as a Unity game) goes to
             * background and is then relaunched by tapping the icon.
             */
            this.rewardedVideoAd.rewardedVideoAdActivityDestroyed = (delegate ()
            {
                if (!this._didCloseRewardFbAds)
                {
                    Debug.Log("@LOG Facebook Ads Rewarded video activity destroyed without being closed first.");
                    Debug.Log("@LOG Facebook Ads Game should resume. User should not get a reward.");
                }
            }); ;
#endif

            // Initiate the request to load the ad.
            this.rewardedVideoAd.LoadAd();
#endif
        }

        protected bool IsRewardFbAdsReady()
        {
#if DEFINE_FACEBOOK_ADS
            if (this._isLoadRewardFbAds && this.rewardedVideoAd)
            {
                return true;
            }
            else
            {
                Debug.Log("@LOG Facebook Ads Ad not loaded. Click load to request an ad.");
                LoadRewardedFB();
            }
#endif
            return false;
        }

        protected bool ShowRewardFB()
        {
#if DEFINE_FACEBOOK_ADS
            if (IsRewardFbAdsReady())
            {
            	this.rewardedVideoAd.Show();
                this._isLoadRewardFbAds = false;
            }
#endif
            return false;
        }
    }
}