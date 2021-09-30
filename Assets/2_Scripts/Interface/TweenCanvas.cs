using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class TweenCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Canvas canvas;

    public delegate void TweenEvent();
    public TweenEvent onFadeOutComplete;
    public TweenEvent onFadeInComplete;

    public LeanTweenType mode;
    public float tweenTime = 1f;

    public bool fadeInOnStart = false;

    private void Start()
    {
        // We shouldn't need to null check these as this component requires them
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();

        if (fadeInOnStart)
        {
            // First ensure no tweens are already running
            LeanTween.cancel(gameObject);

            // Ensure alpha is at our starting point
            canvasGroup.alpha = 0f;
            if (!canvas.enabled)
                canvas.enabled = true;

            LeanTween.alphaCanvas(canvasGroup, 1f, tweenTime);
        }
    }
    public void TweenIn()
    {
        // First ensure no tweens are already running
        LeanTween.cancel(gameObject);

        // Ensure alpha is at our starting point
        canvasGroup.alpha = 0f;
        if (!canvas.enabled)
            canvas.enabled = true;

        // Tween canvas alpha and call complete function when we're done
        LeanTween.alphaCanvas(canvasGroup, 1f, tweenTime).setOnComplete(FadeInComplete);
    }
    public void TweenOut()
    {
        // First ensure no tweens are already running
        LeanTween.cancel(gameObject);

        // Ensure alpha is at our starting point
        canvasGroup.alpha = 1f;

        // Tween canvas alpha and call complete function when we're done
        LeanTween.alphaCanvas(canvasGroup, 0f, tweenTime).setOnComplete(FadeOutComplete);
    }
    public void FadeOutComplete()
    {
        canvas.enabled = false;
        onFadeOutComplete?.Invoke();
    }
    public void FadeInComplete()
    {
        onFadeInComplete?.Invoke();
    }
}