using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MatchMade
{
    public class UIMultiplayer : MonoBehaviour
    {
        public static UIMultiplayer Instance;

        [SerializeField] TMPro.TMP_InputField nameInputField;
        [SerializeField] Button joinLocalButton;
        [SerializeField] Button hostLocalButton;

        public void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        public void Start()
        {
            EnableLocalButtons(true);
        }   

        public void EnableLocalButtons(bool enabled)
        {
            joinLocalButton.interactable = enabled;
            hostLocalButton.interactable = enabled;
        }
    }
}