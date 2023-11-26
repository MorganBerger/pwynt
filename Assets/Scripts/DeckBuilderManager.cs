using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Data;
using UnityEditor;
using TMPro;

public class DeckBuilderManager : MonoBehaviour
{
    public GameObject allCardsPanel;
    public GameObject deckPanel;

    public GameObject cardDetailPanel;

    public GameObject cardList;

    UICardList leftListView;
    UICardList rightListView;

    List<CardObject> allCardsList = new List<CardObject>();
    List<CardObject> storageList;
    List<CardObject> deckList;

    Object[] cards;

    bool needsRefresh = false;

    public GameObject storageLabel;
    public GameObject deckLabel;
    public GameObject errorLabel;
    TextMeshProUGUI storageText;
    TextMeshProUGUI deckText;
    TextMeshProUGUI errorText;

    const int maxNumberInDeck = 40;
    const int minNumberInDeck = 25;

    /// <summary>
    /// LifeCycle
    /// </summary>
    void Awake() {
        Setup();
    }

    void Setup() {
        // cards = Resources.LoadAll("Images/Cards/", typeof(Texture2D));
        cards = Globals.AllCardsObjects;//Resources.LoadAll("Prefabs/CardsObjects/success/", typeof(Card));

        // CreateAllCards();
        CreateAllCards2();

        leftListView = allCardsPanel.GetComponentInChildren<UICardList>();
        rightListView = deckPanel.GetComponentInChildren<UICardList>();

        if (leftListView != null) {
            SetStorageList();
            leftListView.SetCards(storageList);
            needsRefresh = true;
        }

        storageText = storageLabel.GetComponent<TextMeshProUGUI>();
        deckText = deckLabel.GetComponent<TextMeshProUGUI>();
        errorText = errorLabel.GetComponent<TextMeshProUGUI>();
    }


    void Update() {
        if (needsRefresh) {
            needsRefresh = false;
            SetDeckList();
            rightListView.SetCards(deckList);
            UpdateText();
        }
    }

    void UpdateText() {
        storageText.text = "All cards (" + allCardsList.Count + ")";
        deckText.text = "Deck (" + deckList.Count + ")";

        if (deckList.Count > maxNumberInDeck) {
            errorText.text = " - Max. number of cards exceeded (" + maxNumberInDeck + ").";
        } else if (deckList.Count < minNumberInDeck) {
            errorText.text = "- Min. number of cards required (" + minNumberInDeck + ").";
        } else {
            errorText.text = "";
        }
    }

    /// <summary>
    /// Setters for filtered lists
    /// </summary>
    void SetStorageList() {
        var list = new List<CardObject>();
        foreach (var card in allCardsList) {
            list.Add(card);
        }
        storageList = list;
    }
    void SetDeckList() {
        var list = new List<CardObject>();
        foreach (var card in allCardsList) {
            if (card.numberInDeck > 0) {
                for (int i = 0; i < card.numberInDeck; i++) {
                    var newCard = new CardObject(
                        card.ID, 
                        card.name, 
                        card.texture2D, 
                        card.numberInDeck,
                        card.numberSelected, 
                        card.limitInDeck, 
                        card.mode
                    );
                    newCard.mode = UICardMode.Toggle;
                    newCard.numberSelected = 0;
                    list.Add(newCard);
                }
            }
        }
        deckList = list;
    }


    /// <summary>
    /// User intents.
    /// </summary>
    public void AddClicked() {
        foreach(var card in allCardsList) {
            if (card.numberSelected > 0) {
                card.numberInDeck += card.numberSelected;
                card.numberSelected = 0;
            }
        }
        needsRefresh = true;
    }

    public void RemoveClicked() {
        foreach(var card in deckList) { 
            if (card.numberSelected > 0) {
                var target = allCardsList.First(c => card.ID == c.ID);
                target.numberInDeck--;
            }
        }
        needsRefresh = true;
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


    /// <summary>
    /// Private methods.
    /// </summary>
    void CreateAllCards2() {
        allCardsList = cards.Cast<Card>()
                            .OrderBy((card) => card.cardProductionID).Select(c => {
                            return new CardObject("" + c.cardProductionID, c.name, c.texture2D, 0, 0, c.limitPerDeck, UICardMode.NumberSelected);
                        })
                        .ToList();
    }

    void CreateAllCards() {
        foreach(Texture2D t in cards.Cast<Texture2D>()) {
            var card = new CardObject(null, t.name, t, 0, 0, 5, UICardMode.NumberSelected);
            allCardsList.Add(card);
        }
    }

    void InternalSave() {
        StorageHandler handler = new StorageHandler();
        var array = allCardsList.Select(card => {
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
        StorageHandler handler = new StorageHandler();
        var cerealList = (CardObjectCereal[])handler.LoadData("deck1");
        
        if (cerealList.Length == allCardsList.Count) {
            for (int i = 0; i < cerealList.Length; i++) {
                allCardsList[i].CopyData(cerealList[i]);
            }
        }
        needsRefresh = true;
    }
    void InternalClear() {
        foreach(var card in allCardsList) {
            card.numberInDeck = 0;
        }
        needsRefresh = true;
    }
}
