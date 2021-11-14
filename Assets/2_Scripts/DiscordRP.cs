/*
    I will probably need this in the future for implementing Discord services, such as invites to games over Discord! :D
    Code from dragonslaya#5806 in the Mirror Discord - Not aiming to steal, only learn. I WILL be writing my own.

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX

#region Using Statements

using UnityEngine;
using UIWidgets;
using System;
using Discord;
using OMN.Scripts.Helpers;
using OMN.Scripts.Managers;
using Steamworks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#endregion

namespace OMN.Scripts.Discord_Game
{
    public sealed class DiscordRichPresence : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Dialog _messageBox = null;

        #endregion

        #region Discord Activity Events

        /// <summary>
        ///     
        /// </summary>
        /// <param name="secret">The secret key to be able to join user in game.</param>
        private void OnActivityJoin(string secret)
        {
            //Bring the player to the proper menu, so he/she can actually join
            GameObject.Find("Multiplayer Button").GetComponent<Button>().onClick.Invoke();

            //Initiates the steam joining process
            var steamId = new CSteamID(Convert.ToUInt64(secret));

            SteamMatchmaking.JoinLobby(steamId);

            Debug.Log("Now Joining");
        }

        /// <summary>
        ///     Fires when a user asks to join the current user's game.
        /// </summary>
        /// <param name="user">The user asking to join player's game.</param>
        private void OnActivityJoinRequest(ref User user)
        {
            var respondYes = RespondRequestYes(user);
            var respondNo = RespondRequestNo(user);

            _messageBox.Clone().Show(
                "May I Join You?",
                user.Username + " wishes to join you.",
                modal: true,
                modalColor: new Color(0, 0, 0, 0.8f),
                buttons: new DialogActions
                {
                    // Button name and Func<bool>, return true to close dialog, otherwise false
                    {"Yes", respondYes},
                    {"No", respondNo},
                    {"Close", Dialog.Close}
                },
                focusButton: "Close");
        }

        #endregion

        #region Class Specific

        /// <summary>
        ///     Responds back to user asking to join with no.
        /// </summary>
        /// <returns></returns>
        private Func<bool> RespondRequestNo(User user)
        {
            JoinRequestResponse(false, user);

            return () => true;
        }

        /// <summary>
        ///     Responds back to user asking to join with yes.
        /// </summary>
        /// <returns></returns>
        private Func<bool> RespondRequestYes(User user)
        {
            JoinRequestResponse(true, user);

            return () => true;
        }

        /// <summary>
        ///     Responds back to user who asked to join match.
        /// </summary>
        /// <param name="response">The response we want to send back to them.</param>
        /// <param name="user">The user to send the response to.</param>
        /// <returns></returns>
        private void JoinRequestResponse(bool response, User user)
        {
            if (!DiscordManager.Instance.Initialized) return;

            Debug.Log("Join request from: " + user.Id);

            DiscordManager.Instance.ActivityManager.SendRequestReply(user.Id, (ActivityJoinRequestReply)Convert.ToInt32(response), (res) =>
            {
                if (res is Result.Ok)
                {
                    Debug.Log("Responded successfully");
                }
            });
        }

        /// <summary>
        ///     Trigger an update to the activity manager of discord.
        /// </summary>
        private void UpdateActivityManager(Activity activity)
        {
            if (!DiscordManager.Instance.Initialized) return;

            DiscordManager.Instance.ActivityManager.UpdateActivity(activity,
                result => { Debug.Log("Update Presence Mode: " + (result is Result.Ok ? "Success!" : "Failed")); });
        }

        /// <summary>
        ///     Send an update to discord to tell users that this user is currently
        ///     idling in our game waiting to play.
        /// </summary>
        private void UpdateIdlePresenceMode()
        {
            if (!DiscordManager.Instance.Initialized) return;

            var activity = DiscordGameSpecificUtilities.CreatePresenceUpdate("Looking To Play", "Navigating Menus",
                DiscordGameSpecificUtilities.LargeImageKey,
                "Big Red Planet", DiscordGameSpecificUtilities.SmallImageKey, "Big Red Planet");

            // Send update to discord now.
            UpdateActivityManager(activity);
        }

        /// <summary>
        ///     Player is currently playing game solo or in multi player
        ///     update based on data we check against.
        /// </summary>
        private void UpdateIngamePresenceMode()
        {
            if (!DiscordManager.Instance.Initialized) return;

            var pId = SteamworksManager.Instance.CurrentLobbyId.GetHashCode().ToString();

            if (pId is "0" || pId is "")
                pId = null;

            var inMatch = string.IsNullOrEmpty(UNetNetworkManager.Instance.MatchName);

            var activity = DiscordGameSpecificUtilities.CreatePresenceUpdate(
                inMatch ? "Playing Solo" : UNetNetworkManager.Instance.MatchName,
                Utils.GetMapName(SceneManager.GetActiveScene().buildIndex) + " | Night " +
                DiscordGameSpecificUtilities.CurrentNight, DiscordGameSpecificUtilities.LargeImageKey,
                $"{Utils.GetMapName(SceneManager.GetActiveScene().buildIndex)}",
                DiscordGameSpecificUtilities.SmallImageKey,
                "Killed " + DiscordGameSpecificUtilities.EnemysKilled + " enemies", pId,
                pId is null ? default :
                new PartySize
                {
                    CurrentSize = SteamworksManager.Instance.Initialized
                        ? SteamMatchmaking.GetNumLobbyMembers(SteamworksManager.Instance.CurrentLobbyId)
                        : 0,
                    MaxSize = SteamworksManager.Instance.Initialized ? 4 : 0
                },

                pId is null ? null : SteamworksManager.Instance.Initialized ? "MatchTest" : "",
                pId is null ? null : SteamworksManager.Instance.Initialized
                    ? SteamworksManager.Instance.CurrentLobbyId.m_SteamID.ToString()
                    : "", "", SteamworksManager.Instance.Initialized);

            // Send update to discord now.
            UpdateActivityManager(activity);
        }

        /// <summary>
        ///     Scene has changed. Let's update presence based on which scene we are on.
        /// </summary>
        /// <param name="oldScene">The old scene which we came from.</param>
        /// <param name="newScene">The new scene in which we are now in.</param>
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (!DiscordManager.Instance.Initialized) return;

            // Because user can leave to main menu we need to disconnect from our manager
            // for kill and night counts.
            switch (Utils.IsGameMap(newScene.buildIndex))
            {
                case true:

                    UNetNetworkManager.Instance.OnPlayerKilledEnemy += DiscordGameSpecificUtilities.UpdateKills;
                    UNetNetworkManager.Instance.OnClientNightHasFallen += DiscordGameSpecificUtilities.UpdateNights;

                    UpdateIngamePresenceMode();
                    break;
                default:

                    UNetNetworkManager.Instance.OnPlayerKilledEnemy -= DiscordGameSpecificUtilities.UpdateKills;
                    UNetNetworkManager.Instance.OnClientNightHasFallen -= DiscordGameSpecificUtilities.UpdateNights;

                    UpdateIdlePresenceMode();
                    break;
            }
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnSceneChange;

            DiscordManager.Instance.OnDiscordInitialized += OnDiscordInitialized;
        }

        /// <summary>
        ///     Discord has been setup we can now setup our rich presence stuff.
        /// </summary>
        private void OnDiscordInitialized()
        {
            if (!DiscordManager.Instance.Initialized) return;

            DiscordManager.Instance.ActivityManager.OnActivityJoinRequest += OnActivityJoinRequest;
            DiscordManager.Instance.ActivityManager.OnActivityJoin += OnActivityJoin;

            UpdateIdlePresenceMode();
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;

            if (DiscordManager.Instance is null || !DiscordManager.Instance.Initialized) return;

            DiscordManager.Instance.ActivityManager.OnActivityJoinRequest -= OnActivityJoinRequest;
            DiscordManager.Instance.ActivityManager.OnActivityJoin -= OnActivityJoin;
            DiscordManager.Instance.OnDiscordInitialized -= OnDiscordInitialized;
        }

        #endregion
    }
}

#endif*/