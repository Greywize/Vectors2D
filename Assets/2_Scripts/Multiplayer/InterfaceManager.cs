using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace MatchMade
{
    public class InterfaceManager : MonoBehaviour
    {
        [Header("Cursor")]
        public Texture2D cursorTexture;
        public CursorMode cursorMode = CursorMode.Auto;

        public static InterfaceManager Instance;
        public bool dontDestroyOnLoad = false;

        InterfaceNode current;

        private void Awake()
        {
            // Set up static instance so we can access this anywhere
            if (Instance != null)
                Destroy(gameObject);
            Instance = this;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            SetCursor(cursorTexture);

            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }
        private void Start()
        {
            current = FindFirstActive();
        }

        public void SetCursor(Texture2D texture)
        {
            Vector2 hotSpot = new Vector2(texture.width / 2, texture.height / 2);
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }

        #region Navigating
        // Returns the first active canvas found amongst all Interface Nodes 
        public InterfaceNode FindFirstActive()
        {
            foreach (InterfaceNode canvasInterface in FindObjectsOfType<InterfaceNode>())
            {
                if (canvasInterface.gameObject.GetComponent<Canvas>().enabled)
                    return canvasInterface;
            }
            return null;
        }
        public void Navigate(InterfaceNode screen)
        {
            current.Navigate(screen);
            current = screen;
        }
        public void SetCurrent(InterfaceNode newCurrent)
        {
            current = newCurrent;
        }
        #endregion

        #region Application 
        public void Quit()
        {
            Application.Quit();
        }
        #endregion
    }
}