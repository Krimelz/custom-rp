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
	private Shader _shader = default;

	[SerializeField]
	private Pass _pass;
	[field: SerializeField]
	public PixelationSettings PixelationSettings { get; set; }

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

	public Pass Pass => _pass;
}
