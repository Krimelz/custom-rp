Shader "Hidden/Custom RP/Post Effects Stack"
{
	SubShader
	{
		Cull Off
		ZTest Always
		ZWrite Off

		HLSLINCLUDE
		#include "../ShaderLibrary/Common.hlsl"
		#include "PostEffectsStackPasses.hlsl"
		ENDHLSL

		Pass
		{
			Name "Copy"

			HLSLPROGRAM

			#pragma vertex DefaultPassVertex
			#pragma fragment CopyPassFragment

			ENDHLSL
		}

		Pass
		{
			Name "Pixelation"

			HLSLPROGRAM

			#pragma vertex DefaultPassVertex
			#pragma fragment PixelationPassFragment

			ENDHLSL
		}

		Pass
		{
			Name "Grayscale"

			HLSLPROGRAM

			#pragma vertex DefaultPassVertex
			#pragma fragment GrayscalePassFragment

			ENDHLSL
		}

		Pass
		{
			Name "Posterization"

			HLSLPROGRAM

			#pragma vertex DefaultPassVertex
			#pragma fragment PosterizationPassFragment

			ENDHLSL
		}
	}
}