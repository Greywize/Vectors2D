using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Discovery;
using Mirror;
using System.Net;
using System.Net.Sockets;

public class NetworkDiscovery : Mirror.Discovery.NetworkDiscovery
{
    [SerializeField]
    string localIp;
    [SerializeField] [Range(0, 10)]
    int discoveryTimeOut = 5;

    public delegate void DiscoveryEvent();
    public DiscoveryEvent onDiscoveryTimeOut;

    private void OnEnable()
    {
        onDiscoveryTimeOut += StartHost;
        onDiscoveryTimeOut += AdvertiseServer;
    }
    private void OnDisable()
    {
        onDiscoveryTimeOut -= StartHost;
        onDiscoveryTimeOut -= AdvertiseServer;
    }

    public override void Start()
    {
        TextUpdater.updateText?.Invoke("Connecting");
        base.Start();

        localIp = LocalIPAddress();

        StartDiscovery();
        StartCoroutine(StartTimeOutTimer(discoveryTimeOut));
    }

    public void ServerFound(ServerResponse response)
    {
        string address;

        if (response.uri.Host == localIp)
            address = "localhost";
        else
            address = response.uri.Host;

        if (NetworkManager.Instance.clientLogs)
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

    IEnumerator StartTimeOutTimer(int timeOut)
    {
        yield return new WaitForSeconds(timeOut);

        if (!NetworkManager.Instance.isNetworkActive)
        {
            TextUpdater.updateText?.Invoke("Hosting");
            yield return new WaitForSeconds(1);

            StopDiscovery();
            onDiscoveryTimeOut?.Invoke();
        }
    }

    private void StartHost()
    {
        try
        {
            NetworkManager.Instance.StartHost();
        }
        catch (SocketException exception)
        {
            TextUpdater.updateText?.Invoke("Server already exists");
        }
    }
}