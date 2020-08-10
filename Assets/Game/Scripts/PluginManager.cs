using com.F4A.MobileThird;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginManager : FiveSingleton<PluginManager>
{
    private Action _onRewardCompleted, _onRewardFailed;

    private void OnEnable()
    {
        AdsManager.OnRewardedAdCompleted += AdsManager_OnRewardedAdCompleted;
        AdsManager.OnRewardedAdFailed += AdsManager_OnRewardedAdFailed;
        AdsManager.OnRewardedAdSkiped += AdsManager_OnRewardedAdSkiped;
    }

    private void OnDisable()
    {
        AdsManager.OnRewardedAdCompleted -= AdsManager_OnRewardedAdCompleted;
        AdsManager.OnRewardedAdFailed -= AdsManager_OnRewardedAdFailed;
        AdsManager.OnRewardedAdSkiped -= AdsManager_OnRewardedAdSkiped;
    }

    private void AdsManager_OnRewardedAdCompleted(ERewardedAdNetwork adNetwork, string adName, double value)
    {
        _onRewardCompleted?.Invoke();
    }

    private void AdsManager_OnRewardedAdSkiped(ERewardedAdNetwork adNetwork)
    {
        _onRewardFailed?.Invoke();
    }

    private void AdsManager_OnRewardedAdFailed(ERewardedAdNetwork adNetwork)
    {
        _onRewardFailed?.Invoke();
    }


    public bool IsRewardAdsReady()
    {
        return AdsManager.Instance.IsRewardAdsReady();
    }

    public void ShowRewardAds(Action onCompleted, Action onFailed)
    {
        _onRewardCompleted = onCompleted;
        _onRewardFailed = onFailed;

        if(AdsManager.Instance.IsRewardAdsReady())
        {
            AdsManager.Instance.ShowRewardAds();
        }
        else
        {
            _onRewardFailed?.Invoke();
        }
    }
}
