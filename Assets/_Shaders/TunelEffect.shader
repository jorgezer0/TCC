Shader "_Effect/TunelEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise ("Noise", 2D) = "white" {}
		_SmoothSize ("SmoothSize", Range (0,1)) = 0
		_Smoothness ("Smoothness", Range (0,1)) = 0
		_NoiseIntensity ("NoiseIntensity", Range (0,1)) = 0
		_Bright ("Bright", Range (1,5)) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _Noise;
			float _SmoothSize;
			float _Smoothness;
			float _NoiseIntensity;
			float _Bright;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 result = tex2D(_MainTex, i.uv);
				fixed4 noise = tex2D(_Noise, i.uv);
				//fixed4 result = fixed4(0,0,0,0);
				// just invert the colors
				//col.rgb = 1 - col.rgb;
				//col += ((noise*-1) * (_Amount+1))*-1;	
				//col -= (((noise.r-1)*-1) * _Amount)*2;
				result = lerp(result - (_SmoothSize / noise.r)*_Smoothness, result, (_NoiseIntensity));
				result *= _Bright;
				//clip(col.r - _Amount);
				return result;
			}
			ENDCG
		}
	}

}
