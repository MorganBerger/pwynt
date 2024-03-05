using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    public Deck deck;

    public Hand hand;

    public BoardCardRow[] cardRows;

    int lifePoints = 2;

    void Awake() {
        print("Player awake");
        Physics.queriesHitTriggers = true;
        
        deck = GetComponentInChildren<Deck>();
        hand = GetComponentInChildren<Hand>();

        deck.SetDeck(GetDeckFromStorage("deck1"));

        cardRows = GetComponentsInChildren<BoardCardRow>();
    }

    // public BoardCardRow GetCardRow(Battalion by) {
    //     for (int i = 0; i < cardRows.Length; i++) {
    //         var row = cardRows[i];
    //         if (row.acceptedType == by) {
    //             return row;
    //         }
    //     }
    //     return null;
    // }

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
