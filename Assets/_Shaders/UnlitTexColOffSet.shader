Shader "Custom/SimpleUnlitTexturedColoredShader"
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
            float4 _Color;
            float _Offset;

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
                float3 normal : NORMAL;
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : NORMAL;
                fixed4 color : COLOR;
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.xyz += v.normal * _Offset;

				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
        		return o;
            }
            

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}