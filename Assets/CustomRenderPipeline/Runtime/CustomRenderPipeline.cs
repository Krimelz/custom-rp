using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
	private bool _useDynamicBatching;
	private bool _useGPUInstancing;

	private CameraRenderer _renderer = new CameraRenderer();

	public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
	{
		_useDynamicBatching = useDynamicBatching;
		_useGPUInstancing = useGPUInstancing;
		GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
	}

	protected override void Render(ScriptableRenderContext context, Camera[] cameras)
	{

	}

	protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
	{
		for (int i = 0; i < cameras.Count; i++)
		{
			_renderer.Render(context, cameras[i], _useDynamicBatching, _useGPUInstancing);
		}
	}
}
