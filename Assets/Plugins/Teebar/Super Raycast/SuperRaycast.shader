Shader "Teebar/Physics/Super Raycast"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_BumpMap ("Bump Map", 2D) = "white" {}
		_ParallaxMap ("Parallax Map", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque" }
		//
		Blend Off
		Fog { Mode off }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma target 3.0

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform int _UseColor;
			uniform float4 _Color;

			uniform int _Return;
			uniform float3 _Position;
			uniform float _Scale;

			bool _UseBumpMap;
			sampler2D _BumpMap;
			float _BumpScale;

			bool _UseHeightMap;
			sampler2D _ParallaxMap;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;

				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;

				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 worldPos : TEXCOORD2;
			};

			//
			v2f vert(appdata v, out float4 outpos : SV_POSITION)
			{
				v2f o;
				o.normal = v.normal;
				o.tangent = v.tangent;
				o.color = v.color;

				o.texcoord0 = v.texcoord0;
				o.texcoord1 = v.texcoord1;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				
				outpos = UnityObjectToClipPos(v.vertex);
				
				return o;
			}

			float4 frag(v2f input, UNITY_VPOS_TYPE screenPos : VPOS) : COLOR
			{
				bool left = screenPos.x <= .5;
				bool top = screenPos.y <= .5;

				if (top && left)
				{
					// Distance is used to determine position.
					float distance = length(input.worldPos - _Position) / _Scale;
					float3 wNormal = UnityObjectToWorldNormal(input.normal.xyz);

					if (!_UseBumpMap)
						return float4(wNormal * .5 + .5, distance);

					// Reference normal maps.
					float3 wTangent = UnityObjectToWorldDir(input.tangent.xyz);
					// Compute bitangent from cross product of normal and tangent.
					float tangentSign = input.tangent.w * unity_WorldTransformParams.w;
					float3 wBitangent = cross(wNormal, wTangent) * tangentSign;
					// Output the tangent space matrix.
					float3 a = float3(wTangent.x, wBitangent.x, wNormal.x);
					float3 b = float3(wTangent.y, wBitangent.y, wNormal.y);
					float3 c = float3(wTangent.z, wBitangent.z, wNormal.z);

					float2 uv = TRANSFORM_TEX(input.texcoord0.xy, _MainTex);
					float4 tBump = tex2Dlod(_BumpMap, float4(uv, 0, 0));
					float3 uBump = UnpackNormal(tBump);

					float3 normal = float3(
						dot(a, uBump),
						dot(b, uBump),
						dot(c, uBump));
					
					normal.rg *= _BumpScale;
					return float4(normal * .5 + .5, distance);
				}
				// High accuracy distance.
				else if (top && !left)
				{
					// Distance is used to determine position.
					float distance = length(input.worldPos - _Position) / _Scale;
					
					//float2 uv = TRANSFORM_TEX(input.texcoord0.xy, _MainTex);
					//return tex2D(_MainTex, uv);
					return EncodeFloatRGBA(distance);
				}
				// UV.
				else if (!top && left)
				{
					if (_UseHeightMap)
					{
						float2 uv = TRANSFORM_TEX(input.texcoord0.xy, _MainTex);
						return float4(
							input.texcoord0.xy,
							tex2Dlod(_ParallaxMap, float4(uv, 0, 0)).r,
							1
						);
					}
					else
						return float4(input.texcoord0.xy, .5, 1);
				}
				// Texcoord or vertex color.
				else if (!top && !left)
				{
					// Texcoord.
					if (_Return == 0) return input.texcoord0;
					else if (_Return == 1) return input.texcoord0;
					// Vertex Color.
					else if (_Return == 2) return input.color;
					
					// Renderer ID.
					return _Color;
				}

				return 0;
			}
			ENDCG
		}
	}
}
