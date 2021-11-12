using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public bool dontDestroyOnLoad = false;

    CanvasInterface current;
    [SerializeField] CanvasInterface main;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;

        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

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
    public void Navigate(CanvasInterface screen)
    {
        current.Navigate(screen);
        current = screen;
    }
    public void SetCurrent(CanvasInterface newCurrent)
    {
        current = newCurrent;
    }
    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Play()
    {
        // Attempt to join master server
    }
    public void HostLocal()
    {
        // UIMultiplayer.Instance.EnableLocalButtons(false);

        NetworkManager.Instance.networkAddress = "localhost";
        NetworkManager.Instance.StartHost();
    }
    public void JoinLocal()
    {
        // UIMultiplayer.Instance.EnableLocalButtons(false);

        NetworkManager.Instance.networkAddress = "localhost";
        NetworkManager.Instance.StartClient();
    }
}