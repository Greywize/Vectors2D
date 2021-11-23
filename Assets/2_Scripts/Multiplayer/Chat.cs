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
            enableChat.started += ctx => HandleChat();
            enableChat.Enable();
            cancel.started += ctx => UnfocusChat();
            cancel.Enable();

            placeholder = chatField.placeholder.GetComponent<TMP_Text>();

            chatField.characterLimit = characterLimit;
            chatField.onDeselect.AddListener(delegate { UnfocusChat(); });
        }
        private void Start()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;

            placeHolderText = placeholder.text;

            messageStackRectTransform = messageStackTransform.GetComponent<RectTransform>();
        }
        public void HandleChat()
        {
            if (!Player.LocalPlayer)
                return;

            if (mode == ChatMode.Disabled)
                return;

            switch (mode)
            {
                case ChatMode.Idle:
                    FocusChat();
                    break;
                case ChatMode.Active:
                    if (string.IsNullOrWhiteSpace(chatField.text))
                    {
                        UnfocusChat();
                        break;
                    }

                    Player.LocalPlayer.CmdSendChatMessage(Player.LocalPlayer.playerName, chatField.text);

                    if (unselectOnMessageSent)
                    {
                        UnfocusChat();
                    }
                    else
                    {
                        chatField.text = "";
                        chatField.ActivateInputField();
                        chatField.Select();
                    }
                    break;
            }
        }
        [ClientRpc]
        public void RpcRecieveMessage(string senderName, string message)
        {
            GameObject messageObject = Instantiate(messagePrefab, messageStackTransform);
            string prefix = $"[{senderName}] ";
            messageObject.GetComponentInChildren<TMP_Text>().text = prefix + message;

            messageObject.name = $"Message {prefix}";

            UIMessage messageUI = messageObject.GetComponent<UIMessage>();

            if (messages.Count >= maxMessageCount)
            {
                UIMessage temp = messages[messages.Count - 1];
                messages.RemoveAt(messages.Count - 1);
                Destroy(temp.gameObject);

                LayoutRebuilder.ForceRebuildLayoutImmediate(messageStackRectTransform);
            }

            messages.Insert(0, messageUI);
            StartCoroutine(StartMessageFade(messageObject));
            
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
            Player.LocalPlayer.CanControl = false;

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
            StartCoroutine(DeselectChat());
            // Scroll view
            scrollView.enabled = false;
            messageStackRectTransform.LeanSetLocalPosY(0);

            mode = ChatMode.Idle;
            Player.LocalPlayer.CanControl = true; 

            foreach (UIMessage message in messages)
            {
                if (message.Faded)
                    message.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
        IEnumerator DeselectChat()
        {
            yield return new WaitForEndOfFrame();

            // This needs to happen at the end of the frame
            chatField.interactable = false;
        }
        IEnumerator StartMessageFade(GameObject message)
        {
            yield return new WaitForSeconds(messageVisibilityTime - fadeOutTime);

            UIMessage messageUI = message.GetComponent<UIMessage>();

            if (messageUI)
            {
                // IF the chat bar isn't open, fade out
                if (mode != ChatMode.Active)
                {
                    messageUI.Faded = true;
                    messageUI.FadeOut();
                }
                // If it is open, skip the fade
                else
                {
                    messageUI.Faded = true;
                }
            }
        }
    }
}