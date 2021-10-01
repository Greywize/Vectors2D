using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenBrain))]
[RequireComponent(typeof(RectTransform))]
public class UITween : MonoBehaviour
{
    // UI Components
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    TweenBrain tweenBrain;

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
        tweenBrain = GetComponent<TweenBrain>();

        tweenBrain.tweenOut += TweenOut;
        tweenBrain.tweenIn += TweenIn;
    }
    private void OnDisable()
    {
        tweenBrain.tweenOut -= TweenOut;
        tweenBrain.tweenIn -= TweenIn;
    }

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
    }

    private void TweenAlpha(float alpha)
    {
        // Null checks
        if (!canvasGroup)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogWarning($"A CanvasGroup component is required to tween alpha, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (canvasGroup == null)
            {
                Debug.LogWarning($"A Canvas is required to tween alpha, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas.enabled)
            canvas.enabled = true;
        LeanTween.alphaCanvas(canvasGroup, alpha, tweenTime).setEase(tweenType)
            .setOnComplete(() => { if (canvasGroup.alpha == 0) { canvas.enabled = false; } CheckComplete(tweenOut); });
    }
    private void TweenScale(float scale)
    {
        // Null checks
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning($"A Canvas is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!rect)
        {
            rect = GetComponent<RectTransform>();
            if (canvas == null)
            {
                Debug.LogWarning($"A RectTransform is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas.enabled)
            canvas.enabled = true;
        LeanTween.scale(gameObject, Vector3.one * scale, tweenTime).setEase(tweenType)
            .setOnComplete(() => { if (rect.localScale.magnitude == 0) { canvas.enabled = false; } CheckComplete(tweenOut); });
    }
    private void TweenScale(Vector2 scale)
    {
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning($"A Canvas is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!rect)
        {
            rect = GetComponent<RectTransform>();
            if (canvas == null)
            {
                Debug.LogWarning($"A RectTransform is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        LeanTween.scale(gameObject, scale, tweenTime).setEase(tweenType)
            .setOnComplete(() => { if (rect.localScale.magnitude == 0) { canvas.enabled = false; } CheckComplete(tweenOut); });
    }
    private void TweenPosition(Vector2 position)
    {
        LeanTween.moveLocal(gameObject, position, tweenTime).setEase(tweenType)
            .setOnComplete(() => CheckComplete(tweenOut));
    }
    private void TweenRotation(float rotation)
    {
        LeanTween.rotateAroundLocal(gameObject, new Vector3(0, 0, 1), rotation, tweenTime).setEase(tweenType)
            .setOnComplete(() => CheckComplete(tweenOut));
    }

    private void CheckComplete(bool tweeningOut)
    {
        // We only call this funtion if this tween is completed
        completed = true;

        foreach (UITween tween in GetComponents<UITween>())
        {
            if (!tween.completed)
                return;
        }

        if (!tweenBrain)
            tweenBrain = GetComponent<TweenBrain>();

        if (tweeningOut)
            tweenBrain.onTweenOutComplete?.Invoke();
        else
            tweenBrain.onTweenInComplete?.Invoke();
    }
}