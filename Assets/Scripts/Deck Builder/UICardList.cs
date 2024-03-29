using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;


public class UICardList : MonoBehaviour { 
    List<CardObject> cardList = new List<CardObject>();
    List<GameObject> viewList = new List<GameObject>();

    public UnityEvent<CardObject> didClickOnCard;

    public void SetCards(List<CardObject> list) {
        cardList = list;
        UpdateList();
    }

    void UpdateList() {
        viewList.RemoveAll(card => {
            Destroy(card);
            return true;
        });

        GameObject uiCardPrefab = (GameObject)Resources.Load("Prefabs/CardsUI/UICard", typeof(GameObject));

        foreach(var card in cardList) {
            GameObject obj = Instantiate(uiCardPrefab);
            obj.name = card.name;
            var ui = obj.GetComponent<UICard>();
            ui.SetCard(card);

            ui.didClickOnCard.AddListener(DidClickOnCard);

            SetObjTransform(obj, transform);

            viewList.Add(obj);
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