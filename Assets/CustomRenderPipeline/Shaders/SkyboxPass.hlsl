#ifndef CUSTOM_SKYBOX_PASS_INCLUDED
#define CUSTOM_SKYBOX_PASS_INCLUDED

#include "../ShaderLibrary/Common.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 _Color1;
    float4 _Color2;
    float _Blend;
CBUFFER_END

struct Attributes
{
    float3 vertex : POSITION;
    float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings SkyPassVertex(Attributes input)
{
    Varyings output;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    output.vertex = TransformObjectToHClip(TransformObjectToWorld(input.vertex));
    output.uv = input.uv;

    return output;
}

float4 SkyPassFragment(Varyings input) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(input);

    return lerp(_Color1, _Color2, saturate((input.uv.y + 1) * 0.5 + _Blend));
}

#endif // CUSTOM_SKYBOX_PASS_INCLUDED