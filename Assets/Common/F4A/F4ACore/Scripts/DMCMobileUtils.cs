#region Old
//using System.IO;
//using UnityEngine;
//using System;
//using DG.Tweening;

//namespace com.F4A.MobileThird
//{
//	using System.Collections;
//#if UNITY_EDITOR
//    using UnityEditor;
//#endif
//#if UNITY_2018_3_OR_NEWER
//    using UnityEngine.Networking;
//#endif

//    public static class DMCMobileUtils
//	{
//        #region Consts
//        public const string DEFINE_ADMOB = "DEFINE_ADMOB";
//        public const string DEFINE_UNITY_ADS = "DEFINE_UNITY_ADS";
//        public const string DEFINE_VUNGLE = "DEFINE_VUNGLE";
//        public const string DEFINE_CHARTBOOST = "DEFINE_CHARTBOOST";
//        public const string DEFINE_IAP = "DEFINE_IAP";
//        public const string DEFINE_FACEBOOK_SDK = "DEFINE_FACEBOOK_SDK";
//        public const string DEFINE_GAME_SERVICES = "DEFINE_GAMESERVICES";
//        public const string DEFINE_SHEPHERTZ = "DEFINE_SHEPHERTZ";
//        public const string DEFINE_FIREBASE_ANALYTIC = "DEFINE_FIREBASE_ANALYTIC";
//        public const string DEFINE_FIREBASE_CRASHLYTIC = "DEFINE_FIREBASE_CRASHLYTIC";

//        public const string F4A_ENABLE_UNITY_AD = "DEFINE_UNITY_ADS";
//		public const string F4A_ENABLE_ADMOB_AD = "F4A_ENABLE_ADMOB_AD";

//		public const int AD_DONT_REMOVE = 0;
//		public const int AD_REMOVE = 1;

//        // CoreData.Instance.infoData.SavePlayerCoin\((.+)\);
//        // CoreData.Instance.infoData.PlayerCoin = $1;

//        public const string KeyRemoveAds = "RemoveAds";

//        public const string KeySfx = "KeySfx", KeyMusic = "KeyMusic", KeyVibrate = "KeyVibrate";

//        //public static T[] Fill(this T[] array, T value) where T : struct
//        //{
//        //    // number of loop is Log(length arry, 2). ex: 1 -> 2 -> 4 -> 6
//        //    // Log return by value is not pow of 2 is a double (we need int), so we need round up it
//        //    double numberOfLoop = System.Math.Ceiling(System.Math.Log(array.Length, 2));
//        //    // Set value to first element
//        //    array[0] = value;
//        //    // Get size of value type
//        //    int sizeTypeValue = Marshal.SizeOf(value.GetType());
//        //    // start copy elements from 1 to log(array lenght, 2)
//        //    int currentPos = 1;            while (numberOfLoop > 1)
//        //    {
//        //        // Copy previous data to next data
//        //        Buffer.BlockCopy(array, 0, array, currentPos  sizeTypeValue, currentPos  sizeTypeValue);
//        //        // After copy, we shift left currentPos to next posion
//        //        currentPos = currentPos << 1;
//        //        numberOfLoop--;
//        //    }
//        //    // Copy last items with leghth is diff of array lenghth and log(array lenght, 2)
//        //    System.Array.Copy(array, 0, array, currentPos, array.Length - currentPos);
//        //    return array;
//        //}

//        public const string PathFolderData = @"Common/Data";
//        #endregion

//        public static string GetPlatformName()
//        {
//#if UNITY_EDITOR
//            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
//#else
//			return GetPlatformForAssetBundles(Application.platform);
//#endif
//        }

