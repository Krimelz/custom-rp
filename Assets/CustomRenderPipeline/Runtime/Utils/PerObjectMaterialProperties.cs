using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
	[SerializeField]
	private Color _baseColor = Color.white;
	[SerializeField]
	private Texture _baseTexture;

	private static readonly int _baseColorId = Shader.PropertyToID("_BaseColor");
	private static readonly int _baseTextureId = Shader.PropertyToID("_BaseTexture");
	private static MaterialPropertyBlock _block;

	private void Awake()
	{
		OnValidate();
	}

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

	private void OnValidate()
	{
		if (_block == null)
		{
			_block = new MaterialPropertyBlock();
		}

		_block.SetColor(_baseColorId, _baseColor);
		_block.SetTexture(_baseTextureId, _baseTexture);

		GetComponent<Renderer>().SetPropertyBlock(_block);
	}
}
