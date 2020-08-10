using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace com.F4A.MobileThird
{
	[CustomEditor(typeof(F4ACoreManager))]
	[CanEditMultipleObjects]
	public class F4ACoreEditor : Editor
	{
		F4ACoreManager _coreMobile = null;
		
		private void OnEnable(){
			_coreMobile = (F4ACoreManager)target;

            string defines = DMCMobileUtils.GetDefines();
            string[] array = defines.Split(';');

            _coreMobile.DefineAdColony = array.Contains(DMCMobileUtils.DEFINE_ADCOLONY);
            _coreMobile.DefineUnityAds = array.Contains(DMCMobileUtils.DEFINE_UNITY_ADS);
            _coreMobile.DefineAdMob = array.Contains(DMCMobileUtils.DEFINE_ADMOB);
            _coreMobile.DefineAdNative = array.Contains(DMCMobileUtils.DEFINE_AD_NATIVE);
            _coreMobile.DefineCharstboost = array.Contains(DMCMobileUtils.DEFINE_CHARTBOOST);
            _coreMobile.DefineVungle = array.Contains(DMCMobileUtils.DEFINE_VUNGLE);
            _coreMobile.DefineIronSource = array.Contains(DMCMobileUtils.DEFINE_IRONSOURCE);
            _coreMobile.DefineFacebookAds = array.Contains(DMCMobileUtils.DEFINE_FACEBOOK_ADS);

            _coreMobile.DefineIAP = array.Contains(DMCMobileUtils.DEFINE_IAP);
            _coreMobile.DefineFacebookSDK = array.Contains(DMCMobileUtils.DEFINE_FACEBOOK_SDK);
            _coreMobile.DefineGameServices = array.Contains(DMCMobileUtils.DEFINE_GAMESERVICES);
            _coreMobile.DefineUnityAnalytic = array.Contains(DMCMobileUtils.DEFINE_UNITY_ANALYTIC);


            _coreMobile.DefineFirebaseAnalytic = array.Contains(DMCMobileUtils.DEFINE_FIREBASE_ANALYTIC);
            _coreMobile.DefineFirebaseCrashlytic = array.Contains(DMCMobileUtils.DEFINE_FIREBASE_CRASHLYTIC);
            _coreMobile.DefineFirebaseMessaging = array.Contains(DMCMobileUtils.DEFINE_FIREBASE_MESSAGING);
            _coreMobile.DefineFirebaseAuth = array.Contains(DMCMobileUtils.DEFINE_FIREBASE_AUTH);
            _coreMobile.DefineFirebaseRemote = array.Contains(DMCMobileUtils.DEFINE_FIREBASE_REMOTECONFIG);

            _coreMobile.DefineAppflyer = array.Contains(DMCMobileUtils.DEFINE_APPFLYER);
            _coreMobile.DefineMixPanel = array.Contains(DMCMobileUtils.DEFINE_MIXPANEL);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Set Defines"))
            {
                string defines = DMCMobileUtils.GetDefines();
                string definesBefore = defines;
                var array = defines.Split(';');

                defines = CheckDefine(defines, array, _coreMobile.DefineAdColony, DMCMobileUtils.DEFINE_ADCOLONY);
                defines = CheckDefine(defines, array, _coreMobile.DefineUnityAds, DMCMobileUtils.DEFINE_UNITY_ADS);
                defines = CheckDefine(defines, array, _coreMobile.DefineAdMob, DMCMobileUtils.DEFINE_ADMOB);
                defines = CheckDefine(defines, array, _coreMobile.DefineAdNative, DMCMobileUtils.DEFINE_AD_NATIVE);
                defines = CheckDefine(defines, array, _coreMobile.DefineCharstboost, DMCMobileUtils.DEFINE_CHARTBOOST);
                defines = CheckDefine(defines, array, _coreMobile.DefineVungle, DMCMobileUtils.DEFINE_VUNGLE);
                defines = CheckDefine(defines, array, _coreMobile.DefineIronSource, DMCMobileUtils.DEFINE_IRONSOURCE);
                defines = CheckDefine(defines, array, _coreMobile.DefineFacebookAds, DMCMobileUtils.DEFINE_FACEBOOK_ADS);

                defines = CheckDefine(defines, array, _coreMobile.DefineIAP, DMCMobileUtils.DEFINE_IAP);
                defines = CheckDefine(defines, array, _coreMobile.DefineFacebookSDK, DMCMobileUtils.DEFINE_FACEBOOK_SDK);
                defines = CheckDefine(defines, array, _coreMobile.DefineGameServices, DMCMobileUtils.DEFINE_GAMESERVICES);
                defines = CheckDefine(defines, array, _coreMobile.DefineUnityAnalytic, DMCMobileUtils.DEFINE_UNITY_ANALYTIC);

                defines = CheckDefine(defines, array, _coreMobile.DefineFirebaseAnalytic, DMCMobileUtils.DEFINE_FIREBASE_ANALYTIC);
                defines = CheckDefine(defines, array, _coreMobile.DefineFirebaseCrashlytic, DMCMobileUtils.DEFINE_FIREBASE_CRASHLYTIC);
                defines = CheckDefine(defines, array, _coreMobile.DefineFirebaseMessaging, DMCMobileUtils.DEFINE_FIREBASE_MESSAGING);
                defines = CheckDefine(defines, array, _coreMobile.DefineFirebaseAuth, DMCMobileUtils.DEFINE_FIREBASE_AUTH);
                defines = CheckDefine(defines, array, _coreMobile.DefineFirebaseRemote, DMCMobileUtils.DEFINE_FIREBASE_REMOTECONFIG);

                defines = CheckDefine(defines, array, _coreMobile.DefineAppflyer, DMCMobileUtils.DEFINE_APPFLYER);
                defines = CheckDefine(defines, array, _coreMobile.DefineMixPanel, DMCMobileUtils.DEFINE_MIXPANEL);

                if (!definesBefore.Equals(defines))
                {
                    DMCMobileUtils.SetDefines(defines);
                }
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Add AdsManager"))
            {
                _coreMobile.AddAdsManager();
            }
            if (GUILayout.Button("Add SocialManager"))
            {
                _coreMobile.AddSocialManager();
            }
            if (GUILayout.Button("Add IAPManager"))
            {
                _coreMobile.AddIAPManager();
            }
            if (GUILayout.Button("Add BuildManager"))
            {
                _coreMobile.AddBuildManager();
            }
            if (GUILayout.Button("Add ShephertzManager"))
            {
                _coreMobile.AddShephertzManager();
            }
            if (GUILayout.Button("AddPlayFabManager"))
            {
                _coreMobile.AddPlayFabManager();
            }
            if (GUILayout.Button("Add All MobileThird"))
            {
                _coreMobile.AddAllMobileThird();
            }
            serializedObject.Update();
        }

        private string CheckDefine(string defines, string[] arrayDefine, bool enableDefine, string defineString)
        {
            if (enableDefine && !arrayDefine.Contains(defineString))
            {
                defines += ";" + defineString;
            }
            else if (!enableDefine && arrayDefine.Contains(defineString))
            {
                defines = defines.Replace(";" + defineString, "");
                defines = defines.Replace(defineString + ";", "");
            }

            return defines;
        }
    }
}
