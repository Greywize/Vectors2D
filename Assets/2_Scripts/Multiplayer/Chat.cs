using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace MatchMade
{
    [RequireComponent(typeof(TMP_InputField))]
    public class Chat : NetworkBehaviour
    {
        public enum ChatMode { Inactive, Active, }

        ChatMode mode = ChatMode.Inactive;

        [SerializeField] int linesVisible = 5;
        [SerializeField] int characterLimit = 100;

        [Header("Chat")]
        [Tooltip("Prefab that is used as a base for chat messages.")]
        [SerializeField] GameObject textPrfab;
        [Tooltip("The transform used as a parent for the text objects instantiated.")]
        [SerializeField] Transform chatBox;

        TMP_InputField inputField;

        [Header("Control")]
        public InputAction enableChat;

        [Header("Events")]
        public UnityEvent OnMessageSent;
        public UnityEvent OnMessageReceived;

        private void Awake()
        {
            enableChat.started += ctx => HandleChat();
            enableChat.Enable();

            inputField = GetComponent<TMP_InputField>();
        }
        public void HandleChat()
        {
            switch (mode)
            {
                case ChatMode.Inactive:
                    FocusChat();
                    break;
                case ChatMode.Active:
                    if (string.IsNullOrWhiteSpace(inputField.text))
                        UnfocusChat();


                    break;
            }
        }
        private void FocusChat()
        {
            inputField.ActivateInputField();
            inputField.Select();

            mode = ChatMode.Active;
        }
        private void UnfocusChat()
        {
            EventSystem.current.SetSelectedGameObject(null);

            mode = ChatMode.Inactive;
        }
    }
}