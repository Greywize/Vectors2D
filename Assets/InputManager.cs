using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputActionMap interfaceActions;
    public static InputActionMap playerActions;

    private void Awake()
    {
        interfaceActions = GetComponent<PlayerInput>().currentActionMap;
        if (interfaceActions == null)
            Debug.LogWarning($"Missiong action map on PlayerInput component on {gameObject.name}");
        interfaceActions.Enable();
    }
}