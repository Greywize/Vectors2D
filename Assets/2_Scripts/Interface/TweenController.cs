using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TweenController : MonoBehaviour
{
    // Enums
    public enum TweenMode
    {
        CanvasAlpha,
        Scale,
        Size,
        Color,
        Position,
        Rotation,
    }
    // Nested classes
    [System.Serializable]
    public class Tween
    {
        // Determines what we tween, such as the alpha or scale
        public TweenMode mode;
        // The tween curve
        public LeanTweenType ease = LeanTweenType.linear;
        // The length of the tween in seconds
        [Min(0)] [Tooltip("How long the tween will take to complete (In seconds)")]
        public float time = 0.5f;
        [Min(0)] [Tooltip("How long we delay the tween before starting (In seconds)")]
        public float delay;

        // Variables repurposed for each tween mode
        // public float value;
        // public Vector2 vector;
        // It's possible to achieve everything using only a single float and Vector2
        // This worked nicely, but we lost the ability to give each variable an attribute, such as Range & Min

        // Variables used for tweens
        [Range(0,1)] [Tooltip("Tweens the alpha value of a sibling CanvasGroup conponent")]
        public float alpha;
        [Min(0)] [Tooltip("Tweens the scale of the object")]
        public float scale;
        [Tooltip("Tweens the color of the object")]
        public Color color = Color.white;
        [Tooltip("Tweens the width and height of the object relative to the RectTransform's anchor points")]
        public Vector2 sizeVector;
        [Tooltip("Tweens the scale of the object non-uniformly")]
        public Vector2 scaleVector;
        [Tooltip("Tweens the rotation of the object (In degrees)")]
        public float rotation = 180;
        [Tooltip("Tweens the position of the object")]
        public Vector2 positionVector;

        // Logic flags
        public bool uniformScale;
        public bool completed;
        public bool hideOnComplete;
    }
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
    [Min(1)]
    public int startStage;
    public bool loop;
    private int currentStage;

    public List<TweenStage> stages = new List<TweenStage>();

    private void Start()
    {
        if (beginOnStart)
            StartTween(startStage - 1);
    }

    public void StartTween(int stage)
    {
        if (stage > stages.Count + 1 || stages.Count == 0)
        {
            Debug.LogWarning($"Tween stage {stage} doesn't exist on {gameObject.name}");
            return;
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
                Debug.LogWarning($"A tween on {gameObject.name} has a time of zero which won't do anything.");
                return;
            }
            if (tween.ease == LeanTweenType.notUsed)
            {
                Debug.LogWarning($"A tween on {gameObject.name} has an ease type of NotUsed which won't do anything.");
                return;
            }

            // Call the correct tween funciton
            switch (tween.mode)
            {
                case TweenMode.CanvasAlpha:
                    TweenAlpha(tween);
                    break;
                case TweenMode.Scale:
                    if (tween.uniformScale)
                        TweenScale(tween);
                    else
                        TweenScale(tween);
                    break;
                case TweenMode.Size:
                    TweenSize(tween);
                    break;
                case TweenMode.Color:
                    TweenColor(tween);
                    break;
                case TweenMode.Position:
                    TweenPosition(tween);
                    break;
                case TweenMode.Rotation:
                    TweenRotation(tween);
                    break;
            }
        }
    }
    // For when we want to loop all stages
    private void TweenNextStage(int stage)
    {
        if (stage >= stages.Count)
            stage = 0;
        StartTween(stage);
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
            if (canvas == null)
            {
                Debug.LogWarning($"A Canvas is required to tween alpha, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas.enabled)
            canvas.enabled = true;
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

        stages[stage].onComplete?.Invoke();
    }
}