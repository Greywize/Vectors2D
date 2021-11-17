using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MatchMade;
using System.Linq;
using UnityEngine.SceneManagement;

public class NetworkManager : Mirror.NetworkManager
{
    public static NetworkManager Instance;

    [Header("Debug")]
    public bool clientLogs = true;
    public bool serverLogs = true;

    public override void Awake()
    {
        base.Awake();

        // Set up static instance so we can access this anywhere
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }

    #region Server
    public override void OnStartServer()
    {
        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Started.");
    }
    public override void OnStopServer()
    {
        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Stopped.");
    }

    // Called on the server when a client connects - Too early for Client & Target RPCs
    public override void OnServerConnect(NetworkConnection conn)
    {
        ServerManager.Instance.connectionsCount = NetworkServer.connections.Count;

        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Client connected. ID: {conn.connectionId}");
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        ServerManager.Instance.connectionsCount = NetworkServer.connections.Count;
        
        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Client disconnected. ID: {conn.connectionId}");

        NetworkServer.DestroyPlayerForConnection(conn);
    }
    // Called on the server when a client is ready & has loaded the scene - Client & Target RPCs NOT contained on the player object will work after this is called
    public override void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);

        GameObject player = Instantiate(playerPrefab);
        player.name = $"Player {conn.connectionId}";

        // Add player object for connection to the scene
        NetworkServer.AddPlayerForConnection(conn, player);

        // --- > Client & Target RPCs contained on the player object will work from this point

        ServerManager.Instance.TargetUpdateDebugInformation(conn);
    }
    public override void OnServerError(NetworkConnection conn, Exception exception)
    {
        if (serverLogs)
            Debug.LogError($"<color=#33FF99>[Server]</color> <color=#FF333A>{exception.Message}</color>");
    }
    #endregion
    
    #region Client
    public override void OnStartClient()
    {
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Started.");
    }
    public override void OnStopClient()
    {
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Stopped.");
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        if (!NetworkClient.ready) 
            NetworkClient.Ready();

        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Connected to {networkAddress} as {mode}.");
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Disconnected.");

        StopClient();
    }
    public override void OnClientError(Exception exception)
    {
        if (clientLogs)
            Debug.LogError($"<color=#4CC4FF>[Client]</color><color=#FF333A>{exception.Message}</color>");
    }
    #endregion

    #region Host
    public override void OnStartHost()
    {

    }
    public override void OnStopHost()
    {

    }
    #endregion

    public void Disconnect()
    {
        switch (mode)
        {
            case NetworkManagerMode.Offline:
                return;
            case NetworkManagerMode.ServerOnly:
                StopServer();
                return;
            case NetworkManagerMode.ClientOnly:
                StopClient();
                return;
            case NetworkManagerMode.Host:
                StopHost();
                return;
        }
    }
}