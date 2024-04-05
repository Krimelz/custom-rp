Shader "Custom RP/Lit"
{
    Properties
    {
        _BaseTexture ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1.0, 1.0, 1.0, 1.0)

        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.5
        _Smoothness ("Smoothness", Range(0.0, 1.0)) = 0.5

        [Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0.0
        _Cutoff("Cutoff", Range(0.0, 1.0)) = 0.0

        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
    }

    SubShader
    {
        Tags
        {
            "LightMode" = "CustomLit"
        }

        Pass
        {
            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]

            HLSLPROGRAM

            #pragma target 3.5
            #pragma shader_feature _CLIPPING
            #pragma multi_compile_instancing
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment
            #include "LitPass.hlsl"

            ENDHLSL
        }
    }
}
