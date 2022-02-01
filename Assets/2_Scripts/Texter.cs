using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Texter : MonoBehaviour
{
    public delegate void TextUpdateEvent(TMPro.TMP_Text text = null, TweenController tweenController = null, string message = "");
    // Invoke this and pass in a string to change text
    public static TextUpdateEvent updateText;

    public delegate void TweenCompleteEvent();
    // Invoked when finished tweening out
    public static TweenCompleteEvent onOutComplete;
    // Invoked when finished tweening back in
    public static TweenCompleteEvent onInComplete;

    private void OnEnable()
    {
        updateText += UpdateText;
    }
    private void OnDisable()
    {
        updateText -= UpdateText;
    }

    private void UpdateText(TMPro.TMP_Text textObject, TweenController tweenController, string message)
    {
        if (tweenController)
        {
            tweenController.BeginStage(1).onComplete.AddListener(() => {
                textObject.text = message;
                onOutComplete?.Invoke();

                tweenController.BeginStage(0).onComplete.AddListener(() => { 
                    onInComplete?.Invoke(); 
                });
            });
        }
        else
        {
            textObject.text = message;

            if (textObject.color.a == 0)
                textObject.color += new Color(0,0,0,1);

            onInComplete?.Invoke();
        }
    }
}