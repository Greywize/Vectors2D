using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaButton : MonoBehaviour
{
    [Range(0,1)]
    public float AlphaThreshold = 0.1f;

    // Controlls how transparent an image must be to be a button on start
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }
}