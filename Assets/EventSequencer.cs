using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequencer : MonoBehaviour
{
    // Events to be called at the start of runtime
    [Header("On Start")]
    public UnityEvent onStart;

    private void Start()
    {
        onStart.Invoke();
    }
}