using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Custom Render Pipeline", menuName = "Rendering/Custom RP")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
	[SerializeField]
	private Material _defaultMaterial;
	[SerializeField]
	private Material _screenMaterial;

	public override Material defaultMaterial => _defaultMaterial;
	public Material ScreenMaterial => _screenMaterial;

	protected override RenderPipeline CreatePipeline()
	{
		return new CustomRenderPipeline();
	}
}
