using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
	private const string BufferName = "Lighting";
	private const int MaxDirectionalLightCount = 4;

	private static readonly int DirectionalLightCountId = Shader.PropertyToID("_DirectionalLightCount");
	private static readonly int DirectionalLightColorId = Shader.PropertyToID("_DirectionalLightColor");
	private static readonly int DirectionalLightDirectionId = Shader.PropertyToID("_DirectionalLightDirection");

	private static Vector4[] DirectionalLightColors = new Vector4[MaxDirectionalLightCount];
	private static Vector4[] DirectionalLightDirections = new Vector4[MaxDirectionalLightCount];

	private CommandBuffer _commandBuffer = new CommandBuffer()
	{
		name = BufferName,
	};
	private CullingResults _cullingResults;

	public void Setup(ScriptableRenderContext context, CullingResults cullingResults)
	{
		_cullingResults = cullingResults;

		_commandBuffer.BeginSample(BufferName);
		SetupLights();
		_commandBuffer.EndSample(BufferName);

		context.ExecuteCommandBuffer(_commandBuffer);

		_commandBuffer.Clear();
	}

	private void SetupLights()
	{
		NativeArray<VisibleLight> visibleLights = _cullingResults.visibleLights;
		int directionalLightCount = 0;

		for (int i = 0; i < visibleLights.Length; i++)
		{
			VisibleLight light = visibleLights[i];

			if (light.lightType == LightType.Directional)
			{
				SetupDirectionalLight(directionalLightCount++, ref light);

				if (directionalLightCount >= MaxDirectionalLightCount)
				{
					break;
				}
			}
		}

		_commandBuffer.SetGlobalInt(DirectionalLightCountId, visibleLights.Length);
		_commandBuffer.SetGlobalVectorArray(DirectionalLightColorId, DirectionalLightColors);
		_commandBuffer.SetGlobalVectorArray(DirectionalLightDirectionId, DirectionalLightDirections);
	}

	private void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
	{
		DirectionalLightColors[index] = visibleLight.finalColor;
		DirectionalLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
	}
}
