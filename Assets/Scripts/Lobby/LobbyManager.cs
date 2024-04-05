using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager: MonoBehaviour {

    private NetworkManager networkManager;
    private int maxNumberOfPlayers = 2;

    public GameObject playerlist;
    public GameObject waitForP2GO;

    void Start() {
        networkManager = GetComponent<NetworkManager>();
        
        SetupCallbacks();
        CreateLobby();
    }

    void CreateLobby() {
        if (NetworkHelper.isHost) {
            print("Starting as host");
            NetworkManager.Singleton.StartHost();
        } else { 
            print("Starting as Client");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                NetworkHelper.ip, 
                (ushort)NetworkHelper.port
            );
            NetworkManager.Singleton.StartClient();
        }
    }

    void SetupCallbacks() {
        networkManager.ConnectionApprovalCallback = ApprovalCheck;
        networkManager.OnClientConnectedCallback += OnClientConnected;
        networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response) {
        print("Approval check");
        if (networkManager.ConnectedClientsIds.Count >= maxNumberOfPlayers) {
            print("Approval denied, lobby full.");
            response.Approved = false;
            response.Reason = "Lobby is full";
        } else {
            print("Approval approved!");
            response.Approved = true;
            response.CreatePlayerObject = true;
        }
        response.Pending = false;
    }

    private void OnClientConnected(ulong clientId) {
        Debug.Log("client connected: " + clientId);
        var client = networkManager.SpawnManager.GetPlayerNetworkObject(clientId);

        if (client == null) { return; }

        var player = client.GetComponent<LobbyPlayer>();
        
        if (player == null) { return; }
        
        player.playerIndex = (int)clientId;

        print("playerIndex in row: " + player.playerIndex);

        PutPlayerInList(client.transform);

        if (networkManager.ConnectedClientsIds.Count >= maxNumberOfPlayers) {
            Destroy(waitForP2GO);
        }
    }

    private void OnClientDisconnect(ulong clientId) {
        print("Client disconnect: " + clientId);
        if (!networkManager.IsServer && networkManager.DisconnectReason != string.Empty) {
            NetworkHelper.networkError = networkManager.DisconnectReason;
            GoBackToMenu();
        }
    }

    private void PutPlayerInList(Transform t) {
        if (NetworkManager.Singleton.IsServer) {
            t.SetParent(playerlist.transform);
        }
    }

    private void StopNetworking() {
        NetworkManager.Singleton.Shutdown();
    }

    public void GoBackToMenu() {
        SceneManager.LoadScene("MainMenuScene");
        StopNetworking();
        Destroy(gameObject);
    }

    IEnumerator DelayShutdownServer() { 
        yield return new WaitForSeconds(.5f);
        print("Stoping networking");
        StopNetworking();
        Destroy(gameObject);
    }
}
