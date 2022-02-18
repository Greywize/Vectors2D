using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;
    public static InputActionMap interfaceActions;
    public static InputActionMap playerActions;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.SwitchCurrentActionMap("UI");
        interfaceActions = playerInput.currentActionMap;
        interfaceActions.Enable();

        playerInput.SwitchCurrentActionMap("Player");
        playerActions = playerInput.currentActionMap;
    }

    public static void Reset()
    {
        playerInput.enabled = false;
        playerInput.enabled = true;
    }
}