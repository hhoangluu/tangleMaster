using System.Collections;
using UnityEngine;

#if DEFINE_UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace com.F4A.MobileThird
{
    /// <summary>
    /// 
    /// </summary>
	public partial class AdsManager
#if DEFINE_UNITY_ADS
        : IUnityAdsListener
#endif
    {
        private void StartUnityAds()
        {
#if DEFINE_UNITY_ADS
            Advertisement.AddListener(this);
#endif
        }

        private void InitializationUnityAds()
	    {
            RequestUnityAds();
        }

        private bool IsEnableUnityAd()
        {
#if DEFINE_UNITY_ADS
            return adConfigData != null && Advertisement.isInitialized && _unityAppInfo != null
                && adConfigData.unityAdConfig != null && adConfigData.unityAdConfig.enableAd;
#endif
            return false;
        }

        DMCUnityAdsAppInfo _unityAppInfo = null;
        private void RequestUnityAds()
        {
#if DEFINE_UNITY_ADS
            if (adConfigData != null && adConfigData.unityAdConfig != null && adConfigData.unityAdConfig.enableAd)
            {
                _unityAppInfo = adConfigData.unityAdConfig.GetAppInfo();
                if (_unityAppInfo != null && !string.IsNullOrEmpty(_unityAppInfo.IdAd))
                {
                    Debug.Log("RequestUnityAds id: " + _unityAppInfo.IdAd);
                    //------------------ initialize Unity Ads. ----------------------//
                    if (Advertisement.isSupported)
                    { // If the platform is supported,
                        Advertisement.Initialize(_unityAppInfo.IdAd, adConfigData.unityAdConfig.testMode);
                    }
                }
            }
#endif
        }

        protected bool IsRewardedUnityAdReady()
        {
#if DEFINE_UNITY_ADS
            if (IsEnableUnityAd() && Advertisement.IsReady(_unityAppInfo.RewardPlacementId))
            {
                return true;
            }
#endif
            return false;
        }

        protected bool ShowRewardUnityAd()
        {
#if DEFINE_UNITY_ADS
            if (IsRewardedUnityAdReady())
            {
                Debug.Log("@LOG ShowRewardUnityAd id:" + _unityAppInfo.IdAd + "/RewardPlacementId:" + _unityAppInfo.RewardPlacementId);
                Advertisement.Show(_unityAppInfo.RewardPlacementId);
                return true;
            }
            return false;
#else
            return false;
#endif
        }

        protected bool IsVideoUnityAdReady()
        {
#if DEFINE_UNITY_ADS
            if (IsEnableUnityAd() && Advertisement.IsReady(_unityAppInfo.VideoPlacementId))
            {
                return true;
            }
#endif

            return false;
        }

        protected bool ShowVideoUnityAd()
        {
#if DEFINE_UNITY_ADS
            if (IsVideoUnityAdReady())
            {
                Debug.Log("@LOG ShowVideoUnityAd id:" + _unityAppInfo.IdAd + "/VideoPlacementId:" + _unityAppInfo.VideoPlacementId);
                Advertisement.Show(_unityAppInfo.VideoPlacementId);
                return true;
            }
            return false;
#else
            return false;
#endif
        }

        protected bool IsInterstitialUnityAdReady()
        {
#if DEFINE_UNITY_ADS
            if (IsEnableUnityAd())
            {
                if (Advertisement.IsReady(_unityAppInfo.InterstitialPlacementId))
                {
                    return true;
                }
                else if (Advertisement.IsReady(_unityAppInfo.VideoPlacementId))
                {
                    return true;
                }
            }
#endif
            return false;
        }

        protected bool ShowInterstitialUnityAd()
        {
#if DEFINE_UNITY_ADS
            if (IsInterstitialUnityAdReady())
            {
                Debug.Log("@LOG ShowInterstitialUnityAd id:" + _unityAppInfo.IdAd + "/InterstitialPlacementId:" + _unityAppInfo.InterstitialPlacementId);
                Advertisement.Show(_unityAppInfo.InterstitialPlacementId);
                return true;
            }
            else if (IsVideoUnityAdReady())
            {
                return ShowVideoUnityAd();
            }
#endif
            return false;
        }
        
	    protected bool IsBannerUnityAdReady()
	    {
#if DEFINE_UNITY_ADS
		    if (IsEnableUnityAd() && Advertisement.IsReady(_unityAppInfo.BannerPlacementId))
		    {
			    return true;
		    }
#endif
		    return false;
	    }
	    
	    protected bool ShowBannerUnityAd()
	    {
#if DEFINE_UNITY_ADS
		    if (IsBannerUnityAdReady())
		    {
                Debug.Log("@LOG ShowBannerUnityAd id:" + _unityAppInfo.IdAd + "/BannerPlacementId:" + _unityAppInfo.BannerPlacementId);
                Advertisement.Show(_unityAppInfo.BannerPlacementId);
			    return true;
		    }
#endif
		    return false;
	    }

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log("@LOG OnUnityAdsReady placementId:" + placementId);
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log("@LOG OnUnityAdsDidError message:" + message);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            Debug.Log("@LOG OnUnityAdsDidStart placementId:" + placementId);
            if (placementId.Equals(_unityAppInfo.RewardPlacementId))
            {
                _timeScale = Time.timeScale;
                Time.timeScale = 0;
            }
        }

#if DEFINE_UNITY_ADS
        public void OnUnityAdsDidFinish(string placementId, ShowResult result)
        {
            StartCoroutine(IEUnityAdsDidFinish(placementId, result));
        }

        IEnumerator IEUnityAdsDidFinish(string placementId, ShowResult result)
        {
            yield return new WaitForEndOfFrame();
            Debug.LogFormat("@LOG OnUnityAdsDidFinish {0},{1}", placementId, result.ToString());
            
            if (placementId.Equals(_unityAppInfo.RewardPlacementId))
            {
                if (_timeScale > 0) Time.timeScale = _timeScale;
                else Time.timeScale = 1;

                switch (result)
                {
                    case ShowResult.Finished:
                        OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.UnityAds, string.Empty, DMCF4AConst.AmountAdsDefault);
                        break;
                    case ShowResult.Skipped:
                        OnRewardedAdSkiped?.Invoke(ERewardedAdNetwork.UnityAds);
                        break;
                    case ShowResult.Failed:
                        OnRewardedAdFailed?.Invoke(ERewardedAdNetwork.UnityAds);
                        break;
                }
            }
            else if(placementId.Equals(_unityAppInfo.InterstitialPlacementId))
            {
                switch (result)
                {
                    case ShowResult.Finished:
                        OnInterstitialAdClosed?.Invoke(EInterstitialAdNetwork.UnityAds);
                        break;
                    case ShowResult.Skipped:
                        break;
                    case ShowResult.Failed:
                        break;
                }
            }
            else if(placementId.Equals(_unityAppInfo.VideoPlacementId))
            {
                switch (result)
                {
                    case ShowResult.Finished:
                        OnInterstitialAdClosed?.Invoke(EInterstitialAdNetwork.UnityAds);
                        break;
                    case ShowResult.Skipped:
                        break;
                    case ShowResult.Failed:
                        break;
                }
            }
            else
            {
                switch (result)
                {
                    case ShowResult.Finished:
                        break;
                    case ShowResult.Skipped:
                        break;
                    case ShowResult.Failed:
                        break;
                }
            }
        }
#endif
    }
}