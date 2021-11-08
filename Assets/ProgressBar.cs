using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
    Image image;
    RectTransform rectTransform;

    TweenController tweenController;

    public List<Tween> progressCompleteTweens = new List<Tween>();

    float originalWidth, originalHeight;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        tweenController = GetComponent<TweenController>();
    }
    private void Start()
    {
        SetOrigin();
    }
    private void SetOrigin()
    {
        originalWidth = rectTransform.rect.width;
        originalHeight = rectTransform.rect.height;

        originalColor = image.color;
    }
    public void Reset()
    {
        rectTransform.sizeDelta.Set(originalWidth, originalHeight);

        image.color = originalColor;
    }
    public void Complete()
    {
        if (tweenController)
        {
            foreach(Tween tween in progressCompleteTweens)
            {
                tweenController.StartTween(tween);
            }
        }
    }
}