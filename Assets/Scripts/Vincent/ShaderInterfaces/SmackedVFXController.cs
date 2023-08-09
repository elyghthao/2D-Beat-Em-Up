using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class SmackedVFXController : MonoBehaviour {
    [Header("References")]
    public GameObject backSmack;
    public GameObject frontSmack;
    private Material _backSmackMat;
    private Material _frontSmackMat;
    
    [Header("Variables")]
    public float fadeSpeed = 1.0f;

    [Range(0, 1)]
    public float radius = 0;
    public Color frontColor;
    public Color backColor;

    public bool debugMode;

    private bool tweening;
    
    // Cached Strings
    private static readonly int Color = Shader.PropertyToID("_Color");
    private static readonly int Emission = Shader.PropertyToID("_Emission");
    private static readonly int FadeSpeed = Shader.PropertyToID("_FadeSpeed");
    private static readonly int Radius = Shader.PropertyToID("_Radius");

    private void Awake() {
        DOTween.Init();
        transform.Rotate(new Vector3(0, 0, Random.rotation.eulerAngles.z));
        _frontSmackMat = frontSmack.GetComponent<Renderer>().material;
        _backSmackMat = backSmack.GetComponent<Renderer>().material;
    }

    private void Update() {
        if (radius >= 1 && !debugMode) {
            Destroy(gameObject);
        }
        _frontSmackMat.SetColor(Color, frontColor);
        _frontSmackMat.SetColor(Emission, frontColor);
        _frontSmackMat.SetFloat(FadeSpeed, fadeSpeed);
        _frontSmackMat.SetFloat(Radius, radius);
        _backSmackMat.SetColor(Color, backColor);
        _backSmackMat.SetColor(Emission, backColor);
        _backSmackMat.SetFloat(Radius, radius);
        Tween();
    }

    private void Tween() {
        if (!tweening) {
            tweening = true;
            DOTween.To(() => radius, x => radius = x, 1, fadeSpeed * 2.5f).SetEase(Ease.InQuad);
            transform.DOScaleX(3.5f, fadeSpeed).SetEase(Ease.InFlash);
            transform.DOScaleY(3.5f, fadeSpeed).SetEase(Ease.InFlash);
        }
    }
}
