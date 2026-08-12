using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Multiplayer;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using System.Xml.Serialization;
using Unity.Services.Lobbies;
using UnityEngine.SceneManagement;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Net.NetworkInformation;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private LobbyPanelViewUI lobbyPanelViewUI;
    private const int MAX_PLAYERS = 4;
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTick;

    public string LobbyCode { get; private set; }
    public static SessionManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            NetworkManager.Singleton.OnConnectionEvent += ((NetworkManager _nm, ConnectionEventData connectionEventData) =>
            {
                if (connectionEventData.EventType.Equals(ConnectionEvent.PeerConnected) && NetworkManager.Singleton.ConnectedClientsIds.Count <= MAX_PLAYERS / 2)
                {
                    lobbyPanelViewUI.UpdatePlayerCountText();
                    if (Loader.GetActiveScene() == "Main Menu Scene")
                        lobbyPanelViewUI.Hide();
                    if (NetworkManager.Singleton.IsServer && Loader.GetActiveScene() != "Playground")
                    {
                        Loader.LoadNetwork("Playground");
                    }

                }
            });
            NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionCallBack;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void NetworkManager_ConnectionCallBack(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (hostLobby.AvailableSlots < 1)
        {
            response.Approved = false;
            response.Reason = "No availabe slot";
            return;
        }
        else
        {
            response.Approved = true;
        }

    }

    private void Update()
    {
        HeartBeatTickHandler();
    }
    protected async void HeartBeatTickHandler()
    {
        if (hostLobby != null)
        {
            heartbeatTick -= Time.deltaTime;
            if (heartbeatTick < 0f)
            {
                float heartbeatTickMax = 15f;
                heartbeatTick = heartbeatTickMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);

            }
        }
    }
    private async Task<Allocation> AllocateRelay()
    {
        try
        {

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYERS - 1);
            return allocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }
    public async void CreateLobby()
    {
        try
        {
            hostLobby = await LobbyService.Instance.CreateLobbyAsync("Lobby #" + UnityEngine.Random.Range(0, 100), MAX_PLAYERS);

            var allocateRelay = await AllocateRelay();
            LobbyCode = hostLobby.LobbyCode;
            var relayJoinCode = await GetRelayJoinCode(allocateRelay);
            await LobbyService.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)}
            }
            });
            lobbyPanelViewUI.Show();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(allocateRelay.ToRelayServerData("dtls"));
            NetworkManager.Singleton.StartHost();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }
    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return relayJoinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }
    public async void JoinLobby()
    {
        try
        {

            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;
            LobbyCode = joinedLobby.LobbyCode;
            var relayJoin = joinedLobby.Data["RelayJoinCode"].Value;
            var allocation = await JoinRelay(relayJoin);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(allocation.ToRelayServerData("dtls"));

            NetworkManager.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);

        }
    }
    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }
    public async void JoinLobbyWithCode(string code)
    {
        try
        {

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            joinedLobby = lobby;
            LobbyCode = joinedLobby.LobbyCode;
            var relayJoin = joinedLobby.Data["RelayJoinCode"].Value;

            var allocation = await JoinRelay(relayJoin);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(allocation.ToRelayServerData("dtls"));

            NetworkManager.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);

        }
    }
    public void LoadLevelForAll()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
