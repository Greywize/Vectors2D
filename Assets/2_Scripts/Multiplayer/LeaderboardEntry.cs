using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatchMade
{
    public class Entry
    {
        public string name;
        public int placement;
    }
    public class LeaderboardEntry : MonoBehaviour
    {
        Leaderboard leaderboard;
        Player player;
        Entry entry;

        [SerializeField] TMP_Text placementText;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text scoreText;

        public void SetEntry(Player player)
        {
            // Player
            this.player = player;
            // Name
            nameText.text = player.PlayerName;
            // Score
            scoreText.text = player.Score.ToString();
        }
        // public void SetLeaderboard(Leaderboard leaderboard) { this.leaderboard = leaderboard; }
        public void SetPlayer(Player player) { this.player = player; SetEntry(player); }
        public Player GetPlayer() { return player; }
        public void SetName(string name) { nameText.text = player.PlayerName; }
        public string GetName() { return player.PlayerName; }
        public void SetScore(int score) { scoreText.text = score.ToString(); }
        public int GetScore() { return player.Score; }
        public void SetPlacement(int placement) { entry.placement = placement; placementText.text = placement.ToString(); }
        public int GetPlacement() { return entry.placement; }
    }
}