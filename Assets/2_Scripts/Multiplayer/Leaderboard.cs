using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class Leaderboard : NetworkBehaviour
    {
        public static Leaderboard Instance;

        [SerializeField] Transform entriesTransform;
        [SerializeField] GameObject entryPrefab;
        [SerializeField] List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
        [SerializeField] SyncList<Entry> entries = new SyncList<Entry>();

        private void OnEnable()
        {
            ServerManager.Instance.onPlayerJoin += AddEntry;
            ServerManager.Instance.onPlayerLeave += RemoveEntry;
        }
        private void OnDisable()
        {
            ServerManager.Instance.onPlayerJoin -= AddEntry;
            ServerManager.Instance.onPlayerLeave -= RemoveEntry;
        }
        private void Start()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }
        public void AddEntry(Player player)
        {
            // Create a new object
            GameObject entryObject = Instantiate(entryPrefab, entriesTransform);
            // Get the entry component and add it to our list
            LeaderboardEntry entry = entryObject.GetComponent<LeaderboardEntry>();
            leaderboardEntries.Add(entry);

            entry.SetPlayer(player);
        }
        public void RemoveEntry(Player player)
        {
            foreach(LeaderboardEntry entry in leaderboardEntries)
            {
                if (entry.GetPlayer() == player)
                {
                    leaderboardEntries.Remove(entry);
                    Destroy(entry.gameObject);
                }
            }
        }
        public void Sort()
        {

        }
    }
}