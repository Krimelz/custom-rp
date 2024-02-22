using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
	private partial void DrawUnsupportedShaders();
	private partial void DrawGizmos();

#if UNITY_EDITOR
	private static ShaderTagId _unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
	private static ShaderTagId[] _legasyShaderTagIds =
	{
		new ShaderTagId("Always"),
		new ShaderTagId("ForwardBase"),
		new ShaderTagId("PrepassBase"),
		new ShaderTagId("Vertex"),
		new ShaderTagId("VertexLMRGBM"),
		new ShaderTagId("VertexLM"),
	};

	private static Material _errorMaterial;

	private partial void DrawUnsupportedShaders()
	{
		if (_errorMaterial == null)
		{
			_errorMaterial = new Material(Shader.Find(ErrorShader));
		}

		var sortingSettings = new SortingSettings(_camera);
		var drawingSettings = new DrawingSettings(_legasyShaderTagIds[0], sortingSettings)
		{
			overrideMaterial = _errorMaterial
		};
		for (int i = 1; i < _legasyShaderTagIds.Length; i++)
		{
			drawingSettings.SetShaderPassName(i, _legasyShaderTagIds[i]);
		}
		var filteringSettings = FilteringSettings.defaultValue;

		_context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
	}

	private partial void DrawGizmos()
	{
		if (Handles.ShouldRenderGizmos())
		{
			_context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
			_context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
		}
	}
#endif
}
