using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/*
	Add to a Camera.
	Set targetTag to any layer you want to target or manually set renderers to targetable renderers.
	Click play.

	Controls:
		- Middle click = Pan up/down.
		- Scroll wheel = Zoom.
		- Right click  = Rotate around point.
*/
public class SuperRaycastCamera : MonoBehaviour
{
	public static SuperRaycastCamera self;

	[HideInInspector] public RaycastHitRenderer hitInfo;
	[HideInInspector] public Renderer hovered = null;
	[HideInInspector] public bool hit = false;
	[HideInInspector] public Ray ray;
	[HideInInspector] public Vector3 point = Vector3.zero;
	[HideInInspector] public Vector3 normal = Vector3.up;
	[HideInInspector] public Vector3 rayDirection { get { return ray.direction; } }
	[HideInInspector] public bool movingCamera = false;
	[HideInInspector] public Transform cursor;
	
	public bool USE_BUMPMAP = true;
	public bool USE_HEIGHT = true;
	[Range(0, .3f)] public float HEIGHTMAP_SCALE = .025f;
	
	public LayerMask layerMask = -1;
	public float speedScaleFast = 2.5f;
	public float speedScaleSlow = .25f;

	public float rayDistance = 100f;

	//public Renderer[] renderers = new Renderer[0];

	public bool drawGizmos = true;
	public bool canMoveCamera = true;
	public bool castRay = true;
	
	Camera cam;
	Vector3 grabPoint;
	float grabDistance;
	Vector3 lastMousePos;

	public Vector3 rightNormal { get { return cursor.right; } }
	public Vector3 upNormal { get { return cursor.up; } }

	//public Func<Renderer[]> GetRenderersFunc;

	void Start()
	{
		self = this;
		cam = Camera.main;
		
		//GetRenderersFunc = GetRenderersFromTag;

		//if (renderers.Length == 0)
		//	UpdateRendererList();

		var go = new GameObject();
		go.name = "Super Raycast Camera Cursor";
		cursor = go.transform;
	}

	void Update()
	{
		// Ignore if over UI element.
		var e = UnityEngine.EventSystems.EventSystem.current;
		if (e != null && e.IsPointerOverGameObject())
			return;
		
		if (castRay)
			Raycast();
		
		if (canMoveCamera)
			MoveCamera();
	}

	void OnDrawGizmos()
	{
		if (!drawGizmos || !Application.isPlaying)
			return;
		
		// Pivot point.
		Gizmos.color = new Color(1, 1, 1, .5f);
		Gizmos.DrawSphere(grabPoint, Vector3.Distance(transform.position, grabPoint) * .05f);
		
		if (hit)
		{
			var scale = Vector3.Distance(transform.position, point);

			// Ray hit point cursor.
			Gizmos.color = new Color(0, .5f, 1f, .5f);
			Gizmos.matrix = Matrix4x4.TRS(point, Quaternion.LookRotation(normal), Vector3.one);
			Gizmos.DrawCube(Vector3.zero, new Vector3(scale * .05f, scale * .05f, scale * .01f));

			// Bounding box.
			Gizmos.matrix = Matrix4x4.identity;
			var b = hitInfo.renderer.bounds;
			Gizmos.DrawWireCube(b.center, b.size);
			Gizmos.DrawWireCube(b.center, b.size + b.size * .02f);
		}
	}

	public bool Raycast(Ray ray, out RaycastHitRenderer hitInfo, float maxDistance = float.MaxValue)
	{
		//var rens = SuperRaycast.GetIntersecting(ray, renderers, maxDistance);
		return SuperRaycast.Raycast(ray, out hitInfo, float.MaxValue, layerMask);
	}

	// Move object to position, stopping and offsetting from a hit point.
	public void MoveAndOffset(Renderer renderer, Vector3 destination)
	{
		var transform = renderer.transform.root;
		var pos = transform.position;
		var dir = Vector3.Normalize(destination - pos);
		var ray = new Ray(pos - dir, dir);

		// First cast a ray towards the scene objects.
		RaycastHitRenderer hitInfo;
		var dist = Vector3.Distance(pos, destination);
		//var rens = SuperRaycast.GetIntersecting(ray, dist + 1.5f);
		if (SuperRaycast.Raycast(ray, out hitInfo, dist + 1.5f, layerMask))
			OffsetObjectFromHitPoint(hitInfo.point, -dir, renderer);
		else
			transform.position = destination;
	}

	// Move away from a point, based on distance to renderer.
	public void OffsetObjectFromHitPoint(Vector3 point, Vector3 dir, Renderer renderer, float extra = 0f)
	{
		var transform = renderer.transform.root;
		var ray = new Ray(point, dir);
		var originalPos = transform.position;
		transform.position = point + dir * 2f;

		RaycastHitRenderer hitInfo;
		if (SuperRaycast.Raycast(ray, renderer, out hitInfo))
		{
			var dist = 2f - Vector3.Distance(point, hitInfo.point);
			transform.position = point + dir * (dist + extra);
		}
		else
			transform.position = point;
	}

