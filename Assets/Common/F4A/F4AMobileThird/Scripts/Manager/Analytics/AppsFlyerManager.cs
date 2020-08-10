namespace com.F4A.MobileThird
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AppsFlyerManager : SingletonMono<AppsFlyerManager>
    {
        [SerializeField]
        private string _appKey;
        [SerializeField]
        private string _appIosId;
        [SerializeField]
        private bool _isDebug = false;
        private void Start()
        {
#if DEFINE_APPFLYER
            /* Mandatory - set your AppsFlyer’s Developer key. */
            AppsFlyer.setAppsFlyerKey(_appKey);
            //For detailed logging
            AppsFlyer.setIsDebug(_isDebug);
#if UNITY_IOS
            /* Mandatory - set your apple app ID
            NOTE: You should enter the number only and not the "ID" prefix */
            AppsFlyer.setAppID(_appIosId);
            AppsFlyer.getConversionData();

            AppsFlyer.getConversionData ();
            AppsFlyer.trackAppLaunch ();

            //// register to push notifications for iOS uninstall
            //UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert
            //    | UnityEngine.iOS.NotificationType.Badge
            //    | UnityEngine.iOS.NotificationType.Sound);
#elif UNITY_ANDROID
            AppsFlyer.loadConversionData("StartUp");
            /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
            AppsFlyer.init(_appKey, "AppsFlyerTrackerCallbacks");
            AppsFlyer.setAppID(Application.identifier);
            // for getting the conversion data
            //To get the callbacks
            AppsFlyer.createValidateInAppListener("AppsFlyerTrackerCallbacks", "onInAppBillingSuccess", "onInAppBillingFailure");
#endif
            //AppsFlyer.validateReceipt(string publicKey, string purchaseData, string signature, string price, string currency, Dictionary additionalParameters);
#endif
        }

        private bool tokenSent = false;
        private void Update()
        {
#if DEFINE_APPFLYER
#if UNITY_IOS
            //if (!tokenSent) { // tokenSent needs to be defined somewhere (bool tokenSent = false)
            //    byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
            //    if (token != null)
            //    {
            //        AppsFlyer.registerUninstall(token);
            //        tokenSent = true;
            //    }
            //}
#endif
#endif
        }

        public void EventMeasureUninstalls(string token)
        {
#if DEFINE_APPFLYER
#if UNITY_ANDROID
            AppsFlyer.updateServerUninstallToken(token);
#endif
#endif
        }

        public void EventAFPurchase(Dictionary<string, string> purchaseEvent)
        {
#if DEFINE_APPFLYER
            AppsFlyer.trackRichEvent("af_purchase", purchaseEvent);
#endif
        }

        public void EventCancelPurchase(Dictionary<string, string> purchaseEvent)
        {
#if DEFINE_APPFLYER
            //purchaseEvent.Add(AFInAppEvents.CURRENCY, "USD");
            //purchaseEvent.Add(AFInAppEvents.REVENUE, "-200");
            //purchaseEvent.Add(AFInAppEvents.QUANTITY, "1");
            //purchaseEvent.Add(AFInAppEvents.CONTENT_TYPE, "category_a");
            AppsFlyer.trackRichEvent("cancel_purchase", purchaseEvent);
#endif
        }

        public void EventCustom(string eventName, Dictionary<string, string> events)
        {
#if DEFINE_APPFLYER
            try
            {
                AppsFlyer.trackRichEvent(eventName, events);
            }
            catch (Exception ex)
            {
                AndroidNativeFunctions.ShowToast("EventCustom ex:" + ex.Message);
            }
#endif
        }
    }
}