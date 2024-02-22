using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Custom Render Pipeline", menuName = "Rendering/Custom RP")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
	[SerializeField]
	private Material _defaultMaterial;

	public override Material defaultMaterial => _defaultMaterial;

	protected override RenderPipeline CreatePipeline()
	{
		return new CustomRenderPipeline();
	}
}
