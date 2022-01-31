using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class UIOnline : NetworkBehaviour
    {
        public static UIOnline Instance;

        [Header("Debug UI Elements")]
        [SerializeField] TMPro.TMP_Text addressText;
        [SerializeField] TMPro.TMP_Text clientsText;
        [SerializeField] TMPro.TMP_Text clientTypeText;
        [SerializeField] TMPro.TMP_Text pingText;

        public void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        private void Update()
        {
            if (NetworkClient.isConnected)
                pingText.text = $"Ping: {Math.Round(NetworkTime.rtt * 1000)}ms";
        }

        public void Disconnect()
        {
            // NetworkManager.Instance.Disconnect();
        }
        public void UpdateDebugElements()
        {
            addressText.text = NetworkManager.Instance.networkAddress;
        }
        public void UpdateClientType()
        {
            switch (NetworkManager.Instance.mode)
            {
                case NetworkManagerMode.Offline:
                    clientTypeText.text = $"Offline";
                    break;
                case NetworkManagerMode.ServerOnly:
                    clientTypeText.text = $"Server";
                    break;
                case NetworkManagerMode.ClientOnly:
                    clientTypeText.text = $"Client";
                    break;
                case NetworkManagerMode.Host:
                    clientTypeText.text = $"Host";
                    break;
                default:
                    break;
            }
        }
        public void UpdatePlayerCount(int count)
        {
            clientsText.text = $"Players: {count}";
        }
    }
}