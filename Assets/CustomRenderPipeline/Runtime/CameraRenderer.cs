using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
	private const string BufferName = "Render Camera";
	private const string ErrorShader = "Hidden/InternalErrorShader";

	private CommandBuffer _buffer = new() { name = BufferName };
	private ScriptableRenderContext _context;
	private CullingResults _cullingResults;
	private Camera _camera;

	public void Render(ScriptableRenderContext context, Camera camera)
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
		DrawVisibleGeometry();
		DrawUnsupportedShaders();
		DrawGizmos();
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
		_buffer.ClearRenderTarget(
			flags <= CameraClearFlags.Depth, 
			flags <= CameraClearFlags.Color, 
			flags == CameraClearFlags.Color ? _camera.backgroundColor.linear : Color.clear
		);

		_buffer.BeginSample(SampleName);
		ExecuteBuffer();
	}

	private void DrawVisibleGeometry()
	{
		var sortingSettings = new SortingSettings(_camera)
		{
			criteria = SortingCriteria.CommonOpaque
		};
		var drawingSettings = new DrawingSettings(
			UnlitShaderTagId, 
			sortingSettings
		);
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
		_buffer.EndSample(SampleName);
		ExecuteBuffer();
		_context.Submit();
	}

	private void ExecuteBuffer()
	{
		_context.ExecuteCommandBuffer(_buffer);
		_buffer.Clear();
	}
}
