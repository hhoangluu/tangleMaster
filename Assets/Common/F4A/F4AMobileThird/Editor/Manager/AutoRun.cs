using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace com.F4A.MobileThird
{
    [InitializeOnLoad]
    public class AutoRun
    {
        static AutoRun()
        {
            //EditorApplication.update += RunOnce;
        }

        static void RunOnce()
        {
            //EditorApplication.update -= RunOnce;

            if (EditorPrefs.GetBool("AlreadyOpened"))
                return;

            Debug.Log("com.F4A.MobileThird.AutoRun RunOnce");
            string defines = GetDefines();

            string[] array = defines.Split(';');
            defines = "";

            //if (array.Where(define => define.Equals("F4A_MOBILE")).FirstOrDefault() == null)
            //{
            //    defines += ";" + "F4A_MOBILE";
            //}

            array.ToList().ForEach(define =>
            {
	            if (!define.Equals(DMCMobileUtils.DEFINE_FACEBOOK_SDK)
                    && !define.Equals(DMCMobileUtils.DEFINE_GAMESERVICES))
                {
                    defines += define + ";";
                }
            });

            if (Directory.Exists("Assets/FacebookSDK"))
            {
                defines += "DEFINE_FACEBOOK_SDK" + ";";
            }
#if UNITY_ANDROID
            if (Directory.Exists("Assets/GooglePlayGames"))
            {
                defines += "LEADERBOARD" + ";";
            }
#endif

            SetDefines(defines);

            EditorPrefs.SetBool("AlreadyOpened", true);
        }


        static string GetDefines()
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

        static void SetDefines(string defines)
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
    }
}
