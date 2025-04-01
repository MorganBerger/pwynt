using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Data;
using UnityEditor;
using TMPro;
using UnityEngine.SceneManagement;

public class DeckBuilderManager : MonoBehaviour {
    public GameObject allCardsContainer;
    public GameObject cardsInDeckContainer;

    UICardList allCardsListView;
    CardsInDeckList cardsInDeckList;

    // List<CardObject> allCards = new List<CardObject>();
    List<CardData> allCards = new List<CardData>();

    const int maxNumberInDeck = 40;
    const int minNumberInDeck = 25;

    void Awake() {
        Setup();
    }

    public CardScriptableList selfCardList;
    void Setup() {
        if (Globals.cardsList == null) {
            // Globals.cardsList = Resources.Load<CardScriptableList>("Scriptables/AllCardsList");
            Globals.cardsList = selfCardList;
        }

        allCardsListView = allCardsContainer.GetComponentInChildren<UICardList>();
        cardsInDeckList = cardsInDeckContainer.GetComponent<CardsInDeckList>();

        allCardsListView.didClickOnCard.AddListener(DidClickOnCard);

        cardsInDeckList.didLoadDeck.AddListener(DidLoadDeck);
        cardsInDeckList.didRemoveCardFromDeck.AddListener(DidRemoveCard);

        CreateAllCards();

        if (allCardsListView != null) {
            allCardsListView.SetCards(allCards);
        }
    }

    void CreateAllCards() {
        allCards = Globals.playableCards.OrderBy(c => c.productionID).ToList();
        // Removing blank card for DeckBuilder
        // allCards.RemoveAt(0);

        // allCards = Globals.CardsObjects()
        //                     .OrderBy((card) => card.productionID)
        //                     .Select(card => {
        //                     return new CardObject(card);
        //                 })
        //                 .ToList();
    }
    
    void DidClickOnCard(CardData card) {
        cardsInDeckList.AddCard(card);
    }
    // void DidClickOnCard(CardObject card) {
    //     cardsInDeckList.AddCard(card);
    // }

    void DidLoadDeck(CardData[] cards) {
        print("Loading " + cards.Length + " cards.");
        foreach (CardData card in cards) {
            print(card.productionID);
            allCardsListView.EnableCard(card, false);
        }
    }

    void DidRemoveCard(CardData card) {
        allCardsListView.EnableCard(card, true);
    }

    public void GoBackToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
