using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class LTWrapper : MonoBehaviour
{
    #region Tween Functions
    // One function to rule them all, and its overlords- I mean overloads
    public static Tween Tween(CanvasGroup canvasGroup, Tween tween)
    {
        TweenAlpha(canvasGroup, tween);
        return tween;
    }
    public static Tween Tween(RectTransform rectTransform, Tween tween)
    {
        switch (tween.mode)
        {
            case global::Tween.TweenMode.Scale:
                TweenScale(rectTransform, tween);
                break;
            case global::Tween.TweenMode.Size:
                TweenSize(rectTransform, tween);
                break;
        }
        return tween;
    }
    public static Tween Tween(Image image, Tween tween)
    {
        TweenImageColor(image, tween);
        return tween;
    }
    public static Tween Tween(GameObject obj, Tween tween)
    {
        switch (tween.mode)
        {
            case global::Tween.TweenMode.Position:
                TweenPosition(obj, tween);
                break;
            case global::Tween.TweenMode.Rotation:
                TweenRotation(obj, tween);
                break;
        }
        return tween;
    }
    public static Tween Tween(TMP_Text textObject, Tween tween)
    {
        TweenTextColor(textObject, tween);
        return tween;
    }
    public static Tween Tween(Light2D light2D, Tween tween)
    {
        if (!light2D)
        {
            Debug.LogWarning($"A Light2D is required to tween light, but it was missing.");
            return tween;
        }

        TweenLight2DColor(light2D, tween);
        TweenLightIntensity(light2D, tween);
        return tween;
    }
    public static Tween Tween(Camera camera, Tween tween)
    {
        TweenCameraSize(camera, tween);
        return tween;
    }
    public static Tween Tween(ref float value, Tween tween)
    {
        TweenFloat(ref value, tween);
        return tween;
    }

    // LeanTween Wrapper Functions
    private static Tween TweenAlpha(CanvasGroup canvasGroup, Tween tween)
    {
        // Null check
        if (!canvasGroup)
        {
            Debug.LogWarning($"A CanvasGroup component is required to tween alpha, but it was missing.");
            return tween;
        }

        if (tween.time <= 0)
            canvasGroup.alpha = tween.alpha;

        LeanTween.alphaCanvas(canvasGroup, tween.alpha, tween.time)
                    .setDelay(tween.delay)
                    .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenScale(RectTransform rectTransfrom, Tween tween)
    {
        // Null check
        if (!rectTransfrom)
        {
            Debug.LogWarning($"A RectTransform is required to tween scale, but it was missing.");
            return tween;
        }

        if (tween.uniformScale)
        {
            if (tween.time <= 0)
            {
                rectTransfrom.localScale = Vector3.one * tween.scale;
                return tween;
            }

            LeanTween.scale(rectTransfrom, Vector3.one * tween.scale, tween.time)
                .setDelay(tween.delay)
                .setEase(tween.ease)
                .setOnComplete(() => { tween.onComplete?.Invoke(); });
        }
        else
        {
            if (tween.time <= 0)
            {
                rectTransfrom.localScale *= tween.scaleVector;
                return tween;
            }

            LeanTween.scale(rectTransfrom, tween.scaleVector, tween.time)
                .setDelay(tween.delay)
                .setEase(tween.ease)
                .setOnComplete(() => { tween.onComplete?.Invoke(); });
        }

        return tween;
    }
    private static Tween TweenSize(RectTransform rectTransfrom, Tween tween)
    {
        // Null check
        if (!rectTransfrom)
        {
            Debug.LogWarning($"A RectTransform is required to tween size, but it was missing.");
            return tween;
        }

        if (tween.time <= 0)
        {
            rectTransfrom.rect.Set(rectTransfrom.rect.x, rectTransfrom.rect.y, tween.sizeVector.x, tween.sizeVector.y);
            return tween;
        }

        LeanTween.size(rectTransfrom, tween.sizeVector, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenImageColor(Image image, Tween tween)
    {
        // Null check
        if (!image)
        {
            Debug.LogWarning($"An Image is required to tween color, but it was missing.");
            return tween;
        }

        if (!tween.image)
            tween.image = image;

        if (tween.time <= 0)
        {
            image.color = tween.color;
            return tween;
        }

        LeanTween.value(image.gameObject, tween.CallbackImageColor, image.color, tween.color, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenPosition(GameObject obj,Tween tween)
    {
        if (tween.time <= 0)
        {
            obj.transform.position = tween.positionVector;
            return tween;
        }

        LeanTween.moveLocal(obj, tween.positionVector, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenRotation(GameObject obj, Tween tween)
    {
        if (tween.time <= 0)
        {
            obj.transform.Rotate(Vector3.forward, tween.rotation);
            return tween;
        }

        LeanTween.rotateAroundLocal(obj, new Vector3(0, 0, 1), tween.rotation, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenTextColor(TMP_Text textObject, Tween tween)
    {
        // Null check
        if (!textObject)
        {
            Debug.LogWarning($"A text component is required to tween text color, but it was missing.");
            return tween;
        }

        if (!tween.textObject)
            tween.textObject = textObject;

        if (tween.time <= 0)
        {
            textObject.color = tween.textColor;
            return tween;
        }

        LeanTween.value(textObject.gameObject, tween.CallbackTextColor, textObject.color, tween.textColor, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenLight2DColor(Light2D light2D, Tween tween)
    {
        if (!tween.light2D)
            tween.light2D = light2D;

        if (tween.time <= 0)
        {
            light2D.color = tween.lightColor;
            return tween;
        }

        LeanTween.value(light2D.gameObject, tween.CallbackLight2DColor, light2D.color, tween.lightColor, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenLightIntensity(Light2D light2D, Tween tween)
    {
        if (!tween.light2D)
            tween.light2D = light2D;

        if (tween.time <= 0)
        {
            light2D.intensity = tween.lightIntensity;
            return tween;
        }

        LeanTween.value(light2D.gameObject, tween.CallbackLight2DIntensity, light2D.intensity, tween.lightIntensity, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });
        return tween;
    }
    private static Tween TweenCameraSize(Camera cam, Tween tween)
    {
        if (!tween.cam)
            tween.cam = cam;

        if (tween.time <= 0)
        {
            cam.orthographicSize = tween.cameraSize;
            return tween;
        }

        LeanTween.value(cam.gameObject, tween.CallbackCameraSize, cam.orthographicSize, tween.cameraSize, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    private static Tween TweenFloat(ref float value, Tween tween)
    {
        if (tween.time <= 0)
        {
            value = tween.value;
            return tween;
        }

        LeanTween.value(value, tween.value, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { tween.onComplete?.Invoke(); });

        return tween;
    }
    #endregion
}