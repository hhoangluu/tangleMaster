using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

[Serializable]
public struct RaycastHitRenderer
{
	public Vector3 point;		// The point of contact.
	public Vector3 normal;		// The normal at contact.
	public Color color;			// The texture color at contact.
	public Vector2 uv;			// The UV position at contact.
	public Color32 data;		// Contextual data. (DataToReturn)
	public bool hit;

	public float height;		// This will only be set if USE_HEIGHTMAP is on and the material has a _ParallaxMap.

	public Renderer renderer;	// The renderer that was tested against.
	public Vector3 from;
	public Vector3 to;
	public float time;			// 0-1, between from and to.

	public Transform root { get { return renderer.transform.root; } }
	public GameObject rootGameObject { get { return root.gameObject; } }
	public GameObject gameObject { get { return renderer.gameObject; } }

	// Returns the color data as a vector.
	//public Vector4 dataAsVector { get { return new Vector4(data.r, data.g, data.b, data.a); } }

	// Will test again with a smaller line right at the hit point, increasing the accuracy a bit.
	/*public void ImproveAccuracy(float accuracy = 1)
	{
		if (!hit)
			return;
		
		var dir = Vector3.Normalize(this.to - this.from);
		from = point - dir * .04f * accuracy;
		to = point + dir * .04f * accuracy;

		RaycastHitRenderer hitInfo2;
		SuperRaycast.DrawRenderer(from, to, renderer);
		SuperRaycast.ReadWasHit(from, to, out hitInfo2);
		renderer = hitInfo2.renderer;
		point = hitInfo2.point;
		normal = hitInfo2.normal;
		color = hitInfo2.color;
		uv = hitInfo2.uv;
		data = hitInfo2.data;
		hit = hitInfo2.hit;
		time = hitInfo2.time;
	}*/
}

public class SuperRaycast
{
	public static bool USE_BUMPMAP = true;
	public static bool USE_HEIGHTMAP = false;
	public static float HEIGHTMAP_SCALE = .1f;
	//public static bool OPENGL = true;
	//public static bool USE_COMPUTE_SHADER = false;

	static Material _superRaycastMaterial;
	static Material superRaycastMaterial {
		get { return _superRaycastMaterial != null ? _superRaycastMaterial : _superRaycastMaterial = Resources.Load<Material>("Super Raycast"); }
	}

	//static bool cs_Init = false;
	//static int csi_Input = 0;
	//static int csi_Output = 0;
	//static ComputeShader cs_Shader;
	//static int cs_Kernel = 0;
	//static ComputeBuffer cs_Buffer;
	//static Color32[] cs_Output;

	/*static void CS_Init()
	{
		if (cs_Init)
			return;
		
		cs_Shader = Resources.Load<ComputeShader>("Super Raycast CS");
		cs_Kernel = cs_Shader.FindKernel("CSMain");
		cs_Buffer = new ComputeBuffer(1, 4 * 4);
		cs_Output = new Color32[4];
		cs_Init = true;
	}*/

	// 4 pixels are returned during a test.
	// Top Left = normal in RGB and hit distance in A. (Since distance will be between 0-1 you want the line being tested to be as short as possible for maximum accuracy.)
	// Top Right = the color of the _MainTex at the point.
	// Bottom Left = the uv at the hit point.
	// Bottom Right = any of the below DataToReturn. (When testing against a list of Renderers this pixel is used for determining which renderer was hit.)
	public enum DataToReturn
	{
		Texcoord0,
		Texcoord1,
		VertexColor,

		RENDERER_ID
	}

	static Material mat;
	static RenderTexture renderTexture;
	static RenderTexture renderTexture2;
	static Texture2D texture2D;
	static GameObject cameraObject;
	static Camera camera;

	static Color32 color;
	static List<Material> matPool;
	static List<Color32> colors;

	static bool cached = false;
	static int sp_BumpMap = 0;
	static int sp_BumpScale = 0;
	static int sp_UseBumpMap = 0;
	static int sp_Return = 0;
	static int sp_Color = 0;
	static int sp_MainTex = 0;
	static int sp_MainTex_ST = 0;
	static int sp_Position = 0;
	static int sp_Scale = 0;
	static int sp_UseHeightMap = 0;
	static int sp_ParallaxMap = 0;

