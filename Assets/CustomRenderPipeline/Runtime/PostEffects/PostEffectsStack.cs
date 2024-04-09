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

	public void Setup(ScriptableRenderContext context, Camera camera, PostEffectsSettings settings)
	{
		_context = context;
		_camera = camera;
		_settings = camera.cameraType <= CameraType.SceneView ? settings : null;

		ApplySceneViewState();
	}

	public bool IsActive => _settings != null;

	public void Render(int sourceId)
	{
		Draw(sourceId, BuiltinRenderTextureType.CameraTarget, _settings.Pass);
		_context.ExecuteCommandBuffer(_commandBuffer);
		_commandBuffer.Clear();
	}

	private void Draw(RenderTargetIdentifier from, RenderTargetIdentifier to, Pass pass)
	{
		_commandBuffer.BeginSample(pass.ToString());

		_commandBuffer.SetGlobalFloat(Shader.PropertyToID("_PixelationFactor"), _settings.PixelationSettings.Factor);
		_commandBuffer.SetGlobalTexture(_effectSourceId, from);
		_commandBuffer.SetRenderTarget(to, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
		_commandBuffer.DrawProcedural(Matrix4x4.identity, _settings.Material, (int)pass, MeshTopology.Triangles, 3);

		_commandBuffer.EndSample(pass.ToString());
	}
}