//#if UNITY_EDITOR
//        private static string GetPlatformForAssetBundles(BuildTarget target)
//        {
//            switch (target)
//            {
//                case BuildTarget.Android:
//                    return "Android";
//                case BuildTarget.iOS:
//                    return "iOS";
//                case BuildTarget.WebGL:
//                    return "WebGL";
//#if !UNITY_5_3_OR_NEWER
//                case BuildTarget.WebPlayer:
//				return "WebPlayer";
//#endif
//                case BuildTarget.StandaloneWindows:
//                case BuildTarget.StandaloneWindows64:
//                    return "Windows";
//#if !UNITY_2018_1_OR_NEWER
//                case BuildTarget.StandaloneOSXIntel:
//                case BuildTarget.StandaloneOSXIntel64:
//#endif
//#if UNITY_2017_3_OR_NEWER
//                case BuildTarget.StandaloneOSX:
//                    return "OSX";
//#endif
//                // Add more build targets for your own.
//                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
//                default:
//                    return null;
//            }
//        }
//#endif

//        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
//        {
//            switch (platform)
//            {
//                case RuntimePlatform.Android:
//                    return "Android";
//                case RuntimePlatform.IPhonePlayer:
//                    return "iOS";
//                case RuntimePlatform.WebGLPlayer:
//                    return "WebGL";
//#if !UNITY_5_3_OR_NEWER
//                case RuntimePlatform.OSXWebPlayer:
//				case RuntimePlatform.WindowsWebPlayer:
//				return "WebPlayer";
//#endif
//                case RuntimePlatform.WindowsPlayer:
//                    return "Windows";
//                case RuntimePlatform.OSXPlayer:
//                    return "OSX";
//                // Add more build targets for your own.
//                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
//                default:
//                    return null;
//            }
//        }



//        public static bool IsInternetAvailable()
//        {
//            return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
//        }

//        public static string GetStreamingAssetsPath()
//        {
//#if UNITY_EDITOR
//            //return Application.streamingAssetsPath; // Use the build output folder directly.
//            return Path.Combine(Directory.GetCurrentDirectory(), "StreamingAssets");
//#elif UNITY_WEBGL
//			return Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/")+ "/StreamingAssets";
//#elif UNITY_STANDALONE
//            //return Application.streamingAssetsPath;
//            return Application.dataPath + "/StreamingAssets";
//#elif UNITY_ANDROID
//            return "jar:file://" + Application.dataPath + "!/assets";
//#elif UNITY_IOS
//            return Application.dataPath + "/Raw";
//#else
//            return "file://" + Application.streamingAssetsPath;
//#endif
//        }

//		public static void CreateDirectory(string path){
//            DirectoryInfo directoryInfo = new DirectoryInfo(path);
//            if (!directoryInfo.Parent.Exists)
//            {
//                CreateDirectory(directoryInfo.Parent.FullName);
//            }
//            if (!Directory.Exists(path))
//            {
//                Directory.CreateDirectory(path);
//            }
//		}

//		public static T? ParseEnum<T> (string value) where T : struct, IConvertible
//		{
//			T? item = null;

//			foreach (T type in Enum.GetValues(typeof(T))) {
//				if (type.ToString ().ToLower ().Equals (value.Trim ().ToLower ())) {
//					item = type;
//					break;
//				}
//			}

//			return item;
//		}

//        public static void ClearTween(this Tween target)
//        {
//            if (target != null)
//            {
//                target.Kill();
//                target = null;
//            }
//        }

//		#region Double
//		public static void SetDouble(string key, double value)
//		{
//			PlayerPrefs.SetString(key, DoubleToString(value));
//		}

//		public static double GetDouble(string key, double defaultValue)
//		{
//			string defaultVal = DoubleToString(defaultValue);
//			return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
//		}

//		public static double GetDouble(string key)
//		{
//			return GetDouble(key, 0d);
//		}

//		private static string DoubleToString(double target)
//		{
//			return target.ToString("R");
//		}

//		private static double StringToDouble(string target)
//		{
//			if (string.IsNullOrEmpty(target))
//				return 0d;

//			return double.Parse(target);
//		}
//	    #endregion


//		public static double GetCurrentTime()
//		{
//			TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
//			return span.TotalSeconds;
//		}

//		public static double GetCurrentTimeInDays()
//		{
//			TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
//			return span.TotalDays;
//		}

