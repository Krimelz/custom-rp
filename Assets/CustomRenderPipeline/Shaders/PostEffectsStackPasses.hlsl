#ifndef CUSTOM_POST_EFFECTS_PASSES_INCLUDED
#define CUSTOM_POST_EFFECTS_PASSES_INCLUDED

TEXTURE2D(_PostEffectSource);
SAMPLER(sampler_linear_clamp);

float _PixelationFactor;

struct Varyings
{
	float4 positionCS : SV_POSITION;
	float2 screenUV : VAR_SCREEN_UV;
};

float4 GetSource(float2 screenUV)
{
	return SAMPLE_TEXTURE2D_LOD(_PostEffectSource, sampler_linear_clamp, screenUV, 0);
}

Varyings DefaultPassVertex(uint vertexId : SV_VERTEXID)
{
	Varyings output;

	output.positionCS = float4(
		vertexId <= 1 ? -1.0 : 3.0,
		vertexId == 1 ? 3.0 : -1.0,
		0.0, 
		1.0
	);

	output.screenUV = float2(
		vertexId <= 1 ? 0.0 : 2.0,
		vertexId == 1 ? 2.0 : 0.0
	);

	if (_ProjectionParams.x < 0.0)
	{
		output.screenUV.y = 1.0 - output.screenUV.y;
	}

	return output;
}

float4 CopyPassFragment(Varyings input) : SV_TARGET 
{
	return GetSource(input.screenUV);
}

float4 PixelationPassFragment(Varyings input) : SV_TARGET
{
	input.screenUV = round(input.screenUV * _PixelationFactor) / _PixelationFactor;

	return GetSource(input.screenUV);
}

float4 GrayscalePassFragment(Varyings input) : SV_TARGET
{
	float4 source = GetSource(input.screenUV);
	float avg = (source.r + source.g + source.b) / 3.0;

	return float4(avg, avg, avg, 1.0);
}

#endif // CUSTOM_POST_EFFECTS_PASSES_INCLUDED
