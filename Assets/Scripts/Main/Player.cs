using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    public Deck deck;

    public Hand2 hand;

    int lifePoints = 2;

    // Start is called before the first frame update
    void Start() {
        print("Player start");

        Physics.queriesHitTriggers = true;
        
        deck = GetComponentInChildren<Deck>();
        hand = GetComponentInChildren<Hand2>();

        deck.SetDeck(GetDeckFromStorage("deck1"));

        Draw(10);
    }

    CardObjectCereal[] GetDeckFromStorage(string deck) {
        StorageHandler handler = new StorageHandler();
        var cerealDeck = (CardObjectCereal[])handler.LoadData(deck);
        return cerealDeck;
    }

    public void Draw(int nb) {
        hand.AddCards(
            deck.DrawCards(nb)
        );
    }
}
