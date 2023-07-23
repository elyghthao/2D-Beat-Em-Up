using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostEffectsController : MonoBehaviour {
    [FormerlySerializedAs("shockwaveShader")] public Shader postShader;
    private Material _postEffectMaterial;

    public Color screenTint;
    public int blitCount = 4;

    public float upperFeather;
    public float bottomFeather;
    public float rippleIntensity;

    private RenderTexture _finalPostRenderTexture;

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (_postEffectMaterial == null) {
            _postEffectMaterial = new Material(postShader);
        }
        if (_finalPostRenderTexture == null) {
            _finalPostRenderTexture = new RenderTexture(src.width, src.height, 0, src.format);
        }
        _postEffectMaterial.SetColor("_ScreenTint", screenTint);
        _postEffectMaterial.SetFloat("_UpperFeather", upperFeather);
        _postEffectMaterial.SetFloat("_BottomFeather", bottomFeather);
        _postEffectMaterial.SetFloat("_RippleIntensity", rippleIntensity);

        int width = src.width;
        int height = src.height;

        // First Blit
        RenderTexture endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
        Graphics.Blit(src, endRenderTexture, _postEffectMaterial, 3);
        RenderTexture startRenderTexture = endRenderTexture;

        // Downsample
        for (int i = 0; i < blitCount; i++) {
            width /= 2;
            height /= 2;
            endRenderTexture = RenderTexture.GetTemporary(width, height, 0, src.format);
            Graphics.Blit(startRenderTexture, endRenderTexture, _postEffectMaterial, 3);
            RenderTexture.ReleaseTemporary(startRenderTexture);
            startRenderTexture = endRenderTexture;
        }
        
        // Upsample
        for (int i = 0; i < blitCount; i++) {
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
