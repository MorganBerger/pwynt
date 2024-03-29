using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{
    public float cardStep = 0.08f;
    private float cardDepthDiff = 0.0001f;
    
    public List<Card> cardsInHand = new List<Card>();

    public void AddCards(List<Card> cards) {

        UpdateCardsInHandPos(cards.Count, .4f);

        foreach (var card in cards)
        {
            cardsInHand.Add(card);
            card.transform.SetParent(transform);
        }

        StartCoroutine(UpdateNewCardsPos(cards.Count, .4f));
        // StartCoroutine(DelaySetupHover(cards));
    }

    public void TidyUpHand() {
        UpdateCardsInHandPos(0, .2f);
    }

    private void UpdateCardsInHandPos(int newCardsNumber, float animDuration) {
        if (cardsInHand.Count == 0)
            return;
        
        var startPosX = (cardsInHand.Count + newCardsNumber - 1) * cardStep / 2;
        for (int i = 0; i < cardsInHand.Count; i++) {
            var card = cardsInHand[i];
            var newPos = new Vector3(startPosX - cardStep * i, cardDepthDiff * i, 0);

            Move(card, newPos, animDuration);
        }
    }

    IEnumerator UpdateNewCardsPos(int newCardsCount, float animDuration) {
        if (cardsInHand.Count == 0) {
            yield return null;
        }

        var oldNumberOfCards = cardsInHand.Count - newCardsCount;
        var startPosX = (cardsInHand.Count - 1) * cardStep / 2;

        for (int i = oldNumberOfCards; i < cardsInHand.Count; i++)
        {
            var card = cardsInHand[i];
            var newPos = new Vector3(startPosX - cardStep * i, cardDepthDiff * i, 0);

            Move(card, newPos, animDuration);

            StartCoroutine(SetupHover(card, animDuration));

            yield return new WaitForSeconds(0.05f);
        }
    }


    [SerializeField]
    private Card _lastPlayedCard = null;
    [SerializeField]
    private int _lastIndexPlayed = -1;

    public Card Play(Card card) {
        if (cardsInHand.Count == 0) 
            return null;

        var index = cardsInHand.IndexOf(card);
        var resultCard = RemoveAt(index);

        _lastIndexPlayed = index;
        _lastPlayedCard = resultCard;

        return resultCard;
    }

    public void UndoPlay() {
        if (_lastPlayedCard == null)
            return;
        if (_lastIndexPlayed < 0 || _lastIndexPlayed > cardsInHand.Count)
            return;

        _lastPlayedCard.transform.SetParent(transform);
        
        cardsInHand.Insert(_lastIndexPlayed, _lastPlayedCard);
        UpdateCardsInHandPos(0, .2f);
        // StartCoroutine(Prout(_lastPlayedCard));
    }

    private Card RemoveAt(int index) {
        if (index < 0 || index > cardsInHand.Count - 1)
            return null;

        var card = cardsInHand[index];

        card.transform.parent = null;
        cardsInHand.RemoveAt(index);

        UpdateCardsInHandPos(0, .4f);

        return card;
    }

    void Move(Card card, Vector3 pos, float moveDuration) {
        StartCoroutine(CardAnimation.RotateTo(card, Quaternion.identity, moveDuration));
        StartCoroutine(CardAnimation.MoveTo(card, pos, moveDuration));
    }

    IEnumerator SetupHover(Card card, float animDuration) {
        yield return new WaitForSeconds(animDuration);
        var hoverBehaviour = card.GetComponent<HoverableObject>();
        hoverBehaviour.onHover.AddListener(HoverCard);
    }

    // Setup hover listeners when cards are added to hand.
    // IEnumerator DelaySetupHover(List<Card> cards) {
    //     yield return new WaitForSeconds(0.1f * cards.Count + 0.4f);
    //     SetupHover(cards);
    // }

    // void SetupHover(List<Card> cards) {
    //     foreach (var card in cards) {
    //         var cardGO = card.gameObject;
    //         var hoverBehaviour = cardGO.GetComponent<HoverableObject>();
    //         hoverBehaviour.onHover.AddListener(HoverCard);
    //     }
    // }

    [HideInInspector]
    public UnityEvent<Card> CardHovered;
    [HideInInspector]
    public UnityEvent<Card> CardUnhovered;

    void HoverCard(GameObject cardGO, bool hover) {
        var card = cardGO.GetComponent<Card>();
        // if (card == null) return;
        print("Hand.HoverCard('" + cardGO.name + "') -> " + hover);

        if (hover) {
            AnimateHandHover(card);
            CardHovered.Invoke(card);
        } else {
            // UnhoverAllCards();
            AnimateHandUnhover(card);
            CardUnhovered.Invoke(card);
        }
    }

    float hoverAnimationTime = .15f;
    void AnimateHandHover(Card card) {

        var pos = card.transform.localPosition;
        var nextPost = new Vector3(pos.x, pos.y, -.035f);

        var draggable = card.GetComponent<DraggableObject>();
        if (!draggable.isDragging)
            StartCoroutine(CardAnimation.MoveTo(card, nextPost, hoverAnimationTime));
    }

    void AnimateHandUnhover(Card card) {
        var pos = card.transform.localPosition;
        var nextPost = new Vector3(pos.x, pos.y, 0f);

        var draggable = card.GetComponent<DraggableObject>();
        if (!draggable.isDragging)
            StartCoroutine(CardAnimation.MoveTo(card, nextPost, hoverAnimationTime));
    }

    void UnhoverAllCards() {
        foreach (var card in cardsInHand) {

            // if (card.animating) {
            //     print("card '" + card.name + "' is already animating. Cannot animate unhover effect.");
            //     return;
            // }

            var draggable = card.GetComponent<DraggableObject>();
            if (!draggable.isDragging) {
                var pos = card.transform.localPosition;
                var nextPost = new Vector3(pos.x, pos.y, 0f);
                StartCoroutine(CardAnimation.MoveTo(card, nextPost, hoverAnimationTime));
            }
        }
    }
}
