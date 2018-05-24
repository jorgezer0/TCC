Shader "Custom/SimpleUnlitTexturedColoredShaderLightMaped"
{
    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        _MainTex ("Texture", 2D) = "white" {}
        _NormalTex("Normal Map", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _Offset ("Offset", Range (-1,1)) = 0
    }
    SubShader
    {
    	ZTest On
        Pass
        {
        	Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"

            // texture we will sample
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NormalTex;
			float4 _NormalTex_ST;
            float4 _Color;
            float _Offset;

			struct appdata_lightmap {
       			float4 vertex : POSITION;
        		float2 uv : TEXCOORD0;
       			float2 uv1 : TEXCOORD1;
       			float3 normal : NORMAL;
       			float4 tangent : TANGENT;
      		};

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float2 uv1 : TEXCOORD1;
                float3 tbn[3] : TEXCOORD2; // TEXCOORD2; TEXCOORD3;
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : NORMAL;
                fixed4 color : COLOR;
            };

            // vertex shader
            v2f vert (appdata_lightmap v)
            {
                v2f o;
                v.vertex.xyz += v.normal * _Offset;
				o.vertex = UnityObjectToClipPos(v.vertex);

        		o.uv = TRANSFORM_TEX(v.uv, _NormalTex);
        		o.uv1 = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

        		float3 normal = UnityObjectToWorldNormal(v.normal);
				float3 tangent = UnityObjectToWorldNormal(v.tangent);
				float3 bitangent = cross(tangent, normal);

				o.tbn[0] = tangent;
				o.tbn[1] = bitangent;
				o.tbn[2] = normal;

        		return o;
            }
            

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
            	float3 tangentNormal = tex2D(_NormalTex, i.uv) * 2 - 1;
				float3 surfaceNormal = i.tbn[2];
				float3 worldNormal = float3(i.tbn[0] * tangentNormal.r + i.tbn[1] * tangentNormal.g + i.tbn[2] * tangentNormal.b);

                // sample texture and return it
                float4 col = tex2D(_MainTex, i.uv) * _Color;
                half3 lightMap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
				float4 lightMapDir = UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd, unity_Lightmap, i.uv1);

				lightMap = DecodeDirectionalLightmap(lightMap, lightMapDir, worldNormal);

				col.rgb *= lightMap;

                return col;
            }
            ENDCG
        }
    }
}