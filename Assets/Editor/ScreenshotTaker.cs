#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

public class ScreenshotTaker
{
    [MenuItem("Screenshot/Take")]
    public static void Grab()
    {
        ScreenCapture.CaptureScreenshot($"Assets/Editor/Screenshots/{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.png", 1);
    }
}
#endif