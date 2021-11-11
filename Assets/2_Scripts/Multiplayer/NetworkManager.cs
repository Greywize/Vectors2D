using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MatchMade;
using System.Linq;

public class NetworkManager : Mirror.NetworkManager
{
    public static NetworkManager Instance;

    public override void Awake()
    {
        base.Awake();

        if (Instance != null)
            Destroy(this);
        Instance = this;
    }

    #region Server
    public override void OnStartServer()
    {
        Debug.Log($"<color=#33FF99>[Server]</color> Started.");
    }
    public override void OnStopServer()
    {
        Debug.Log($"<color=#33FF99>[Server]</color> Stopped.");
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log($"<color=#33FF99>[Server]</color> Client connected. ID: {conn.connectionId}");
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        ServerManager.Instance.TargetOnClientDisconnect(conn);

        Debug.Log($"<color=#33FF99>[Server]</color> Client disconnected. ID: {conn.connectionId}");
        NetworkServer.DestroyPlayerForConnection(conn);

        ServerManager.Instance.UpdatePlayerCount();
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        // This is called only once we know the connection is ready & can recieve RPCs
        ServerManager.Instance.OnServerConnect(conn);
    }
    public override void OnServerError(NetworkConnection conn, Exception exception)
    {
        Debug.Log($"<color=#33FF99>[Server]</color> <color=#FF333A>{exception.Message}</color>");
    }
    #endregion

    #region Client
    public override void OnStartClient()
    {
        Debug.Log($"<color=#4CC4FF>[Client]</color> Started.");
    }
    public override void OnStopClient()
    {
        Debug.Log($"<color=#4CC4FF>[Client]</color> Stopped.");
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log($"<color=#4CC4FF>[Client]</color> Connected.");
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log($"<color=#4CC4FF>[Client]</color> Disconnected.");

        // In the event we are disconnected but the NetworkClient is still active,
        // if (NetworkClient.active)
        //     NetworkClient.Disconnect();
    }
    public override void OnClientError(Exception exception)
    {
        Debug.LogError($"<color=#4CC4FF>[Client]</color> <color=#FF333A>{exception.Message}</color>");
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