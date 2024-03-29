using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            // If network as not started basically. Game instance is netheir a client or a server (host is both).
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
                StartButtons();
            }
            else {
                // If game instance has been set as `client`, `host` or `server`
                StatusLabels();
                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons() {
            if (GUILayout.Button("Host")) { 
                NetworkManager.Singleton.StartHost();
            }
            if (GUILayout.Button("Client")) { 
                NetworkManager.Singleton.StartClient(); 
            }
            if (GUILayout.Button("Server")) { 
                NetworkManager.Singleton.StartServer();
            }
        }

        static void StatusLabels()
        {
            // Set description labels
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                // If game instance is server, ask every clients to move.
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                    print("server asked to move");
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds) {
                        print("connected client: '" + uid + "' will be asked to move");
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
                    }
                }
                else { // If game instance is client or host, ask locally spawn instance to move.
                    print("client asked to move");
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<HelloWorldPlayer>();
                    player.Move();
                }
            }
        }
    }
}