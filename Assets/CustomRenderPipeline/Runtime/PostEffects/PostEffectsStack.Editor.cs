using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public partial class PostEffectsStack
{
	private partial void ApplySceneViewState();

#if UNITY_EDITOR

	private partial void ApplySceneViewState()
	{
		if (_camera.cameraType == CameraType.SceneView 
			&& !SceneView.currentDrawingSceneView.sceneViewState.showImageEffects)
		{
			_settings = null;
		}
	}

#endif
}