//		public static double GetCurrentTimeInMills()
//		{
//			TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
//			return span.TotalMilliseconds;
//		}

//		public static T GetRandom<T>(params T[] arr)
//		{
//			return arr[UnityEngine.Random.Range(0, arr.Length)];
//		}

//		public static bool IsActionAvailable(String action, int time, bool availableFirstTime = true)
//		{
//			if (!PlayerPrefs.HasKey(action + "_time")) // First time.
//			{
//				if (availableFirstTime == false)
//				{
//					SetActionTime(action);
//				}
//				return availableFirstTime;
//			}

//			int delta = (int)(GetCurrentTime() - GetActionTime(action));
//			return delta >= time;
//		}

//		public static double GetActionDeltaTime(String action)
//		{
//			if (GetActionTime(action) == 0)
//				return 0;
//			return GetCurrentTime() - GetActionTime(action);
//		}

//		public static void SetActionTime(String action)
//		{
//			SetDouble(action + "_time", GetCurrentTime());
//		}

//		public static void SetActionTime(String action, double time)
//		{
//			SetDouble(action + "_time", time);
//		}

//		public static double GetActionTime(String action)
//		{
//			return GetDouble(action + "_time");
//		}

//		public static void OpenURL(string url){
//			if(!string.IsNullOrEmpty(url)){
//				#if UNITY_EDITOR
//				Application.OpenURL(url);
//			#elif UNITY_WEBGL
//				//Application.ExternalEval("window.open(\"" + url + "\",\"_blank\")");
//				openWindow(url);
//			#else
//				Application.OpenURL(url);
//			#endif
//			}
//		}

//#if UNITY_WEBGL
//		[DllImport("__Internal")]
//		private static extern void openWindow(string url);
//#endif



//        #region Get Data From Server
//        public static IEnumerator AsyncGetDataFromUrl(string url,
//#if UNITY_2018_3_OR_NEWER
//			Action<UnityWebRequest> action
//#else
//			Action<WWW> action
//#endif
//		)
//		{
//			if (!string.IsNullOrEmpty(url))
//			{
//#if UNITY_2018_3_OR_NEWER
//                UnityWebRequest www = UnityWebRequest.Get(url);
//                www.SendWebRequest();
//#else
//				WWW www = new WWW(url);
//#endif
//                if (action != null) action(www);
//			}
//			yield return null;
//		}

//		public static IEnumerator AsyncGetDataFromUrl(string url, Action<string> action)
//		{
//			string textData = "";
//			if (!string.IsNullOrEmpty(url))
//			{
//#if UNITY_2018_3_OR_NEWER
//                UnityWebRequest www = UnityWebRequest.Get(url);
//                www.SendWebRequest();
//#else
//				WWW www = new WWW(url);
//#endif

//                while (!www.isDone)
//				{
//					yield return null;
//				}
//				yield return www;

//				if (!string.IsNullOrEmpty(www.error))
//				{
//					textData = "";
//				}
//				else
//				{
//#if UNITY_2018_3_OR_NEWER
//					textData = www.downloadHandler.text;
//#else
//					textData = www.text;
//#endif
//				}
//			}
//			if (action != null) action(textData);
//			yield return null;
//		}

//		public static IEnumerator AsyncGetDataFromUrl(string url, TextAsset textAssetDefault, Action<string> action)
//		{
//			string textData = "";
//			if (!string.IsNullOrEmpty(url))
//			{
//#if UNITY_2018_3_OR_NEWER
//				UnityWebRequest www = UnityWebRequest.Get(url);
//                www.SendWebRequest();
//#else
//				WWW www = new WWW(url);
//#endif
//				while (!www.isDone)
//				{
//					yield return null;
//				}
//				yield return www;

