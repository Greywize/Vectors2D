using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
[DefaultExecutionOrder(+1)]
public class TextUpdater : MonoBehaviour
{
    private TMPro.TMP_Text textObject;

    private void Start()
    {
        textObject = GetComponent<TMPro.TMP_Text>();
    }

    private void OnEnable()
    {
        InterfaceManager.Instance.onLoadingScreenEvent += UpdateText;
    }
    private void OnDisable()
    {
        InterfaceManager.Instance.onLoadingScreenEvent -= UpdateText;
    }

    private void UpdateText(string message)
    {
        textObject.text = message;
    }
}