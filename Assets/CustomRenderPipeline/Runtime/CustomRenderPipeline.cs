﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
	private CameraRenderer _renderer = new CameraRenderer();

	public CustomRenderPipeline()
	{
		GraphicsSettings.useScriptableRenderPipelineBatching = true;
	}

	protected override void Render(ScriptableRenderContext context, Camera[] cameras)
	{

	}

	protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
	{
		for (int i = 0; i < cameras.Count; i++)
		{
			_renderer.Render(context, cameras[i]);
		}
	}
}
