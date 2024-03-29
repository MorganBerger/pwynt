using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections;

// void CheckIP(){
//     var myExtIPWWW = WWW("http://checkip.dyndns.org");
//     if(myExtIPWWW==null) return;
//     yield myExtIPWWW;
//     myExtIP=myExtIPWWW.data;
//     myExtIP=myExtIP.Substring(myExtIP.IndexOf(":")+1);
//     myExtIP=myExtIP.Substring(0,myExtIP.IndexOf("<"));
//     // print(myExtIP);
// }

public static class IPManager {
    public enum ADDRESSFAM {
        IPv4, IPv6
    }

    public static string GetIP(ADDRESSFAM Addfam) {
        string ret = "";
        List<string> IPs = GetAllIPs(Addfam, false);
        if (IPs.Count > 0) {
            ret = IPs[IPs.Count - 1];
        }
        return ret;
    }

    public static List<string> GetAllIPs(ADDRESSFAM Addfam, bool includeDetails) {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6) {
            return null;
        }

        List<string> output = new List<string>();

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()) {

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            bool isCandidate = (item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            // as of MacOS (10.13) and iOS (12.1), OperationalStatus seems to be always "Unknown".
            isCandidate = isCandidate && item.OperationalStatus == OperationalStatus.Up;
#endif

            if (isCandidate)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses) {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4) {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork) {
                            string s = ip.Address.ToString();
                            if (includeDetails) {
                                s += "  " + item.Description.PadLeft(6) + item.NetworkInterfaceType.ToString().PadLeft(10);
                            }
                            output.Add(s);
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6) {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6) {
                            output.Add(ip.Address.ToString());
                        }
                    }
                }
            }
        }
        return output;
    }
}

// public enum ADDRESSFAM {
//     IPv4, IPv6
// }

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