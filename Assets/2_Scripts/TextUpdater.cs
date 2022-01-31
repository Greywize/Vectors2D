using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
[DefaultExecutionOrder(-1)]
public class TextUpdater : MonoBehaviour
{
    public static TMPro.TMP_Text textObject;
    private static TweenController tweenController;

    public delegate void TextUpdateEvent(string text);
    public static TextUpdateEvent updateText;

    [SerializeField]
    static AnimationCurve blinkCurve;

    public void Awake()
    {
        textObject = GetComponent<TMPro.TMP_Text>();
        tweenController = GetComponent<TweenController>();
    }

    private void OnEnable()
    {
        updateText += UpdateText;
    }
    private void OnDisable()
    {
        updateText -= UpdateText;
    }

    private void UpdateText(string message)
    {
        if (tweenController)
        {
            tweenController.BeginStage(1).onComplete.AddListener(() => {
                textObject.text = message;
                tweenController.BeginStage(0);
            });
        }
        else
        {
            textObject.text = message;
        }
    }
}