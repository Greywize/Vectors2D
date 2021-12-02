/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LobbySystem
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text nameText;
        Player player;

        private void Start()
        {
            
        }
        public void SetPlayer(Player player)
        {
            this.player = player;
            nameText.text = "Player " + player.playerIndex.ToString();
        }
        public Player GetPlayer()
        {
            return player;
        }
    }
}*/