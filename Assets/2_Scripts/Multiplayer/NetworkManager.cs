using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MatchMade;
using System.Linq;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class NetworkManager : Mirror.NetworkManager
{
    public static NetworkManager Instance;
    private NetworkDiscovery networkDiscovery;

    public static string localPlayerName;

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
    public override void Start()
    {
        base.Start();

        networkDiscovery = GetComponent<NetworkDiscovery>();
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
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Server.Spawn(conn);
    }
    // Called on the server when a client connects - Too early for Client & Target RPCs
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Client connected. ID: {conn.connectionId}");
    }
    // Called on the server and a client disconnects, including the host
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Client disconnected. ID: {conn.connectionId}");

        NetworkServer.RemovePlayerForConnection(conn, true);
    }
    // Called on the server when a client is ready & has loaded the scene - Client & Target RPCs NOT contained on the player object will work after this is called
    public override void OnServerReady(NetworkConnection conn)
    {
        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Ready.");
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        if (serverLogs)
            Debug.Log($"<color=#33FF99>[Server]</color> Scene changed.");
    }
    // Call on server when the transport raises an exception
    public override void OnServerError(NetworkConnection conn, Exception exception)
    {
        if (serverLogs)
            Debug.LogError($"<color=#33FF99>[Server]</color> <color=#FF333A>{exception.Message}</color>");
    }
    #endregion
    
    #region Client
    // Invoked when the client is started
    public override void OnStartClient()
    {
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Started.");
    }
    // Invoked when the client is stopped
    public override void OnStopClient()
    {
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Stopped.");
    }
    // Called on the client when we connect to a server
    public override void OnClientConnect(NetworkConnection conn)
    { 
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Connected to {networkAddress} as {mode}.");
    }
    // Called on the client when disconnected from server
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Disconnected.");
    }
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (clientLogs)
            Debug.Log($"<color=#4CC4FF>[Client]</color> Scene changed.");
    }
    // Called on the client when transport raises an exception
    public override void OnClientError(Exception exception)
    {
        if (clientLogs)
            Debug.LogError($"<color=#4CC4FF>[Client]</color><color=#FF333A>{exception.Message}</color>");
    }
    #endregion

    // We shouldn't need to use these
    #region Host
    public override void OnStartHost()
    {

    }
    public override void OnStopHost()
    {

    }
    #endregion
}