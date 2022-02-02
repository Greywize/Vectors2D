using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRotator : MonoBehaviour
{
    Rigidbody2D rigidBody;
    RectTransform rectTransform;

    [SerializeField]
    float currentTorque;
    [SerializeField]
    [Tooltip("The time it takes for one revolution (in seconds)")]
    float rps = 20;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();

        TweenRotation();
    }

    private void TweenRotation()
    {
        LeanTween.rotateAroundLocal(rectTransform, new Vector3(0, 0, 1), 360, rps).setRepeat(-1);
    }
}