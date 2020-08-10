using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.F4A.MobileThird
{
    [CustomEditor(typeof(SocialManager))]
    [CanEditMultipleObjects]
    public class SocialEditor : Editor
    {
	    SocialManager socialManager;

        private void OnEnable()
        {
            socialManager = (SocialManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

			if (GUILayout.Button ("Save Info")) {
				socialManager.SaveInfo ();
			}

			if (GUILayout.Button ("Load Info")) {
				socialManager.LoadInfo ();
			}
			
			//GUILayout.Space(10);
			//if (GUILayout.Button ("Load Game Services")) {
			//	socialManager.LoadGameServices ();
			//}
        }
    }
}