using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class ServerManager : NetworkBehaviour
    {
        public static ServerManager Instance;
        public delegate void ServerEvent(Player player);
        public ServerEvent onPlayerJoin;
        public ServerEvent onPlayerLeave;

        [SyncVar(hook = "SyncConnectionsCount")]
        public int connectionsCount;
        public bool Active { get { if (Instance) return NetworkClient.active; return false; } }

        private void Awake()
        {
            // Set up static instance so we can access this anywhere
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }
        public override void OnStartClient()
        {
            // We're running the hook here as clients don't run it automatically 
            // when recieving value for the first time when joining the server
            SyncConnectionsCount(connectionsCount, connectionsCount);
        }
        [Server]
        public void OnServerConnectedAndReady(Player player)
        {
            onPlayerJoin.Invoke(player);
        }
        public void OnServerDisconnect(Player player)
        {
            onPlayerLeave.Invoke(player);
        }
        // Called on the client when it first joins
        [TargetRpc]
        public void TargetUpdateDebugInformation(NetworkConnection conn)
        {
            UIOnline.Instance.UpdateDebugElements();
            UIOnline.Instance.UpdateClientType();
        }
        [ClientRpc]
        public void RpcAddLeaderboardEntry(Player player)
        {
            if (Leaderboard.Instance)
                Leaderboard.Instance.AddEntry(player);
            else
                Debug.Log($"Cannot add {player.PlayerName} to leaderboard. Leaderboard is null.");
        }
        [ClientRpc]
        public void RpcRemoveLeaderboardEntry(Player player)
        {
            if (Leaderboard.Instance)
                Leaderboard.Instance.RemoveEntry(player);
            else
                Debug.Log($"Cannot remove {player.PlayerName} from leaderboard. Leaderboard is null.");
        }
        // Connections Count SyncVar hook
        // Updates the debug text element displaying connected client count
        private void SyncConnectionsCount(int oldCount, int newCount)
        {
            // Set it here in case the hook was called without changing the value (If the server called the hook instead of changing the value)
            connectionsCount = newCount;

            UIOnline.Instance.UpdatePlayerCount(newCount);
        }
    }
}