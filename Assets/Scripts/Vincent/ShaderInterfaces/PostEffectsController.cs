using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class PostEffectsController : MonoBehaviour {
   public Shader postShader;

   public Color screenTint;
   public int blitCount = 4;

   public float upperFeather;
   public float bottomFeather;
   public float rippleIntensity;

   private RenderTexture _finalPostRenderTexture;
   private Material _postEffectMaterial;
   private static readonly int ScreenTint = Shader.PropertyToID("_ScreenTint");
   private static readonly int UpperFeather = Shader.PropertyToID("_UpperFeather");
   private static readonly int BottomFeather = Shader.PropertyToID("_BottomFeather");
   private static readonly int RippleIntensity = Shader.PropertyToID("_RippleIntensity");

   private void OnRenderImage(RenderTexture src, RenderTexture dest) {
      if (_postEffectMaterial == null) _postEffectMaterial = new Material(postShader);
      if (_finalPostRenderTexture == null)
         _finalPostRenderTexture = new RenderTexture(src.width, src.height, 0, src.format);
      _postEffectMaterial.SetColor(ScreenTint, screenTint);
      _postEffectMaterial.SetFloat(UpperFeather, upperFeather);
      _postEffectMaterial.SetFloat(BottomFeather, bottomFeather);
      _postEffectMaterial.SetFloat(RippleIntensity, rippleIntensity);

      var width = src.width;
      var height = src.height;

      // First Blit
      var endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
      Graphics.Blit(src, endRenderTexture, _postEffectMaterial, 3);
      var startRenderTexture = endRenderTexture;

      // Downsample
      for (var i = 0; i < blitCount; i++) {
         width /= 2;
         height /= 2;
         endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
         Graphics.Blit(startRenderTexture, endRenderTexture, _postEffectMaterial, 3);
         RenderTexture.ReleaseTemporary(startRenderTexture);
         startRenderTexture = endRenderTexture;
      }

      // Upsample
      for (var i = 0; i < blitCount; i++) {
         width *= 2;
         height *= 2;
         endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
         Graphics.Blit(startRenderTexture, endRenderTexture, _postEffectMaterial, 3);
         RenderTexture.ReleaseTemporary(startRenderTexture);
         startRenderTexture = endRenderTexture;
      }

      // Output
      //Graphics.Blit(src, _finalPostRenderTexture, _postEffectMaterial, 1);
      Shader.SetGlobalTexture("_GlobalRenderTexture", startRenderTexture);
      Graphics.Blit(startRenderTexture, dest);
      RenderTexture.ReleaseTemporary(startRenderTexture);
   }
}