using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MatchMade;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TMP_InputField nameField;

    InputActionMap controls;

    InputAction confirm;
    InputAction cancel;

    bool validating = false;

    private void OnEnable()
    {
        SetupControls();

        confirm.performed += ctx => ValidateAndConnect();
        cancel.performed += ctx => Cancel();
    }
    private void OnDisable()
    {
        confirm.performed -= ctx => ValidateAndConnect();
        cancel.performed -= ctx => Cancel();
    }

    public void ValidateAndConnect()
    {
        if (validating)
            return;
        validating = true;

        if (string.IsNullOrWhiteSpace(nameField.text))
        {
            return;
        }

        InterfaceManager.Instance.FadeHalf().onComplete.AddListener(() =>
        {
            InterfaceManager.Instance.SetTransitionMessage("Connecting");

            NetworkDiscovery.Instance.StartDiscovery();
        });
    }
    void Connect()
    {
        NetworkManager.Instance.StartClient();
    }
    void Cancel()
    {
        if (!validating)
            return;
        validating = false;

        NetworkDiscovery.Instance.StopDiscovery();
        InterfaceManager.Instance.SetTransitionMessage("");

        InterfaceManager.Instance.FadeIn();
    }
    void SetupControls()
    {
        // --- Set up action map
        controls = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        controls.Enable();
        confirm = controls.FindAction("Confirm");
        confirm.Enable();
        cancel = controls.FindAction("Cancel");
        cancel.Enable();
    }
}