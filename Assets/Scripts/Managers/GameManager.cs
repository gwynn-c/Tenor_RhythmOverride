using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int TargetFrameRate = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        Application.targetFrameRate = TargetFrameRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
