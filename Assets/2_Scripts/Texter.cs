using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Texter : MonoBehaviour
{
    public delegate void TextUpdateEvent(TMPro.TMP_Text text, string message = "");
    // Invoke this and pass in a string to change text
    public static TextUpdateEvent updateText;

    public delegate void TweenCompleteEvent();
    // Invoked when finished tweening out
    public static TweenCompleteEvent onOutComplete;
    // Invoked when finished tweening back in
    public static TweenCompleteEvent onInComplete;

    [Header("Tween Out")]
    public Tween TextWhiteOut;
    [Header("Tween In")]
    public Tween TextWhiteIn;

    private void OnEnable()
    {
        updateText += UpdateText;
    }
    private void OnDisable()
    {
        updateText -= UpdateText;
    }

    private void UpdateText(TMPro.TMP_Text textObject, string message)
    {
        LTWrapper.Tween(textObject, TextWhiteOut).onComplete += () => {
            // Tween out finished, invoke delegate
            onOutComplete?.Invoke();
            // Change text
            textObject.text = message;
            // Tween back in
            LTWrapper.Tween(textObject, TextWhiteIn).onComplete += () => {
                // Tween in finished, invoke delegate
                onInComplete?.Invoke();
            };
        };
    }
}