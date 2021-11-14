using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MatchMade
{
    // Contains functions dedicated to handling and moving between CanvasInterfaces
    // This project doesn't utilise them, so some code is commented out
    public class InterfaceManager : MonoBehaviour
    {
        public static InterfaceManager Instance;

        public delegate void AutoHostEvent();
        public AutoHostEvent autoHost;
        public delegate void DisconnectEvent();
        public DisconnectEvent onFailedToConnect;
        public UnityEvent hostInternal;

        public bool dontDestroyOnLoad = false;

        // CanvasInterface current;

        // [SerializeField] CanvasInterface main;
        [SerializeField] TMPro.TMP_InputField addressField;
        [SerializeField] TMPro.TMP_Text addressErrorText;
        [SerializeField] Button playButton;
        [SerializeField] Button joinLocalButton;
        [SerializeField] Button hostLocalButton;
        [SerializeField] GameObject connectingIndicator;

        public TweenController tweenController;

        private void Awake()
        {
            // Set up static instance so we can access this anywhere
            if (Instance != null)
                Destroy(gameObject);
            Instance = this;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        private void Start()
        {
            if (NetworkManager.Instance == null)
                Debug.Log("No NetworkManager found in scene.");

            // current = FindFirstActive();
        }
        // Returns the first active canvas found amongst all CanvasInterfaces 
        /*public CanvasInterface FindFirstActive()
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
        }*/
        public void EnableLocalButtons(bool enabled)
        {
            playButton.interactable = enabled;
            joinLocalButton.interactable = enabled;
            hostLocalButton.interactable = enabled;
        }
        public void AddressError(string error)
        {
            addressErrorText.SetText(error);
        }
        #region Buttons 
        public void Play()
        {
            if (string.IsNullOrWhiteSpace(addressField.text))
            {
                AddressError("Invalid Address");
                return;
            }
            else
            {
                // Clear error message
                if (addressErrorText.text != "")
                    AddressError("");

                tweenController.StartStage(2);
                connectingIndicator.SetActive(true);
                EnableLocalButtons(false);

                // Subscribe Host function to our AutoHost event in case we fail to join
                autoHost += Host;

                NetworkManager.Instance.networkAddress = addressField.text;

                // Attempt to join master server
                NetworkManager.Instance.StartClient();
            }
        }
        public void Host()
        {
            if (string.IsNullOrWhiteSpace(addressField.text))
            {
                AddressError("Invalid Address");
                return;
            }
            else
            {
                AddressError("");
            }

            // Clear error message
            if (addressErrorText.text != "")
                AddressError("");

            EnableLocalButtons(false);

            tweenController.StartStage(1);
            tweenController.stages[1].onComplete.AddListener(() =>
            {
                NetworkManager.Instance.networkAddress = addressField.text;
                try
                {
                    NetworkManager.Instance.StartHost();
                }
                catch (SocketException exception)
                {
                    EnableLocalButtons(true);

                    tweenController.StartStage(0);
                    AddressError($"Address {NetworkManager.Instance.networkAddress} already has a server");
                }
            });
        }
        public void Join()
        {
            if (string.IsNullOrWhiteSpace(addressField.text))
            {
                AddressError("Invalid Address");
                return;
            }
            else
            {
                AddressError("");
            }

            tweenController.StartStage(2);
            connectingIndicator.SetActive(true);
            EnableLocalButtons(false);

            // Set up onFailedToConnect event
            // Called in OnClientDiscconet in the NetworkManager
            onFailedToConnect += () => 
            { 
                AddressError("Failed to connect");
                connectingIndicator.SetActive(false);
                tweenController.StartStage(0);
                EnableLocalButtons(true);
            };

            NetworkManager.Instance.networkAddress = addressField.text;
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