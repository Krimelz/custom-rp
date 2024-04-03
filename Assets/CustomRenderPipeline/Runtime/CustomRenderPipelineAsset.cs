using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRenderPipeline.Runtime
{
	[CreateAssetMenu(fileName = "Custom Render Pipeline", menuName = "Rendering/Custom RP")]
	public class CustomRenderPipelineAsset : RenderPipelineAsset
	{
		[SerializeField]
		private Material _defaultMaterial;

		[SerializeField]
		bool _useDynamicBatching = true;
		[SerializeField]
		bool _useGPUInstancing = true;
		[SerializeField]
		bool _useSRPBatcher = true;

		public override Material defaultMaterial => _defaultMaterial;

		protected override RenderPipeline CreatePipeline()
		{
			return new CustomRenderPipeline(
				_useDynamicBatching,
				_useGPUInstancing,
				_useSRPBatcher
			);
		}
	}
}
