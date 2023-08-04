using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class PostEffectsController : MonoBehaviour {
   public Shader postShader;
   private Camera _cam;

   public Color screenTint;
   public int blitCount = 4;

   public float upperFeather;
   public float bottomFeather;
   public float rippleIntensity;
   public float rippleSpeed;
   public GameObject position;

   private RenderTexture _finalPostRenderTexture;
   private Material _postEffectMaterial;
   private static readonly int ScreenTint = Shader.PropertyToID("_ScreenTint");
   private static readonly int UpperFeather = Shader.PropertyToID("_UpperFeather");
   private static readonly int BottomFeather = Shader.PropertyToID("_BottomFeather");
   private static readonly int RippleIntensity = Shader.PropertyToID("_RippleIntensity");
   private static readonly int GlobalRenderTexture = Shader.PropertyToID("_GlobalRenderTexture");
   private static readonly int Position = Shader.PropertyToID("_Position");
   private static readonly int RippleSpeed = Shader.PropertyToID("_RippleSpeed");
   private static readonly int MainTex = Shader.PropertyToID("_MainTex");

   private void Awake() {
      _cam = GetComponent<Camera>();
   }

   private void OnRenderImage(RenderTexture src, RenderTexture dest) {
      if (_postEffectMaterial == null) _postEffectMaterial = new Material(postShader);
      if (_finalPostRenderTexture == null)
         _finalPostRenderTexture = new RenderTexture(src.width, src.height, 0, src.format);
      _postEffectMaterial.SetTexture(MainTex, src);
      _postEffectMaterial.SetColor(ScreenTint, screenTint);
      _postEffectMaterial.SetFloat(UpperFeather, upperFeather);
      _postEffectMaterial.SetFloat(BottomFeather, bottomFeather);
      _postEffectMaterial.SetFloat(RippleIntensity, rippleIntensity);
      _postEffectMaterial.SetFloat(RippleSpeed, rippleSpeed);
      //_postEffectMaterial.SetVector(Position, _cam.WorldToViewportPoint(position.transform.position));
      //Debug.Log("Translating: " + position.transform.position + " to screenspace: " + _cam.WorldToViewportPoint(position.transform.position));

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
      Graphics.Blit(src, _finalPostRenderTexture, _postEffectMaterial, 3);
      Shader.SetGlobalTexture(GlobalRenderTexture, startRenderTexture);
      Graphics.Blit(startRenderTexture, dest);
      RenderTexture.ReleaseTemporary(startRenderTexture);
   }
}