//				if (!string.IsNullOrEmpty(www.error))
//				{
//					textData = "";
//				}
//				else
//				{
//#if UNITY_2018_3_OR_NEWER
//					//textData = www.downloadHandler.text;
//                    textData = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
//#else
//                    textData = System.Text.Encoding.UTF8.GetString(www.bytes);
//#endif
//                }
//			}
//			if(string.IsNullOrEmpty(textData) && textAssetDefault)
//			{
//				textData = textAssetDefault.text;
//			}

//			if (action != null) action(textData);
//			yield return null;
//		}

//        #endregion

//#if UNITY_EDITOR
//        #region Defines
//        public static string GetDefines()
//        {
//            string defines = "";

//#if UNITY_ANDROID
//            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
//#elif UNITY_IOS
//            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS);
//#elif UNITY_WEBGL
//            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
//#elif UNITY_STANDALONE
//            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
//#else
//#endif
//            return defines;
//        }

//        public static void SetDefines(string defines)
//        {
//#if UNITY_ANDROID
//            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
//#elif UNITY_IOS
//            PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, defines);
//#elif UNITY_WEBGL
//            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, defines);
//#elif UNITY_STANDALONE
//            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
//#else
//#endif
//        }
//        #endregion
//#endif
//    }
//}

#endregion

namespace com.F4A.MobileThird
{
	using System.IO;
	using UnityEngine;
	using System;
	using DG.Tweening;
    using System.Collections;
#if UNITY_EDITOR
    using UnityEditor;
#endif
#if UNITY_2018_3_OR_NEWER
    using UnityEngine.Networking;
#endif

    public static class DMCMobileUtils
    {
        #region Consts
        public const string DEFINE_ADCOLONY = "DEFINE_ADCOLONY";
        public const string DEFINE_ADMOB = "DEFINE_ADMOB";
        public const string DEFINE_AD_NATIVE = "DEFINE_AD_NATIVE";
        public const string DEFINE_UNITY_ADS = "DEFINE_UNITY_ADS";
        public const string DEFINE_VUNGLE = "DEFINE_VUNGLE";
        public const string DEFINE_CHARTBOOST = "DEFINE_CHARTBOOST";
        public const string DEFINE_IRONSOURCE = "DEFINE_IRONSOURCE";
        public const string DEFINE_WEBVIEW = "DEFINE_WEBVIEW";
        public const string DEFINE_FACEBOOK_ADS = "DEFINE_FACEBOOK_ADS";

        public const string DEFINE_IAP = "DEFINE_IAP";

        public const string DEFINE_FACEBOOK_SDK = "DEFINE_FACEBOOK_SDK";
        public const string DEFINE_GAMESERVICES = "DEFINE_GAMESERVICES";

        public const string DEFINE_SHEPHERTZ = "DEFINE_SHEPHERTZ";
        public const string DEFINE_UNITY_ANALYTIC = "DEFINE_UNITY_ANALYTIC";

        public const string DEFINE_FIREBASE_ANALYTIC = "DEFINE_FIREBASE_ANALYTIC";
        public const string DEFINE_FIREBASE_CRASHLYTIC = "DEFINE_FIREBASE_CRASHLYTIC";
        public const string DEFINE_FIREBASE_MESSAGING = "DEFINE_FIREBASE_MESSAGING";
        public const string DEFINE_FIREBASE_AUTH = "DEFINE_FIREBASE_AUTH";
        public const string DEFINE_FIREBASE_REMOTECONFIG = "DEFINE_FIREBASE_REMOTECONFIG";

        public const string DEFINE_APPFLYER = "DEFINE_APPFLYER";
        public const string DEFINE_MIXPANEL = "DEFINE_MIXPANEL";

        public const int AD_DONT_REMOVE = 0;
        public const int AD_REMOVE = 1;

        // CoreData.Instance.infoData.SavePlayerCoin\((.+)\);
        // CoreData.Instance.infoData.PlayerCoin = $1;
        
        public const string KeyRemoveAdsOld = "RemoveAds";
        public const string KeyRemoveAds = "F4A_RemoveAds";

        public const string KeySfx = "KeySfx", KeyMusic = "KeyMusic", KeyVibrate = "KeyVibrate";

