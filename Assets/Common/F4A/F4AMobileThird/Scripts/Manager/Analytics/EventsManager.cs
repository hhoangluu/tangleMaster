namespace com.F4A.MobileThird
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EventsManager : SingletonMono<EventsManager>
    {
        public void LogEvent(string nameEvent, Dictionary<string, string> events, bool isAddInfoCountry = true)
        {
            try
            {
                Debug.Log("@LOG EventsManager LogEvent nameEvent:" + nameEvent);

                if(isAddInfoCountry)
                {
                    events.Add("country", F4ACoreManager.Instance.ipComponent.country);
                    events.Add("city", F4ACoreManager.Instance.ipComponent.city);
                    events.Add("deviceModel", F4ACoreManager.Instance.ipComponent.deviceModel);
                    events.Add("countryCode", F4ACoreManager.Instance.ipComponent.countryCode);
                    events.Add("os", Application.platform.ToString());
                    events.Add("version", Application.version);
                }

                FirebaseManager.Instance.LogEvent(nameEvent, events);
                AppsFlyerManager.Instance.EventCustom(nameEvent, events);
                AnalyticManager.Instance.LogFacebookEvents(nameEvent, parameters: events);
                MixpanelManager.Instance.LogEvent(nameEvent, events);
            }
            catch(Exception ex)
            {
                Debug.Log("@LOG EventsManager LogEvent ex:" + ex.Message);
            }
        }

        public void LogEvent(string nameEvent, Dictionary<string, object> events)
        {
            try
            {
                Debug.Log("@LOG EventsManager LogEvent nameEvent:" + nameEvent);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                if(events != null)
                {
                    foreach(var pair in events)
                    {
                        parameters[pair.Key] = pair.Value.ToString();
                    }
                }
                LogEvent(nameEvent, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log("@LOG EventsManager LogEvent ex:" + ex.Message);
            }
        }

        public void LogEvent(string nameEvent)
        {
            LogEvent(nameEvent, new Dictionary<string, string>());
        }

        public void LogEvent(string nameEvent, string paramName, string paramValue)
        {
            LogEvent(nameEvent, new Dictionary<string, string>() { { paramName, paramValue } });
        }
    }
}