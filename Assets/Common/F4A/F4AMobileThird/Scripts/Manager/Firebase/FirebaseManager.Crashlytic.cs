namespace com.F4A.MobileThird
{
#if DEFINE_FIREBASE_CRASHLYTIC
    using Firebase;
    using Firebase.Crashlytics;
#endif
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class FirebaseManager
    {
        protected void InitializeCrashlytics()
        {
#if DEFINE_FIREBASE_CRASHLYTIC
            FirebaseApp.LogLevel = LogLevel.Error;
#endif
        }

        // Causes an error that will crash the app at the platform level (Android or iOS)
        public void ThrowUncaughtException()
        {
            Debug.Log("Causing a platform crash.");
            throw new InvalidOperationException("Uncaught exception created from UI.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void LogCaughtException()
        {
            Debug.Log("FirebaseManager.Crashlytic LogCaughtException Catching an logging an exception.");
#if DEFINE_FIREBASE_CRASHLYTIC
            try
            {
                throw new InvalidOperationException("This exception should be caught");
            }
            catch (Exception ex)
            {
                Crashlytics.LogException(ex);
            }
#endif
        }

        /// <summary>
        /// Write to the Crashlytics session log
        /// </summary>
        /// <param name="s"></param>
        public void WriteCustomLog(String s)
        {
            Debug.Log("Logging message to Crashlytics session: " + s);
#if DEFINE_FIREBASE_CRASHLYTIC
            Crashlytics.Log(s);
#endif
        }

        /// <summary>
        /// Add custom key / value pair to Crashlytics session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCustomKey(String key, String value)
        {
            Debug.Log("Setting Crashlytics Custom Key: <" + key + " / " + value + ">");
#if DEFINE_FIREBASE_CRASHLYTIC
            Crashlytics.SetCustomKey(key, value);
#endif
        }

        /// <summary>
        /// Set User Identifier for this Crashlytics session 
        /// </summary>
        /// <param name="id"></param>
        public void SetUserIdCrashlytic(String id)
        {
            Debug.Log("Setting Crashlytics user identifier: " + id);
#if DEFINE_FIREBASE_CRASHLYTIC
            Crashlytics.SetUserId(id);
#endif
        }
    }
}