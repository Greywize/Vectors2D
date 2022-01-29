using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchMade
{
    // Acts like a node in a tree of nodes, much like a state machine
    // Allows for navigation to and from other interfaces using set transitions
    [RequireComponent(typeof(Canvas))]
    public class InterfaceNode : MonoBehaviour
    {
        [Tooltip("Set to false to disable navigation to and from other nodes.")]
        [SerializeField] bool navigation = true;
        Canvas canvas;

        // Called when we navigate to this interface
        [SerializeField] Tween tweenIn;
        // Called when we navigate to another interface
        [SerializeField] Tween tweenOut;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        // Navigates the UI to the target interface
        public void Navigate(InterfaceNode target)
        {
            if (!target.navigation)
            {
                Debug.Log($"{target} interface has disabled navigating.");
                return;
            }

            // Hide this canvas
            Hide();
            // Show the target canvas
            target.Show();
            // Update the global current canvas
            if (InterfaceManager.Instance)
               InterfaceManager.Instance.SetCurrent(target);
        }
        private void Show()
        {
            // Enable canvas
            if (!canvas.enabled)
                canvas.enabled = true;
        }
        private void Hide()
        {
            // Disable canvas
            if (canvas.enabled)
                canvas.enabled = false;
        }
    }
}