using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class ServerManager : NetworkBehaviour
    {
        public static ServerManager Instance;

        [SyncVar(hook = "RpcUpdatePlayerCount")]
        public int connectionsCount;

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
            connectionsCount = NetworkServer.connections.Count;
        }
        // Called on the server when a client disconnects
        [Server]
        public void OnServerDisconnect(NetworkConnection conn)
        {
            connectionsCount = NetworkServer.connections.Count;
        }
        [Server]
        // Called on the server when a client is ready & has loaded the scene
        public void OnServerReady(NetworkConnection conn)
        {
            connectionsCount = NetworkServer.connections.Count;

            GameObject player = Instantiate(NetworkManager.Instance.playerPrefab);
            player.name = $"Player {conn.connectionId}";

            NetworkServer.AddPlayerForConnection(conn, player);

            TargetUpdateClientTypeText(conn);
        }
        [Client]

        // BUG: Doesn't get called on clients in builds
        private void RpcUpdatePlayerCount(int oldCount, int newCount)
        {
            UILobby.Instance.UpdatePlayerCount(newCount);
        }
        [TargetRpc]
        public void TargetUpdateClientTypeText(NetworkConnection conn)
        {
            UILobby.Instance.UpdateClientType();
        }
    }
}