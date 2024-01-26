using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{
    float cardStep = 0.08f;

    internal List<Card> cardsInHand = new List<Card>();

    void Start() {
        
    }

    // Several things happen when adding cards: 
    // - First we have to move the existing cards in hand to the left to leave some space for the one we're adding.
    // - Then play the draw (rotate/move) animation for each card, from deck to hand.
    // - Then we setup the onHover listener for each card after a lilte delay (basically wait for the draw animation to finish)
    public void AddCards(List<Card> cards) {
        MoveHand(cards.Count);
        StartCoroutine(AnimateDraw(cards));
        StartCoroutine(DelaySetupHover(cards));
    } 

    // Play the 
    private void MoveHand(int newCardsCount) {
        var distanceToMove = cardStep * newCardsCount / 2;

        if (cardsInHand.Count == 0)
            distanceToMove -= cardStep / 2;
        
        var oldpos = transform.localPosition;
        var newPos = new Vector3(oldpos.x + distanceToMove, oldpos.y, oldpos.z);
        StartCoroutine(CardAnimation.MoveTo(transform, newPos, .4f));
    }

    float lastY = 0;
    private IEnumerator AnimateDraw(List<Card> cards) {
        int i = 0;

        float cardPosX = -cardStep * cardsInHand.Count;

        foreach (var card in cards) {
            card.transform.SetParent(transform);
            cardsInHand.Add(card);
            var pos = new Vector3(cardPosX, lastY - 0.0001f * i, 0);

            AnimateDraw(card, pos);

            yield return new WaitForSeconds(0.1f);
            cardPosX -= cardStep;
            i++;
        }
        lastY = -cardsInHand.Count * 0.0001f;
    }

    void AnimateDraw(Card card, Vector3 pos) {
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
            card.hoverEnabled = true;
            card.onHover.AddListener(HoverCard);
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
