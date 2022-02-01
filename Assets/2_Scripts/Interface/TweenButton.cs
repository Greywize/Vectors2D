using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenButton : MonoBehaviour
{
    public LeanTweenType mode;
    public float tweenTime = 0.05f;
    public float scale = 0.75f;

    public void Tween()
    {
        // First ensure no tweens are already running
        LeanTween.Cancel(gameObject);

        transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, new Vector2(scale, scale), tweenTime / 2)
            .setEase(mode)
            .setOnComplete(TweenReturn);
    }
    public void TweenReturn()
    {
        LeanTween.scale(gameObject, Vector2.one, tweenTime / 2).setEase(mode);
    }
}