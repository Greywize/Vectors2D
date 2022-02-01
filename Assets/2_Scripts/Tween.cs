using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

[System.Serializable]
public class Tween
{
    public TMPro.TMP_Text textObject;
    public RectTransform rectTransform;
    public GameObject gameObject;
    public Light2D light2D;
    public Image image;

    public enum TweenMode
    {
        CanvasAlpha,
        Scale,
        Size,
        Color,
        TextColor,
        Position,
        Rotation,
        Light,
        Value,
    }
    // Determines what we tween, such as the alpha or scale of the object
    public TweenMode mode;
    // The tween curve
    public LeanTweenType ease = LeanTweenType.linear;
    [Min(0)]
    [Tooltip("How long the tween will take to complete (In seconds)")]
    public float time = 0.25f;
    [Min(0)]
    [Tooltip("How long we delay the tween before starting (In seconds)")]
    public float delay;

    public delegate void TweenEvent();
    public TweenEvent onComplete;

    // Variables repurposed for each tween mode
    // public float value;
    // public Vector2 vector;
    // It's possible to achieve everything using only a single float and Vector2
    // This worked nicely, but we lost the ability to give each variable an attribute
    // Such as Range & Min, which makes things more readable

    // Variables used for tweens
    [Tooltip("A custom value for other scripts and objects to make use of.")]
    public float value;
    [Range(0, 1)]
    [Tooltip("Tweens the alpha value of a sibling CanvasGroup conponent")]
    public float alpha;
    [Min(0)]
    [Tooltip("Tweens the scale of the object")]
    public float scale;
    [Min(0)]
    [Tooltip("Tweens the intesity of a light")]
    public float lightIntensity;
    [Tooltip("Tweens the color of a light")]
    public Color color = Color.white;
    [Tooltip("Tweens the color of the object")]
    public Color lightColor = Color.white;
    [Tooltip("Tweens the color of a text component")]
    public Color textColor = Color.white;
    [Tooltip("Tweens the width and height of the object relative to the RectTransform's anchor points")]
    public Vector2 sizeVector;
    [Tooltip("Tweens the scale of the object non-uniformly")]
    public Vector2 scaleVector;
    [Tooltip("Tweens the rotation of the object (In degrees)")]
    public float rotation;
    [Tooltip("Tweens the position of the object")]
    public Vector2 positionVector;

    // Logic flags
    public bool uniformScale;
    public bool completed;

    public void CallbackImageColor(Color color)
    {
        if (image)
            image.color = color;
    }
    public void CallbackTextColor(Color color)
    {
        if (textObject)
            textObject.color = color;
    }
    public void CallbackLight2DColor(Color color)
    {
        if (light2D)
            light2D.color = color;
    }
    public void CallbackLight2DIntensity(float intensity)
    {
        if (light2D)
            light2D.intensity = intensity;
    }
}