using Unity.Netcode;
using UnityEngine;
using System.Net;
using System.Collections;

namespace HelloWorld
{   
    public class HelloWorldPlayer : NetworkBehaviour
    {
        static string GetPublicIP()
        {
            string url = "http://checkip.dyndns.org";
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        IEnumerator askForIp() {
            print("asking for ip");
            print(GetPublicIP());
            
            yield return null;
        }

        public override void OnNetworkSpawn() {
            StartCoroutine(askForIp());
            
            if (IsOwner) {
                print("network has spawned player as owner");
                Move();
            } else {
                print("network has spawned player as NOT owner");
            }
            print("network local object id: " + NetworkManager.Singleton.LocalClientId);
            var IP = NetworkManager.Singleton.LocalClient.PlayerObject;
        }

        public void Move() {
            SubmitPositionRequestServerRpc();
        }

        [Rpc(SendTo.Server)]
        void SubmitPositionRequestServerRpc(RpcParams rpcParams = default) {
            print("submitting new pos for " + name);
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        static Vector3 GetRandomPositionOnPlane() {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update() {
            transform.position = Position.Value;
        }
    }
}