        //public static T[] Fill(this T[] array, T value) where T : struct
        //{
        //    // number of loop is Log(length arry, 2). ex: 1 -> 2 -> 4 -> 6
        //    // Log return by value is not pow of 2 is a double (we need int), so we need round up it
        //    double numberOfLoop = System.Math.Ceiling(System.Math.Log(array.Length, 2));
        //    // Set value to first element
        //    array[0] = value;
        //    // Get size of value type
        //    int sizeTypeValue = Marshal.SizeOf(value.GetType());
        //    // start copy elements from 1 to log(array lenght, 2)
        //    int currentPos = 1;            while (numberOfLoop > 1)
        //    {
        //        // Copy previous data to next data
        //        Buffer.BlockCopy(array, 0, array, currentPos  sizeTypeValue, currentPos  sizeTypeValue);
        //        // After copy, we shift left currentPos to next posion
        //        currentPos = currentPos << 1;
        //        numberOfLoop--;
        //    }
        //    // Copy last items with leghth is diff of array lenghth and log(array lenght, 2)
        //    System.Array.Copy(array, 0, array, currentPos, array.Length - currentPos);
        //    return array;
        //}

        public const string PathFolderData = @"Common/Data";
        #endregion

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			return GetPlatformForAssetBundles(Application.platform);
#endif
        }

#if UNITY_EDITOR
        private static string GetPlatformForAssetBundles(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
#if !UNITY_5_3_OR_NEWER
                case BuildTarget.WebPlayer:
				return "WebPlayer";
#endif
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
#if !UNITY_2018_1_OR_NEWER
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
#endif
#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return "OSX";
#endif
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
#endif

        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
#if !UNITY_5_3_OR_NEWER
                case RuntimePlatform.OSXWebPlayer:
				case RuntimePlatform.WindowsWebPlayer:
				return "WebPlayer";
#endif
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }



        public static bool IsInternetAvailable()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }

        public static string GetStreamingAssetsPath()
        {
#if UNITY_EDITOR
            //return Application.streamingAssetsPath; // Use the build output folder directly.
            return Path.Combine(Directory.GetCurrentDirectory(), "StreamingAssets");
#elif UNITY_WEBGL
			return Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/")+ "/StreamingAssets";
#elif UNITY_STANDALONE
            //return Application.streamingAssetsPath;
            return Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IOS
            return Application.dataPath + "/Raw";
#else
            return "file://" + Application.streamingAssetsPath;
#endif
        }

        public static void CreateDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Parent.Exists)
            {
                CreateDirectory(directoryInfo.Parent.FullName);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static T? ParseEnum<T>(string value) where T : struct, IConvertible
        {
            T? item = null;

            foreach (T type in Enum.GetValues(typeof(T)))
            {
                if (type.ToString().ToLower().Equals(value.Trim().ToLower()))
                {
                    item = type;
                    break;
                }
            }

            return item;
        }

        public static void ClearTween(this Tween target)
        {
            if (target != null)
            {
                target.Kill();
                target = null;
            }
        }

        #region Double
        public static void SetDouble(string key, double value)
        {
            PlayerPrefs.SetString(key, DoubleToString(value));
        }

        public static double GetDouble(string key, double defaultValue)
        {
            string defaultVal = DoubleToString(defaultValue);
            return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
        }

        public static double GetDouble(string key)
        {
            return GetDouble(key, 0d);
        }

        private static string DoubleToString(double target)
        {
            return target.ToString("R");
        }

        private static double StringToDouble(string target)
        {
            if (string.IsNullOrEmpty(target))
                return 0d;

            return double.Parse(target);
        }
        #endregion


        public static double GetCurrentTime()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalSeconds;
        }

        public static double GetCurrentTimeInDays()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalDays;
        }

        public static double GetCurrentTimeInMills()
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalMilliseconds;
        }

        public static T GetRandom<T>(params T[] arr)
        {
            return arr[UnityEngine.Random.Range(0, arr.Length)];
        }

        public static bool IsActionAvailable(String action, int time, bool availableFirstTime = true)
        {
            if (!PlayerPrefs.HasKey(action + "_time")) // First time.
            {
                if (availableFirstTime == false)
                {
                    SetActionTime(action);
                }
                return availableFirstTime;
            }

            int delta = (int)(GetCurrentTime() - GetActionTime(action));
            return delta >= time;
        }

        public static double GetActionDeltaTime(String action)
        {
            if (GetActionTime(action) == 0)
                return 0;
            return GetCurrentTime() - GetActionTime(action);
        }

        public static void SetActionTime(String action)
        {
            SetDouble(action + "_time", GetCurrentTime());
        }

        public static void SetActionTime(String action, double time)
        {
            SetDouble(action + "_time", time);
        }

        public static double GetActionTime(String action)
        {
            return GetDouble(action + "_time");
        }

        public static void OpenURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
