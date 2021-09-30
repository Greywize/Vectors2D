using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- Instance
    public static GameManager Instance;

    // --- Enums
    public enum GameState
    {
        Menu,
        Running,
        Paused,
    }

    // --- Properties
    public static GameState State { get; }
    public static bool Paused => (State == GameState.Paused);

    // --- Member variables

    private void Awake()
    {
        // If an instance already exists
        if (Instance != null)
            // Destroy this object
            Destroy(this);
        // Otherwise, this is the instance
        Instance = this;
        // So don't destroy on scene change
        DontDestroyOnLoad(this);
    }
}