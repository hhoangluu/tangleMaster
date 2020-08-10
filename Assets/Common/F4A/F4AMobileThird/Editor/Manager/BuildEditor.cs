using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.F4A.MobileThird
{
    [CustomEditor(typeof(BuildManager))]
    [CanEditMultipleObjects]
    public class BuildEditor : Editor
    {
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			BuildManager buildManager = (BuildManager)target;

			if(GUILayout.Button("Save Build Info"))
			{
				buildManager.SaveBuildInfo ();
			}

			if(GUILayout.Button("Load Build Info"))
			{
				buildManager.LoadBuildInfo ();
			}

            GUILayout.Space(25);

            if (GUILayout.Button("Setup Build Android"))
            {
                buildManager.SetupInfoBuildAndroid();
            }

            if (GUILayout.Button("Setup Build iOS"))
			{
				buildManager.SetupInfoBuildiOS ();
			}

			//if(GUILayout.Button("Setup Build Android x86"))
			//{
			//	buildManager.SetupInfoBuildAndroidX86 ();
			//}

			//if(GUILayout.Button("Setup Build Android ARMv7"))
			//{
			//	buildManager.SetupInfoBuildAndroidARMv7 ();
			//}

            if (GUILayout.Button("Setup Build Amazon"))
            {
                buildManager.SetupInfoBuildAmazon();
            }
			
			if (GUILayout.Button("Setup Build Samsung"))
            {
                buildManager.SetupInfoBuildSamsung();
            }
        }
    }
}
