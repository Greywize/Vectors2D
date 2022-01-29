using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MatchMade
{
    public class MM_Button : Selectable
    {
        [SerializeField] [Range(0, 1)]
        [Tooltip("Determines how opaque the sprite must be before it is detected by the pointer.")]
        private float AlphaThreshold = 0.1f;
        [SerializeField] [Tooltip("Controls whether the button will automatically deselect after it has been pressed.")]
        private bool autoDeselect = true;

        protected override void Start()
        {
            base.Start();

            GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            if (autoDeselect && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                StartCoroutine(DeselectNextFrame());
            }
        }

        IEnumerator DeselectNextFrame()
        {
            yield return new WaitForEndOfFrame();

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}