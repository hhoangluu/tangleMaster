namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // AdsManager.AdColony
    public partial class AdsManager
    {
        [Header("ADCOLONY")]
        [SerializeField]
        private bool _isAdColonyReady = false;
#if DEFINE_ADCOLONY
        private AdColony.InterstitialAd _adColony = null;
#endif
        private List<string> _adColonyZoneIds = new List<string>();

        protected void InitAdColony()
        {
            if (IsEnableAdColony())
            {
                ConfigAdColony();
#if DEFINE_ADCOLONY
                AdColony.Ads.OnRequestInterstitial += AdColony_OnRequestInterstitial;
                AdColony.Ads.OnExpiring += AdColony_OnExpiring;
                AdColony.Ads.OnRewardGranted += AdColony_OnRewardGranted;
#endif
            }
        }

        private void ConfigAdColony()
        {
#if DEFINE_ADCOLONY
            if(IsEnableAdColony())
            {
                var app = adConfigData.adColonyConfigData.GetAppInfo();
                if (app != null && app.Zones.Length > 0)
                {
                    AdColony.AppOptions options = new AdColony.AppOptions();
                    // Indicates the GDPR requirement of the user. 
                    // If it's true, the user's subject to the GDPR laws. 
                    // If you set it to false, the value of consent string will be ignored.
                    options.GdprRequired = true;

                    // Your user's consent string. 
                    // In this case, the user has given consent to store and process personal information.
                    options.GdprConsentString = "1";

                    _adColonyZoneIds.Clear();
                    foreach (var zone in app.Zones)
                    {
                        _adColonyZoneIds.Add(zone.ZoneId);
                    }
                    AdColony.Ads.Configure(app.AppId, options, _adColonyZoneIds.ToArray());
                }
            }
#endif
        }

        private void AdColony_OnRewardGranted(string zone, bool success, string nameAd, int value)
        {
            OnRewardedAdCompleted?.Invoke(ERewardedAdNetwork.AdColony, nameAd, value);
        }

#if DEFINE_ADCOLONY
        private void AdColony_OnExpiring(AdColony.InterstitialAd ad)
        {
            _isAdColonyReady = false;
        }

        private void AdColony_OnRequestInterstitial(AdColony.InterstitialAd ad)
        {
            _adColony = ad;
            _isAdColonyReady = true;
        }
#endif

        private bool IsEnableAdColony()
        {
            return adConfigData != null && adConfigData.adColonyConfigData != null && adConfigData.adColonyConfigData.EnableAd;
        }

        private bool RequestAdColony()
        {
#if DEFINE_ADCOLONY
            if(IsEnableAdColony() && _adColonyZoneIds != null && _adColonyZoneIds.Count > 0)
            {
                string zoneId = _adColonyZoneIds[Random.Range(0, _adColonyZoneIds.Count)];
                AdColony.Ads.RequestInterstitialAd(zoneId, null);
                return true;
            }
#endif
            return false;
        }

        private bool IsAdColonyReady()
        {
#if DEFINE_ADCOLONY
            if (IsEnableAdColony())
            {
                return _adColony != null && _isAdColonyReady;
            }
#endif
            return false;
        }

        private bool ShowAdColony()
        {
#if DEFINE_ADCOLONY
            if (IsAdColonyReady())
            {
                AdColony.Ads.ShowAd(_adColony);
                return true;
            }
            else
            {
                RequestAdColony();
            }
            _isAdColonyReady = false;
#endif
            return false;
        }
    }
}