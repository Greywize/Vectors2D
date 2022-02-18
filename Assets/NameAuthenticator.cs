using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;
using Mirror;

public class NameAuthenticator : MonoBehaviour
{
    TMP_InputField nameField;
    InputAction submit;

    [SerializeField]
    MultiTween inputFieldTween;

    [SerializeField]
    BinaryTween menuTween;
    [SerializeField]
    Canvas menuCanvas;

    private void OnEnable()
    {
        InputManager.Reset();
        submit = InputManager.interfaceActions.FindAction("Submit");

        submit.started += ctx => Authenticate();
    }
    private void OnDisable()
    {
        submit.started -= ctx => Authenticate();
    }
    private void Start()
    {
        nameField = GetComponent<TMP_InputField>();

        submit.Enable();
    }
    public void OnValueChanged()
    {
        if (inputFieldTween)
            inputFieldTween.BeginStage(0).onComplete.AddListener(() => { inputFieldTween.BeginStage(1); });
    }
    public void SelectField()
    {
        if (inputFieldTween)
            inputFieldTween.BeginStage(2);
    }
    public void DeselectField()
    {
        if (inputFieldTween)
            inputFieldTween.BeginStage(3);
    }
    public void Authenticate()
    {
        if (!nameField)
        {
            Debug.LogWarning($"Cannot authenticate player name. Missing input field on {gameObject.name}.");
            return;
        }

        string name = nameField.text;

        // If the name is invalid we should indicate this to the player
        if (string.IsNullOrWhiteSpace(name))
        {
            if (inputFieldTween)
                inputFieldTween.BeginStage(4);
        }
        // If the name is valid, begin spawn sequence
        else
        {
            if (inputFieldTween)
            {
                inputFieldTween.BeginStage(5);
            }
            StartCoroutine(SpawnSequence());
        }
    }

    IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(0.5f);
        // Set local player name
        NetworkManager.localPlayerName = nameField.text;
        // If connected
        if (NetworkClient.active)
        {
            // If not ready, ready up
            if (!NetworkClient.ready) NetworkClient.Ready();

            // Fade menu UI
            menuTween.TweenOut().onComplete = () => {
                menuCanvas.enabled = false;

                // Send a message to the server indicating we want to be spawned
                NetworkClient.AddPlayer();
            };
        }
    }
}