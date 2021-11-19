using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using static TweenController;

namespace MatchMade
{
    // Contains functions dedicated to handling and moving between CanvasInterfaces
    // This project doesn't utilise them, so some code is commented out
    public class InterfaceManager : MonoBehaviour
    {
        public static InterfaceManager Instance;

        public bool dontDestroyOnLoad = false;

        [Header("Transition")]
        [SerializeField] TweenController transitionTween;
        [SerializeField] TMP_Text transitionMessageText;

        CanvasInterface current;

        private void Awake()
        {
            // Set up static instance so we can access this anywhere
            if (Instance != null)
                Destroy(gameObject);
            Instance = this;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }
        private void Start()
        {
            current = FindFirstActive();
        }

        #region Transitioning
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
        public TweenStage FadeOut()
        {
            transitionTween.BeginStage(0);

            return transitionTween.stages[0];
        }
        public TweenStage FadeIn()
        {
            transitionTween.BeginStage(1);

            return transitionTween.stages[1];
        }
        public TweenStage FadeHalf()
        {
            transitionTween.BeginStage(2);

            return transitionTween.stages[2];
        }
        public void SetTransitionMessage(string message)
        {
            transitionMessageText.text = message;
        }
        #endregion

        #region Buttons 
        public void Disconnect()
        {
            NetworkManager.Instance.Disconnect();
        }
        public void Quit()
        {
            Application.Quit();
        }
        #endregion
    }
}