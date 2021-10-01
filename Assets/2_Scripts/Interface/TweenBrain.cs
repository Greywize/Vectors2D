using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenBrain : MonoBehaviour
{
    // The tween brain
    // Calls all tween functions on this gameObject
    // Required by all tweens

    // Call TweenIn & TweenOut from other scripts to call all tweens for this gameObject at once

    public delegate void TweenEvent();
    public TweenEvent tweenOut;
    public TweenEvent tweenIn;

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