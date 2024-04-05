using UnityEngine;

namespace CustomRenderPipeline.Runtime.Utils
{
	[DisallowMultipleComponent]
	public class PerObjectMaterialMetallic : PerObjectMaterialProperties
	{
		[SerializeField, Range(0f, 1f)]
		private float _metallic = 0.5f;

		private static readonly int _metallicId = Shader.PropertyToID("_Metallic");

		[ContextMenu("Randomize Metallic")]
		private void RandomizeMetallic()
		{
			_metallic = Random.value;

			OnValidate();
		}

		protected override void OnValidate()
		{
			if (_block == null)
			{
				_block = new MaterialPropertyBlock();
			}

			_block.SetFloat(_metallicId, _metallic);

			base.OnValidate();
		}
	}
}
