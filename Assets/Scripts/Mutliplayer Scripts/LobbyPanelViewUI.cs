using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Unity.Netcode;

public class LobbyPanelViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TextMeshProUGUI numberOfPlayersConnectedText;
    public void Show()
    {
        gameObject.SetActive(true);
        joinCodeText.SetText(SessionManager.Instance.LobbyCode);
    }
    public void UpdatePlayerCountText()
    {
        numberOfPlayersConnectedText.SetText(NetworkManager.Singleton.ConnectedClientsIds.Count + "/4 connected");

    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
