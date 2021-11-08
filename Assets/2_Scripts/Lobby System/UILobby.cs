using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LobbySystem
{
    public class UILobby : MonoBehaviour
    {
        public static UILobby Instance;

        public CanvasInterface mainCanvas;
        public CanvasInterface lobbyCanvas;

        [Header("Host / Join UI Components")]
        [SerializeField] TMPro.TMP_InputField matchIDInput;
        [SerializeField] Button joinButton;
        [SerializeField] Button hostButton;
        [Header("Lobby")]
        [SerializeField] Transform playerPrefabContainer;
        [SerializeField] GameObject playerLobbyPrefab;
        [SerializeField] TMPro.TMP_Text matchIDText;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }

        public void Join()
        {
            // Disable UI interactions to prevent spam clicking
            EnableInteraction(false);

            Player.LocalPlayer.JoinGame(matchIDInput.text);
        }
        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                InterfaceManager.Instance.current.Navigate(lobbyCanvas);

                matchIDText.text = matchID;

                SpawnPlayerPrefab(Player.LocalPlayer);
            }
            else
            {
                EnableInteraction(true);
            }
        }
        public void Host()
        {
            // Disable UI interactions to prevent spam clicking
            EnableInteraction(false);

            // Call HostGame from our local player
            Player.LocalPlayer.HostGame();
        }
        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                InterfaceManager.Instance.current.Navigate(lobbyCanvas);

                matchIDText.text = matchID;

                SpawnPlayerPrefab(Player.LocalPlayer);
            }
            else
            {
                EnableInteraction(true);
            }
        }
        // Whether the lobby buttons & match ID input field should be interactable 
        private void EnableInteraction(bool enabled)
        {
            hostButton.interactable = enabled;
            joinButton.interactable = enabled;
            matchIDInput.interactable = enabled;
        }

        public void SpawnPlayerPrefab(Player player)
        {
            GameObject newPlayer = Instantiate(playerLobbyPrefab, playerPrefabContainer);
            newPlayer.GetComponent<UIPlayer>().SetPlayer(player);
        }
    }
}