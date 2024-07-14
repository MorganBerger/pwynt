using System;
using System.Linq;
using UnityEngine;

public enum PlayerType {
    main, opponent
}

public class Player : MonoBehaviour
{
    public Deck _deck;

    public PlayerType playerType;

    public Hand _hand;

    public string chosenDeck = "deck uno";

    public BoardCardRow[] cardRows;

    int lifePoints = 2;

    void Awake() {
        // print("Player awake" + playerType);
        Physics.queriesHitTriggers = true;
        
        _deck = GetComponentInChildren<Deck>();

        if (_deck == null) { print("deck (" + playerType + ") is null"); }
        _hand = GetComponentInChildren<Hand>();

        if (_hand == null) { print("hand (" + playerType + ") is null"); }

        cardRows = GetComponentsInChildren<BoardCardRow>();
    }

    private void SetDeckMainPlayer() {
        var chosenDeck = GameHelper.chosenDeck;
        if (chosenDeck.Length == 0) {
            chosenDeck = "something";
            // chosenDeck = "1234";
            // chosenDeck = "2rows";
            // chosenDeck = "deck uno";
            // chosenDeck = "full";
            print("should not happen");
        }
        print("player start chosen deck: " + chosenDeck);

        // int[] deck = GetDeckFromStorage(chosenDeck);
        int[] deck = (int[])DeckStorageHandler.LoadDeck(chosenDeck);
        print("deck Length: " + deck.Length);
        _deck.SetDeck(deck);
    }


    public virtual void SetDeck() {
        SetDeckMainPlayer();
    }

    void Start() {
        print("Player start, type: " + playerType);

        SetDeck();
        Draw(4);
    }

    public virtual void Play(int cardID) {}

    public void Draw(int nb) {
        _hand.AddCards(
            _deck.DrawCards(nb)
        );
    }
}
