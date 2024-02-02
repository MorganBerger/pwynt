using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hand2 : MonoBehaviour
{
    public float cardStep = 0.08f;
    private float cardDepthDiff = 0.0001f;
    
    internal List<Card> cardsInHand = new List<Card>();

    public void AddCards(List<Card> cards) {

        UpdateCardsInHandPos(cards.Count);

        foreach (var card in cards)
        {
            cardsInHand.Add(card);
            card.transform.SetParent(transform);
        }

        StartCoroutine(UpdateNewCardsPos(cards.Count));
        StartCoroutine(DelaySetupHover(cards));
    }

    private void UpdateCardsInHandPos(int newCardsNumber) {
        var funcStr = "- UpdateCardsInHandPos(int newCardsNumber = " + newCardsNumber + "): ";

        if (cardsInHand.Count == 0) { 
            print(funcStr + "no cards in hands, aborting.");
            return;
        }

        var startPosX = (cardsInHand.Count + newCardsNumber - 1) * cardStep / 2;

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            var card = cardsInHand[i];
            var newPos = new Vector3(startPosX - cardStep * i, -cardDepthDiff * i, 0);

            Move(card, newPos);
        }
    }

    IEnumerator UpdateNewCardsPos(int newCardsCount) {
        var funcStr = "- UpdateNewCardsPos(List<Card> newCards [count: " + newCardsCount + "]): ";
        if (cardsInHand.Count == 0) {
            print(funcStr + "no cards in hand, should not happen. Aborting.");
            yield return null;
        }

        var oldNumberOfCards = cardsInHand.Count - newCardsCount;
        var startPosX = (cardsInHand.Count - 1) * cardStep / 2;

        for (int i = oldNumberOfCards; i < cardsInHand.Count; i++)
        {
            var card = cardsInHand[i];
            var newPos = new Vector3(startPosX - cardStep * i, -cardDepthDiff * i, 0);

            Move(card, newPos);
            yield return new WaitForSeconds(0.05f);
        }
    }


    [SerializeField]
    private Card _lastPlayedCard = null;
    [SerializeField]
    private int _lastIndexPlayed = -1;

    public void UndoPlay() {
        print("Undoing play");
        if (_lastPlayedCard == null)
            return;
        print("Last played card good");
        _lastPlayedCard.transform.SetParent(transform);

        if (_lastIndexPlayed < 0 || _lastIndexPlayed > cardsInHand.Count)
            return;
        print("Good index");
        
        cardsInHand.Insert(_lastIndexPlayed, _lastPlayedCard);
        UpdateCardsInHandPos(0);
    }

    public Card Play(Card card) {
        if (cardsInHand.Count == 0) 
            return null;

        var index = cardsInHand.IndexOf(card);
        var resultCard = RemoveAt(index);

        _lastIndexPlayed = index;
        _lastPlayedCard = resultCard;

        return resultCard;
    }

    private Card RemoveAt(int index) {
        if (index < 0 || index > cardsInHand.Count - 1){ 
            return null;
        }

        var card = cardsInHand[index];

        card.transform.parent = null;
        cardsInHand.RemoveAt(index);

        UpdateCardsInHandPos(0);

        return card;
    }

    void Move(Card card, Vector3 pos) {
        StartCoroutine(CardAnimation.RotateTo(card.transform, Quaternion.identity, .4f));
        StartCoroutine(CardAnimation.MoveTo(card.transform, pos, .4f));
    }

    // Setup hover listeners when cards are added to hand.
    IEnumerator DelaySetupHover(List<Card> cards) {
        yield return new WaitForSeconds(0.1f * cards.Count + 0.5f);
        SetupHover(cards);
    }

    void SetupHover(List<Card> cards) {
        foreach (var card in cards) {
            // card.hoverEnabled = true;
            // card.onHover.AddListener(HoverCard);
        }
    }

    [HideInInspector]
    public UnityEvent<Card> CardHovered;
    [HideInInspector]
    public UnityEvent UIShouldHide;

    void HoverCard(Card card, bool hover) {
        if (hover) {
            CardHovered.Invoke(card);
        } else {
            UIShouldHide.Invoke();
        }
    }
}
