using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TweenBrain : MonoBehaviour
{
    // The tween brain
    // Calls all tween functions on this gameObject

    // Call TweenIn & TweenOut from other scripts to call all tweens for this gameObject at once

    public delegate void TweenEvent();
    public TweenEvent tweenOut;
    public TweenEvent tweenIn;

    public bool tweenOnStart = false;
    public bool loop = false;

    public UnityEvent onTweenOutComplete;
    public UnityEvent onTweenInComplete;

    private void Start()
    {
        if (tweenOnStart)
            TweenOut();

        if (loop)
        {
            onTweenOutComplete.AddListener(TweenIn);
            onTweenInComplete.AddListener(TweenOut);
        }
    }

    public void TweenOut()
    {
        LeanTween.cancel(gameObject);

        tweenOut?.Invoke();
    }
    public void TweenIn()
    {
        LeanTween.cancel(gameObject);

        tweenIn?.Invoke();
    }
}