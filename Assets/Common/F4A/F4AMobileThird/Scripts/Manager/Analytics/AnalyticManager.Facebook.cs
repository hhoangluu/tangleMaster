namespace com.F4A.MobileThird
{
#if DEFINE_FACEBOOK_SDK
    using Facebook.Unity;
#endif
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class AnalyticManager
    {
        public void LogFacebookEvents(string appEventName, float? valueToSum = default(float?), Dictionary<string, object> parameters = null)
        {
#if DEFINE_FACEBOOK_SDK
            FB.LogAppEvent(appEventName, valueToSum, parameters);
#endif
        }

        public void LogFacebookEvents(string appEventName, float? valueToSum = default(float?), Dictionary<string, string> parameters = null)
        {
#if DEFINE_FACEBOOK_SDK
            var dic = new Dictionary<string, object>();
            foreach(var para in parameters)
            {
                dic[para.Key] = (object)para.Value;
            }

            LogFacebookEvents(appEventName, valueToSum, dic);
#endif
        }
    }
}
