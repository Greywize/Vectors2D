using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenController))]
[RequireComponent(typeof(RectTransform))]
public class UITween : MonoBehaviour
{
    // --- ToDo
    // Turns this class into a struct which TweenBrain can contain multiple of,
    // rather than having multiple UITweens on an object for each individual tween we want

    // UI Components
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    TweenController tweenBrain;

    // See https://easings.net/ for a visual explanation of each tween type
    public LeanTweenType tweenType = LeanTweenType.linear;

    public TweenMode tweenMode;
    public enum TweenMode
    {
        Alpha,
        Scale,
        Position,
        Rotation
    }

    #region Tween Values
    [Min(0)] [Tooltip("How long the tween will last (in seconds)")]
    public float tweenTime = 0.5f;
    [SerializeField] [Range(0, 1)]
    private float alphaOut;
    [SerializeField] [Range(0, 1)]
    private float alphaIn = 1;
    public bool uniformScale = true;
    [SerializeField]
    private float scaleOut;
    [SerializeField]
    private Vector2 scaleOutVector;
    [SerializeField]
    private float scaleIn = 1;
    [SerializeField]
    private Vector2 scaleInVector = Vector2.one;
    [SerializeField]
    private Vector2 positionOut;
    [SerializeField]
    private Vector2 positionIn;
    [SerializeField] [Tooltip("Rotation to tween out (in degrees)")]
    private float rotationOut;
    [SerializeField] [Tooltip("Rotation to tween in (in degrees)")]
    private float rotationIn;
    #endregion
    #region Internal Logic Variables
    public bool completed = false;
    private bool tweenOut = false;
    #endregion

    private void OnEnable()
    {
        tweenBrain = GetComponent<TweenController>();

        //tweenBrain.tweenOut += TweenOut;
        //tweenBrain.tweenIn += TweenIn;
    }
    private void OnDisable()
    {
        //tweenBrain.tweenOut -= TweenOut;
        //tweenBrain.tweenIn -= TweenIn;
    }
    /*
    private void TweenIn()
    {
        completed = false;
        tweenOut = false;

        if (tweenTime <= 0)
        {
            Debug.LogWarning($"A tween time of zero or less on {gameObject.name} won't do anything!");
            return;
        }
        if (tweenType == LeanTweenType.notUsed)
        {
            Debug.LogWarning($"A tween type of NotUsed on {gameObject.name} won't do anything!");
            return;
        }

        switch (tweenMode)
        {
            case TweenMode.Alpha:
                TweenAlpha(alphaIn);
                break;
            case TweenMode.Scale:
                if (uniformScale)
                    TweenScale(scaleIn);
                else
                    TweenScale(scaleInVector);
                break;
            case TweenMode.Position:
                TweenPosition(positionIn);
                break;
            case TweenMode.Rotation:
                TweenRotation(rotationIn);
                break;
        }
    }
    private void TweenOut()
    {
        completed = false;
        tweenOut = true;

        if (tweenTime <= 0)
        {
            Debug.LogWarning($"A tween time of zero or less on {gameObject.name} won't do anything!");
            return;
        }
        if (tweenType == LeanTweenType.notUsed)
        {
            Debug.LogWarning($"A tween type of NotUsed on {gameObject.name} won't do anything!");
            return;
        }

        switch (tweenMode)
        {
            case TweenMode.Alpha:
                TweenAlpha(alphaOut);
                break;
            case TweenMode.Scale:
                if (uniformScale)
                    TweenScale(scaleOut);
                else
                    TweenScale(scaleOutVector);
                break;
            case TweenMode.Position:
                TweenPosition(positionOut);
                break;
            case TweenMode.Rotation:
                TweenRotation(rotationOut);
                break;
        }
    }*/
}