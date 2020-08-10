namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Analytics;

    public partial class AnalyticManager : SingletonMono<AnalyticManager>
    {
        public void LoginEvent()
        {
#if DEFINE_UNITY_ANALYTIC
            Analytics.CustomEvent("Login");
#endif
        }

        public void CustomeEvent(string customEventName, IDictionary<string, object> eventData = null)
        {
#if DEFINE_UNITY_ANALYTIC
            if (eventData != null)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                foreach(var data in eventData)
                {
                    parameters[data.Key] = data.Value;
                }
                parameters["country"] = F4ACoreManager.Instance.ipComponent.country;
                parameters["city"] = F4ACoreManager.Instance.ipComponent.city;
                parameters["deviceModel"] = F4ACoreManager.Instance.ipComponent.deviceModel;
                parameters["countryCode"] = F4ACoreManager.Instance.ipComponent.countryCode;
                parameters["os"] = Application.platform.ToString();
                parameters["version"] = Application.version.ToString();
                Analytics.CustomEvent(customEventName, parameters);
            }
            else Analytics.CustomEvent(customEventName);
#endif
        }
    }
}