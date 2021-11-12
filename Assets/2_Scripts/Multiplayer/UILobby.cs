using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class UILobby : NetworkBehaviour
    {
        public static UILobby Instance;

        [SerializeField] TMPro.TMP_Text clientsText;
        [SerializeField] TMPro.TMP_Text clientTypeText;


        public void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        public void Disconnect()
        {
            NetworkManager.Instance.Disconnect();
        }
        public void UpdateClientType()
        {
            Debug.Log($"<color=#4CC4FF>[Client]</color> Type is {NetworkManager.Instance.mode}");
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
            Debug.Log($"<color=#4CC4FF>[Client]</color> Clients: {count}");
            clientsText.text = $"Players: {count}";
        }
    }
}