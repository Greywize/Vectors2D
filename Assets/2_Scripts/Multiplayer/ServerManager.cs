using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class ServerManager : NetworkBehaviour
    {
        public static ServerManager Instance;

        [SyncVar(hook = "SyncConnectionsCount")]
        public int connectionsCount;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        public override void OnStartClient()
        {
            SyncConnectionsCount(connectionsCount, connectionsCount);
        }

        #region Server
        // Called on the server when a client connects
        [Server]
        public void OnServerConnect(NetworkConnection conn)
        {
            
        }
        // Called on the server when a client disconnects
        [Server]
        public void OnServerDisconnect(NetworkConnection conn)
        {

        }
        [Server]
        // Called on the server when a client is ready & has loaded the scene
        public void OnServerReady(NetworkConnection conn)
        {
            
        }
        #endregion

        [TargetRpc]
        public void TargetOnClientReady(NetworkConnection conn)
        {
            UILobby.Instance.UpdateClientType();
        }

        private void SyncConnectionsCount(int oldCount, int newCount)
        {
            // Set it here in case the hook was called without changing the value (If the server called the hook instead of changing the value)
            connectionsCount = newCount;

            UILobby.Instance.UpdatePlayerCount(newCount);
        }
    }
}