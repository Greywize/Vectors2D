using System.Collections;
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
            {
                UILobby.Instance.SpawnPlayerPrefab(this);
            }
        }
        
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
            UILobby.Instance.HostSuccess(success, matchID);
        }

        public void JoinGame(string matchID)
        {
            // Call for the server to host a game with the our match id
            CmdJoinGame(matchID);
        }

        [Command]
        // Server side command for hosting a game
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
            // Match hosting failed
            else
            {
                Debug.LogWarning($"<color=red>Failed to join match.</color>");
                TargetJoinGame(false, matchID);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID)
        {
            UILobby.Instance.JoinSuccess(success, matchID);
        }
    }
}