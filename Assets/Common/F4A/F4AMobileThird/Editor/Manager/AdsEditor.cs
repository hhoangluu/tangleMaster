using UnityEngine;
using UnityEditor;
using System.Linq;

namespace com.F4A.MobileThird
{
	[CustomEditor (typeof(AdsManager))]
	[CanEditMultipleObjects]
	public class AdsEditor : Editor
	{
		AdsManager adsManager;

        private void OnEnable ()
		{
			adsManager = (AdsManager)target;
        }

		public override void OnInspectorGUI ()
		{
            GUILayout.Label("-- OnRewardedAdCompleted");
            GUILayout.Label("-- OnRewardedAdSkiped");
            GUILayout.Label("-- OnRewardedAdFailed");
            GUILayout.Label("-- OnVideodAdCompleted");
            GUILayout.Label("-- OnInterstitialAdClosed");
            GUILayout.Space(20);

            base.OnInspectorGUI();

            serializedObject.Update();

            serializedObject.ApplyModifiedProperties ();

			if (GUILayout.Button ("Save Info")) {
				adsManager.SaveInfo ();
			}

			if (GUILayout.Button ("Load Info")) {
				adsManager.LoadInfo ();
			}
		}
    }
}