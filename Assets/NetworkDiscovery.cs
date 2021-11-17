using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Discovery;

public class NetworkDiscovery : Mirror.Discovery.NetworkDiscovery
{
    public static NetworkDiscovery Instance;

    void Start()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }
}
