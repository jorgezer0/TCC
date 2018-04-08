Shader "Unlit/AcidShader"
{
	Properties
	{
		_MainColor ("Tint Color", COLOR) = (1,1,1,1)
		_SecondaryColor ("Second Tint Color", COLOR) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTexture ("Noise Texture", 2D) = "white" {}
		_NoiseIntensity ("Intensity of Noise Used", RANGE(0,2)) = 100
		_NoiseOscilation ("Animation Noise Value", RANGE(0,1)) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseTexture;
			float4 _NoiseTexture_ST;
			half _NoiseIntensity;
			half _NoiseOscilation;
			half4 _MainColor;
			half4 _SecondaryColor;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				//Traditional UV Mapping 
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.uv2 = TRANSFORM_TEX(v.uv, _NoiseTexture);

				//Worldspace mapping
				half3 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
				o.uv = TRANSFORM_TEX(float2 (worldPos.x, worldPos.z), _MainTex);
				o.uv2 = TRANSFORM_TEX(float2 (worldPos.x, worldPos.z), _NoiseTexture);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col2 = tex2D(_NoiseTexture, i.uv2) * _NoiseIntensity;
				fixed4 col = tex2D(_MainTex, i.uv + half2(sin(col2.x),cos(col2.y)) * _NoiseOscilation) * lerp(_MainColor, _SecondaryColor, col2.x);
				

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
