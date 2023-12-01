using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    // public static GameManager Instance;

    public GameObject playerContainer;
    Player[] players;
    public Board board;

    public GameObject deckObject;

    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start() {
        players = new Player[1];
        players = GetComponentsInChildren<Player>();

        print("player count: " + players.Length);
        // var prefab = Resources.Load("Prefabs/Standards/Player", typeof(GameObject));
        // var playerObj = (GameObject)Instantiate(prefab);

        // playerObj.transform.localPosition = Vector3.zero;
        // playerObj.transform.SetParent(playerContainer.transform);

        // var player = playerObj.GetComponent<Player>();
        // player.SetDeckContainer(deckObject);

        // players[0] = player;
        // players[0].deckContainer = deckObject;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
