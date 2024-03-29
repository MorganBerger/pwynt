using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager: MonoBehaviour {

    public GameObject playerlist;

    void Start() {
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
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId) {
        Debug.Log($"client connected: {clientId}");
        var client = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);

        var player = client.GetComponent<LobbyPlayer>();
        player.playerIndex = (int)clientId;

        print("playerIndex in row: " + player.playerIndex);

        if (client == null) {
            print("client is null");
        }

        PutPlayerInList(client.transform);
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
        // StartCoroutine(DelayShutdownServer());
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
