using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public bool dontDestroyOnLoad = true;

    [HideInInspector]
    public CanvasInterface current;
    public CanvasInterface main;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;

        Screen.SetResolution(1920, 1080, false);

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        current = FindFirstActive();
    }

    // Returns the first active canvas found amongst all CanvasInterfaces 
    public CanvasInterface FindFirstActive()
    {
        foreach (CanvasInterface canvasInterface in FindObjectsOfType<CanvasInterface>())
        {
            if (canvasInterface.gameObject.GetComponent<Canvas>().enabled)
                return canvasInterface;
        }
        return null;
    }

    public void NavigateToMainMenu()
    {
        current.Navigate(main);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}