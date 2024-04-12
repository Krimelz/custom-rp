using UnityEditor;
using UnityEngine;

public partial class PostEffectsStack
{
	partial void ApplySceneViewState();

#if UNITY_EDITOR

	partial void ApplySceneViewState()
	{
		if (_camera.cameraType == CameraType.SceneView 
			&& !SceneView.currentDrawingSceneView.sceneViewState.showImageEffects)
		{
			_settings = null;
		}
	}

#endif
}
