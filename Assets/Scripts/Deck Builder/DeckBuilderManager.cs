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

    List<CardObject> allCards = new List<CardObject>();

    const int maxNumberInDeck = 40;
    const int minNumberInDeck = 25;

    void Awake() {
        Setup();
    }

    void Setup() {
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
        allCards = Globals.AllCardsObjects.Cast<Card>()
                            .OrderBy((card) => card.cardProductionID).Select(c => {
                            return new CardObject("" + c.cardProductionID, c.name, c.fullName, c.level, c.texture2D, c.thumbnail, 0, 0, c.limitPerDeck, UICardMode.NumberSelected);
                        })
                        .ToList();
    }
    
    void DidClickOnCard(CardObject card) {
        cardsInDeckList.AddCard(card);
    }

    void DidLoadDeck(CardObject[] cards) {
        foreach (CardObject card in cards) {
            allCardsListView.EnableCard(card, false);
        }
    }

    void DidRemoveCard(CardObject card) {
        allCardsListView.EnableCard(card, true);
    }

    public void GoBackToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
