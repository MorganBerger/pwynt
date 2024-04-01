using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Data;
using UnityEditor;
using TMPro;

public class DeckBuilderManager : MonoBehaviour {
    public GameObject allCardsContainer;
    public GameObject cardsInDeckContainer;

    UICardList allCardsListView;
    CardsInDeckList cardsInDeckList;

    List<CardObject> allCards = new List<CardObject>();
    TMP_InputField deckNameTextfield;

    const int maxNumberInDeck = 40;
    const int minNumberInDeck = 25;

    void Awake() {
        Setup();
    }

    void Setup() {
        allCardsListView = allCardsContainer.GetComponentInChildren<UICardList>();
        cardsInDeckList = cardsInDeckContainer.GetComponent<CardsInDeckList>();

        allCardsListView.didClickOnCard.AddListener(DidClickOnCard);
        cardsInDeckList.didRemoveCardFromDeck.AddListener(DidRemoveCard);

        CreateAllCards();

        if (allCardsListView != null) {
            allCardsListView.SetCards(allCards);
        }
    }
    
    void DidClickOnCard(CardObject card) {
        cardsInDeckList.AddCard(card);
    }

    void DidRemoveCard(CardObject card) {
        allCardsListView.EnableCard(card);
    }

    void CreateAllCards() {
        allCards = Globals.AllCardsObjects.Cast<Card>()
                            .OrderBy((card) => card.cardProductionID).Select(c => {
                            return new CardObject("" + c.cardProductionID, c.name, c.fullName, c.level, c.texture2D, c.thumbnail, 0, 0, c.limitPerDeck, UICardMode.NumberSelected);
                        })
                        .ToList();
    }


    public void Save() {      
        InternalSave();
    }

    public void Load() {
        InternalLoad();
    }

    public void ClearDeck() {
        InternalClear();
    }

    void InternalSave() {
        StorageHandler handler = new StorageHandler();
        var array = allCards.Select(card => {
            return new CardObjectCereal(
                card.ID,
                card.name,
                card.numberInDeck, 
                card.numberSelected, 
                card.limitInDeck, 
                card.mode
            );
        }).ToArray();
        handler.SaveData(array, "deck1");
    }
    void InternalLoad() {
        // StorageHandler handler = new StorageHandler();
        // var cerealList = (CardObjectCereal[])handler.LoadData("deck1");
        
        // if (cerealList.Length == allCards.Count) {
        //     for (int i = 0; i < cerealList.Length; i++) {
        //         allCards[i].CopyData(cerealList[i]);
        //     }
        // }
    }
    void InternalClear() {
        // foreach(var card in allCards) {
        //     card.numberInDeck = 0;
        // }
    }
}
