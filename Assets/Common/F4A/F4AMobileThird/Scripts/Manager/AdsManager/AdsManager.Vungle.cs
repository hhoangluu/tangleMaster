using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.F4A.MobileThird
{
	public partial class AdsManager 
	{
		private bool _vungleInit = false;
		private bool IsEnableVungle()
		{
			return adConfigData != null && adConfigData.vungleConfig != null && adConfigData.vungleConfig.enableAd;
		}
		private bool InitVungle()
		{
#if DEFINE_VUNGLE
			Debug.Log("InitVungle");
			if (IsEnableVungle())
			{
				string appId = adConfigData.vungleConfig.GetAppId();
				Vungle.init(appId);
				Vungle.onInitializeEvent += Vungle_onInitializeEvent;

				Vungle.onAdStartedEvent += Vungle_onAdStartedEvent;
				Vungle.onAdFinishedEvent += Vungle_onAdFinishedEvent;
				Vungle.adPlayableEvent += Vungle_adPlayableEvent;
				return true;
			}
#endif
			return false;
		}

		private void VungleLoadAd()
		{
#if DEFINE_VUNGLE
			if (IsEnableVungle())
			{
				string placementID = adConfigData.vungleConfig.GetPlacementId();
				Vungle.loadAd(placementID);
			}
#endif
        }

        private void Vungle_onAdStartedEvent(string placementID)
        {
        }

        private void Vungle_adPlayableEvent(string placementID, bool adPlayable)
		{
			Debug.Log("Vungle Ad's playable state has been changed! placementID " + placementID + ". Now: " + adPlayable);
			adConfigData.vungleConfig.SetLoadAd(placementID, adPlayable);
		}

#if DEFINE_VUNGLE
		private void Vungle_onAdFinishedEvent(string placementId, AdFinishedEventArgs args)
		{
			Debug.Log("Ad finished - placementID " + placementId + ", was call to action clicked:" + args.WasCallToActionClicked + ", is completed view:"
					+ args.IsCompletedView);
			OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.Vungle, "vungle", DMCF4AConst.AmountAdsDefault);
			VungleLoadAd();
		}
#endif

		private void Vungle_onInitializeEvent()
		{
			_vungleInit = true;
			VungleLoadAd();
		}

		private bool IsRewardVungleReady()
		{
#if DEFINE_VUNGLE
			//return IsEnableVungle() && adConfigData.vungleConfig.IsLoadAd();
			return IsEnableVungle() && Vungle.isAdvertAvailable(adConfigData.vungleConfig.GetPlacementIdCurrent());
#endif
            return false;
		}

		private bool ShowRewardVungleAd()
		{
#if DEFINE_VUNGLE
			if (IsRewardVungleReady())
			{
				Vungle.playAd(adConfigData.vungleConfig.GetPlacementIdCurrent());
				return true;
			}
			else
			{
				VungleLoadAd();
				return false;
			}
#endif
            return false;
		}
	}
}