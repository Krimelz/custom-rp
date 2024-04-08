Shader "Custom RP/Skybox"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.0, 0.0, 0.0, 0.0)
        _Color2 ("Color 2", Color) = (1.0, 1.0, 1.0, 1.0)
        _Blend ("Lerp", Range(-1.0, 1.0)) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Background"
            "Queue" = "Background"
        }

        ZWrite Off
        Cull Off
        Fog
        {
            Mode Off
        }

        Pass
        {
            HLSLPROGRAM

            #pragma target 3.5
            #pragma vertex SkyPassVertex
            #pragma fragment SkyPassFragment
            #include "SkyboxPass.hlsl"

            ENDHLSL
        }
    }
}
