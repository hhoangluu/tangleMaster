using UnityEngine;
using UnityEditor;

using System.Linq;

namespace com.F4A.MobileThird
{
	[CustomEditor (typeof(PlayFabManager))]
	[CanEditMultipleObjects]
	public class PlayFabEditor : Editor
	{
		PlayFabManager playfabManager;

		//SerializedProperty IsLoginFacebookSuccessed;

		void OnEnable ()
		{
			playfabManager = (PlayFabManager)target;

			//IsLoginFacebookSuccessed = serializedObject.FindProperty ("IsLoginFacebookSuccessed");

			string defines = "";

			#if UNITY_ANDROID
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android);
			#elif UNITY_IOS
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
			#elif UNITY_WEBGL
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
			#elif UNITY_STANDALONE
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
			#else
			#endif
			if (defines.Contains ("PLAY_FAB"))
				playfabManager.EnablePlayFab = true;
			else
				playfabManager.EnablePlayFab = false;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			//playfabManager.EnablePlayFab = EditorGUILayout.Toggle ("Enable Play Fab", playfabManager.EnablePlayFab);

			string defines = "";

			#if UNITY_ANDROID
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android);
			#elif UNITY_IOS
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
			#elif UNITY_WEBGL
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
			#elif UNITY_STANDALONE
			defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
			#else
			#endif
			string[] array = defines.Split (';');
			defines = "";
			array.ToList ().ForEach (define => {
				if (!define.Equals ("PLAY_FAB")) {
					defines += define + ";";
				}
			});
			if (playfabManager.EnablePlayFab)
				defines += "PLAY_FAB" + ";";

			#if UNITY_ANDROID
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, defines);
			#elif UNITY_IOS
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defines);
			#elif UNITY_WEBGL
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, defines);
			#elif UNITY_STANDALONE
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
			#else
			#endif
		}
	}
}