using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

[DisallowMultipleComponent]
public class MultiTween : MonoBehaviour
{
    // Nested classes
    [System.Serializable]
    public class TweenSet
    {
        // A list of tweens for this stage
        public List<Tween> tweens = new List<Tween>();
        // Event called when all tweens are completed
        public UnityEvent onComplete = new UnityEvent();
    }

    // UI Components
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private TMP_Text textObject;
    private Light2D light2D;
    private Camera cam;
    private Image image;

    // Variables
    [Tooltip("Begins tweening on play")]
    public bool beginSetOnStart;
    public int startingSet;
    [Tooltip("Continue looping all tween sets")]
    public bool loop;
    [Tooltip("Plays all sets once")]
    public bool playAll;

    private int currentStage;

    public List<TweenSet> sets = new List<TweenSet>();

    private void Start()
    {
        if (beginSetOnStart)
            BeginStage(startingSet);
    }
    // Does the same thing as BeginStage but returns nothing, makeing it so UI can call it
    public void QueueStage(int stage)
    {
        BeginStage(stage);
    }
    public TweenSet BeginStage(int stage)
    {
        if (stage >= sets.Count || sets.Count == 0)
        {
            Debug.LogWarning($"Tween stage {stage} doesn't exist on {gameObject.name}");
            return sets[stage];
        }

        LeanTween.Cancel(gameObject);

        currentStage = stage;

        // Run each tween in the current stage
        foreach(Tween tween in sets[stage].tweens)
        {
            // Set completed to be false
            tween.completed = false;
            
            if (tween.ease == LeanTweenType.notUsed)
            {
                Debug.LogWarning($"A tween on {gameObject.name} has an ease type of NotUsed.");

                return sets[stage];
            }

            // Call the correct tween funciton
            switch (tween.mode)
            {
                case Tween.TweenMode.CanvasAlpha:
                    if (!canvasGroup)
                        canvasGroup = GetComponent<CanvasGroup>();
                    LTWrapper.Tween(canvasGroup, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.Scale:
                    if (!rectTransform)
                        rectTransform = GetComponent<RectTransform>();
                    LTWrapper.Tween(rectTransform, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.Size:
                    if (!rectTransform)
                        rectTransform = GetComponent<RectTransform>();
                    LTWrapper.Tween(rectTransform, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.Color:
                    if (!image)
                        image = GetComponent<Image>();
                    LTWrapper.Tween(image, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.TextColor:
                    if (!textObject)
                        textObject = GetComponent<TMP_Text>();
                    LTWrapper.Tween(textObject, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.Light:
                    if (!light2D)
                        light2D = GetComponent<Light2D>();
                    LTWrapper.Tween(light2D, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.CameraSize:
                    if (!cam)
                        cam = GetComponent<Camera>();
                    LTWrapper.Tween(cam, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.Position:
                    LTWrapper.Tween(gameObject, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
                case Tween.TweenMode.Rotation:
                    LTWrapper.Tween(gameObject, tween).onComplete += () => { OnAnyTweenComplete(tween); };
                    break;
            }
        }

        return sets[stage];
    }
    // Called by every tween when it completes
    private void OnAnyTweenComplete(Tween tween)
    {
        tween.completed = true;

        CheckStageComplete(currentStage);
    }
    private void CheckStageComplete(int stage)
    {
        if (stage < sets.Count)
        {
            foreach (Tween tween in sets[stage].tweens)
            {
                // If any tween isn't done, return
                if (!tween.completed)
                    return;
            }
            // All tweens are done so call stage complete 
            OnStageComplete(stage);
        }
    }
    private void OnStageComplete(int set)
    {
        if (loop)
        {
            currentStage++;
            if (set >= sets.Count)
                set = 0;
            BeginStage(currentStage);
        }
        else if (playAll)
        {
            currentStage++;
            if (currentStage < sets.Count)
                BeginStage(currentStage);
        }

        sets[set].onComplete?.Invoke();
    }
    public void StopAllTweens()
    {
        LeanTween.Cancel(gameObject);
    }
}