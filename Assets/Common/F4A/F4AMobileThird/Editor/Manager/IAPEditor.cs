using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace com.F4A.MobileThird
{
    [CustomEditor(typeof(IAPManager))]
    [CanEditMultipleObjects]
    public class IAPEditor : Editor
    {
	    IAPManager iapManager;

        private void OnEnable()
        {
	        iapManager = (IAPManager)target;
        }

        public override void OnInspectorGUI()
	    {
		    GUILayout.Label("OnBuyPurchaseSuccessed(string id)");
		    GUILayout.Label("OnBuyPurchaseFailed(string id,string failureReason)");
		    GUILayout.Label("OnRestorePurchases(bool result)");

            base.OnInspectorGUI();

		    serializedObject.Update();
            
            serializedObject.ApplyModifiedProperties();

			if (GUILayout.Button ("Save Info IAP")) {
				iapManager.SaveInfo ();	
			}

			if (GUILayout.Button ("Load Info IAP")) {
				iapManager.LoadInfo ();
			}
        }
    }
}
