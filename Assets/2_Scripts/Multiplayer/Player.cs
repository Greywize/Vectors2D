using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MatchMade
{
    public class Player : NetworkBehaviour
    {
        public static Player LocalPlayer;

        private void Start()
        {
            if (isLocalPlayer)
                LocalPlayer = this;

            // Called as soon as player prefab is spawned in, meaning we're ready and able to handle RPCs
            // ServerManager.Instance.CmdUpdatePlayerCount();
        }
    }
}