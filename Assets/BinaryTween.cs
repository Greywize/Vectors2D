using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

[DefaultExecutionOrder(+1)]
public class BinaryTween : MonoBehaviour
{
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    GameObject obj;
    TMP_Text textObject;
    Light2D light2D;
    Camera cam;
    Image image;

    [Header("Tween In")]
    [SerializeField] Tween tweenIn;
    [Header("Tween Out")]
    [SerializeField] Tween tweenOut;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        obj = GetComponent<GameObject>();
        textObject = GetComponent<TMP_Text>();
        light2D = GetComponent<Light2D>();
        cam = GetComponent<Camera>();
        image = GetComponent<Image>();
    }

    public void TweenIn()
    {
        switch (tweenIn.mode)
        {
            case Tween.TweenMode.CanvasAlpha:
                LTWrapper.Tween(canvasGroup, tweenIn);
                break;
            case Tween.TweenMode.Scale:
                LTWrapper.Tween(rectTransform, tweenIn);
                break;
            case Tween.TweenMode.Size:
                LTWrapper.Tween(rectTransform, tweenIn);
                break;
            case Tween.TweenMode.Color:
                LTWrapper.Tween(image, tweenIn);
                break;
            case Tween.TweenMode.TextColor:
                LTWrapper.Tween(textObject, tweenIn);
                break;
            case Tween.TweenMode.Position:
                LTWrapper.Tween(obj, tweenIn);
                break;
            case Tween.TweenMode.Rotation:
                LTWrapper.Tween(obj, tweenIn);
                break;
            case Tween.TweenMode.Light:
                LTWrapper.Tween(light2D, tweenIn);
                break;
            case Tween.TweenMode.CameraSize:
                LTWrapper.Tween(cam, tweenIn);
                break;
            case Tween.TweenMode.CustomValue:
                Debug.LogWarning($"Cannot tween a custom value with the BinaryTween component on {gameObject.name}");
                break;
        }
    }
    public void TweenOut()
    {
        switch (tweenOut.mode)
        {
            case Tween.TweenMode.CanvasAlpha:
                LTWrapper.Tween(canvasGroup, tweenOut);
                break;
            case Tween.TweenMode.Scale:
                LTWrapper.Tween(rectTransform, tweenOut);
                break;
            case Tween.TweenMode.Size:
                LTWrapper.Tween(rectTransform, tweenOut);
                break;
            case Tween.TweenMode.Color:
                LTWrapper.Tween(image, tweenOut);
                break;
            case Tween.TweenMode.TextColor:
                LTWrapper.Tween(textObject, tweenOut);
                break;
            case Tween.TweenMode.Position:
                LTWrapper.Tween(obj, tweenOut);
                break;
            case Tween.TweenMode.Rotation:
                LTWrapper.Tween(obj, tweenOut);
                break;
            case Tween.TweenMode.Light:
                LTWrapper.Tween(light2D, tweenOut);
                break;
            case Tween.TweenMode.CameraSize:
                LTWrapper.Tween(cam, tweenOut);
                break;
            case Tween.TweenMode.CustomValue:
                Debug.LogWarning($"Cannot tween a custom value with the BinaryTween component on {gameObject.name}");
                break;
        }
    }
}