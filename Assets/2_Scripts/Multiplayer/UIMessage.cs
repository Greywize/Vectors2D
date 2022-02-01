using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MultiTween;

public class UIMessage : MonoBehaviour
{
    [HideInInspector]
    public MultiTween tweenController;

    public bool Faded { get; set; }

    private void Start()
    {
        tweenController = GetComponent<MultiTween>();
    }
    public TweenSet FadeOut()
    {
        tweenController.BeginStage(0);

        return tweenController.sets[0];
    }
}