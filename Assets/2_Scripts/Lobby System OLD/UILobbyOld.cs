/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LobbySystem
{
    public class UILobbyOld : MonoBehaviour
    {
        public static UILobbyOld Instance;

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
        [SerializeField] Button startButton;
        [SerializeField] Button leaveButton;

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

            Player.LocalPlayer.JoinGame(matchIDInput.text.ToUpper());
        }
        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                startButton.interactable = false;
                InterfaceManager.Instance.Navigate(lobbyCanvas);

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
                startButton.interactable = true;
                InterfaceManager.Instance.Navigate(lobbyCanvas);

                matchIDText.text = matchID;

                SpawnPlayerPrefab(Player.LocalPlayer);
            }
            else
            {
                EnableInteraction(true);
            }
        }
        public void Leave()
        {
            Player.LocalPlayer.LeaveGame();
        }
        public void LeaveSuccess()
        {
            InterfaceManager.Instance.Navigate(mainCanvas);

            EnableInteraction(true);
        }
        public void RemovePlayerPrefab(GameObject player)
        {
            for (int i = 0; i < playerPrefabContainer.childCount; i++)
            {
                GameObject prefab = playerPrefabContainer.GetChild(i).gameObject;

                if (prefab.GetComponent<UIPlayer>().GetPlayer() == player)
                {
                    Destroy(prefab);
                    break;
                }
            }

        }
        public void DisconnectFromMasterServer()
        {
            NetworkManager.Instance.Disconnect();
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
}*/