	static void CacheShaderIDs()
	{
		if (cached)
			return;
		
		cached = true;
		var flags = BindingFlags.NonPublic | BindingFlags.Static;
		var fields = typeof(SuperRaycast).GetFields(flags);
			
		for (int i = 0; i < fields.Length; i++)
		{
			var name = fields[i].Name;
			if (name.StartsWith("sp"))
			{
				var pName = name.Replace("sp", "");
				var sp = Shader.PropertyToID(pName);
				fields[i].SetValue(null, sp);
			}
		}
	}

	static readonly float pullBack = .01f; // How far to pull the camera back from the renderer bound.

	#region Initialize.

	static void SetupCamera(Ray ray)//Vector3 from, Vector3 dire// Vector3 to)
	{
		if (camera == null || cameraObject == null)
		{
			cameraObject = GameObject.Find("SuperRaycast Camera");
			if (cameraObject == null)
			{
				cameraObject = new GameObject("SuperRaycast Camera");
				cameraObject.hideFlags = HideFlags.HideAndDontSave;
				camera = cameraObject.AddComponent<Camera>();
				camera.useOcclusionCulling = false;
				camera.clearFlags = CameraClearFlags.SolidColor;
				camera.backgroundColor = new Color(0, 0, 0, 0);
				camera.cullingMask = 1 << 31; // Only render this objects layer.
				camera.fieldOfView = 1;
				camera.orthographic = true;
				camera.orthographicSize = .0001f;
				camera.nearClipPlane = 0;
				camera.farClipPlane = 1000f;
				camera.allowMSAA = false;
			}
			else
				camera = cameraObject.GetComponent<Camera>();
		}
		else
			camera = cameraObject.GetComponent<Camera>();
		
		camera.enabled = true;

		//var dir = Vector3.Normalize(to - from);
		camera.transform.position = ray.origin - ray.direction * pullBack;//from - dir * pullBack;
		camera.transform.forward = ray.direction;//dir;
	}

	static void CopyMaterialData(Material from, Material to)
	{
		// Get MainTex.
		if (from.HasProperty(sp_MainTex))
		{
			to.mainTexture = from.mainTexture;
			to.SetVector(sp_MainTex_ST, from.GetVector(sp_MainTex_ST));
		}
		else
		{
			to.mainTexture = null;
			to.SetVector(sp_MainTex_ST, new Vector4(1, 1, 0, 0));
		}

		// Get BumpMap.
		if (USE_BUMPMAP && from.HasProperty(sp_BumpMap))
		{
			var tex = from.GetTexture(sp_BumpMap);
			if (tex != null)
			{
				to.SetTexture(sp_BumpMap, tex);
				to.SetInt(sp_UseBumpMap, 1);

				if (from.HasProperty(sp_BumpScale))
					to.SetFloat(sp_BumpScale, from.GetFloat(sp_BumpScale));
				else
					to.SetFloat(sp_BumpScale, 1);
			}
			else
				to.SetInt(sp_UseBumpMap, 0);
		}
		else
			to.SetInt(sp_UseBumpMap, 0);

		// Get HeightMap.
		if (USE_HEIGHTMAP && from.HasProperty(sp_ParallaxMap))
		{
			var tex = from.GetTexture(sp_ParallaxMap);
			if (tex != null)
			{
				to.SetTexture(sp_ParallaxMap, tex);
				to.SetInt(sp_UseHeightMap, 1);
			}
			else
				to.SetInt(sp_UseHeightMap, 0);
		}
		else
			to.SetInt(sp_UseHeightMap, 0);
	}

	#endregion

	#region Read data.

	static RenderTexture GetRenderTexture()
	{
		if (renderTexture == null)
		{
			renderTexture = new RenderTexture(2, 2, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
			renderTexture.enableRandomWrite = true;
			renderTexture.Create();
			renderTexture.filterMode = FilterMode.Point;
			renderTexture.wrapMode = TextureWrapMode.Clamp;
		}

		return renderTexture;
	}

	static Texture2D GetTexture()
	{
		// Create texture2d.
		if (texture2D == null)
		{
			texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false);
			texture2D.filterMode = FilterMode.Point;
			texture2D.wrapMode = TextureWrapMode.Clamp;
		}

		return texture2D;
	}

