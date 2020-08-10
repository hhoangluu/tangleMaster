//using System;
//using UnityEngine;

//#if F4A_MOBILE
//using com.F4A.MobileThird;
//#endif

//public class F4AManager : MonoBehaviour
//    {
//	    private static F4AManager instance = null;
//	    public static F4AManager Instance
//	    {
//		    get { return instance; }
//		    protected set
//		    {
//			    instance = value;
//		    }
//	    }
	    
//	    #region Unity Method
//	    private void Awake()
//	    {
//		    if (Instance != null)
//		    {
//			    Destroy(gameObject);
//		    }
//		    else
//		    {
//			    Instance = this;
//			    DontDestroyOnLoad(gameObject);
//		    }
//	    }
//	    #endregion

//	    public void ShowInterstitialAd()
//	    {
//#if F4A_MOBILE
//            AdsManager.Instance.ShowInterstitialAds();
//#endif
//	    }

//	    public bool IsRewardAdReady()
//	    {
//#if F4A_MOBILE
//		    return AdsManager.Instance.IsRewardAdsReady();
//#else
//		    return false;
//#endif

//	    }

//	    public void ShowRewardAd()
//	    {
//#if F4A_MOBILE
//		    AdsManager.Instance.ShowRewardAds();
//#endif
//	    }
//    }