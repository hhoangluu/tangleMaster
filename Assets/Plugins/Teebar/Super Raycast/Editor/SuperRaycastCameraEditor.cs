using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SuperRaycastCamera))]
public class SuperRaycastCameraEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var t = (SuperRaycastCamera)target;

	}
}