	static Color[] ReadTexture_OPENGL()
	{
		// Change format.
		var rt = GetRenderTexture();
		Graphics.SetRenderTarget(rt);

		// Read render texture.
		var t = GetTexture();
		t.ReadPixels(new Rect(0, 0, 2, 2), 0, 0);
		t.Apply();

		Graphics.SetRenderTarget(null);

		return t.GetPixels();
	}

	static Color32[] ReadTexture()
	{
		// Change format.
		var rt = GetRenderTexture();
		Graphics.SetRenderTarget(rt);

		// Intermediary. (ARGB32.)
		if (renderTexture2 == null)
		{
			renderTexture2 = new RenderTexture(2, 2, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			renderTexture2.filterMode = FilterMode.Point;
			renderTexture2.wrapMode = TextureWrapMode.Clamp;
		}

		Graphics.Blit(null, renderTexture2);
		Graphics.SetRenderTarget(renderTexture2);

		// Read render texture.
		var t = GetTexture();
		t.ReadPixels(new Rect(0, 0, 2, 2), 0, 0);
		t.Apply();

		Graphics.SetRenderTarget(null);

		return t.GetPixels32();
	}

	static Color C32toC(Color32 c)
	{
		return new Color(
			c.r / 255f,
			c.g / 255f,
			c.b / 255f,
			c.a / 255f
		);
	}

	internal static bool ReadWasHit(Vector3 from, Vector3 to, out RaycastHitRenderer hit)
	{
		Color[] clrs;

		/*if (USE_COMPUTE_SHADER)
		{
			CS_Init();

			cs_Shader.SetTexture(cs_Kernel, "inputTex", renderTexture);
			cs_Shader.SetBuffer(cs_Kernel, "outputClr", cs_Buffer);
			cs_Shader.Dispatch(cs_Kernel, 4, 1, 1);
			
			cs_Buffer.GetData(cs_Output);
			clrs32 = cs_Output;
		}
		else
		{*/
		/*if (OPENGL)
			clrs = ReadTexture_OPENGL();
		else*/
		{
			var clrs32 = ReadTexture();
			clrs = new Color[]
			{
				C32toC(clrs32[0]),
				C32toC(clrs32[1]),
				C32toC(clrs32[2]),
				C32toC(clrs32[3])
			};
		}
		
		hit = new RaycastHitRenderer();
		hit.from = from;
		hit.to = to;

		var clr = clrs[0];

		// TL RGB = Normal.
		hit.normal = new Vector3(
			clr.r * 2f - 1f,
			clr.g * 2f - 1f,
			clr.b * 2f - 1f);
		
		// TR = Distance.
		//hit.color = clrs[1];
		hit.time = DecodeFloatRGBA(clrs[1]);
		hit.point = Vector3.Lerp(from, to, hit.time - pullBack);

		// BL RG = UV.xy.
		clr = clrs[2];
		hit.uv = new Vector2(clr.r, clr.g);

		if (USE_HEIGHTMAP)
		{
			hit.height = clr.b * 2f - 1f;
			hit.point += hit.normal * hit.height * HEIGHTMAP_SCALE;
		}
		else
			hit.height = 0f;

		// BR RGBA = texcoord (0-2) or vertex color.
		hit.data = clrs[3];

		// A will be 0 if there was no collision.
		return hit.hit = (clr.a != 0);
	}

	static float DecodeFloatRGBA(Color enc)
	{
		var kDecodeDot = new Vector4(1.0f, 1f/255.0f, 1f/65025.0f, 1f/160581375.0f);
		return Vector4.Dot(enc, kDecodeDot);
	}

	#endregion

	#region Drawing.

	internal static void DrawRenderer(Vector3 from, Vector3 to, Renderer r, DataToReturn dataToReturn = DataToReturn.Texcoord1, Texture texture = null)
	{
		SetupCamera(new Ray(from, to - from));
		CacheShaderIDs();

		var lastLayer = r.gameObject.layer;
		var lastMaterials = r.sharedMaterials;
		var mat = superRaycastMaterial;

		if (r.sharedMaterial != null)
			CopyMaterialData(r.sharedMaterial, mat);

		// Setup material.
		mat.SetInt(sp_Return, (int)dataToReturn);
		mat.SetVector(sp_Position, from);//cameraObject.transform.position);
		mat.SetFloat(sp_Scale, Vector3.Distance(from, to));

		var newMaterials = new Material[lastMaterials.Length];
		for (int i = 0; i < lastMaterials.Length; i++)
			newMaterials[i] = mat;
		r.sharedMaterials = newMaterials;

		// Set render texture.
		camera.targetTexture = GetRenderTexture();

		// Go to proper layer.
		r.gameObject.layer = 31;
		
		// Render.
		camera.Render();

		// Reset properties.
		r.gameObject.layer = lastLayer;
		r.sharedMaterials = lastMaterials;

		// Disable camera.
		camera.targetTexture = null;
		camera.enabled = false;
	}

	#endregion

	#region Raycast helpers.

	// The first point where the ray hits a boundary.
	static Vector3 GetBoundsP1(Bounds bounds, Ray ray)
	{
		float distance;

		// Is ray inside bound?
		if (bounds.Contains(ray.origin))
			return ray.origin;
		// Does ray intersect bound?
		else if (bounds.IntersectRay(ray, out distance))
			return ray.origin + ray.direction * distance;
		else
			return Vector3.zero;
	}

	// The oposite of the first renderer bound hit point.
	static Vector3 GetBoundsP2(Bounds bounds, Ray ray, Vector3 firstPoint)
	{
		float distance;

		// Create a ray coming from oposite direction.
		var inverseRay = new Ray(firstPoint + ray.direction * 100f, -ray.direction);
		bounds.IntersectRay(inverseRay, out distance);

		// Second collision point.
		return inverseRay.origin + inverseRay.direction * distance;
	}

	#endregion

	#region Helpers for testing against many renderers at once.

	struct R_Data
	{
		public int layer;
		public Vector3 from;
		public Vector3 to;
		public Material[] materials;
	}

	// 19007 unique colors.
	static Color32 GetRHColor()
	{
		var c = color;

		// Cycle through the colors.
		color.r += 20;
		if (color.r >= 240)
		{
			color.r = 0;
			color.g += 20;
			
			if (color.g >= 240)
			{
				color.g = 0;
				color.b += 20;
				
				if (color.b >= 240)
				{
					color.b = 0;
					color.a += 20;
				}
			}
		}

		return c;
	}

	static Material GetMatFromPool(int index)
	{
		Material mat = null;

		if (matPool == null || matPool.Count == index)
		{
			if (matPool == null)
			{
				color = new Color32(0, 0, 0, 20);
				matPool = new List<Material>();
				colors = new List<Color32>();
			}

			colors.Add(GetRHColor());

			mat = GameObject.Instantiate(superRaycastMaterial);
			matPool.Add(mat);

			return mat;
		}
		else
			mat = matPool[index];
		
		mat.SetInt(sp_Return, (int)DataToReturn.RENDERER_ID);
		mat.SetColor(sp_Color, colors[index]);

		return mat;
	}

	#endregion

	#region Public Methods.

	public static bool Raycast(Ray ray, out RaycastHitRenderer hitInfo, float maxDistance = float.MaxValue)
	{
		var rens = GetRenderers();

		if (rens.Length > 0)
			return Raycast(ray, rens, out hitInfo, maxDistance);

		hitInfo = default(RaycastHitRenderer);
		return false;
	}

	public static bool Raycast(Ray ray, out RaycastHitRenderer hitInfo, float maxDistance, LayerMask layer)
	{
		var rens = GetRenderers(layer);

		if (rens.Length > 0)
			return Raycast(ray, rens, out hitInfo, maxDistance);

		hitInfo = default(RaycastHitRenderer);
		return false;
	}

	/// <summary>
	/// Test for a collision on all the renderers of a GameObject.
	/// </summary>
	/// <returns><c>true</c>, if hit happens, <c>false</c> otherwise.</returns>
	/// <param name="renderer">Renderer.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="hitInfo">Data on the collision.</param>
	public static bool Raycast(Ray ray, GameObject gameObject, out RaycastHitRenderer hitInfo)
	{
		var renderers = gameObject.GetComponentsInChildren<Renderer>();
		return Raycast(ray, renderers, out hitInfo);
	}

	/// <summary>
	/// Test for a collision on the ray.
	/// </summary>
	/// <returns><c>true</c>, if hit happens, <c>false</c> otherwise.</returns>
	/// <param name="renderer">Renderer.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="hitInfo">Data on the collision.</param>
	public static bool Raycast(Ray ray, Renderer[] renderers, out RaycastHitRenderer hitInfo, float maxDistance = float.MaxValue)//, bool improve = true)//, Texture texture = null)
	{
		if (renderers == null || renderers.Length == 0)
		{
			hitInfo = new RaycastHitRenderer();
			return false;
		}

		renderers = GetIntersecting(ray, renderers, maxDistance);

		if (renderers.Length == 0)
		{
			hitInfo = default(RaycastHitRenderer);
			return false;
		}

		if (renderers.Length == 1)
		{
			var hit = Raycast(ray, renderers[0], out hitInfo);
			
			if (hit)
				return Vector3.Distance(ray.origin, hitInfo.point) <= maxDistance;
			else
				return false;
		}

		if (renderers.Length > 10000)
			Debug.Log("There are more than 10,000 renderers. Only the first 10,000 will be used.");

		var clamped = Mathf.Min(renderers.Length, 10000);
		var from = ray.origin;
		var to = ray.origin + ray.direction * maxDistance;
		SetupCamera(ray);
		CacheShaderIDs();

		var rdata = new R_Data[clamped];

		// Get render texture.
		camera.targetTexture = GetRenderTexture();

		var count = 0;
		var matIndex = 0;

		for (int i = 0; i < clamped; i++)
		{
			var r = renderers[i];

			if (r == null)
				continue;

			// Track previous state.
			var p1 = GetBoundsP1(r.bounds, ray);
			var p2 = GetBoundsP2(r.bounds, ray, p1);
			rdata[i] = new R_Data
			{
				layer = r.gameObject.layer,
				materials = r.sharedMaterials,
				from = p1,
				to = p2
			};

			count++;

			// Create new material list.
			var mat = GetMatFromPool(matIndex++);

			// Set the main texture.
			if (r.sharedMaterial != null)
			{
				// Apply main texture.
				CopyMaterialData(r.sharedMaterial, mat);

				var rd = rdata[i];
				var scale = Vector3.Distance(rd.from, rd.to);
				mat.SetVector(sp_Position, rd.from);
				mat.SetFloat(sp_Scale, scale);
			}

			// Apply material.
			if (r.sharedMaterials.Length == 1)
				r.sharedMaterial = mat;
			else
			{
				var newMaterials = new Material[r.sharedMaterials.Length];
				for (int j = 0; j < r.sharedMaterials.Length; j++)
					newMaterials[j] = mat;

				r.sharedMaterials = newMaterials;
			}

			// Go to proper layer.
			r.gameObject.layer = 31;
		}

		// Render.
		camera.Render();

		// Disable camera.
		camera.targetTexture = null;
		camera.enabled = false;

		// Read hit data.
		ReadWasHit(from, to, out hitInfo);
		var hitColor = hitInfo.data;
		hitInfo.renderer = null;

		hitColor = new Color32(
			(byte)(20 * (int)Math.Floor(hitColor.r / 20d)),
			(byte)(20 * (int)Math.Floor(hitColor.g / 20d)),
			(byte)(20 * (int)Math.Floor(hitColor.b / 20d)),
			(byte)(20 * (int)Math.Floor(hitColor.a / 20d))
		);

		// Determine which renderer was hit.
		for (int i = 0; i < clamped; i++)
		{
			var r = renderers[i];

			if (r == null)
				continue;

			var rd = rdata[i];

			if (CompareColors(colors[i], hitColor))
			{
				hitInfo.renderer = r;
				hitInfo.point = Vector3.Lerp(rd.from, rd.to, hitInfo.time);
			}
			
			// Reset properties.
			r.gameObject.layer = rd.layer;
			r.sharedMaterials = rd.materials;
		}

		var dist = Vector3.Distance(ray.origin, hitInfo.point);
		return hitInfo.renderer != null && dist <= maxDistance;
	}

	static bool CompareColors(Color32 c1, Color32 c2)
	{
		return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b && c1.a == c2.a;
	}

	/// <summary>
	/// Get Renderers on the ray.
	/// </summary>
	/// <returns>The Renderers on the ray.</returns>
	/// <param name="ray">Ray.</param>
	/// <param name="renderers">Renderers.</param>
	public static Renderer[] GetIntersecting(Ray ray, Renderer[] renderers, float maxDistance = float.MaxValue)
	{
		if (renderers == null || renderers.Length == 0)
			return renderers;
		
		var list = new List<Renderer>();

		for (int i = 0; i < renderers.Length; i++)
		{
			var distance = 0f;
			var intersects = renderers[i].bounds.IntersectRay(ray, out distance);
			if (intersects && distance <= maxDistance)
				list.Add(renderers[i]);
		}

		return list.ToArray();
	}

	public static Renderer[] GetRenderers()
	{
		return GameObject.FindObjectsOfType<Renderer>();
	}

	public static Renderer[] GetRenderers(LayerMask layer)
	{
		return GameObject.FindObjectsOfType<Renderer>()
			.Where(x => layer == (layer | (1 << x.gameObject.layer)))//x.gameObject.layer == layer.value)
			.ToArray();
	}

	/// <summary>
	/// Test for a collision on a ray.
	/// </summary>
	/// <returns><c>true</c>, if hit happens, <c>false</c> otherwise.</returns>
	/// <param name="renderer">Renderer.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="hitInfo">Data on the collision.</param>
	/// <param name="dataToReturn">Data to return. (Tex Coordinate, or Vertex Color.)</param>
	/// <param name="texture">(Optional) Texture, this will be used instead of the _MainTex of the renderer.</param>
	public static bool Raycast(Ray ray, Renderer renderer, out RaycastHitRenderer hitInfo, DataToReturn dataToReturn = DataToReturn.Texcoord1, Texture texture = null)
	{
		if (renderer == null)
		{
			hitInfo = new RaycastHitRenderer();
			return false;
		}

		var p1 = GetBoundsP1(renderer.bounds, ray);
		if (p1 != Vector3.zero)
		{
			var p2 = GetBoundsP2(renderer.bounds, ray, p1);
			return SuperRaycast.LineTest(renderer, p1, p2, out hitInfo, dataToReturn, texture);
		}
		else
			hitInfo = new RaycastHitRenderer();

		return false;
	}

	/// <summary>
	/// Test for a collision between two points.
	/// </summary>
	/// <returns><c>true</c>, if hit happens, <c>false</c> otherwise.</returns>
	/// <param name="renderer">Renderer.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="hitInfo">Data on the collision.</param>
	/// <param name="dataToReturn">Data to return. (Tex Coordinate, or Vertex Color.)</param>
	/// <param name="texture">(Optional) Texture, this will be used instead of the _MainTex of the renderer.</param>
	public static bool LineTest(Renderer renderer, Vector3 from, Vector3 to, out RaycastHitRenderer hitInfo, DataToReturn dataToReturn = DataToReturn.Texcoord1, Texture texture = null)
	{
		if (renderer == null)
		{
			hitInfo = new RaycastHitRenderer();
			return false;
		}

		DrawRenderer(from, to, renderer, dataToReturn, texture);

		var hit = ReadWasHit(from, to, out hitInfo);
		hitInfo.renderer = renderer;
		return hit;
	}

	/// <summary>
	/// Cleans up resources used to speed up initialization.
	/// </summary>
	public static void CleanUp()
	{
		if (mat != null) GameObject.DestroyImmediate(mat);
		if (renderTexture != null) GameObject.DestroyImmediate(renderTexture);
		if (renderTexture2 != null) GameObject.DestroyImmediate(renderTexture2);
		if (texture2D != null) GameObject.DestroyImmediate(texture2D);
		if (cameraObject != null) GameObject.DestroyImmediate(cameraObject);

		mat = null;
		renderTexture = null;
		renderTexture2 = null;
		texture2D = null;
		cameraObject = null;

		if (matPool != null)
		{
			foreach (var m in matPool)
				GameObject.DestroyImmediate(m);
			matPool = null;
		}

		if (colors != null)
		{
			colors.Clear();
			colors = null;
		}

		/*cs_Output = null;
		if (cs_Buffer != null)
			cs_Buffer.Dispose();
		cs_Buffer = null;
		cs_Shader = null;
		cs_Init = false;*/
	}

	#endregion
}
