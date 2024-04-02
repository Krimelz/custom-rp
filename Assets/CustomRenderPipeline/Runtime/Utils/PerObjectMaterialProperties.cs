using UnityEngine;

namespace CustomRenderPipeline.Runtime.Utils
{
	[DisallowMultipleComponent]
	public class PerObjectMaterialProperties : MonoBehaviour
	{
		protected static MaterialPropertyBlock _block;

		private void Awake()
		{
			OnValidate();
		}

		protected virtual void OnValidate()
		{
			GetComponent<Renderer>().SetPropertyBlock(_block);
		}
	}
}
