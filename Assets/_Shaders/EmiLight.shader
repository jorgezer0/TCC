Shader "Custom/EmiLight" {
Properties {
    _Illumi ("Illumi Color", Color) = (1,1,1,1) 
}
 
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 100
    
    Pass {
        Lighting On
        Material {
        Emission [_Illumi]
        }
    }
}
}