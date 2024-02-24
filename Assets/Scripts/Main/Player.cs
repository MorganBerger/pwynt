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

    void Awake() {
        print("Player awake");
        Physics.queriesHitTriggers = true;
        
        deck = GetComponentInChildren<Deck>();
        hand = GetComponentInChildren<Hand2>();

        deck.SetDeck(GetDeckFromStorage("deck1"));
    }

    void Start() {
        print("Player start");
        Draw(1);
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
