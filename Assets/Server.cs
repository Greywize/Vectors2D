using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Server : NetworkBehaviour
{
    public static GameObject playerPrefab;

    private void Start()
    {
        if (NetworkManager.singleton)
            playerPrefab = NetworkManager.singleton.playerPrefab;
    }

    [Server]
    public static void Spawn(NetworkConnection conn)
    {
        if (!NetworkManager.singleton)
            return;

        GameObject player = Instantiate(playerPrefab);

        player.name = $"{playerPrefab.name} {NetworkManager.localPlayerName}";

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}