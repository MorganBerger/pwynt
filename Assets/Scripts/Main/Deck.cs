using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;

public struct ListExtension {
    public static List<T> Shuffle<T>(List<T> list) {
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

    public static List<T> Swap<T>(List<T> list, int indexA, int indexB) {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
        return list;
    }

}

public class Deck : MonoBehaviour
{
    List<CardBehaviour> cardsInDeck = new List<CardBehaviour>();
    public List<CardBehaviour> shuffledDeck = new List<CardBehaviour>();

    private GameObject _cardPrefab;
    private void Start() {
        _cardPrefab = Globals.cardPrefab;
    }

    private void instantiateCard(int cardId) {
        GameObject cardGO = Instantiate(_cardPrefab);
        CardBehaviour cardBehaviour= cardGO.GetComponent<CardBehaviour>();

        CardData newCardData = Globals.CardScriptableForID(cardId);
        cardBehaviour.SetCardData(newCardData);
        
        cardsInDeck.Add(cardBehaviour);
    }

    private void Organise() {
        shuffledDeck = ListExtension.Shuffle(cardsInDeck);
        drawIndex = shuffledDeck.Count - 1;
        
        SetPositions();
    }

    public void SetBlankDeck(int numberOfCards) {
        for (int i = 0; i < numberOfCards; i++) {
            instantiateCard(0); // '0.BlankCard'.productionID = 0
        }
        Organise();
    }

    public void SetDeck(int[] deck) {
        foreach (var cardId in deck) {
            instantiateCard(cardId);
        }
        Organise();
    }

    [HideInInspector]
    public UnityEvent OnDeckClick;
    void OnMouseDown() {
        OnDeckClick.Invoke();
    }

    private void SetPositions() {
        int i = 0;
        float cardDepth = 0.000685f;
        
        foreach (var card in shuffledDeck) {
            card.transform.SetParent(transform);
            card.transform.localPosition = new Vector3(0, i * cardDepth, 0);
            card.transform.localRotation = Quaternion.Euler(0, 0, 180);
            i++;
        }
    }

    CardBehaviour DrawSingleCard() {
        if (drawIndex > -1) {
            drawIndex--;
            return shuffledDeck[drawIndex + 1];
        }
        return null;
    }

    public int drawIndex;
    public List<CardBehaviour> DrawCards(int nb) {
        var list = new List<CardBehaviour>();
        
        for (int i = 0; i < nb; i++) {
            var card = DrawSingleCard();
            if (card != null) {
                list.Add(card);
            }
        }
        return list;
    }
}
