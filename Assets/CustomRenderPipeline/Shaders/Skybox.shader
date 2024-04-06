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
            #include "../ShaderLibrary/Common.hlsl"

            float4 _Color1;
            float4 _Color2;
            float _Blend;

            struct Attributes
            {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings SkyPassVertex(Attributes input)
            {
                Varyings output;

                output.vertex = TransformObjectToHClip(TransformObjectToWorld(input.vertex));
                output.uv = input.uv;

                return output;
            }

            float4 SkyPassFragment(Varyings input) : SV_TARGET
            {
                return lerp(_Color1, _Color2, saturate((input.uv.y + 1) * 0.5 + _Blend));
            }

            ENDHLSL
        }
    }
}
