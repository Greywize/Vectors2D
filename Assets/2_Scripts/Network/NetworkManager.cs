using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManager : Mirror.NetworkManager
{
    /*public static NetworkManager Instance;

    public override void Awake()
    {
        base.Awake();

        // If there is already an instance
        if (Instance != null)
            Destroy(this);
        // Otherwise, this is our instance
        Instance = this;
    }*/

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
        // InterfaceManager.SwitchMenuState(InterfaceManager.MenuMode.Game);
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
        // InterfaceManager.SwitchMenuState(InterfaceManager.MenuMode.Multiplayer);
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

        // InterfaceManager.SwitchMenuState(InterfaceManager.MenuMode.Multiplayer);
    }
}