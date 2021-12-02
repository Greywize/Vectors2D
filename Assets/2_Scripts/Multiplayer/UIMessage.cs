using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TweenController;

public class UIMessage : MonoBehaviour
{
    [HideInInspector]
    public TweenController tweenController;

    public bool Faded { get; set; }

    private void Start()
    {
        tweenController = GetComponent<TweenController>();
    }
    public TweenStage FadeOut()
    {
        tweenController.BeginStage(0);

        return tweenController.stages[0];
    }
}