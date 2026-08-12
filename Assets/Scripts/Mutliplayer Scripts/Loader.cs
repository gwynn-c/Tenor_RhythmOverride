using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : NetworkBehaviour
{

    public static Loader Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static string GetActiveScene()
    {
        return SceneManager.GetActiveScene().ToString();
    }
    public static void LoadNetwork(string targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);

    }
    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);

    }
}
