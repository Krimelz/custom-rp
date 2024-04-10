using UnityEngine;
using UnityEngine.Rendering;

public partial class PostEffectsStack
{
	private const string BufferName = "Post Effects";

	private CommandBuffer _commandBuffer = new()
	{
		name = BufferName,
	};
	private ScriptableRenderContext _context;
	private Camera _camera;
	private PostEffectsSettings _settings;

	private int _effectSourceId = Shader.PropertyToID("_PostEffectSource");
	private int _effectTempId = Shader.PropertyToID("_PostEffectTemp1");

	public void Setup(ScriptableRenderContext context, Camera camera, PostEffectsSettings settings)
	{
		_context = context;
		_camera = camera;
		_settings = camera.cameraType <= CameraType.SceneView ? settings : null;

		ApplySceneViewState();
	}

	public bool IsActive => _settings != null && _settings.Passes.Length > 0;

	public void Render(int sourceId)
	{
		_commandBuffer.GetTemporaryRT(_effectTempId, _camera.pixelWidth, _camera.pixelHeight);

		int from = sourceId;
		int to = _effectTempId;

		for (int i = 0; i < _settings.Passes.Length - 1; i++)
		{
			Draw(from, to, _settings.Passes[i]);
			(from, to) = (to, from);
		}

		Draw(from, BuiltinRenderTextureType.CameraTarget, _settings.Passes[_settings.Passes.Length - 1]);

		_commandBuffer.ReleaseTemporaryRT(_effectTempId);

		_context.ExecuteCommandBuffer(_commandBuffer);
		_commandBuffer.Clear();
	}

	private void Draw(RenderTargetIdentifier from, RenderTargetIdentifier to, Pass pass)
	{
		_commandBuffer.BeginSample(pass.ToString());

		_commandBuffer.SetGlobalFloat(Shader.PropertyToID("_PixelationFactor"), _settings.PixelationFactor);
		_commandBuffer.SetGlobalFloat(Shader.PropertyToID("_GrayscaleFactor"), _settings.GrayscaleFactor);
		_commandBuffer.SetGlobalFloat(Shader.PropertyToID("_PosterizationFactor"), _settings.PosterizationFactor);
		_commandBuffer.SetGlobalTexture(_effectSourceId, from);

		_commandBuffer.SetRenderTarget(to, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
		_commandBuffer.DrawProcedural(Matrix4x4.identity, _settings.Material, (int)pass, MeshTopology.Triangles, 3);

		_commandBuffer.EndSample(pass.ToString());
	}
}
