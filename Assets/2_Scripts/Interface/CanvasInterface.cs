using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchMade
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasInterface : MonoBehaviour
    {
        Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        public void Navigate(CanvasInterface target)
        {
            // Hide this canvas
            Hide();
            // Show the target canvas
            target.Show();
            // Update the global current canvas
            // if (InterfaceManager.Instance)
            //    InterfaceManager.Instance.SetCurrent(target);
        }
        public void Show()
        {
            // Enable canvas
            if (!canvas.enabled)
                canvas.enabled = true;
        }
        public void Hide()
        {
            // Disable canvas
            canvas.enabled = false;
        }
    }
}