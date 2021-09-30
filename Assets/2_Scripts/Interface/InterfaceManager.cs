using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;
    public NetworkManager networkManager;

    public static MenuMode mode;
    public MenuMode startingScreen;
    public enum MenuMode
    {
        Multiplayer,
        Loading,
        Game,
    }

    // --- All UI Canvases
    public Canvas multiplayerCanvas;
    public Canvas loadingCanvas;
    public Canvas gameCanvas;

    // A reference the current active canvas
    private Canvas currentCanvas;

    // A list later filled with all canvases
    public List<Canvas> canvases = new List<Canvas>();

    public delegate void TextEvent(string message);
    public TextEvent onLoadingScreenEvent;

    private void Awake()
    {
        // If there is already an instance
        if (Instance != null)
            Destroy(gameObject);
        // Otherwise, this is our instance
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        FillCanvasList();

        Instance.currentCanvas = canvases[(int)startingScreen];

        Screen.SetResolution(1280, 720, false);
    }

    // Fill the list with each canvas matching their enum index
    private void FillCanvasList()
    {
        if (multiplayerCanvas)
            canvases.Insert((int)MenuMode.Multiplayer, multiplayerCanvas);
        if (loadingCanvas)
            canvases.Insert((int)MenuMode.Loading, loadingCanvas);
        if (gameCanvas)
            canvases.Insert((int)MenuMode.Game, gameCanvas);
        // Add a line for each new canvas
    }

    // Switches the UI to the given menu
    public static void SwitchMenuState(MenuMode newMode)
    {
        if (!Instance.currentCanvas)
            Instance.currentCanvas = Instance.canvases[(int)mode];

        // Disable current canvas
        if (Instance.currentCanvas.enabled)
            Instance.currentCanvas.enabled = false;

        // Set new UI mode
        mode = newMode;

        if (Instance.canvases[(int)newMode] != null)
        {
            // Get the reference to the UI screen we're switching to
            Instance.currentCanvas = Instance.canvases[(int)newMode];

            // Enable canvas
            if (!Instance.currentCanvas.enabled)
                Instance.currentCanvas.enabled = true;
        }
    }

    public void SwitchToLoadingScreen(string message)
    {
        SwitchMenuState(MenuMode.Loading);

        onLoadingScreenEvent?.Invoke(message);
    }

    #region Button Functions
    public void Join()
    {
        SwitchToLoadingScreen("Connecting...");

        networkManager.StartClient();
    }
    public void Host()
    {
        SwitchToLoadingScreen("Hosting...");

        networkManager.StartHost();
    }
    public void CancelLoad()
    {
        SwitchMenuState(MenuMode.Multiplayer);
    }
    #endregion
}