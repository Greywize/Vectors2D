using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestroy : MonoBehaviour
{
    GameObject Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = gameObject;

        DontDestroyOnLoad(Instance);
    }
}