using UnityEngine;

namespace CustomRenderPipeline.Runtime.Utils
{
	[DisallowMultipleComponent]
	public class PerObjectMaterialSmoothness : PerObjectMaterialProperties
	{
		[SerializeField, Range(0f, 1f)]
		private float _smoothness = 0.5f;

		private static readonly int _smoothnessId = Shader.PropertyToID("_Smoothness");

		[ContextMenu("Randomize Smoothness")]
		private void RandomizeSmoothness()
		{
			_smoothness = Random.value;

			OnValidate();
		}

		protected override void OnValidate()
		{
			if (_block == null)
			{
				_block = new MaterialPropertyBlock();
			}

			_block.SetFloat(_smoothnessId, _smoothness);

			base.OnValidate();
		}
	}
}
