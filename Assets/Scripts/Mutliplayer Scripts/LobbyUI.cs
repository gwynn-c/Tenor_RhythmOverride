using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LobbyUI : MonoBehaviour
{
    [SerializeReference] private Button joinGameButton;
    [SerializeReference] private Button joinWithCodeGameButton;
    [SerializeReference] private TMP_InputField joinWithCodeInputField;

    [SerializeReference] private Button createGameButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        createGameButton.onClick.AddListener(() =>
        {
            SessionManager.Instance.CreateLobby();
        }
        );
        joinGameButton.onClick.AddListener(() =>
        {
            SessionManager.Instance.JoinLobby();
        });

        joinWithCodeGameButton.onClick.AddListener(() =>
        {
            SessionManager.Instance.JoinLobbyWithCode(joinWithCodeInputField.text);
        });
    }

}
