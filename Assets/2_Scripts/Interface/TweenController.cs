using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class TweenController : MonoBehaviour
{
    // Enums
    public enum TweenMode
    {
        Alpha,
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
        // public float vector;
        // This worked nicely, but we lost the ability to give each variable an attribute, such as Range & Min

        // Variables used for tweens
        [Range(0,1)] [Tooltip("Tweens the alpha value of a sibling CanvasGroup conponent")]
        public float alpha;
        [Min(0)] [Tooltip("Tweens the scale of the object")]
        public float scale;
        [Tooltip("Tweens the width and height of the object")]
        public Vector2 sizeVector;
        [Tooltip("Tweens the rotation of the object (In degrees)")]
        public float rotation = 180;
        [Tooltip("Tweens the scale of the object non-uniformly")]
        public Vector2 scaleVector;
        [Tooltip("Tweens the position of the object")]
        public Vector2 positionVector;
        [Tooltip("Tweens the color of the object")]
        public Color color = Color.white;

        // Logic flags
        public bool uniformScale;
        public bool completed;
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
    
    public List<TweenStage> stages = new List<TweenStage>();

    // Variables
    public bool beginOnStart;
    public int startStage;
    public bool loop;
    private int currentStage;

    private void Start()
    {
        if (beginOnStart)
        {
            StartTween(startStage);
        }
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
                case TweenMode.Alpha:
                    TweenAlpha(tween.alpha, tween.time, tween.delay, tween.ease);
                    break;
                case TweenMode.Scale:
                    if (tween.uniformScale)
                        TweenScale(tween.scale, tween.time, tween.delay, tween.ease);
                    else
                        TweenScale(tween.scaleVector, tween.time, tween.delay, tween.ease);
                    break;
                case TweenMode.Size:
                    TweenSize(tween.sizeVector, tween.time, tween.delay, tween.ease);
                    break;
                case TweenMode.Position:
                    TweenPosition(tween.positionVector, tween.time, tween.delay, tween.ease);
                    break;
                case TweenMode.Rotation:
                    TweenRotation(tween.rotation, tween.time, tween.delay, tween.ease);
                    break;
                case TweenMode.Color:
                    TweenColor(tween.color, tween.time, tween.delay, tween.ease);
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
    private void TweenAlpha(float alpha, float time, float delay, LeanTweenType ease)
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
            if (canvasGroup == null)
            {
                Debug.LogWarning($"A Canvas is required to tween alpha, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas.enabled)
            canvas.enabled = true;
        LeanTween.alphaCanvas(canvasGroup, alpha, time)
            .setDelay(delay)
            .setEase(ease)
            .setOnComplete(OnAnyStageComplete);
    }
    private void TweenScale(float scale, float time, float delay, LeanTweenType ease)
    {
        // Null checks
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning($"A Canvas is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!rect)
        {
            rect = GetComponent<RectTransform>();
            if (canvas == null)
            {
                Debug.LogWarning($"A RectTransform is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!canvas.enabled)
            canvas.enabled = true;
        LeanTween.scale(gameObject, Vector3.one * scale, time)
            .setDelay(delay)
            .setEase(ease)
            .setOnComplete(OnAnyStageComplete);
    }
    private void TweenScale(Vector2 scale, float time, float delay, LeanTweenType ease)
    {
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning($"A Canvas is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        if (!rect)
        {
            rect = GetComponent<RectTransform>();
            if (canvas == null)
            {
                Debug.LogWarning($"A RectTransform is required to tween scale, but it was missing on {gameObject.name}.");
                return;
            }
        }
        LeanTween.scale(gameObject, scale, time)
            .setDelay(delay)
            .setEase(ease)
            .setOnComplete(OnAnyStageComplete);
    }
    private void TweenSize(Vector2 size, float time, float delay, LeanTweenType ease)
    {
        if (!rect)
        {
            rect = GetComponent<RectTransform>();
            if (canvas == null)
            {
                Debug.LogWarning($"A RectTransform is required to tween size, but it was missing on {gameObject.name}.");
                return;
            }
        }
        LeanTween.size(rect, size, time)
            .setDelay(delay)
            .setEase(ease)
            .setOnComplete(OnAnyStageComplete);
    }
    private void TweenPosition(Vector2 position, float time, float delay, LeanTweenType ease)
    {
        LeanTween.moveLocal(gameObject, position, time)
            .setDelay(delay)
            .setEase(ease)
            .setOnComplete(OnAnyStageComplete);
    }
    private void TweenRotation(float rotation, float time, float delay, LeanTweenType ease)
    {
        LeanTween.rotateAroundLocal(gameObject, new Vector3(0, 0, 1), rotation, time)
            .setDelay(delay)
            .setEase(ease)
            .setOnComplete(OnAnyStageComplete);
    }
    private void TweenColor(Color color, float time, float delay, LeanTweenType ease)
    {
        LeanTween.color(gameObject, color, time)
            .setDelay(delay)
            .setEase(ease)
            .setOnComplete(OnAnyStageComplete);
    }
    private void CheckStageComplete(int stage)
    {

    }
    // Called when any stage completes all tweens
    private void OnAnyStageComplete()
    {
        if (loop)
        {
            currentStage++;
            TweenNextStage(currentStage);
        }
    }
}