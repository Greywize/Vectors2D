using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CanvasInterface : MonoBehaviour
{
    Canvas canvas;
    CanvasGroup canvasGroup;
    TweenController tweenController;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        tweenController = GetComponent<TweenController>();
    }

    public void Navigate(CanvasInterface target)
    {
        // Hide this canvas
        Hide();
        if (tweenController)
        {
            // Once the tween stage is finished, call Show() on our target
            tweenController.stages[0].onComplete.AddListener(() => { target.Show(); });
        }
        else
        {
            // Without a tweenController, just call Show() on our target
            target.Show();
        }
    }
    public void Show()
    {
        // First enable our canvas
        if (!canvas.enabled)
            canvas.enabled = true;
        // Begin stage one on the controller, which is typically 'tween in'
        if (tweenController)
            tweenController.StartTween(1);
    }
    public void Hide()
    {
        if (tweenController)
        {
            // Begin stage two on the controller, which is typically 'tween out'
            tweenController.StartTween(0);
            // Temporarily add the HideComplete to the onComplete event of this stage
            tweenController.stages[0].onComplete.AddListener(() => { canvas.enabled = false; });
        }
        else
        {
            canvas.enabled = false;
        }
    }
}