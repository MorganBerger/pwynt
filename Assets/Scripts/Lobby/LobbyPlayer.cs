using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class LobbyPlayer : NetworkBehaviour {

    public int playerIndex;

    public override void OnNetworkSpawn() {
        print("Spawned player");
    }

    public override void OnNetworkObjectParentChanged(NetworkObject parentNetworkObject) {
        print("NetworkObjectParentChanged for player: " + playerIndex);
        transform.SetSiblingIndex(playerIndex);
        transform.localScale = new Vector3(1, 1, 1);
    }
}
