namespace com.F4A.MobileThird
{
#if DEFINE_MIXPANEL
    using mixpanel;
#endif
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [HelpURL("https://developer.mixpanel.com/docs/unity")]
    public class MixpanelManager : SingletonMono<MixpanelManager>
    {
        public void LogEvent(string nameEvent, Dictionary<string, string> events)
        {
#if DEFINE_MIXPANEL
            var props = new Value();
            foreach(var e in events)
            {
                props[e.Key] = e.Value;
            }

            Mixpanel.Track(nameEvent, props);
#endif
        }
        public void LogEvent(string nameEvent)
        {
#if DEFINE_MIXPANEL
            Mixpanel.StartTimedEvent(nameEvent);
            Mixpanel.Track(nameEvent);
#endif
        }

        public void LogEvent(string nameEvent, string paramName, string paramValue)
        {
            LogEvent(nameEvent, new Dictionary<string, string>() { { paramName, paramValue } });
        }
    }
}