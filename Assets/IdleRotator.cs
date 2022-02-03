using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRotator : MonoBehaviour
{
    Rigidbody2D rigidBody;
    RectTransform rectTransform;

    [SerializeField]
    [Tooltip("The time it takes for one revolution (in seconds)")]
    float rps = 1;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        rigidBody.AddTorque(-rps);
    }
}