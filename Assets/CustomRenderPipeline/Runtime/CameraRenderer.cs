using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRenderPipeline.Runtime
{
	public partial class CameraRenderer
	{
		private const string BufferName = "Render Camera";

		private static readonly int FrameBufferId = Shader.PropertyToID("_FrameBuffer");

		private CommandBuffer _commandBuffer = new() 
		{ 
			name = BufferName 
		};
		private ScriptableRenderContext _context;
		private CullingResults _cullingResults;
		private Camera _camera;
		private Lighting _lighting = new();
		private PostEffectsStack _postEffectsStack = new PostEffectsStack();

		public void Render(ScriptableRenderContext context, Camera camera, 
			PostEffectsSettings postEffectsSettings, bool useDynamicBatching, bool useGPUInstancing)
		{
			_context = context;
			_camera = camera;

			PrepareBuffer();
			PrepareForSceneView();

			if (!Cull())
			{
				return;
			}

			Setup();
			_lighting.Setup(context, _cullingResults);
			_postEffectsStack.Setup(context, camera, postEffectsSettings);
			DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
			DrawUnsupportedShaders();
			DrawGizmosBeforeEffects();
			if (_postEffectsStack.IsActive)
			{
				_postEffectsStack.Render(FrameBufferId);
			}
			DrawGizmosAfterEffects();
			Cleanup();
			Submit();
		}

		private bool Cull()
		{
			if (!_camera.TryGetCullingParameters(out var parameters))
			{
				return false;
			}

			_cullingResults = _context.Cull(ref parameters);

			return true;

		}

		private void Setup()
		{
			_context.SetupCameraProperties(_camera);

			CameraClearFlags flags = _camera.clearFlags;

			if (_postEffectsStack.IsActive)
			{
				if (flags > CameraClearFlags.Color)
				{
					flags = CameraClearFlags.Color;
				}

				_commandBuffer.GetTemporaryRT(FrameBufferId, _camera.pixelWidth, _camera.pixelHeight, 
					32, FilterMode.Bilinear, RenderTextureFormat.Default
				);
				_commandBuffer.SetRenderTarget(FrameBufferId, 
					RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
				);
			}

			_commandBuffer.ClearRenderTarget(
				flags <= CameraClearFlags.Depth,
				flags <= CameraClearFlags.Color,
				flags == CameraClearFlags.Color ? _camera.backgroundColor.linear : Color.clear
			);

			_commandBuffer.BeginSample(SampleName);
			ExecuteBuffer();
		}

		private void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
		{
			var sortingSettings = new SortingSettings(_camera)
			{
				criteria = SortingCriteria.CommonOpaque
			};
			var drawingSettings = new DrawingSettings(
				UnlitShaderTagId,
				sortingSettings
			)
			{
				enableDynamicBatching = useDynamicBatching,
				enableInstancing = useGPUInstancing,
			};
			drawingSettings.SetShaderPassName(1, LitShaderTagId);
			var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

			_context.DrawRenderers(
				_cullingResults,
				ref drawingSettings,
				ref filteringSettings
			);
			_context.DrawSkybox(_camera);

			sortingSettings.criteria = SortingCriteria.CommonTransparent;
			drawingSettings.sortingSettings = sortingSettings;
			filteringSettings.renderQueueRange = RenderQueueRange.transparent;

			_context.DrawRenderers(
				_cullingResults,
				ref drawingSettings,
				ref filteringSettings
			);
		}

		private void Submit()
		{
			_commandBuffer.EndSample(SampleName);
			ExecuteBuffer();
			_context.Submit();
		}

		private void ExecuteBuffer()
		{
			_context.ExecuteCommandBuffer(_commandBuffer);
			_commandBuffer.Clear();
		}

		private void Cleanup()
		{
			if (_postEffectsStack.IsActive)
			{
				_commandBuffer.ReleaseTemporaryRT(FrameBufferId);
			}
		}
	}
}
