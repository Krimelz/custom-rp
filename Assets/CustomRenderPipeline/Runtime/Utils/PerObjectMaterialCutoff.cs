using UnityEngine;

namespace CustomRenderPipeline.Runtime.Utils
{
	[DisallowMultipleComponent]
	public class PerObjectMaterialCutoff : PerObjectMaterialProperties
	{
		[SerializeField, Range(0f, 1f)]
		private float _cutoff = 0.5f;

		private static readonly int _cutoffId = Shader.PropertyToID("_Cutoff");

		[ContextMenu("Randomize Cutoff")]
		private void RandomizeColor()
		{
			_cutoff = Random.value;

			OnValidate();
		}

		protected override void OnValidate()
		{
			if (_block == null)
			{
				_block = new MaterialPropertyBlock();
			}

			_block.SetFloat(_cutoffId, _cutoff);

			base.OnValidate();
		}
	}
}
