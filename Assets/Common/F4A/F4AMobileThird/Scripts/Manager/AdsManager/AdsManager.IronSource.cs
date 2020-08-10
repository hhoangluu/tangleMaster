namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class AdsManager
    {
        [SerializeField]
        private DMCIronSourceComponent ironSourceComp;
		
        private void InitIronSource()
        {
#if DEFINE_IRONSOURCE
            //IronSource.Agent.init(ironSourceComp.GetAppId(), IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
            //IronSource.Agent.init(ironSourceComp.GetAppId());
            IronSource.Agent.init(ironSourceComp.GetAppId(), IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
            IronSource.Agent.validateIntegration();
            //IronSource.Agent.setAdaptersDebug(true);
#endif

            if(!IsRemoveAds())
            {
                RegisterEventIronSourceBanner();
                LoadIronSourceBanner();
            }

            RegisterEventIronSourceReward();

            if (!IsRemoveAds())
            {
                RegisterEventIronSourceInterstitial();
                LoadIronSourceInterstitial();
            }
        }

        private void RegisterEventIronSourceReward()
        {
#if DEFINE_IRONSOURCE
            IronSourceEvents.onRewardedVideoAdOpenedEvent += IronSourceEvents_onRewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += IronSourceEvents_onRewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += IronSourceEvents_onRewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += IronSourceEvents_onRewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += IronSourceEvents_onRewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += IronSourceEvents_onRewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdLoadFailedDemandOnlyEvent += IronSourceEvents_onRewardedVideoAdLoadFailedDemandOnlyEvent;
#endif
        }

#if DEFINE_IRONSOURCE
        private void IronSourceEvents_onRewardedVideoAdLoadFailedDemandOnlyEvent(string arg1, IronSourceError arg2)
        {
            Debug.Log("@LOG_F4A IronSourceEvents_onRewardedVideoAdShowFailedEvent error:" + arg2.getDescription());
        }
        private void IronSourceEvents_onRewardedVideoAdOpenedEvent()
        {
        }
		
        private void IronSourceEvents_onRewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            OnRewardedAdFailed?.Invoke(ERewardedAdNetwork.IronSource);
            Debug.Log("@LOG_F4A IronSourceEvents_onRewardedVideoAdShowFailedEvent error:" + error.getDescription());
        }

        private void IronSourceEvents_onRewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.IronSource, placement.getRewardName(), placement.getRewardAmount());
        }

        private void IronSourceEvents_onRewardedVideoAdEndedEvent()
        {
        }

        private void IronSourceEvents_onRewardedVideoAdStartedEvent()
        {
        }
		
		private void IronSourceEvents_onRewardedVideoAdClosedEvent()
        {
            OnRewardedAdSkiped?.Invoke(ERewardedAdNetwork.IronSource);
        }
#endif


        private void RegisterEventIronSourceInterstitial()
        {
#if DEFINE_IRONSOURCE
            IronSourceEvents.onInterstitialAdReadyEvent += IronSourceEvents_onInterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += IronSourceEvents_onInterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += IronSourceEvents_onInterstitialAdShowSucceededEvent;
            //IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            //IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            //IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += IronSourceEvents_onInterstitialAdClosedEvent;
#endif
        }

        private void LoadIronSourceInterstitial()
        {
#if DEFINE_IRONSOURCE
            IronSource.Agent.loadInterstitial();
#endif
        }

        private bool IsIronSourceInterstitialReady()
        {
#if DEFINE_IRONSOURCE
            return IronSource.Agent.isInterstitialReady();
#endif
            return false;
        }

        private bool ShowIronSourceInterstitial()
        {
#if DEFINE_IRONSOURCE
            if (IsIronSourceInterstitialReady())
            {
                Debug.Log("@LOG ShowIronSourceInterstitial");
                IronSource.Agent.showInterstitial();
                return true;
            }
            else
            {
                LoadIronSourceInterstitial();
            }
#endif
            return false;
        }

#if DEFINE_IRONSOURCE
        private void IronSourceEvents_onInterstitialAdReadyEvent()
        {
        }

        private void IronSourceEvents_onInterstitialAdShowSucceededEvent()
        {
            //AndroidNativeFunctions.ShowToast("IronSourceEvents_onInterstitialAdShowSucceededEvent");
        }

        private void IronSourceEvents_onInterstitialAdClosedEvent()
        {
            //AndroidNativeFunctions.ShowToast("IronSourceEvents_onInterstitialAdClosedEvent");
            LoadIronSourceInterstitial();
            OnInterstitialAdClosed?.Invoke(EInterstitialAdNetwork.IronSource);
            //StartCoroutine(IEWaitForSeconds(0.1f, ()=>
            //{
            //    LoadIronSourceInterstitial();
            //}));
        }

        private void IronSourceEvents_onInterstitialAdLoadFailedEvent(IronSourceError error)
        {
        }
#endif

        private bool IsIronSourceRewardReady()
        {
#if DEFINE_IRONSOURCE
            Debug.Log("@LOG IsIronSourceRewardReady");
            return IronSource.Agent.isRewardedVideoAvailable();
#endif
            return false;
        }

        private bool ShowIronSourceReward()
        {
#if DEFINE_IRONSOURCE
            if (IsIronSourceRewardReady())
            {
                Debug.Log("@LOG ShowIronSourceReward");
                IronSource.Agent.showRewardedVideo();
                return true;
            }
#endif
            return false;
        }


        private void LoadIronSourceBanner()
        {
#if DEFINE_IRONSOURCE
            var size = new IronSourceBannerSize(Screen.width, 250);
            switch (adConfigData.AdPosition)
            {
                case EAdPosition.Top:
                   IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.TOP);
                   break;
                case EAdPosition.Bottom:
                   IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
                   break;

                // case EAdPosition.Top:
                //     IronSource.Agent.loadBanner(size, IronSourceBannerPosition.TOP);
                //     break;
                // case EAdPosition.Bottom:
                //     IronSource.Agent.loadBanner(size, IronSourceBannerPosition.BOTTOM);
                //     break;
            }
#endif
        }

        private void RegisterEventIronSourceBanner()
        {
#if DEFINE_IRONSOURCE
            IronSourceEvents.onBannerAdClickedEvent += IronSourceEvents_onBannerAdClickedEvent;
            IronSourceEvents.onBannerAdLoadedEvent += IronSourceEvents_onBannerAdLoadedEvent;
#endif
        }

        private void IronSourceEvents_onBannerAdClickedEvent()
        {
        }

        //Invoked once the banner has loaded
		private void IronSourceEvents_onBannerAdLoadedEvent() 
		{
            OnBannerAdLoad?.Invoke(EBannerAdNetwork.IronSource);
		}

        private bool IsIronSourceBannerReady()
        {
            return true;
        }

        private bool ShowIronSourceBanner()
        {
#if DEFINE_IRONSOURCE
            IronSource.Agent.displayBanner();
            OnBannerShow?.Invoke(EBannerAdNetwork.IronSource);
            return true;
#endif
            return false;
        }

        private bool HideIronSourceBanner()
        {
#if DEFINE_IRONSOURCE
            IronSource.Agent.hideBanner();
            return true;
#endif
            return false;
        }

        private bool DestroyIronSourceBanner()
        {
#if DEFINE_IRONSOURCE
            IronSource.Agent.destroyBanner();
            return true;
#endif
            return false;
        }
    }
}