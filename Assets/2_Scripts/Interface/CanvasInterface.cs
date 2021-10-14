using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInterface : MonoBehaviour
{
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Show()
    {
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (!canvas)
                return;
        }
        canvas.enabled = true;
    }
    public void Hide()
    {
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (!canvas)
                return;
        }
        canvas.enabled = false;
    }
}