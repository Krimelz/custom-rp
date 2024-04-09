using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRenderPipeline.Runtime
{
	public class CustomRenderPipeline : RenderPipeline
	{
		private bool _useDynamicBatching;
		private bool _useGPUInstancing;

		private CameraRenderer _renderer = new CameraRenderer();
		private PostEffectsSettings _postEffectsSettings;

		public CustomRenderPipeline(PostEffectsSettings postEffectsSettings, 
			bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
		{
			_postEffectsSettings = postEffectsSettings;
			_useDynamicBatching = useDynamicBatching;
			_useGPUInstancing = useGPUInstancing;
			GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
			GraphicsSettings.lightsUseLinearIntensity = true;
		}

		protected override void Render(ScriptableRenderContext context, Camera[] cameras)
		{

		}

		protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
		{
			for (int i = 0; i < cameras.Count; i++)
			{
				_renderer.Render(context, cameras[i], _postEffectsSettings, _useDynamicBatching, _useGPUInstancing);
			}
		}
	}
}