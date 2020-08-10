using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace com.F4A.MobileThird
{
    public enum ETypePlatform
    {
        Android,// this is android arm vs x86
        IOS,
        Editor,
        //Android_x86,
        //Android_Arm,
        Amazon,
		Samsung,
    }

    [System.Serializable]
    public class InfoPlatformBuild
    {
        public ETypePlatform TypePlatform;
        public string ProductName;
        public string CompanyName;
        public string GameName;
        public string BundleIdentifier;
        public string Version;
        public int BundleVersionCode;


#if UNITY_EDITOR
        // android
        public ScriptingImplementation scriptingImplementation = ScriptingImplementation.IL2CPP;
#if UNITY_2019_4_OR_NEWER
        public AndroidSdkVersions minSdkVersion = AndroidSdkVersions.AndroidApiLevel19;
#else
        public AndroidSdkVersions minSdkVersion = AndroidSdkVersions.AndroidApiLevel16;
#endif

        // iOS
        public iOSSdkVersion iOSSdkVersion = iOSSdkVersion.DeviceSDK;
        
#if UNITY_2017_3_OR_NEWER
	    public AndroidBuildSystem androidBuildSystem = AndroidBuildSystem.Gradle;
#else
        public AndroidBuildSystem androidBuildSystem = AndroidBuildSystem.Internal;
#endif
#endif
    }

    [AddComponentMenu("F4A/BuildManager")]
    public class BuildManager : ManualSingletonMono<BuildManager>
    {
        public InfoPlatformBuild[] InfoBuilds;

        protected override void Initialization()
        {
        }


        public InfoPlatformBuild GetInfoPlatform()
        {
            InfoPlatformBuild info = null;
#if UNITY_IOS
			info = InfoBuilds.Where(v => v.TypePlatform == ETypePlatform.IOS).FirstOrDefault();
#else
            info = InfoBuilds.Where(v => v.TypePlatform == ETypePlatform.Android).FirstOrDefault();
#endif
            return info;
        }

        public string GetGameName()
        {
            //var info = GetInfoPlatform();
            //if(Application.platform == RuntimePlatform.Android)
            //{
            //    return Application.installerName;
            //}
            //else
            //{
            //    if (info != null) return info.GameName;
            //}
            return Application.productName;
        }

        public string GetCompanyName()
        {
            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    return Application.companyName;
            //}
            //else
            //{
            //    var info = GetInfoPlatform();
            //    if (info != null) return info.CompanyName;
            //}
            return Application.companyName;
        }

        public string GetVersion(){
            //var info = GetInfoPlatform();
            //if (info != null) return info.Version;
            //else return "Version Demo";

            return Application.version;
        }

        public int GetVersionCode()
        {
            var info = GetInfoPlatform();
            if (info != null) return info.BundleVersionCode;
            else return 1;
        }


        public void SetupInfoBuildiOS()
        {
#if UNITY_EDITOR

			InfoPlatformBuild info = InfoBuilds.Where (v => v.TypePlatform == ETypePlatform.IOS).FirstOrDefault ();
			if (info != null) {
				PlayerSettings.productName = info.ProductName;
				PlayerSettings.companyName = info.CompanyName;
#if UNITY_5_6_OR_NEWER
                PlayerSettings.applicationIdentifier = info.BundleIdentifier;
#else
				PlayerSettings.bundleIdentifier = info.BundleIdentifier;
#endif
                PlayerSettings.bundleVersion = info.Version;
				PlayerSettings.iOS.buildNumber = info.BundleVersionCode.ToString ();
				PlayerSettings.iOS.sdkVersion = info.iOSSdkVersion;
			}
#endif
        }

//        public void SetupInfoBuildAndroidX86()
//        {
//#if UNITY_EDITOR
//			InfoPlatformBuild info = InfoBuilds.Where (v => v.TypePlatform == ETypePlatform.Android_x86).FirstOrDefault ();
//			if (info != null) {
//				PlayerSettings.productName = info.ProductName;
//				PlayerSettings.companyName = info.CompanyName;
//#if UNITY_5_6_OR_NEWER
//                PlayerSettings.applicationIdentifier = info.BundleIdentifier;
//#else
//				PlayerSettings.bundleIdentifier = info.BundleIdentifier;
//#endif
//                PlayerSettings.Android.bundleVersionCode = info.BundleVersionCode;
//				PlayerSettings.bundleVersion = info.Version;
//				PlayerSettings.Android.minSdkVersion = info.minSdkVersion;
//				#if UNITY_2018
//				PlayerSettings.Android.targetArchitectures = AndroidArchitecture.X86;
//	            #else
//				PlayerSettings.Android.targetDevice = AndroidTargetDevice.x86;
//	            #endif
	            
//				#if UNITY_2018
//				EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
//				#elif UNITY_2017
//				EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
//				#else
//				#endif
//			}
//#endif
//        }

//        public void SetupInfoBuildAndroidARMv7()
//        {
//#if UNITY_EDITOR
//			InfoPlatformBuild info = InfoBuilds.Where (v => v.TypePlatform == ETypePlatform.Android_Arm).FirstOrDefault ();
//			if (info != null) {
//				PlayerSettings.productName = info.ProductName;
//				PlayerSettings.companyName = info.CompanyName;
//#if UNITY_5_6_OR_NEWER
//                PlayerSettings.applicationIdentifier = info.BundleIdentifier;
//#else
//				PlayerSettings.bundleIdentifier = info.BundleIdentifier;
//#endif
//                PlayerSettings.Android.bundleVersionCode = info.BundleVersionCode;
//				PlayerSettings.bundleVersion = info.Version;
//                PlayerSettings.Android.minSdkVersion = info.minSdkVersion;
//				#if UNITY_2018
//				PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
//	            #else
//				PlayerSettings.Android.targetDevice = AndroidTargetDevice.ARMv7;
//	            #endif
	            
//	            #if UNITY_2018
//				EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
//				#elif UNITY_2017
//				EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
//				#else
//				#endif
//			}
//#endif
//        }

        [ContextMenu("Setup Build Android")]
        public void SetupInfoBuildAndroid()
        {
#if UNITY_EDITOR
            InfoPlatformBuild info = InfoBuilds.Where(v => v.TypePlatform == ETypePlatform.Android).FirstOrDefault();
            if (info != null)
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, info.scriptingImplementation);
                PlayerSettings.productName = info.ProductName;
                PlayerSettings.companyName = info.CompanyName;
#if UNITY_5_6_OR_NEWER
                PlayerSettings.applicationIdentifier = info.BundleIdentifier;
#else
				PlayerSettings.bundleIdentifier = info.BundleIdentifier;
#endif
                PlayerSettings.Android.bundleVersionCode = info.BundleVersionCode;
                PlayerSettings.bundleVersion = info.Version;
                PlayerSettings.Android.minSdkVersion = info.minSdkVersion;
                PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
#if UNITY_2018_1_OR_NEWER
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
#else
				PlayerSettings.Android.targetDevice = AndroidTargetDevice.All;
#endif

#if UNITY_2018_1_OR_NEWER
                EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
#elif UNITY_2017
				EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
#else
#endif
            }
#endif
        }

        public void SetupInfoBuildAmazon()
        {
#if UNITY_EDITOR
            InfoPlatformBuild info = InfoBuilds.Where(v => v.TypePlatform == ETypePlatform.Amazon).FirstOrDefault();
            if (info != null)
            {
                PlayerSettings.productName = info.ProductName;
                PlayerSettings.companyName = info.CompanyName;
#if UNITY_5_6_OR_NEWER
                PlayerSettings.applicationIdentifier = info.BundleIdentifier;
#else
				PlayerSettings.bundleIdentifier = info.BundleIdentifier;
#endif
                PlayerSettings.Android.bundleVersionCode = info.BundleVersionCode;
                PlayerSettings.bundleVersion = info.Version;
	            PlayerSettings.Android.minSdkVersion = info.minSdkVersion;
                #if UNITY_2018
	            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
	            #else
	            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
	            #endif
	            
	            #if UNITY_2018
	            EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
				#elif UNITY_2017
	            EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
				#else
				#endif
            }
#endif
        }
		
		public void SetupInfoBuildSamsung()
        {
#if UNITY_EDITOR
            InfoPlatformBuild info = InfoBuilds.Where(v => v.TypePlatform == ETypePlatform.Samsung).FirstOrDefault();
            if (info != null)
            {
                PlayerSettings.productName = info.ProductName;
                PlayerSettings.companyName = info.CompanyName;
#if UNITY_5_6_OR_NEWER
                PlayerSettings.applicationIdentifier = info.BundleIdentifier;
#else
				PlayerSettings.bundleIdentifier = info.BundleIdentifier;
#endif
                PlayerSettings.Android.bundleVersionCode = info.BundleVersionCode;
                PlayerSettings.bundleVersion = info.Version;
                PlayerSettings.Android.minSdkVersion = info.minSdkVersion;
                #if UNITY_2018
	            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
	            #else
	            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
	            #endif
	            
	            #if UNITY_2018
	            EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
				#elif UNITY_2017
	            EditorUserBuildSettings.androidBuildSystem = info.androidBuildSystem;
				#else
				#endif
            }
#endif
        }

        public void SaveBuildInfo()
        {
#if UNITY_EDITOR
			var str = JsonConvert.SerializeObject (InfoBuilds);
			string path = Application.dataPath + "/Common/Data/BuildInfo.txt";

			System.IO.StreamWriter file = new System.IO.StreamWriter(path);
			file.WriteLine(str);
			file.Close();

			UnityEditor.AssetDatabase.Refresh ();
#endif
        }

        public void LoadBuildInfo()
        {
#if UNITY_EDITOR
			string path = Application.dataPath + "/Common/Data/BuildInfo.txt";

			string text = System.IO.File.ReadAllText(path);
			InfoBuilds = JsonConvert.DeserializeObject<InfoPlatformBuild[]> (text);
#endif

        }
    }
}