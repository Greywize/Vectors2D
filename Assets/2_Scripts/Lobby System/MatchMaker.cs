using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Mirror;

namespace LobbySystem
{
    [System.Serializable]
    public class Match
    {
        public string matchID;
        public SyncListGameObject players = new SyncListGameObject();

        // Match constructor that takes a match id and a player, which is probably the host
        public Match(string matchID, GameObject host)
        {
            this.matchID = matchID;

            players.Add(host);
        }
        // Default constructor
        public Match() { }
    }

    [System.Serializable]
    public class SyncListGameObject : SyncList<GameObject> { }
    [System.Serializable]
    public class SyncListMatch : SyncList<Match> { }

    public class MatchMaker : NetworkBehaviour
    {
        public static MatchMaker Instance;

        public SyncListMatch matches = new SyncListMatch();
        public SyncList<string> matchIDs = new SyncList<string>();

        private void Awake()
        {
            // Instance
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        public bool HostGame(string matchID, GameObject host, out int playerIndex)
        {
            playerIndex = -1;
            if (!matchIDs.Contains(matchID))
            {
                // Add the match and the matchID to our lists
                matchIDs.Add(matchID);
                matches.Add(new Match(matchID, host));
                Debug.Log($"[MatchMaker] Match created with ID: {matchID}");

                playerIndex = 1;

                return true;
            }
            else
            {
                Debug.Log("[MatchMaker] Match with ID " + matchID + " already exists.");
                return false;
            }
        }
        public bool JoinGame(string matchID, GameObject player, out int playerIndex)
        {
            playerIndex = -1;
            if (matchIDs.Contains(matchID))
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].matchID == matchID)
                    {
                        matches[i].players.Add(player);

                        playerIndex = matches[i].players.Count;

                        break;
                    }
                }

                Debug.Log($"[MatchMaker] Match joined | ID: {matchID}");
                return true;
            }
            else
            {
                Debug.Log("[MatchMaker] Match with ID " + matchID + " does not exists.");
                return false;
            }
        }

        // Generate a random string of five characters & numbers
        public static string GetRandomMatchID()
        {
            string id = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);
                if (random < 26)
                    id += (char)(random + 65);
                else
                    id += (random - 26).ToString();
            }
            Debug.Log("[MatchMaker] Generated ID: " + id.ToString());
            return id;
        }
    }

    public static class MatchExstensions
    {   
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            return new System.Guid(hashBytes);
        }
    }
}