#if UNITY_WEBGL
				//Application.ExternalEval("window.open(\"" + url + "\",\"_blank\")");
				openWindow(url);
#else
                Application.OpenURL(url);
#endif
            }
        }

#if UNITY_WEBGL
		[DllImport("__Internal")]
		private static extern void openWindow(string url);
#endif



        #region Get Data From Server
        public static IEnumerator AsyncGetDataFromUrl(string url,
#if UNITY_2018_3_OR_NEWER
            Action<UnityWebRequest> action
#else
			Action<WWW> action
#endif
        )
        {
            if (!string.IsNullOrEmpty(url))
            {
#if UNITY_2018_3_OR_NEWER
                UnityWebRequest www = UnityWebRequest.Get(url);
                www.SendWebRequest();
#else
				WWW www = new WWW(url);
#endif
                if (action != null) action(www);
            }
            yield return null;
        }

        public static IEnumerator AsyncGetDataFromUrl(string url, Action<string> action)
        {
            string textData = "";
            if (!string.IsNullOrEmpty(url))
            {
#if UNITY_2018_3_OR_NEWER
                UnityWebRequest www = UnityWebRequest.Get(url);
                www.SendWebRequest();
#else
				WWW www = new WWW(url);
#endif

                while (!www.isDone)
                {
                    yield return null;
                }
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    textData = "";
                }
                else
                {
#if UNITY_2018_3_OR_NEWER
                    textData = www.downloadHandler.text;
#else
					textData = www.text;
#endif
                }
            }
            if (action != null) action(textData);
            yield return null;
        }

        public static IEnumerator AsyncGetDataFromUrl(string url, TextAsset textAssetDefault, Action<string> action)
        {
            string textData = "";
            if (!string.IsNullOrEmpty(url))
            {
#if UNITY_2018_3_OR_NEWER
                UnityWebRequest www = UnityWebRequest.Get(url);
                www.SendWebRequest();
#else
				WWW www = new WWW(url);
#endif
                while (!www.isDone)
                {
                    yield return null;
                }
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    textData = "";
                }
                else
                {
#if UNITY_2018_3_OR_NEWER
                    //textData = www.downloadHandler.text;
                    textData = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
#else
                    textData = System.Text.Encoding.UTF8.GetString(www.bytes);
#endif
                }
            }
            if (string.IsNullOrEmpty(textData) && textAssetDefault)
            {
                textData = textAssetDefault.text;
            }

            if (action != null) action(textData);
            yield return null;
        }

        #endregion

#if UNITY_EDITOR
        #region Defines
        public static string GetDefines()
        {
            string defines = "";

#if UNITY_ANDROID
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
#elif UNITY_IOS
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS);
#elif UNITY_WEBGL
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
#elif UNITY_STANDALONE
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
#else
#endif
            return defines;
        }

        public static void SetDefines(string defines)
        {
#if UNITY_ANDROID
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
#elif UNITY_IOS
            PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, defines);
#elif UNITY_WEBGL
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, defines);
#elif UNITY_STANDALONE
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
#else
#endif
        }
        #endregion
#endif
    }
}