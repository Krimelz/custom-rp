#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

#define MAX_DIRECTIONAL_LIGHT_COUNT 4

CBUFFER_START(_CustomLight)
	int _DirectionalLightCount;
	float4 _DirectionalLightColor[MAX_DIRECTIONAL_LIGHT_COUNT];
	float4 _DirectionalLightDirection[MAX_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END

struct Light 
{
	float3 color;
	float3 direction;
};

int GetDirectionalLightCount() 
{
	return _DirectionalLightCount;
}

Light GetDirectionalLight(int index) 
{
	Light light;
	light.color = _DirectionalLightColor[index].rgb;
	light.direction = _DirectionalLightDirection[index].xyz;

	return light;
}

#endif // CUSTOM_LIGHT_INCLUDED