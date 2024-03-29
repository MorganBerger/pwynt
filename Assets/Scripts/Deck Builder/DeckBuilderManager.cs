using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Data;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Collections;
// using System.Numerics;

public class DeckBuilderManager : MonoBehaviour
{
    public GameObject allCardsContainer;
    public GameObject cardsInDeckContainer;

    UICardList allCardsListView;

    List<CardObject> allCards = new List<CardObject>();

    const int maxNumberInDeck = 40;
    const int minNumberInDeck = 25;

    void Awake() {
        Setup();
    }

    void Setup() {
        allCardsListView = allCardsContainer.GetComponentInChildren<UICardList>();
        allCardsListView.didClickOnCard.AddListener(DidClickOnCard);

        CreateAllCards();

        if (allCardsListView != null) {
            allCardsListView.SetCards(allCards);
        }
    }

    public ScrollRect scrollRect;

    bool shouldScrollToBottom = false;

    void DidClickOnCard(CardObject card) {
        print("clicked on '" + card.name + "'");
        GameObject cardIndeckPrefab = (GameObject)Resources.Load("Prefabs/CardsUI/CardInDeckRow", typeof(GameObject));
        
        GameObject cardInDeckObject = Instantiate(cardIndeckPrefab);
        CardInDeckRow cardInDeckRow = cardInDeckObject.GetComponent<CardInDeckRow>();

        cardInDeckRow.transform.SetParent(cardsInDeckContainer.transform);
        cardInDeckRow.transform.localScale = Vector3.one;
        cardInDeckRow.transform.localPosition = Vector3.zero;

        cardInDeckRow.SetCard(card);

        // shouldScrollToBottom = true;
        // scrollRect.verticalNormalizedPosition = 0;
        // var scroller = scrollView.verticalScroller;
        // scroller.value = scroller.highValue > 0 ? scroller.highValue : 0;
        // scrollView.ScrollTo();
        StartCoroutine(waitToScroll());
    }

    IEnumerator waitToScroll() {
        yield return new WaitForEndOfFrame();
        // yield return new WaitForEndOfFrame
        scrollRect.verticalNormalizedPosition = 0;
    }

    // void LateUpdate() {
    //     if (shouldScrollToBottom) {
    //         shouldScrollToBottom = false;
    //         scrollRect.verticalNormalizedPosition = 0;
    //     }
    // }

    void Update() {

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