	// Aligns an object to the hit point, pushing it away based on it's size.
	public void OffsetObjectFromHitPoint(Renderer renderer, float extra = 0f)
	{
		OffsetObjectFromHitPoint(point, normal, renderer, extra);
	}

	void Raycast()
	{
		SuperRaycast.USE_BUMPMAP = USE_BUMPMAP;
		SuperRaycast.USE_HEIGHTMAP = USE_HEIGHT;
		SuperRaycast.HEIGHTMAP_SCALE = HEIGHTMAP_SCALE;

		ray = cam.ScreenPointToRay(Input.mousePosition);

		if (SuperRaycast.Raycast(ray, out hitInfo, float.MaxValue, layerMask))
		{
			// Cast it again using the hit info normal, instead of the camera direction.
			SuperRaycast.LineTest(hitInfo.renderer, hitInfo.point, hitInfo.point - hitInfo.normal, out hitInfo);

			hit = true;
			point = hitInfo.point;
			normal = hitInfo.normal;
			hovered = hitInfo.renderer;

			// Set the pivot point to the hit point if middle click, right click, or zoom.
			if (Input.GetMouseButtonDown(0) ||
				Input.GetMouseButtonDown(1) ||
				Input.GetMouseButtonDown(2) ||
				Input.mouseScrollDelta.y != 0)
			{
				grabPoint = point;
				//rayDistance = Vector3.Distance(transform.position, grabPoint);
			}
		}
		else
		{
			hit = false;
			point = transform.position + ray.direction * defaultPivotDistance;
			normal = ray.direction;
			hovered = null;

			if (Input.GetMouseButtonUp(0) || 
				Input.GetMouseButtonUp(1) ||
				Input.GetMouseButtonUp(2) ||
				Input.mouseScrollDelta.y != 0)
			{
				grabPoint = point;
				//rayDistance = 100f;
			}
		}

		cursor.position = point;
		cursor.up = normal;
	}

	public float maxSpeed = 1f;
	public float defaultPivotDistance = 5f;

	void MoveCamera()
	{
		movingCamera = false;

		// Right click is rotate.
		if (Input.GetMouseButtonDown(1))
		{
			movingCamera = true;
			lastMousePos = Input.mousePosition;
		}

		if (Input.GetMouseButton(1))
		{
			movingCamera = true;
			var delta = lastMousePos - Input.mousePosition;
			lastMousePos = Input.mousePosition;

			var speed = Input.GetKey(KeyCode.LeftShift) ? speedScaleFast : Input.GetKey(KeyCode.LeftControl) ? speedScaleSlow : 1f;
			transform.RotateAround(grabPoint, transform.up, -delta.x * .2f * speed);
			transform.RotateAround(grabPoint, transform.right, delta.y * .2f * speed);
			var e = transform.eulerAngles;
			e.z = 0;
			transform.eulerAngles = e;
		}

		// Middle click is pan.
		if (Input.GetMouseButtonDown(2))
		{
			movingCamera = true;
			grabDistance = Vector3.Distance(grabPoint, transform.position);
			lastMousePos = Input.mousePosition;
		}

		if (Input.GetMouseButton(2))
		{
			movingCamera = true;
			var delta = lastMousePos - Input.mousePosition;
			lastMousePos = Input.mousePosition;

			var speed = Input.GetKey(KeyCode.LeftShift) ? speedScaleFast : Input.GetKey(KeyCode.LeftControl) ? speedScaleSlow : 1f;
			transform.position +=
				transform.up * Mathf.Clamp(delta.y * grabDistance * .005f * speed, -maxSpeed, maxSpeed) +
				transform.right * Mathf.Clamp(delta.x * grabDistance * .005f * speed, -maxSpeed, maxSpeed);
		}

		// Mouse wheel to zoom to point.
		if (Input.mouseScrollDelta.y != 0)
		{
			movingCamera = true;
			var fwd = Vector3.Normalize(grabPoint - transform.position);

			// Get speed based on distance.
			const float MIN_ZOOM = .001f;
			const float MAX_ZOOM = 2f;
			var dist = Vector3.Distance(grabPoint, transform.position);
			var speed = Mathf.Clamp(dist * .1f, MIN_ZOOM, MAX_ZOOM);

			// Scale it if necessary.
			var goSlow = !hit || Input.GetKey(KeyCode.LeftControl);
			var goFast = Input.GetKey(KeyCode.LeftShift);
			speed *= goFast ? speedScaleFast : goSlow ? speedScaleSlow : 1f;
			
			transform.position += fwd * Mathf.Clamp(Input.mouseScrollDelta.y * speed, -maxSpeed, maxSpeed);
		}
	}
}
