using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperRaycastDemo
{
	[ExecuteInEditMode]
	public class RaycastExample : MonoBehaviour
	{
		public Renderer target;

		Ray ray;

		public bool useBumpMap = true;
		[Range(0, 1)] public float improveScale = 0;
		public float pointSize = .005f;

		[Header("Returned Data")]
		public bool hit;
		public Color hitColor;
		public Vector2 hitUV;
		public Vector3 hitNormal;
		public Vector3 hitPoint;

		LineRenderer lineRenderer;
		Transform child;

		public bool showNormal = true;

		void LateUpdate()
		{
			if (target == null)
				return;

			if (lineRenderer == null)
				lineRenderer = GetComponent<LineRenderer>();
			
			if (child == null)
				child = transform.GetChild(0);

			SuperRaycast.USE_BUMPMAP = useBumpMap;
			lineRenderer.SetPosition(0, transform.position);

			RaycastHitRenderer hitInfo;
			ray = new Ray(transform.position, transform.forward);

			if (SuperRaycast.Raycast(ray, target, out hitInfo))
			{
				hit = true;
				
				hitPoint = hitInfo.point;
				hitNormal = hitInfo.normal;
				hitColor = hitInfo.color;
				hitUV = hitInfo.uv;

				child.position = hitPoint;
				lineRenderer.SetPosition(1, hitPoint);
				lineRenderer.SetPosition(2, showNormal ? hitPoint + hitNormal * .075f : hitPoint);
				lineRenderer.startColor = lineRenderer.endColor = Color.Lerp(Color.white, hitColor, .5f);
			}
			else
			{
				hit = false;
				child.position = ray.origin + ray.direction * 4f;
				lineRenderer.SetPosition(1, child.position);
				lineRenderer.SetPosition(2, child.position);
				lineRenderer.startColor = lineRenderer.endColor = Color.red;
			}
		}

		/*void OnDrawGizmos()
		{
			if (hit)
			{
				var c = hitColor;
				c.a = 1;

				Gizmos.color = c;
				Gizmos.DrawLine(transform.position, hitPoint);
				Gizmos.DrawSphere(hitPoint, pointSize);
				Gizmos.DrawLine(hitPoint, hitPoint + hitNormal * .1f);

				//Gizmos.color = Color.white;
				//Gizmos.DrawLine(from, to);

				//Gizmos.color = Color.black;
				//Gizmos.DrawLine(fromAcc, toAcc);
			}
			else
			{
				Gizmos.color = Color.black;
				Gizmos.DrawRay(ray);
			}
		}*/
	}
}