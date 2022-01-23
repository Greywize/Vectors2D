using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MatchMade;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] bool host;
    CanvasInterface canvasInterface;

    [SerializeField] PlayerInput playerInput;
    InputActionMap controls;

    InputAction confirm;
    InputAction cancel;

    [Header("UI Elements")]
    [SerializeField] TweenController mainTweenController;
    [SerializeField] TweenController loadingTweenController;
    [SerializeField] TweenController loadingBarTweenController;
    [SerializeField] TMP_InputField nameField;
    [SerializeField] TMP_InputField addressField;

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
    private void Awake()
    {
        canvasInterface = GetComponent<CanvasInterface>();   
    }
    public void ValidateAndConnect()
    {
        if (validating)
            return;

        if (string.IsNullOrWhiteSpace(nameField.text))
        {
            return;
        }

        NetworkManager.Instance.localPlayerName = nameField.text;

        if (host)
        {
            NetworkManager.Instance.networkAddress = "localhost";
            NetworkManager.Instance.StartHost();
            validating = true;
            return;
        }

        StopAllCoroutines();

        InterfaceManager.Instance.SetTransitionMessage("Connecting");

        loadingTweenController.BeginStage(1);
        loadingBarTweenController.BeginStage(0);
        mainTweenController.BeginStage(0).onComplete.AddListener(() =>
        {
            if (addressField.text == "localhost")
            {
                NetworkDiscovery.Instance.StartDiscovery();
            }
            else
            {
                NetworkManager.Instance.networkAddress = addressField.text;
                NetworkManager.Instance.StartClient();
            }
            StartCoroutine(TimeOutTimer());
        });

        validating = true;
    }
    private IEnumerator TimeOutTimer()
    {
        NetworkManager.Instance.StopClient();
        yield return new WaitForSeconds(10);
        InterfaceManager.Instance.SetTransitionMessage("Failed to connect :<");
        loadingBarTweenController.BeginStage(1);
        yield return new WaitForSeconds(1);
        Cancel();
    }
    void Cancel()
    {
        if (!validating)
            return;

        StopCoroutine(TimeOutTimer());

        loadingBarTweenController.BeginStage(2);
        mainTweenController.BeginStage(1);
        loadingTweenController.BeginStage(0);
        NetworkDiscovery.Instance.StopDiscovery();

        validating = false;
    }
    void Connect()
    {
        StopCoroutine(TimeOutTimer());

        NetworkManager.Instance.StartClient();
    }
    void SetupControls()
    {
        // --- Set up action map
        controls = playerInput.actions.FindActionMap("UI");
        controls.Enable();
        confirm = controls.FindAction("Confirm");
        confirm.Enable();
        cancel = controls.FindAction("Cancel");
        cancel.Enable();
    }
}