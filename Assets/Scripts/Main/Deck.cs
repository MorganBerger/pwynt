using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System;
using UnityEngine.Events;

struct ListExtension {
    public static List<T> Shuffle<T>(List<T> list)
    {
        var list2 = list;
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list2.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list2[k];
            list2[k] = list2[n];
            list2[n] = value;
        }
        return list2;
    }
}


public class Deck : MonoBehaviour
{
    List<Card> cardsInDeck = new List<Card>();
    public List<Card> filteredCards = new List<Card>();

    public GameObject handGameObject;
    Hand hand;

    void Awake() {
        hand = handGameObject.GetComponent<Hand>();
    }

    public void SetDeck(CardObjectCereal[] cereals) {
        print("cereals: " + cereals.Length);
        foreach (var card in cereals) {
            if (card.numberInDeck == 0) { continue; }
                        
            var obj = (GameObject)Resources.Load("Cards/3Ds/" + card.name, typeof(GameObject));

            var cardWithStats = Globals.AllCardsObjects
                                .Cast<Card>()
                                .First(c => c.cardProductionID + "" == card.ID);

            if (cardWithStats == null) { continue; }
            if (obj == null) { continue; }
            
            for (int y = 0; y < card.numberInDeck; y++) {
                var cardObj = Instantiate(obj);
                cardObj.name = obj.name;
                var cardComponent = cardObj.GetComponent<Card>();
                cardComponent.CopyCard(cardWithStats);

                cardObj.transform.SetParent(transform);
                cardObj.transform.localPosition = Vector3.zero;
                cardObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                cardsInDeck.Add(cardComponent);
            }
        }
        filteredCards = ListExtension.Shuffle(cardsInDeck);
        drawIndex = filteredCards.Count - 1;
        SetPositions();
    }

    [HideInInspector]
    public UnityEvent OnDeckClick;
    void OnMouseDown() {
        OnDeckClick.Invoke();
    }

    void SetPositions() {
        int i = 0;
        float cardDepth = 0.000685f;
        foreach (var card in filteredCards) {
            card.gameObject.transform.localPosition = new Vector3(0, i * cardDepth, 0);
            i++;
        }
    }

    Card DrawSingleCard() {
        if (drawIndex > -1) {
            drawIndex--;
            return filteredCards[drawIndex + 1];
        }
        return null;
    }

    public int drawIndex;
    public List<Card> DrawCards(int nb) {
        var list = new List<Card>();
        
        for (int i = 0; i < nb; i++) {
            var card = DrawSingleCard();
            if (card != null) 
                list.Add(card);
        }
        return list;
    }
}
