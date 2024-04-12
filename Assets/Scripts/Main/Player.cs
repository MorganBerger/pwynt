using UnityEngine;

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

        var chosenDeck = GameHelper.chosenDeck;
        if (chosenDeck.Length == 0) {
            chosenDeck = "deck uno";
        }
        print("player start chosen deck: " + chosenDeck);

        deck.SetDeck(GetDeckFromStorage(chosenDeck));
        Draw(5);
    }

    CardObjectCereal[] GetDeckFromStorage(string deck) {
        var cerealDeck = (CardObjectCereal[])DeckStorageHandler.LoadDeck(deck);
        return cerealDeck;
    }

    public void Draw(int nb) {
        hand.AddCards(
            deck.DrawCards(nb)
        );
    }
}
