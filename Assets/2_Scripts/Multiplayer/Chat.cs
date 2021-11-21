using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MatchMade
{
    [RequireComponent(typeof(TMP_InputField))]
    public class Chat : NetworkBehaviour
    {
        public static Chat Instance;

        public enum ChatMode { Idle, Active, Disabled }

        ChatMode mode = ChatMode.Idle;

        [SerializeField] int visibleMessageLimit = 5;
        [SerializeField] int characterLimit = 50;
        [SerializeField] float messageVisibilityTime = 10f;
        [SerializeField] float fadeOutTime = 1.5f;
        [SerializeField] bool unselectOnMessageSent = false;

        [Header("Chat")]
        [Tooltip("Prefab that is used as a base for chat messages.")]
        [SerializeField] GameObject messagePrefab;
        [Tooltip("The transform used as a parent for the text objects instantiated.")]
        [SerializeField] Transform messageStackTransform;

        TMP_InputField inputField;
        TMP_Text placeholder;

        [Header("Control")]
        public InputAction enableChat;
        public InputAction cancel;

        [Header("Events")]
        public UnityEvent OnMessageSent;
        public UnityEvent OnMessageReceived;

        [SerializeField] List<GameObject> messages = new List<GameObject>();

        private void Awake()
        {
            enableChat.started += ctx => HandleChat();
            enableChat.Enable();
            cancel.started += ctx => UnfocusChat();
            cancel.Enable();

            inputField = GetComponent<TMP_InputField>();
            placeholder = inputField.placeholder.GetComponent<TMP_Text>();

            inputField.characterLimit = characterLimit;
        }
        private void Start()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
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
                    if (string.IsNullOrWhiteSpace(inputField.text))
                    {
                        UnfocusChat();
                        break;
                    }

                    CmdSendChatMessage(inputField.text);

                    if (unselectOnMessageSent)
                    {
                        UnfocusChat();
                    }
                    else
                    {
                        inputField.text = "";
                        inputField.ActivateInputField();
                        inputField.Select();
                    }
                    break;
            }
        }
        [Command(requiresAuthority = false)]
        public void CmdSendChatMessage(string message)
        {
            RpcRecieveMessage(message);
        }
        [ClientRpc]
        public void RpcRecieveMessage(string message)
        {
            GameObject messageObject = Instantiate(messagePrefab, messageStackTransform);

            messageObject.GetComponentInChildren<TMP_Text>().text = message;

            if (messages.Count > visibleMessageLimit)
            {
                GameObject temp = messages[messages.Count - 1];
                messages.RemoveAt(messages.Count - 1);

                Destroy(temp);
            }
            messages.Insert(0, messageObject);
            StartCoroutine(TimeOutMessage(messageObject));
        }
        public void FocusChat()
        {
            inputField.interactable = true;

            inputField.ActivateInputField();
            inputField.Select();
            placeholder.text = "";

            mode = ChatMode.Active;

            Player.LocalPlayer.CanControl = false;
        }
        public void UnfocusChat()
        {
            inputField.text = "";
            // ToDo - Store the initial place holder text and re-use that instead of hard-coding it
            placeholder.text = "Press [Enter] to chat";
            inputField.DeactivateInputField();

            inputField.interactable = false;

            mode = ChatMode.Idle;

            Player.LocalPlayer.CanControl = true;
        }
        IEnumerator TimeOutMessage(GameObject message)
        {
            yield return new WaitForSeconds(messageVisibilityTime);

            // First check if it's still there
            if (message)
            {
                TweenController controller = message.GetComponent<TweenController>();

                controller.stages[0].tweens[0].time = fadeOutTime;
                controller.BeginStage(0).onComplete.AddListener(() =>
                {
                    // Check again as we could have deleted it by reaching the visible message limit by now
                    if (message)
                    {
                        messages.Remove(message);
                        Destroy(message);
                    }
                });
            }
        }
    }
}