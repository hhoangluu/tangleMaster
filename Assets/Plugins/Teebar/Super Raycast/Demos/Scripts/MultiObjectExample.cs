using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperRaycastDemo
{
	public class MultiObjectExample : MonoBehaviour
	{
		//Renderer[] renderers;

		public Material matDefault;
		public Material matSelected;
		public GameObject cursor;
		public GameObject prefab1;
		public GameObject prefab2;
		public UnityEngine.UI.Text text1;
		public UnityEngine.UI.Text text2;
		//public TMPro.TextMeshProUGUI text3;
		public Camera cam;

		public bool useBumpMap = true;
		public bool useHeightMap = true;
		[Range(0, 2)] public float heightMapScale = 1f;
		//public bool openGL = true;
		//public bool improveAccuracy = false;

		public LayerMask objectLayer;

		Ray ray;
		Renderer lastSelected;
		RaycastHitRenderer hitInfo;

		[Range(0, 10000)] public int generateCount = 2000;
		
		void Start()
		{
			var rens = new List<Renderer>();

			for(int i = 0; i < Mathf.Clamp(generateCount, 0, 2000); i++)
			{
				var go = GameObject.Instantiate(Random.value > .5f ? prefab1 : prefab2);
				go.layer = objectLayer.value;
				go.transform.position =
					transform.forward * Random.Range(-10f, 10f) +
					transform.right * Random.Range(-10f, 10f) +
					transform.up * Random.Range(-10f, 10f);

				var ren = go.GetComponent<Renderer>();
				ren.sharedMaterial = matDefault;
				rens.Add(ren);
			}

			prefab1.SetActive(false);
			prefab2.SetActive(false);

			//renderers = SuperRaycast.GetRenderers(objectLayer);

			UpdateText();
		}

		[ContextMenu("Clean Up")]
		void Clear()
		{
			SuperRaycast.CleanUp();
		}

		void Update()
		{
			// Toggle openGL mode.
			/*if (Input.GetKeyDown(KeyCode.O))
			{
				openGL = !openGL;
				UpdateText();
			}*/

			if (Input.GetKeyDown(KeyCode.B))
			{
				useBumpMap = !useBumpMap;
				UpdateText();
			}

			if (Input.GetKeyDown(KeyCode.H))
			{
				useHeightMap = !useHeightMap;
				UpdateText();
			}

			/*if (Input.GetKeyDown(KeyCode.I))
			{
				improveAccuracy = !improveAccuracy;
				UpdateText();
			}*/

			// Deselect last material.
			if (lastSelected != null)
			{
				lastSelected.sharedMaterial = matDefault;
				lastSelected = null;
			}

			// Get camera ray.
			ray = cam.ScreenPointToRay(Input.mousePosition);

			// Enable/disable bumpmap testing, and test which renderer was hit.
			SuperRaycast.USE_BUMPMAP = useBumpMap;
			SuperRaycast.USE_HEIGHTMAP = useHeightMap;
			SuperRaycast.HEIGHTMAP_SCALE = heightMapScale;
			//SuperRaycast.OPENGL = openGL;

			// Test against the renderer.
			if (SuperRaycast.Raycast(ray, out hitInfo, float.MaxValue, objectLayer))
			{
				Debug.Log(hitInfo.renderer.name + "      " + hitInfo.time);

				// Set material as selected.
				lastSelected = hitInfo.renderer;
				lastSelected.sharedMaterial = matSelected;

				cursor.SetActive(true);
				cursor.transform.position = hitInfo.point;
				cursor.transform.forward = hitInfo.normal;
			}
			else
			{
				cursor.transform.position = ray.origin + ray.direction * 2f;
			}
		}

		void UpdateText()
		{
			var text = "None of these " + generateCount + " objects has a collider.\n";
			text += "WSAD to move camera.\n";
			text += "Click+Drag to rotate camera.\n";
			text += "Q+E to move up+down.\n\n";

			text += "Use [B]ump Maps: " + useBumpMap + "\n";
			text += "Use [H]eight Maps: " + useHeightMap + "\n";
			//text += "[I]mprove Accuracy: " + improveAccuracy + "\n";
			//text += "[O]pen GL Mode: " + openGL + "\n";

			text1.text = text2.text = text;
			//text3.text = text;
		}
	}
}