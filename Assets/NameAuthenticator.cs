using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

public class NameAuthenticator : MonoBehaviour
{
    TMP_InputField nameField;
    InputAction submit;
    [SerializeField]
    MultiTween multiTween;
    Light2D light2D;

    private void Start()
    {
        nameField = GetComponent<TMP_InputField>();
        light2D = GetComponent<Light2D>();

        submit = InputManager.interfaceActions.FindAction("Submit");
        submit.performed += ctx => Authenticate();

        submit.Enable();
    }
    public void OnValueChanged()
    {
        if (multiTween)
            multiTween.BeginStage(0).onComplete.AddListener(() => { multiTween.BeginStage(1); });
    }
    public void SelectField()
    {
        if (multiTween)
            multiTween.BeginStage(2);
    }
    public void DeselectField()
    {
        if (multiTween)
            multiTween.BeginStage(3);
    }
    public void Authenticate()
    {
        if (!nameField)
        {
            Debug.LogWarning($"Cannot authenticate player name. Missing input field on {gameObject.name}.");
            return;
        }

        string name = nameField.text;

        if (string.IsNullOrWhiteSpace(name))
        {
            if (multiTween)
                multiTween.BeginStage(4);
        }
        else
        {
            if (multiTween)
                multiTween.BeginStage(5);
        }
    }
}