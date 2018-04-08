// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SimpleUnlitTexturedColoredShaderLightMaped"
{
    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _Offset ("Offset", Range (-1,1)) = 0
    }
    SubShader
    {
    	ZTest On
        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag
            #include "UnityCG.cginc"

            // texture we will sample
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Offset;

              // sampler2D unity_Lightmap;
      		  // float4 unity_LightmapST;

            // vertex shader inputs
//            struct appdata
//            {
//                float4 vertex : POSITION; // vertex position
//                float2 uv : TEXCOORD0; // texture coordinate
//                float3 normal : NORMAL;
//            };

			struct appdata_lightmap {
       			float4 vertex : POSITION;
        		float2 texcoord : TEXCOORD0;
       			float2 texcoord1 : TEXCOORD1;
       			float3 normal : NORMAL;
      		};

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float2 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : NORMAL;
                fixed4 color : COLOR;
            };

            // vertex shader
            v2f vert (appdata_lightmap v)
            {
                v2f o;
                v.vertex.xyz += v.normal * _Offset;

//                o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = v.uv;
                // UnityCG.cginc - Transforms 2D UV by scale/bias property
        		// #define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
        		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

        		// Use `unity_LightmapST` NOT `unity_Lightmap_ST`
        		o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
        		return o;
            }
            

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
                return col;
            }
            ENDCG
        }
    }
}