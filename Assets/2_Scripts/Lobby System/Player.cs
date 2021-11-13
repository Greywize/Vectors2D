/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace LobbySystem
{
    public class Player : NetworkBehaviour
    {
        public static Player LocalPlayer;
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        NetworkMatch networkMatch;

        private void Start()
        {
            networkMatch = GetComponent<NetworkMatch>();

            // If this is our local player
            if (isLocalPlayer)
                LocalPlayer = this;
            else
                UILobbyOld.Instance.SpawnPlayerPrefab(LocalPlayer);
        }

        #region Host Match
        public void HostGame()
        {
            // Get a random match id from MatchMaker
            string matchID = MatchMaker.GetRandomMatchID();

            // Call for the server to host a game with the our match id
            CmdHostGame(matchID);
        }
        [Command]
        // Server side command for hosting a game
        void CmdHostGame(string matchID)
        {
            this.matchID = matchID;
            // Host a game - If successful..
            if (MatchMaker.Instance.HostGame(matchID, gameObject, out playerIndex))
            {
                Debug.Log($"<color=green>Match hosted successfully.</color>");
                networkMatch.matchId = matchID.ToGuid();
                TargetHostGame(true, matchID);
            }
            // Match hosting failed
            else
            {
                Debug.LogWarning($"<color=red>Failed to host match.</color>");
                TargetHostGame(false, matchID);
            }
        }
        [TargetRpc]
        void TargetHostGame (bool success, string matchID)
        {
            UILobbyOld.Instance.HostSuccess(success, matchID);
        }
        #endregion

        #region Join Match
        public void JoinGame(string matchID)
        {
            // Call command to join game using the matchID
            CmdJoinGame(matchID);
        }
        [Command]
        // Server side command for joining a game
        void CmdJoinGame(string matchID)
        {
            this.matchID = matchID;
            // Join a game - If successful..
            if (MatchMaker.Instance.JoinGame(matchID, gameObject, out playerIndex))
            {
                Debug.Log($"<color=green>Match joined successfully.</color>");
                networkMatch.matchId = matchID.ToGuid();
                TargetJoinGame(true, matchID);
            }
            // Failed to join match
            else
            {
                Debug.LogWarning($"<color=red>Failed to join match.</color>");
                TargetJoinGame(false, matchID);
            }
        }
        [TargetRpc]
        void TargetJoinGame(bool success, string matchID)
        {
            UILobbyOld.Instance.JoinSuccess(success, matchID);
        }
        #endregion

        #region Lobby
        public void StartGame()
        {
            // Send a command to the server to start the game
            CmdStartGame();
        }
        [Command]
        // Server side command for hosting a game
        void CmdStartGame()
        {
            MatchMaker.Instance.StartGame();
            Debug.Log($"<color=green>Starting game.</color>");
            RpcStartGame();
        }
        [ClientRpc]
        void RpcStartGame()
        {
            // Load game scene additively
        }
        public void LeaveGame()
        {
            // Request to leave the game
            CmdLeaveGame(gameObject);
        }
        [Command]
        // Called on the server when a player requests to leave the game
        void CmdLeaveGame(GameObject player)
        {
            MatchMaker.Instance.LeaveGame(player);
            Debug.Log($"<color=green>Leaving game.</color>");
            RpcRemovePlayerPrefab(player);
            TargetLeaveGame();
        }
        [TargetRpc]
        void TargetLeaveGame()
        {
            UILobbyOld.Instance.LeaveSuccess();
        }
        [ClientRpc]
        void RpcRemovePlayerPrefab(GameObject player)
        {
            UILobbyOld.Instance.RemovePlayerPrefab(player);
        }
        #endregion
    }
}*/