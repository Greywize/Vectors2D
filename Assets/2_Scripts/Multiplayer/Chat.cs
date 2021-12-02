using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MatchMade
{
    public class Chat : NetworkBehaviour
    {
        public static Chat Instance;

        public enum ChatMode { Idle, Active, Disabled }

        ChatMode mode = ChatMode.Idle;

        [SerializeField] int maxMessageCount = 5;
        [SerializeField] int characterLimit = 50;
        [SerializeField] float messageVisibilityTime = 10f;
        [SerializeField] float fadeOutTime = 1.5f;
        [SerializeField] bool unselectOnMessageSent = false;

        [Header("Chat")]
        [Tooltip("Input field for the chat bar.")]
        [SerializeField] TMP_InputField chatField;
        [Tooltip("Prefab that is used as a base for chat messages.")]
        [SerializeField] GameObject messagePrefab;
        [Tooltip("The transform used as a parent for the text objects.")]
        [SerializeField] Transform messageStackTransform;
        [SerializeField] ScrollRect scrollView;

        RectTransform messageStackRectTransform;

        [Header("Controls")]
        public InputAction enableChat;
        public InputAction cancel;

        [Header("Events")]
        public UnityEvent OnMessageSent;
        public UnityEvent OnMessageReceived;

        TMP_Text placeholder;
        string placeHolderText;

        List<UIMessage> messages = new List<UIMessage>();

        private void Awake()
        {
            // Add chat function to input delegate and enable it
            enableChat.started += ctx => HandleChat();
            enableChat.Enable();
            // Add cancel function to input delegate and enable it
            cancel.started += ctx => UnfocusChat();
            cancel.Enable();

            placeholder = chatField.placeholder.GetComponent<TMP_Text>();

            chatField.characterLimit = characterLimit;
            // Unfocust the chat whenever we deselect it by clicking off of it
            chatField.onDeselect.AddListener(delegate { UnfocusChat(); });
        }
        private void Start()
        {
            // Set up static instance
            if (Instance != null)
                Destroy(this);
            Instance = this;

            // Store the initial placeholder text for later use
            placeHolderText = placeholder.text;

            messageStackRectTransform = messageStackTransform.GetComponent<RectTransform>();
        }
        public void HandleChat()
        {
            // Only run for the local player
            if (!Player.LocalPlayer)
                return;
            // In case we're disabled
            if (mode == ChatMode.Disabled)
                return;

            switch (mode)
            {
                case ChatMode.Idle:
                    FocusChat();
                    break;

                case ChatMode.Active:
                    // Unfocuse chat if the chatbar is empty
                    if (string.IsNullOrWhiteSpace(chatField.text))
                    {
                        UnfocusChat();
                        break;
                    }

                    Player.LocalPlayer.CmdSendChatMessage(Player.LocalPlayer.PlayerName, chatField.text);

                    if (unselectOnMessageSent)
                    {
                        UnfocusChat();
                    }
                    else
                    {
                        // Clear chat bar and reselect it
                        chatField.text = "";
                        chatField.ActivateInputField();
                        chatField.Select();
                    }
                    break;
            }
        }
        [ClientRpc]
        // Called by the server for all clients whenever a client sends a request to send a message to chat
        public void RpcRecieveMessage(string senderName, string message)
        {
            // Create the chat object
            GameObject messageObject = Instantiate(messagePrefab, messageStackTransform);
            // Set up the prefix string
            string prefix = $"[{senderName}] ";
            messageObject.GetComponentInChildren<TMP_Text>().text = prefix + message;
            // Set the name of the gameObject to be something recognizable
            messageObject.name = $"Message {prefix}";
            
            UIMessage messageUI = messageObject.GetComponent<UIMessage>();
            // If we've reached the history limit
            if (messages.Count >= maxMessageCount)
            {
                // Remove the oldest message from the list and destroy its object
                UIMessage temp = messages[messages.Count - 1];
                messages.RemoveAt(messages.Count - 1);
                Destroy(temp.gameObject);
            }
            // Add the message UI object to the list and begin its fade
            messages.Insert(0, messageUI);
            StartCoroutine(StartMessageFade(messageObject));

            // Rebuild the layout groups of the chat bar and its children
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageStackRectTransform);
        }
        public void FocusChat()
        {
            // Chat bar
            chatField.interactable = true;
            chatField.ActivateInputField();
            chatField.Select();
            placeholder.text = "";

            // Scroll view
            scrollView.enabled = true;

            mode = ChatMode.Active;
            // Disable the character controls
            Player.LocalPlayer.CanControl = false;

            // Make all existing messages visible
            foreach (UIMessage message in messages)
            {
                message.tweenController.StopAllTweens();
                message.GetComponent<CanvasGroup>().alpha = 1;
            }
        }
        public void UnfocusChat()
        {
            // Chat bar
            placeholder.text = placeHolderText;
            chatField.text = "";
            StartCoroutine(SetUninteractable());
            // Scroll view
            scrollView.enabled = false;
            messageStackRectTransform.LeanSetLocalPosY(0);

            mode = ChatMode.Idle;
            // Enable the character controls
            Player.LocalPlayer.CanControl = true;

            // Make all old messages disappear
            foreach (UIMessage message in messages)
            {
                // If it's just been sent, ignore it
                if (message.Faded)
                    message.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
        IEnumerator SetUninteractable()
        {
            yield return new WaitForEndOfFrame();

            // This needs to happen at the end of the frame
            chatField.interactable = false;
        }
        // Coroutine that fades the chat message to be transparent after a while
        IEnumerator StartMessageFade(GameObject message)
        {
            yield return new WaitForSeconds(messageVisibilityTime - fadeOutTime);

            UIMessage messageUI = message.GetComponent<UIMessage>();

            if (messageUI)
            {
                // If the chat bar isn't open, fade out
                if (mode != ChatMode.Active)
                {
                    // Set to true immediately so it becomes completely opaque if we open the chat bar and it is only halfway faded
                    messageUI.Faded = true;
                    messageUI.FadeOut();
                }
                // If the chat bar is open, skip the fade
                else
                {
                    messageUI.Faded = true;
                }
            }
        }
    }
}