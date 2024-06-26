﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace CustomRenderPipeline.Runtime
{
	public partial class CameraRenderer
	{
		partial void DrawUnsupportedShaders();
		partial void DrawGizmosBeforeEffects();
		partial void DrawGizmosAfterEffects();
		partial void PrepareForSceneView();
		partial void PrepareBuffer();

#if UNITY_EDITOR
		private const string ErrorShader = "Hidden/InternalErrorShader";

		private static readonly ShaderTagId[] LegasyShaderTagIds =
		{
			new("Always"),
			new("ForwardBase"),
			new("PrepassBase"),
			new("Vertex"),
			new("VertexLMRGBM"),
			new("VertexLM"),
		};

		private static Material _errorMaterial;

		private string SampleName { get; set; }

		partial void DrawUnsupportedShaders()
		{
			if (_errorMaterial == null)
			{
				_errorMaterial = new Material(Shader.Find(ErrorShader));
			}

			var sortingSettings = new SortingSettings(_camera);
			var drawingSettings = new DrawingSettings(LegasyShaderTagIds[0], sortingSettings)
			{
				overrideMaterial = _errorMaterial
			};
			for (int i = 1; i < LegasyShaderTagIds.Length; i++)
			{
				drawingSettings.SetShaderPassName(i, LegasyShaderTagIds[i]);
			}
			var filteringSettings = FilteringSettings.defaultValue;

			_context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
		}

		partial void DrawGizmosBeforeEffects()
		{
			if (Handles.ShouldRenderGizmos())
			{
				_context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
			}
		}

		partial void DrawGizmosAfterEffects()
		{
			if (Handles.ShouldRenderGizmos())
			{
				_context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
			}
		}

		partial void PrepareForSceneView()
		{
			if (_camera.cameraType == CameraType.SceneView)
			{
				ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
			}
		}

		partial void PrepareBuffer()
		{
			Profiler.BeginSample("Editor Only");
			_commandBuffer.name = SampleName = _camera.name;
			Profiler.EndSample();
		}
#else
	private string SampleName => BufferName;
#endif
	}
}
