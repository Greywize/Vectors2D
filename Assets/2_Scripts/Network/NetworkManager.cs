using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManager : Mirror.NetworkManager
{
    #region Server
    public override void OnStartServer()
    {
        base.OnStartServer();
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }
    #endregion

    #region Client
    public override void OnStartClient()
    {
        
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        
    }
    #endregion

    #region Host
    public override void OnStartHost()
    {
        base.OnStartHost();
    }
    public override void OnStopHost()
    {
        base.OnStopHost();
    }
    #endregion
    public void Host()
    {
        StartHost();
    }
    public void Disconnect()
    {
        Debug.Log(mode);

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