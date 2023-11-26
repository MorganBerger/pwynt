using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class ListExtension {
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

    public void SetDeck(CardObjectCereal[] cereals) {
        foreach (var card in cereals) {
            if (card.numberInDeck == 0) { continue; }
            
            print(card.name);
            
            var obj = (GameObject)Resources.Load("Cards/3Ds/" + card.name, typeof(GameObject));
            var cardWithStats = Globals.AllCardsObjects
                                .Cast<Card>()
                                .First(c => c.cardProductionID + "" == card.ID);

            if (cardWithStats == null) { continue; }
            if (obj == null) { continue; }
            
            for (int y = 0; y < card.numberInDeck; y++) {
                var cardObj = Instantiate(obj);
                var cardComponent = cardObj.GetComponent<Card>();
                cardComponent.CopyCard(cardWithStats);
                cardObj.transform.SetParent(transform);
                cardObj.transform.localPosition = Vector3.zero;
                cardObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                cardsInDeck.Add(cardComponent);
            }
        }
        filteredCards = ListExtension.Shuffle(cardsInDeck);
        SetPositions();
    }

    void SetPositions() {
        int i = 0;
        foreach (var card in filteredCards) {
            card.gameObject.transform.localPosition = new Vector3(0, 0.1f + i * 0.000685f, 0);
            i++;
        }
    }

    void Awake() {

    }

    void Shuffle(List<Card> cardsInDeck) {

    }

    Card Draw() {
        return null;
    }
}
