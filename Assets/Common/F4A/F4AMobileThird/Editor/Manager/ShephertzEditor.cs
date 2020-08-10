using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace com.F4A.MobileThird
{
    [CustomEditor(typeof(ShephertzManager))]
    [CanEditMultipleObjects]
    public class ShephertzEditor : Editor
    {
        ShephertzManager shephertzManager;
        private void OnEnable()
        {
            shephertzManager = target as ShephertzManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            if (GUILayout.Button("Save Info"))
            {
                shephertzManager.SaveInfo();
            }

            if (GUILayout.Button("Load Info"))
            {
                shephertzManager.LoadInfo();
            }

			GUILayout.Space (10);
            if (GUILayout.Button("Help"))
            {
                Application.OpenURL("http://api.shephertz.com/app42-dev.php");
                Application.OpenURL("https://apphq.shephertz.com/register/app42Login");
            }

			GUILayout.Space (10);
			if (GUILayout.Button ("Save Gift code To Server")) {
				shephertzManager.SaveGiftcodeToServer ();
			}

            serializedObject.ApplyModifiedProperties();
        }
    }
}