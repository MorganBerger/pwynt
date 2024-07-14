using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Hand : MonoBehaviour
{
    public static IList<T> Swap<T>(IList<T> list, int indexA, int indexB) {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        return list;
    }

    public float cardStep = 0.08f;
    private float cardDepthDiff = 0.0001f;
    
    public List<CardBehaviour> cardsInHand = new List<CardBehaviour>();
    // public List<Card> cardsInHand = new List<Card>();

    // public void AddCards(List<Card> cards) {
    public void AddCards(List<CardBehaviour> cards) {

        UpdateCardsInHandPos(cards.Count, .4f);

        foreach (var card in cards) {
            cardsInHand.Add(card);
            card.transform.SetParent(transform);
        }

        StartCoroutine(UpdateNewCardsPos(cards.Count, .4f));
        // StartCoroutine(DelaySetupHover(cards));
    }

    public void TidyUpHand() {
        UpdateCardsInHandPos(0, .2f);
    }
    
    private bool CardPosNeedsUpdate(CardBehaviour card, Vector3 newPos) {
    // private bool CardPosNeedsUpdate(Card card, Vector3 newPos) {
        var cardP = card.transform.localPosition;
        return cardP.x != newPos.x || cardP.y != newPos.y;
    }

    private void UpdateCardsInHandPosExcept(int newCardsNumber, float animDuration, int except) {
        if (cardsInHand.Count == 0)
            return;
        
        var startPosX = (cardsInHand.Count + newCardsNumber - 1) * cardStep / 2;
        for (int i = 0; i < cardsInHand.Count; i++) {
            if (i == except) { continue; }
            var card = cardsInHand[i];
            var newPos = new Vector3(startPosX - cardStep * i, cardDepthDiff * i, 0);

            Move(card, newPos, animDuration);
        }
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
    // private Card _lastPlayedCard = null;
    private CardBehaviour _lastPlayedCard = null;
    [SerializeField]
    private int _lastIndexPlayed = -1;

    public CardBehaviour Play(CardBehaviour card) {
    // public Card Play(Card card) {
        if (cardsInHand.Count == 0) 
            return null;

        var index = cardsInHand.IndexOf(card);
        var resultCard = RemoveAt(index);

        _lastIndexPlayed = index;
        _lastPlayedCard = resultCard;

        return resultCard;
    }

    public bool InsertBackCard() {
        if (_lastPlayedCard == null)
            return false;
        if (_lastIndexPlayed < 0 || _lastIndexPlayed > cardsInHand.Count)
            return false;

        cardsInHand.Insert(_lastIndexPlayed, _lastPlayedCard);
        return true;
    }

    private int _lastIndexOfArrival = -1;
    private int _lastIndexOfOrigin = -1;

    public void UpdateSoloCardPos(CardBehaviour card) {
    // public void UpdateSoloCardPos(Card card) {

        var localPos = card.transform.localPosition;
        var length = cardStep * cardsInHand.Count;
        var normalizedPos = length - (localPos.x + length / 2);
        
        var indexOfArrival = (int)(normalizedPos / cardStep);
        var indexOfOrigin = cardsInHand.IndexOf(card);

        if (indexOfOrigin < 0 || indexOfArrival < 0 || indexOfArrival > cardsInHand.Count - 1) return;

        var startPosX = (cardsInHand.Count - 1) * cardStep / 2;
        var originPosX = startPosX - cardStep * indexOfOrigin;

        if (_lastIndexOfArrival != indexOfArrival) {

            print("-- '" + indexOfOrigin + "' -> '" + indexOfArrival + "' | originPos: " + originPosX);

            cardsInHand = ListExtension.Swap(cardsInHand, indexOfOrigin, indexOfArrival);

            var newPos = new Vector3(originPosX, cardDepthDiff * indexOfArrival, 0);
            var cardToSwap = cardsInHand[indexOfArrival];

            Move(cardToSwap, newPos, 0.08f);

            _lastIndexOfArrival = indexOfArrival;
        }
    }

    public void MoveCardToIndex(CardBehaviour card, int index) {
    // public void MoveCardToIndex(Card card, int index) {
        if (index < 0) return; 
        if (index > cardsInHand.Count - 1) return;

        var cardIndex = cardsInHand.IndexOf(card);
        if (cardIndex < 0) return;

        cardsInHand = ListExtension.Swap(cardsInHand, cardIndex, index);
        UpdateCardsInHandPosExcept(0, 0.15f, index);
    }

    public void MoveCard(CardBehaviour card, int steps) {
    // public void MoveCard(Card card, int steps) {
        var index = cardsInHand.IndexOf(card);

        print("card index: " + index + ", steps: " + steps);
        if (index < 0) { return; } 
        if (steps == 0) { return;}

        var targetIndex = index + steps;

        if (targetIndex < 0) {
            targetIndex = 0;
        }
        if (targetIndex > cardsInHand.Count - 1) {
            targetIndex = cardsInHand.Count - 1;
        }

        print("------- switching cards '" + index + "'  & '" + targetIndex + "'");

        cardsInHand = ListExtension.Swap(cardsInHand, index, targetIndex);

        UpdateCardsInHandPosExcept(0, 0.15f, targetIndex);
    }

    public void UndoPlay(bool animated = true) {
        if (InsertBackCard()) {
            _lastPlayedCard.transform.SetParent(transform);

            if  (animated)
                UpdateCardsInHandPos(0, .2f);
        }
    }

    // private Card RemoveAt(int index) {
    private CardBehaviour RemoveAt(int index) {
        if (index < 0 || index > cardsInHand.Count - 1)
            return null;

        var card = cardsInHand[index];

        card.transform.parent = null;
        cardsInHand.RemoveAt(index);

        UpdateCardsInHandPos(0, .4f);

        return card;
    }

    void Move(CardBehaviour card, Vector3 pos, float moveDuration) {
    // void Move(Card card, Vector3 pos, float moveDuration) {
        if (!CardPosNeedsUpdate(card, pos)) return;

        // print("card needs update: " + card.name);

        StartCoroutine(CardAnimation.RotateTo(card, Quaternion.identity, moveDuration));
        StartCoroutine(CardAnimation.MoveTo(card, pos, moveDuration));
    }

    IEnumerator SetupHover(CardBehaviour card, float animDuration) {
    // IEnumerator SetupHover(Card card, float animDuration) {
        yield return new WaitForSeconds(animDuration);
        var hoverBehaviour = card.GetComponent<HoverableObject>();

        if (hoverBehaviour != null) {
            hoverBehaviour.onHover.AddListener(HoverCard);
        }
    }

    [HideInInspector]
    // public UnityEvent<Card> CardHovered;
    public UnityEvent<CardBehaviour> CardHovered;
    [HideInInspector]
    // public UnityEvent<Card> CardUnhovered;
    public UnityEvent<CardBehaviour> CardUnhovered;

    public bool hoverEnabled = true;
    void HoverCard(GameObject cardGO, bool hover) {
        if (!hoverEnabled) { return; }

        // var card = cardGO.GetComponent<Card>();
        var card = cardGO.GetComponent<CardBehaviour>();
    
        if (hover) {
            AnimateHandHover(card);
            CardHovered.Invoke(card);
        } else {
            AnimateHandUnhover(card);
            CardUnhovered.Invoke(card);
        }
    }

    float hoverAnimationTime = .15f;
    void AnimateHandHover(CardBehaviour card) {
    // void AnimateHandHover(Card card) {

        var pos = card.transform.localPosition;
        var nextPost = new Vector3(pos.x, pos.y, -.035f);

        var draggable = card.GetComponent<DraggableObject>();
        if (!draggable.isDragging)
            StartCoroutine(CardAnimation.MoveTo(card, nextPost, hoverAnimationTime));
    }

    void AnimateHandUnhover(CardBehaviour card) {
    // void AnimateHandUnhover(Card card) {
        var pos = card.transform.localPosition;
        var nextPost = new Vector3(pos.x, pos.y, 0f);

        var draggable = card.GetComponent<DraggableObject>();
        if (!draggable.isDragging)
            StartCoroutine(CardAnimation.MoveTo(card, nextPost, hoverAnimationTime));
    }

    void UnhoverAllCards() {
        foreach (var card in cardsInHand) {
            var draggable = card.GetComponent<DraggableObject>();
            if (!draggable.isDragging) {
                var pos = card.transform.localPosition;
                var nextPost = new Vector3(pos.x, pos.y, 0f);
                StartCoroutine(CardAnimation.MoveTo(card, nextPost, hoverAnimationTime));
            }
        }
    }
}
