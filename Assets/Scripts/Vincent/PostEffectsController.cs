using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostEffectsController : MonoBehaviour {
    [FormerlySerializedAs("shockwaveShader")] public Shader postShader;
    private Material postEffectMaterial;

    public Color screenTint;
    public int blitCount = 4;

    public float upperFeather;
    public float bottomFeather;
    public float rippleIntensity;

    private RenderTexture finalPostRenderTexture;

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (postEffectMaterial == null) {
            postEffectMaterial = new Material(postShader);
        }
        if (finalPostRenderTexture == null) {
            finalPostRenderTexture = new RenderTexture(src.width, src.height, 0, src.format);
        }
        postEffectMaterial.SetColor("_ScreenTint", screenTint);
        postEffectMaterial.SetFloat("_UpperFeather", upperFeather);
        postEffectMaterial.SetFloat("_BottomFeather", bottomFeather);
        postEffectMaterial.SetFloat("_RippleIntensity", rippleIntensity);

        int width = src.width;
        int height = src.height;

        // First Blit
        RenderTexture endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
        Graphics.Blit(src, endRenderTexture, postEffectMaterial, 3);
        RenderTexture startRenderTexture = endRenderTexture;

        // Downsample
        for (int i = 0; i < blitCount; i++) {
            width /= 2;
            height /= 2;
            endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
            Graphics.Blit(startRenderTexture, endRenderTexture, postEffectMaterial, 3);
            RenderTexture.ReleaseTemporary(startRenderTexture);
            startRenderTexture = endRenderTexture;
        }
        
        // Upsample
        for (int i = 0; i < blitCount; i++) {
            width *= 2;
            height *= 2;
            endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
            Graphics.Blit(startRenderTexture, endRenderTexture, postEffectMaterial, 3);
            RenderTexture.ReleaseTemporary(startRenderTexture);
            startRenderTexture = endRenderTexture;
        }
        
        // Output
        //Graphics.Blit(src, finalPostRenderTexture, postEffectMaterial, 1);
        Shader.SetGlobalTexture("_GlobalRenderTexture", startRenderTexture);
        Graphics.Blit(startRenderTexture, dest);
        RenderTexture.ReleaseTemporary(startRenderTexture);
    }
}
