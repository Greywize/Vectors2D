using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Discovery;
using Mirror;
using System.Net;
using System.Net.Sockets;

public class NetworkDiscovery : Mirror.Discovery.NetworkDiscovery
{
    public static NetworkDiscovery Instance;

    [SerializeField] string localIp;

    public override void Start()
    {
        base.Start();

        if (Instance != null)
            Destroy(this);
        Instance = this;

        localIp = LocalIPAddress();
    }

    public void ServerFound(ServerResponse response)
    {
        // StopDiscovery();

        string address;
        
        if (response.uri.Host == localIp)
            address = "localhost";
        else
            address = response.uri.Host;

        Debug.Log($"<color=#4CC4FF>[Client]</color> Server found at {address}.");

        NetworkManager.Instance.networkAddress = address;
        NetworkManager.Instance.StartClient();
    }
    
    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}