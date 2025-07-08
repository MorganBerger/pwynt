using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager: NetworkBehaviour {

    private NetworkManager networkManager;
    private int maxNumberOfPlayers = 2;

    public GameObject playerlist;
    public GameObject waitForP2GO;

    public Button readyButton;
    public TextMeshProUGUI  waitForHostLabel;

    NetworkVariable<bool> AllPlayersReady = new NetworkVariable<bool>(false);
    NetworkVariable<bool> IsGameStarting = new NetworkVariable<bool>(false);

    void Start() {
        networkManager = NetworkManager.Singleton;
        
        SetupCallbacks();
        CreateLobby();
    }

    void CreateLobby() {
        if (NetworkHelper.isHost) {
            print("Starting as host");
            networkManager.StartHost();

            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Let's fight!";
            readyButton.interactable = false;
            readyButton.onClick.AddListener(OnClickStartGame);

            waitForHostLabel.gameObject.SetActive(false);

        } else { 
            print("Starting as Client");
            networkManager.GetComponent<UnityTransport>().SetConnectionData(
                NetworkHelper.ip, 
                (ushort)NetworkHelper.port
            );
            networkManager.StartClient();

            readyButton.gameObject.SetActive(false);
            waitForHostLabel.gameObject.SetActive(true);
        }
    }

    void SetupCallbacks() {
        networkManager.ConnectionApprovalCallback = ApprovalCheck;
        networkManager.OnClientConnectedCallback += OnClientConnected;
        networkManager.OnClientDisconnectCallback += OnClientDisconnect;

        IsGameStarting.OnValueChanged += IsGameStartingChanged;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response) {
        print("Approval check");
        if (networkManager.ConnectedClientsIds.Count >= maxNumberOfPlayers) {
            print("Approval denied, lobby full.");
            response.Approved = false;
            response.Reason = "Lobby is full";
        } else {
            print("Approval is approved!");
            response.Approved = true;
            response.CreatePlayerObject = true;
        }
        response.Pending = false;
    }

    private void OnClientConnected(ulong clientId) {
        Debug.Log((networkManager.IsHost ? "HOST ->" : "CLIENT ->") + "client connected: " + clientId);
        var client = networkManager.SpawnManager.GetPlayerNetworkObject(clientId);

        if (client == null) { 
            print("client object is NULL");
            return;
        }

        if (networkManager.ConnectedClientsIds.Count >= maxNumberOfPlayers) {
            Destroy(waitForP2GO);
        }

        var player = client.GetComponent<LobbyPlayer>();
        
        if (player == null) { return; }
    
        player.playerIndex = (int)clientId;
        print("playerIndex in row: " + player.playerIndex);

        PutPlayerInList(client.transform);
    }

    private void OnClientDisconnect(ulong clientId) {
        print("Client disconnect: " + clientId);
        if (!networkManager.IsServer && networkManager.DisconnectReason != string.Empty) {
            NetworkHelper.networkError = networkManager.DisconnectReason;
            GoBackToMenu();
        }
    }

    private void PutPlayerInList(Transform t) {
        if (networkManager.IsServer) {
            t.SetParent(playerlist.transform);
        }
    }

    private void StopNetworking() {
        networkManager.Shutdown();
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

    void Update() {
        if (networkManager.IsHost) {
            var ready = CheckForReadyness();
            readyButton.interactable = ready;
            if (AllPlayersReady.Value != ready) {
                AllPlayersReady.Value = ready;
            }
        }
        if (networkManager.IsClient) {
            if (!IsGameStarting.Value) {
                waitForHostLabel.text = AllPlayersReady.Value ? "Waiting for host to start the game..." : "Waiting for all players to be ready...";
            } else {
                waitForHostLabel.text = "Game is starting... " + startingGameCountDown;
            }
        }
    }

    int startingGameCountDown = 5;

    bool CheckForReadyness() {
        var allPlayersReady = false;
        if (networkManager.ConnectedClientsIds.Count < 2) { return false; }

        foreach (ulong uid in networkManager.ConnectedClientsIds) {
            var player = networkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<LobbyPlayer>();
            if (!player.IsReady.Value) {
                allPlayersReady = false;
                break;
            } else {
                allPlayersReady = true;
            }
        }
        return allPlayersReady;
    }

    void OnClickStartGame() {
        StartGameRpc();
    }

    [Rpc(SendTo.Server)]
    void StartGameRpc(RpcParams rpcParams = default) {
        IsGameStarting.Value = true;
    }

    void IsGameStartingChanged(bool oldValue, bool newValue) {
        print("IsGameStarting changed from: " + oldValue + ", to: " + newValue);
        if (!oldValue && newValue) {
            if (networkManager.IsServer) {
                StartCoroutine(DelayStartGame());
            }
            StartCoroutine(CountDownStartGame());
        }
    }

    IEnumerator CountDownStartGame() {
        print("Delay Start game");
        while (startingGameCountDown > 0) {
            yield return new WaitForSeconds(1);
            startingGameCountDown--;
        }
    }

    IEnumerator DelayStartGame() {
        print("Delay Start game");
        yield return new WaitForSeconds(5);
        networkManager.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
