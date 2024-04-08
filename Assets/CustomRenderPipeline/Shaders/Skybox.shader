Shader "Custom RP/Skybox"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _BottomColor ("Bottom Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _BlendFactor ("Lerp", Range(-1.0, 1.0)) = 0.0
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
