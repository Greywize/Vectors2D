using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TweenController : MonoBehaviour
{
    // Nested classes
    [System.Serializable]
    public class TweenStage
    {
        // A list of tweens for this stage
        public List<Tween> tweens = new List<Tween>();
        // Event called when all tweens are completed
        public UnityEvent onComplete = new UnityEvent();
    }

    // UI Components
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Image image;

    // Variables
    public bool beginOnStart;
    public int startStage;
    [Tooltip("Loops all stages continuously")]
    public bool loop;
    [Tooltip("Plays all stages once")]
    public bool playAll;
    private int currentStage;

    public List<TweenStage> stages = new List<TweenStage>();

    private void Start()
    {
        if (beginOnStart)
            BeginStage(startStage);
    }

    public Tween StartTween(Tween tween)
    {
        // Set completed to be false
        tween.completed = false;

        // Error checking
        if (tween.time == 0)
        {
            Debug.Log($"A tween on {gameObject.name} has a time of zero, which won't do anything");
            return tween;
        }
        if (tween.ease == LeanTweenType.notUsed)
        {
            Debug.LogWarning($"A tween on {gameObject.name} has an ease type of NotUsed which won't do anything.");
            return tween;
        }

        // Call the correct tween funciton
        switch (tween.mode)
        {
            case Tween.TweenMode.CanvasAlpha:
                TweenAlpha(tween);
                break;
            case Tween.TweenMode.Scale:
                if (tween.uniformScale)
                    TweenScale(tween);
                else
                    TweenScale(tween);
                break;
            case Tween.TweenMode.Size:
                TweenSize(tween);
                break;
            case Tween.TweenMode.Color:
                TweenColor(tween);
                break;
            case Tween.TweenMode.Position:
                TweenPosition(tween);
                break;
            case Tween.TweenMode.Rotation:
                TweenRotation(tween);
                break;
        }

        return tween;
    }

    public TweenStage BeginStage(int stage)
    {
        if (stage > stages.Count + 1 || stages.Count == 0)
        {
            Debug.LogWarning($"Tween stage {stage} doesn't exist on {gameObject.name}");
            return stages[stage];
        }

        LeanTween.cancel(gameObject);

        currentStage = stage;

        // Run each tween in the current stage
        foreach(Tween tween in stages[stage].tweens)
        {
            // Set completed to be false
            tween.completed = false;
            
            // Error checking
            if (tween.time == 0)
            {
                Debug.Log($"A tween on {gameObject.name} has a time of zero, which won't do anything.");

                return stages[stage];
            }
            if (tween.ease == LeanTweenType.notUsed)
            {
                Debug.LogWarning($"A tween on {gameObject.name} has an ease type of NotUsed which won't do anything.");

                return stages[stage];
            }

            // Call the correct tween funciton
            switch (tween.mode)
            {
                case Tween.TweenMode.CanvasAlpha:
                    TweenAlpha(tween);
                    break;
                case Tween.TweenMode.Scale:
                    if (tween.uniformScale)
                        TweenScale(tween);
                    else
                        TweenScale(tween);
                    break;
                case Tween.TweenMode.Size:
                    TweenSize(tween);
                    break;
                case Tween.TweenMode.Color:
                    TweenColor(tween);
                    break;
                case Tween.TweenMode.Position:
                    TweenPosition(tween);
                    break;
                case Tween.TweenMode.Rotation:
                    TweenRotation(tween);
                    break;
            }
        }

        return stages[stage];
    }
    // For when we want to loop all stages
    private void TweenNextStage(int stage)
    {
        if (stage >= stages.Count)
            stage = 0;
        BeginStage(stage);
    }
    private void TweenAlpha(Tween tween)
    {
        // Null checks
        if (!canvasGroup)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogWarning($"A CanvasGroup component is required to tween alpha, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (canvas && !canvas.enabled)
                canvas.enabled = true;
        }
        LeanTween.alphaCanvas(canvasGroup, tween.alpha, tween.time)
                    .setDelay(tween.delay)
                    .setEase(tween.ease)
                    .setOnComplete(() => { OnAnyTweenComplete(tween); });
    }
    private void TweenScale(Tween tween)
    {
        // Null checks
        if (!rect)
        {
            rect = GetComponent<RectTransform>();
            if (rect == null)
            {
                Debug.LogWarning($"A RectTransform is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas.enabled)
            canvas.enabled = true;
        if (tween.uniformScale)
        {
            LeanTween.scale(gameObject, Vector3.one * tween.scale, tween.time)
                .setDelay(tween.delay)
                .setEase(tween.ease)
                .setOnComplete(() => { OnAnyTweenComplete(tween); });
        }
        else
        {
            LeanTween.scale(gameObject, tween.scaleVector, tween.time)
                .setDelay(tween.delay)
                .setEase(tween.ease)
                .setOnComplete(() => { OnAnyTweenComplete(tween); });
        }
    }
    private void TweenSize(Tween tween)
    {
        if (!rect)
        {
            rect = GetComponent<RectTransform>();
            if (rect == null)
            {
                Debug.LogWarning($"A RectTransform is required to tween size, but it was missing on {gameObject.name}.");
                return;
            }
        }
        LeanTween.size(rect, tween.sizeVector, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { OnAnyTweenComplete(tween); });
    }
    private void TweenColor(Tween tween)
    {
        if (!image)
        {
            image = GetComponent<Image>();
            if (image == null)
            {
                Debug.LogWarning($"An Image is required to tween color, but it was missing on {gameObject.name}.");
                return;
            }
        }
        LeanTween.value(image.gameObject, ColorChangeCallback, image.color, tween.color, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { OnAnyTweenComplete(tween); });
    }
    private void TweenPosition(Tween tween)
    {
        LeanTween.moveLocal(gameObject, tween.positionVector, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { OnAnyTweenComplete(tween); });
    }
    private void TweenRotation(Tween tween)
    {
        LeanTween.rotateAroundLocal(gameObject, new Vector3(0, 0, 1), tween.rotation, tween.time)
            .setDelay(tween.delay)
            .setEase(tween.ease)
            .setOnComplete(() => { OnAnyTweenComplete(tween); });
    }
    // Used by TweenColor() to change the color of the image at each step of the tween
    private void ColorChangeCallback(Color color)
    {
        image.color = color;
    }
    // Called by every tween when it completes
    private void OnAnyTweenComplete(Tween tween)
    {
        tween.completed = true;

        if (tween.hideOnComplete)
        {
            if (!canvas)
            {
                canvas = GetComponent<Canvas>();
                if (canvas == null)
                {
                    Debug.LogWarning($"A Canvas is required to tween alpha, but it was missing on {gameObject.name}.");
                    return;
                }
            }
            if (canvas.enabled)
                canvas.enabled = false;
        }    

        CheckStageComplete(currentStage);
    }
    private void CheckStageComplete(int stage)
    {
        foreach(Tween tween in stages[stage].tweens)
        {
            // If any tween isn't done, return
            if (!tween.completed)
                return;
        }

        // All tweens are done so call stage complete 
        OnStageComplete(stage);
    }
    private void OnStageComplete(int stage)
    {
        if (loop)
        {
            currentStage++;
            TweenNextStage(currentStage);
        }
        else if (playAll)
        {
            currentStage++;
            if (currentStage < stages.Count)
                BeginStage(currentStage);
        }

        stages[stage].onComplete?.Invoke();
    }
    public void StopAllTweens()
    {
        LeanTween.cancel(gameObject);
    }
}