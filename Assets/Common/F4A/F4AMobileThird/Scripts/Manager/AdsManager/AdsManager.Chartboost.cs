using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DEFINE_CHARTBOOST
using ChartboostSDK;
#endif
using System;

namespace com.F4A.MobileThird
{
    public partial class AdsManager
    {
        private bool _isRewardChartboostReady = false;

        private void InitChartboost()
        {
#if DEFINE_CHARTBOOST
            Chartboost.didInitialize += Chartboost_didInitialize;

            Chartboost.didCacheRewardedVideo += Chartboost_didCacheRewardedVideo;
            Chartboost.didCloseRewardedVideo += Chartboost_didCloseRewardedVideo;
            Chartboost.didFailToLoadRewardedVideo += Chartboost_didFailToLoadRewardedVideo;
            Chartboost.didClickRewardedVideo += Chartboost_didClickRewardedVideo;
            Chartboost.didDismissRewardedVideo += Chartboost_didDismissRewardedVideo;
            Chartboost.didCompleteRewardedVideo += Chartboost_didCompleteRewardedVideo;

            //ChartboostSDK.CBSettings
#endif
        }

#if DEFINE_CHARTBOOST
        private void Chartboost_didInitialize(bool status)
        {
            if(IsEnableChartboostAd())
            {
                Chartboost.setAutoCacheAds(true);
                Chartboost.cacheInterstitial(CBLocation.Default);
                CacheRewardedChartboost();
            }
        }

        /// <summary>
		///   Called after a rewarded video has been loaded from the Chartboost API
		///   servers and cached locally. Implement to be notified of when a rewarded video has
		///	  been loaded from the Chartboost API servers and cached locally for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
        private void Chartboost_didCacheRewardedVideo(CBLocation location)
        {
            _isRewardChartboostReady = true;
        }

        private void Chartboost_didCloseRewardedVideo(CBLocation localtion)
        {
            _isRewardChartboostReady = false;
            CacheRewardedChartboost();
        }

        private void Chartboost_didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error)
        {
            _isRewardChartboostReady = false;
            CacheRewardedChartboost();
        }

        private void Chartboost_didClickRewardedVideo(CBLocation location)
        {
            _isRewardChartboostReady = false;
            CacheRewardedChartboost();
        }

        private void Chartboost_didDismissRewardedVideo(CBLocation location)
        {
            _isRewardChartboostReady = false;
            CacheRewardedChartboost();
        }

        private void Chartboost_didCompleteRewardedVideo(CBLocation location, int reward)
        {
            OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.Chartboost);
            _isRewardChartboostReady = false;
            CacheRewardedChartboost();
        }
#endif

        private bool IsEnableChartboostAd()
        {
            return adConfigData != null && adConfigData.chartboostConfigData != null && adConfigData.chartboostConfigData.EnableAd;
        }
        private void ReloadChartboostSetting()
        {
        }

        private void CacheRewardedChartboost()
        {
#if DEFINE_CHARTBOOST
            if (IsEnableChartboostAd())
            {
                Chartboost.cacheRewardedVideo(CBLocation.Default);
            }
#endif
        }

        private bool IsRewardChartboostReady()
        {
#if DEFINE_CHARTBOOST
            if (IsEnableChartboostAd())
            {
                return Chartboost.hasRewardedVideo(CBLocation.Default);
            }
#endif
            return false;
        }

        private bool ShowRewardCharstboost()
        {
#if DEFINE_CHARTBOOST
            if (IsRewardChartboostReady())
            {
                Chartboost.showRewardedVideo(CBLocation.Default);
                return true;
            }
#endif
            return false;
        }
    }
}