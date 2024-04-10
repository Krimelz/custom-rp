using System;
using UnityEngine;

[Serializable]
public struct PixelationSettings
{
	[Min(1f)]
	public float Factor;
}

[CreateAssetMenu(menuName = "Rendering/Post Effects Settings")]
public class PostEffectsSettings : ScriptableObject
{
	[SerializeField]
	private Shader _shader;
	[SerializeField]
	private Pass[] _passes;

	[field: SerializeField, Range(2, 512)]
	public int PixelationFactor { get; set; } = 32;

	[field: SerializeField, Range(-8f, 8f)]
	public float GrayscaleFactor { get; set; } = 1f;

	[field: SerializeField, Range(1f, 32f)]
	public float PosterizationFactor { get; set; } = 4f;

	[NonSerialized]
	private Material _material;

	public Material Material
	{
		get
		{
			if (_material == null && _shader != null)
			{
				_material = new Material(_shader);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}

			return _material;
		}
	}

	public Pass[] Passes => _passes;
}
