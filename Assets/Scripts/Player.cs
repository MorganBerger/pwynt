using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Deck deck;
    Card[] hand;
    int lifePoints;
    // GameObject deckContainer;

    // public void SetDeckContainer(GameObject container) {
    //     this.deckContainer = container;
    // }

    void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {
        print("Player start");
        
        deck = GetComponentInChildren<Deck>();

        // var prefab = Resources.Load("Prefabs/Standards/Deck", typeof(GameObject));
        // var deckObj = (GameObject)Instantiate(prefab);

        // deckObj.transform.SetParent(deckContainer.transform);
        // deckObj.transform.localPosition = Vector3.zero;

        // deck = deckObj.GetComponent<Deck>();

        StorageHandler handler = new StorageHandler();
        var cerealDeck = (CardObjectCereal[])handler.LoadData("deck1");

        deck.SetDeck(cerealDeck);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayCard() {

    }
    void DrawCard() {

    }
    void EndTurn() {

    }
}
