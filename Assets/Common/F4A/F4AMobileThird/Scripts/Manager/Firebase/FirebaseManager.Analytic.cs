namespace com.F4A.MobileThird
{
#if DEFINE_FIREBASE_ANALYTIC
    using Firebase.Analytics;
#endif
    using System;
    using System.Collections.Generic;
    //using System.Threading.Tasks;
    using UnityEngine;

    public partial class FirebaseManager
    {
        protected void InitializeAnalytics()
        {
#if DEFINE_FIREBASE_ANALYTIC
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            Debug.Log("Set user properties.");
            // Set the user's sign up method.
            FirebaseAnalytics.SetUserProperty(
              FirebaseAnalytics.UserPropertySignUpMethod,
              "Google");
            // Set the user ID.
            FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
            // Set default session duration values.
            //FirebaseAnalytics.SetMinimumSessionDuration(new TimeSpan(0, 0, 10));
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

            AnalyticsLogin();
#endif
        }

        public void LogEvent(string name, string parameterName, long parameterValue)
        {
#if DEFINE_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
#endif
        }
        public void LogEvent(string name, string parameterName, int parameterValue)
        {
#if DEFINE_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
#endif
        }
        public void LogEvent(string name)
        {
#if DEFINE_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(name);
#endif
        }
        public void LogEvent(string name, string parameterName, string parameterValue)
        {
#if DEFINE_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
#endif
        }
        public void LogEvent(string name, string parameterName, double parameterValue)
        {
#if DEFINE_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
#endif
        }

        public void LogEvent(string nameEvent, Dictionary<string, string> values)
        {
#if DEFINE_FIREBASE_ANALYTIC
            List<Parameter> parameters = new List<Parameter>();
            foreach (var pair in values)
            {
                parameters.Add(new Parameter(pair.Key, pair.Value.ToString()));
            }
            
            FirebaseAnalytics.LogEvent(nameEvent, parameters.ToArray());
#endif
        }

        public void LogEvent(string nameEvent, Dictionary<string, object> values)
        {
#if DEFINE_FIREBASE_ANALYTIC
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (var pair in values)
            {
                if (pair.Value != null) parameters[pair.Key] = pair.Value.ToString();
                else parameters[pair.Key] = string.Empty;
            }

            LogEvent(nameEvent, parameters);
#endif
        }

        public void LogEventWithInfoCountry(string name, Dictionary<string, object> values)
        {
#if DEFINE_FIREBASE_ANALYTIC
            List<Parameter> parameters = new List<Parameter>();
            foreach (var pair in values)
            {
                parameters.Add(new Parameter(pair.Key, pair.Value.ToString()));
            }
            
            FirebaseAnalytics.LogEvent(name, parameters.ToArray());
#endif
        }
        

        //#if DEFINE_FIREBASE_ANALYTIC
        //        public void LogEvent(EFirebaseEventAnalytic eventAnalytic, params Parameter[] parameters)
        //        {
        //            switch (eventAnalytic)
        //            {
        //                case EFirebaseEventAnalytic.EventPostScore:
        //                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPostScore, parameters);
        //                    break;
        //                case EFirebaseEventAnalytic.EventLogin:
        //                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin, parameters);
        //                    break;
        //                case EFirebaseEventAnalytic.EventLevelUp:
        //                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp, parameters);
        //                    break;
        //                case EFirebaseEventAnalytic.EventJoinGroup:
        //                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventJoinGroup, parameters);
        //                    break;
        //            }
        //        }
        //#endif

        public void AnalyticsLogin()
        {
#if DEFINE_FIREBASE_ANALYTIC
            // Log an event with no parameters.
            Debug.Log("Logging a login event.");
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
#endif
        }

        public void AnalyticsAppOpen()
        {
#if DEFINE_FIREBASE_ANALYTIC
            // Log an event with no parameters.
            Debug.Log("Logging a login event.");
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
#endif
        }

        public void AnalyticsLevelStart(int level)
        {
#if DEFINE_FIREBASE_ANALYTIC
            // Log an event with no parameters.
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, "level", level);
#endif
        }

        public void AnalyticsLevelEnd(int level)
        {
#if DEFINE_FIREBASE_ANALYTIC
            // Log an event with no parameters.
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, "level", level);
#endif
        }

        public void AnalyticOpenScene(string sceneName)
	    {
#if DEFINE_FIREBASE_ANALYTIC
		    FirebaseAnalytics.LogEvent("OpenScene", "scene_name", sceneName);
#endif
        }

        public void AnalyticsScore(int score)
        {
#if DEFINE_FIREBASE_ANALYTIC
            // Log an event with an int parameter.
            Debug.Log("Logging a post-score event.");
            FirebaseAnalytics.LogEvent(
              FirebaseAnalytics.EventPostScore,
              FirebaseAnalytics.ParameterScore,
              score);
#endif
        }

        public void AnalyticsLevelUp(int level, string parameterCharacter)
        {
#if DEFINE_FIREBASE_ANALYTIC
            // Log an event with multiple parameters.
            Debug.Log("Logging a level up event.");
            FirebaseAnalytics.LogEvent(
              FirebaseAnalytics.EventLevelUp,
              new Parameter(FirebaseAnalytics.ParameterLevel, level),
              new Parameter(FirebaseAnalytics.ParameterCharacter, parameterCharacter));
#endif
        }

        // Reset analytics data for this app instance.
        public void ResetAnalyticsData()
        {
#if DEFINE_FIREBASE_ANALYTIC
            Debug.Log("Reset analytics data.");
            FirebaseAnalytics.ResetAnalyticsData();
#endif
        }


//        // Get the current app instance ID.
//        public Task<string> AsyncDisplayAnalyticsInstanceId()
//        {
//#if DEFINE_FIREBASE_ANALYTIC
//            return FirebaseAnalytics.GetAnalyticsInstanceIdAsync().ContinueWith(task =>
//            {
//                if (task.IsCanceled)
//                {
//                    Debug.Log("App instance ID fetch was canceled.");
//                }
//                else if (task.IsFaulted)
//                {
//                    Debug.Log(String.Format("Encounted an error fetching app instance ID {0}",
//                                            task.Exception.ToString()));
//                }
//                else if (task.IsCompleted)
//                {
//                    Debug.Log(String.Format("App instance ID: {0}", task.Result));
//                }
//                return task;
//            }).Unwrap();
//#else
//            return null;
//#endif
//        }
    }
}