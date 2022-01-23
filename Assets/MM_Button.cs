using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MatchMade
{
    public class MM_Button : Selectable
    {
        [SerializeField] [Range(0, 1)]
        private float AlphaThreshold = 0.1f;

        protected override void Start()
        {
            base.Start();

            GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
        }
    }
}