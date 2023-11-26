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
        players = new Player[1];

        var prefab = Resources.Load("Prefabs/Standards/Player", typeof(GameObject));
        var playerObj = (GameObject)Instantiate(prefab);

        playerObj.transform.localPosition = Vector3.zero;
        playerObj.transform.SetParent(playerContainer.transform);

        players[0] = playerObj.GetComponent<Player>();
        // players[0].deckContainer = deckObject;
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }
}
