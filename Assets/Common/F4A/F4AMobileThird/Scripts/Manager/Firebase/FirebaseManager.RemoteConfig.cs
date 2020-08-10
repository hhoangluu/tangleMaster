namespace com.F4A.MobileThird
{
#if DEFINE_FIREBASE_REMOTECONFIG
    using Firebase.Extensions;
    using Firebase.RemoteConfig;
#endif
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;

    public enum ERemoteKey
    {
        ConfigScores,
    }


    public partial class FirebaseManager
    {
        private Dictionary<string, object> _remoteConfigs = new Dictionary<string, object>();
        public Dictionary<string, object> RemoteConfigs
        {
            get { return _remoteConfigs; }
        }

        public bool IsGetRemoteConfigs { get; set; }

        private void SetDefaultRemoteConfig()
        {
            // These are the values that are used if we haven't fetched data from the
            // server
            // yet, or if we ask for values that the server doesn't have:
            _remoteConfigs.Add(ERemoteKey.ConfigScores.ToString(), "1000, 1500, 2000");

#if DEFINE_FIREBASE_REMOTECONFIG
            FirebaseRemoteConfig.SetDefaults(_remoteConfigs);
#endif
        }

        private void InitializeRemote()
        {
#if DEFINE_FIREBASE_REMOTECONFIG
            FetchDataAsync();
#endif
        }


        public string GetValueRemote(string key)
        {
#if DEFINE_FIREBASE_REMOTECONFIG
            var str = FirebaseRemoteConfig.GetValue(key).StringValue;
            //AndroidNativeFunctions.ShowToast("@LOG GetValueRemote key:" + key + " - values:" + str);
            return str;
#endif
            return string.Empty;
        }

#if DEFINE_FIREBASE_REMOTECONFIG
        public Task FetchDataAsync()
        {
            Debug.Log("@LOG FetchDataAsync");
            // FetchAsync only fetches new data if the current data is older than the provided
            // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
            // By default the timespan is 12 hours, and for production apps, this is a good
            // number.  For this example though, it's set to a timespan of zero, so that
            // changes in the console will always show up immediately.
            Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(System.TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
#endif

        void FetchComplete(Task fetchTask)
        {
            /*
            if (fetchTask.IsCanceled)
            {
                AndroidNativeFunctions.ShowToast("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                AndroidNativeFunctions.ShowToast("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                AndroidNativeFunctions.ShowToast("Fetch completed successfully!");
            }
            */

#if DEFINE_FIREBASE_REMOTECONFIG
            var info = FirebaseRemoteConfig.Info;
            Debug.Log("@LOG FetchComplete info:" + info);
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.ActivateFetched();
                    MergeAllKeys();
                    // AndroidNativeFunctions.ShowToast(String.Format("Remote data loaded and ready (last fetch time {0}).",
                    //                        info.FetchTime));
                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            // AndroidNativeFunctions.ShowToast("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            // AndroidNativeFunctions.ShowToast("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    // AndroidNativeFunctions.ShowToast("Latest Fetch call still pending.");
                    break;
            }
#endif
        }

        public void MergeAllKeys()
        {
#if DEFINE_FIREBASE_REMOTECONFIG
            IEnumerable<string> keys = FirebaseRemoteConfig.Keys;
            foreach (string key in keys)
            {
                try
                {
                    if (RemoteConfigs.ContainsKey(key))
                    {
                        if (RemoteConfigs[key] is string)
                        {
                            RemoteConfigs[key] = FirebaseRemoteConfig.GetValue(key).StringValue;
                        }
                        else if (RemoteConfigs[key] is float)
                        {
                            RemoteConfigs[key] = (float)FirebaseRemoteConfig.GetValue(key).DoubleValue;
                        }
                        else if (RemoteConfigs[key] is int)
                        {
                            RemoteConfigs[key] = (int)FirebaseRemoteConfig.GetValue(key).LongValue;
                        }
                        else if (RemoteConfigs[key] is int)
                        {
                            RemoteConfigs[key] = FirebaseRemoteConfig.GetValue(key).StringValue;
                        }
                    }
                    else
                    {
                        RemoteConfigs[key] = FirebaseRemoteConfig.GetValue(key).StringValue;
                    }
                    Debug.Log("@LOG RemoteConfigs key: " + key + "/value:" + RemoteConfigs[key].ToString());
                }
                catch (Exception)
                {
                    Debug.Log("@LOG Invalid key: " + key);
                }
            }

            if (RemoteConfigs != null) Debug.Log("@LOG RemoteConfigs Lenght:" + RemoteConfigs.Count);
            IsGetRemoteConfigs = true;
#endif
        }
    }
}