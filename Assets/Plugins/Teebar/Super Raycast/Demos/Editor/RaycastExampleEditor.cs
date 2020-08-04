using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SuperRaycastDemo
{
	[CustomEditor(typeof(RaycastExample)), CanEditMultipleObjects]
	public class RaycastExampleEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Clean Up"))
				SuperRaycast.CleanUp();

			// Render the UV hit point, for the fun of it.
			var t = (RaycastExample)target;
			if (t.target != null && t.target.sharedMaterial != null)
			{
				var w = Screen.width;
				var r = GUILayoutUtility.GetRect(w, w, w, w);
				r.width = w - 20;
				r.height = w - 20;

				// Draw materials _MainTex.
				var texture = t.target.sharedMaterial.mainTexture;
				if (texture != null)
					EditorGUI.DrawTextureTransparent(r, texture);

				var x = r.x + t.hitUV.x * r.width;
				var y = 0f;
				if (SystemInfo.graphicsDeviceVersion.StartsWith("Direct"))
					y = t.hitUV.y;
				else
					y = 1f - t.hitUV.y;
				y = r.y + y * r.height;

				// Draw hit UV.
				if (t.hit)
					EditorGUI.DrawRect(new Rect(x - 8, y - 8, 16, 16), Color.white);
			}
		}
	}
}