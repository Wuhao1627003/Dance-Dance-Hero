using UnityEngine;
using CW.Common;

namespace SpaceGraphicsToolkit.Jovian
{
	/// <summary>This component allows you to generate the <b>ScatteringTex</b> setting for cloudsphere materials.</summary>
	[ExecuteInEditMode]
	[RequireComponent(typeof(SgtJovian))]
	[HelpURL(SgtCommon.HelpUrlPrefix + "SgtJovianScattering")]
	[AddComponentMenu(SgtCommon.ComponentMenuPrefix + "Jovian ScatteringTex")]
	public class SgtJovianScatteringTex : MonoBehaviour
	{
		/// <summary>The transition style between the day and night.</summary>
		public SgtEase.Type SunsetEase { set { if (sunsetEase != value) { sunsetEase = value; DirtyTexture(); } } get { return sunsetEase; } } [SerializeField] private SgtEase.Type sunsetEase = SgtEase.Type.Smoothstep;

		/// <summary>The start point of the sunset (0 = dark side, 1 = light side).</summary>
		public float SunsetStart { set { if (sunsetStart != value) { sunsetStart = value; DirtyTexture(); } } get { return sunsetStart; } } [SerializeField] [Range(0.0f, 1.0f)] private float sunsetStart = 0.4f;

		/// <summary>The end point of the sunset (0 = dark side, 1 = light side).</summary>
		public float SunsetEnd { set { if (sunsetEnd != value) { sunsetEnd = value; DirtyTexture(); } } get { return sunsetEnd; } } [SerializeField] [Range(0.0f, 1.0f)] private float sunsetEnd = 0.6f;

		/// <summary>The sharpness of the sunset red channel transition.</summary>
		public float SunsetSharpnessR { set { if (sunsetSharpnessR != value) { sunsetSharpnessR = value; DirtyTexture(); } } get { return sunsetSharpnessR; } } [SerializeField] private float sunsetSharpnessR = 1.0f;

		/// <summary>The sharpness of the sunset green channel transition.</summary>
		public float SunsetSharpnessG { set { if (sunsetSharpnessG != value) { sunsetSharpnessG = value; DirtyTexture(); } } get { return sunsetSharpnessG; } } [SerializeField] private float sunsetSharpnessG = 1.0f;

		/// <summary>The sharpness of the sunset blue channel transition.</summary>
		public float SunsetSharpnessB { set { if (sunsetSharpnessB != value) { sunsetSharpnessB = value; DirtyTexture(); } } get { return sunsetSharpnessB; } } [SerializeField] private float sunsetSharpnessB = 1.0f;

		[System.NonSerialized]
		private Texture2D generatedTexture;

		[System.NonSerialized]
		private SgtJovian cachedJovian;

		private static int _SGT_ScatteringTex = Shader.PropertyToID("_SGT_ScatteringTex");

		public Texture2D GeneratedTexture
		{
			get
			{
				return generatedTexture;
			}
		}

		public void DirtyTexture()
		{
			UpdateTexture();
		}

#if UNITY_EDITOR
		/// <summary>This method allows you to export the generated texture as an asset.
		/// Once done, you can remove this component, and set the <b>SgtJovian</b> component's <b>ScatteringTex</b> setting using the exported asset.</summary>
		[ContextMenu("Export Texture")]
		public void ExportTexture()
		{
			var importer = CwHelper.ExportTextureDialog(generatedTexture, "Jovian Scattering");

			if (importer != null)
			{
				importer.textureCompression  = UnityEditor.TextureImporterCompression.Uncompressed;
				importer.alphaSource         = UnityEditor.TextureImporterAlphaSource.FromInput;
				importer.wrapMode            = TextureWrapMode.Clamp;
				importer.filterMode          = FilterMode.Trilinear;
				importer.anisoLevel          = 16;
				importer.alphaIsTransparency = true;

				importer.SaveAndReimport();
			}
		}
#endif

		protected virtual void OnEnable()
		{
			cachedJovian = GetComponent<SgtJovian>();

			cachedJovian.OnSetProperties += HandleSetProperties;

			UpdateTexture();
		}

		protected virtual void OnDisable()
		{
			cachedJovian.OnSetProperties -= HandleSetProperties;
		}

		protected virtual void OnDestroy()
		{
			CwHelper.Destroy(generatedTexture);
		}

		protected virtual void OnDidApplyAnimationProperties()
		{
			UpdateTexture();
		}

#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			UpdateTexture();
		}
#endif

		private void HandleSetProperties(Material properties)
		{
			properties.SetTexture(_SGT_ScatteringTex, generatedTexture);
		}

		private void UpdateTexture()
		{
			var width = 64;

			// Destroy if invalid
			if (generatedTexture != null)
			{
				if (generatedTexture.width != width || generatedTexture.height != 1)
				{
					generatedTexture = CwHelper.Destroy(generatedTexture);
				}
			}

			// Create?
			if (generatedTexture == null)
			{
				generatedTexture = CwHelper.CreateTempTexture2D("Scattering (Generated)", width, 1, TextureFormat.ARGB32);

				generatedTexture.wrapMode = TextureWrapMode.Clamp;
			}

			var stepU = 1.0f / (width  - 1);

			for (var x = 0; x < width; x++)
			{
				WritePixel(stepU * x, x);
			}

			generatedTexture.Apply();
		}

		private void WritePixel(float u, int x)
		{
			var sunsetU = Mathf.InverseLerp(sunsetEnd, sunsetStart, u);
			var color   = default(Color);

			color.r = SgtEase.Evaluate(sunsetEase, 1.0f - CwHelper.Sharpness(sunsetU, sunsetSharpnessR));
			color.g = SgtEase.Evaluate(sunsetEase, 1.0f - CwHelper.Sharpness(sunsetU, sunsetSharpnessG));
			color.b = SgtEase.Evaluate(sunsetEase, 1.0f - CwHelper.Sharpness(sunsetU, sunsetSharpnessB));
			color.a = (color.r + color.g + color.b) / 3.0f;

			generatedTexture.SetPixel(x, 0, CwHelper.ToLinear(CwHelper.Saturate(color)));
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit.Jovian
{
	using UnityEditor;
	using TARGET = SgtJovianScatteringTex;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class SgtJovianScatteringTex_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			var dirtyTexture = false;

			Draw("sunsetEase", ref dirtyTexture, "The transition style between the day and night.");
			BeginError(Any(tgts, t => t.SunsetStart >= t.SunsetEnd));
				Draw("sunsetStart", ref dirtyTexture, "The start point of the sunset (0 = dark side, 1 = light side).");
				Draw("sunsetEnd", ref dirtyTexture, "The end point of the sunset (0 = dark side, 1 = light side).");
			EndError();
			Draw("sunsetSharpnessR", ref dirtyTexture, "The sharpness of the sunset red channel transition.");
			Draw("sunsetSharpnessG", ref dirtyTexture, "The sharpness of the sunset green channel transition.");
			Draw("sunsetSharpnessB", ref dirtyTexture, "The sharpness of the sunset blue channel transition.");

			if (dirtyTexture == true) Each(tgts, t => t.DirtyTexture(), true, true);
		}
	}
}
#endif