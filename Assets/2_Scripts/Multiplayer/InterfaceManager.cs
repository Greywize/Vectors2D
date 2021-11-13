using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchMade
{
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

            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        private void Start()
        {
            if (NetworkManager.Instance == null)
                Debug.Log("No NetworkManager found in scene.");

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

        #region Buttons 
        public void Play()
        {
            // Attempt to join master server
        }
        public void HostLocal()
        {
            UIMultiplayer.Instance.EnableLocalButtons(false);

            NetworkManager.Instance.networkAddress = "localhost";
            NetworkManager.Instance.StartHost();
        }
        public void JoinLocal()
        {
            UIMultiplayer.Instance.EnableLocalButtons(false);

            NetworkManager.Instance.networkAddress = "localhost";
            NetworkManager.Instance.StartClient();
        }
        public void Disconnect()
        {
            NetworkManager.Instance.Disconnect();
        }
        public void QuitApplication()
        {
            Application.Quit();
        }
        #endregion
    }
}