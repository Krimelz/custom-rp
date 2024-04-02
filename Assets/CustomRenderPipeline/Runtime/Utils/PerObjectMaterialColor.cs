using UnityEngine;

namespace CustomRenderPipeline.Runtime.Utils
{
	[DisallowMultipleComponent]
	public class PerObjectMaterialColor : PerObjectMaterialProperties
	{
		[SerializeField]
		private Color _baseColor = Color.white;

		private static readonly int _baseColorId = Shader.PropertyToID("_BaseColor");

		[ContextMenu("Randomize Color")]
		private void RandomizeColor()
		{
			_baseColor = Color.HSVToRGB(
				Random.value,
				1f,
				1f
			);

			OnValidate();
		}

		[ContextMenu("Randomize Alpha")]
		private void RandomizeAlpha()
		{
			_baseColor.a = Random.value;

			OnValidate();
		}

		[ContextMenu("Clear Color and Alpha")]
		private void ClearColorAndAlpha()
		{
			_baseColor = Color.white;

			OnValidate();
		}

		protected override void OnValidate()
		{
			if (_block == null)
			{
				_block = new MaterialPropertyBlock();
			}

			_block.SetColor(_baseColorId, _baseColor);

			base.OnValidate();
		}
	}
}
