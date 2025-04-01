using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


public class UICardList : MonoBehaviour { 
    List<CardData> cardList = new List<CardData>();
    List<UICard> uiCardList = new List<UICard>();

    // public UnityEvent<CardObject> didClickOnCard;
    public UnityEvent<CardData> didClickOnCard;

    public void SetCards(List<CardData> list) {
        cardList = list;
        UpdateList();
    }

    // public void SetCards(List<CardObject> list) {
    //     cardList = list;
    //     UpdateList();
    // }

    public void EnableCard(CardData card, bool enabled) {
        var uiCard = uiCardList.Find(uiCard => {
            return uiCard.GetCard().productionID == card.productionID;
        });
        print("Enabling: " + card.fullName);
        uiCard.SetEnabled(enabled);
    }

    void UpdateList() {

        uiCardList.RemoveAll(card => {
            Destroy(card.gameObject);
            return true;
        });

        GameObject uiCardPrefab = (GameObject)Resources.Load("Prefabs/CardsUI/UICard", typeof(GameObject));

        foreach(var card in cardList) {
            GameObject obj = Instantiate(uiCardPrefab);
            obj.name = card.name;

            var uiCard = obj.GetComponent<UICard>();
            uiCard.SetCard(card);

            uiCard.didClickOnCard.AddListener(DidClickOnCard);

            uiCardList.Add(uiCard);

            SetObjTransform(obj, transform);
        }
    }

    void DidClickOnCard(CardData card) {
        didClickOnCard.Invoke(card);
    }
    // void DidClickOnCard(CardObject card) {
    //     didClickOnCard.Invoke(card);
    // }

    void SetObjTransform(GameObject obj, Transform target) {     
        obj.transform.SetParent(target);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
    }
}