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

        [ClientRpc]
        public void RpcUpdatePlayerCount()
        {
            Debug.Log($"<color=#4CC4FF>[Client Rpc]</color> Updating connected players text. Players connected: {ServerManager.Instance.clientCount}");
            clientsText.text = $"Players: {ServerManager.Instance.clientCount}";
        }
    }
}