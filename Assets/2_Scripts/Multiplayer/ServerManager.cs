using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class ServerManager : NetworkBehaviour
    {
        public static ServerManager Instance;

        [SyncVar]
        public int clientCount;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        // Called on the server when a client connects
        [Server]
        public void OnServerConnect(NetworkConnection conn)
        {
            GameObject player = Instantiate(NetworkManager.Instance.playerPrefab);
            player.name = $"Player {conn.connectionId}";

            NetworkServer.AddPlayerForConnection(conn, player);

            TargetOnClientConnect(conn);

            UpdatePlayerCount();
        }
        // Called on the server when a client disconnects
        [Server]
        public void OnServerDisconnect(NetworkConnection conn)
        {
            UpdatePlayerCount();
        }

        [Command(requiresAuthority = false)]
        public void CmdUpdatePlayerCount()
        {
            UpdatePlayerCount();
        }
        [Server]
        public void UpdatePlayerCount()
        {
            clientCount = NetworkServer.connections.Count;

            Debug.Log($"<color=#33FF99>[Server]</color> Players connected: {clientCount}");

            UILobby.Instance.RpcUpdatePlayerCount();
        }
        [TargetRpc]
        public void TargetOnClientConnect(NetworkConnection target)
        {
            Debug.Log($"<color=#4CC4FF>[Target Rpc]</color> Client is ready.");
        }
        [TargetRpc]
        public void TargetOnClientDisconnect(NetworkConnection target)
        {
            Debug.Log($"<color=#4CC4FF>[Target Rpc]</color> Client disconnecting.");
        }
    }
}