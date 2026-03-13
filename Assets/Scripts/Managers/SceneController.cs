using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public static SceneController _instance { get; private set; }


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}