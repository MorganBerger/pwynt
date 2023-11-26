using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Deck deck;
    Card[] hand;
    int lifePoints;
    public GameObject deckContainer;

    void Awake() {
        
        var prefab = Resources.Load("Prefabs/Standards/Deck", typeof(GameObject));
        var deckObj = (GameObject)Instantiate(prefab);

        deckObj.transform.localPosition = Vector3.zero;
        deckObj.transform.SetParent(transform);

        deck = deckObj.GetComponent<Deck>();

        StorageHandler handler = new StorageHandler();
        var cerealDeck = (CardObjectCereal[])handler.LoadData("deck1");

        deck.SetDeck(cerealDeck);
        deck.transform.SetParent(transform);
        // deck.transform.SetParent(deckContainer.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
