using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    private static SceneHelper Instance;
    [SerializeField]
    string startScene;
    string sceneName;
    bool loaded;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != startScene && !loaded)
        {
            loaded = false;
            SceneManager.LoadScene(startScene);
        }
    }
}