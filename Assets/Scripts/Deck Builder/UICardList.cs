using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


public class UICardList : MonoBehaviour { 
    List<CardObject> cardList = new List<CardObject>();
    List<UICard> uiCardList = new List<UICard>();

    public UnityEvent<CardObject> didClickOnCard;

    public void SetCards(List<CardObject> list) {
        cardList = list;
        UpdateList();
    }

    public void EnableCard(CardObject card, bool enabled) {
        var uiCard = uiCardList.Find(uiCard => {
            return uiCard.GetCard().ID == card.ID;
        });
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

    void DidClickOnCard(CardObject card) {
        didClickOnCard.Invoke(card);
    }

    void SetObjTransform(GameObject obj, Transform target) {     
        obj.transform.SetParent(target);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
    }
}