using System.Collections;
using System.Linq;
using UnityEngine;

public class Opponent: Player {

    public GameObject magicSlotGO;

    private void SetDeckOpponent() {
        print("setting blank deck, number: " + GameHelper.numberOfCardsInOpponentDeck);
        _deck.SetBlankDeck(GameHelper.numberOfCardsInOpponentDeck);
    }

    public override void SetDeck() {
        SetDeckOpponent();
    }

    public override void Play(int cardID) {
        PlayOpponent(cardID);
    }

    public void PlayOpponent(int cardID) {
        System.Random random = new System.Random();
        var index = random.Next(0, _hand.cardsInHand.Count);
        var targetCard = _hand.cardsInHand[index];

        var cardData = Globals.CardDataForID(cardID);
        targetCard.SetCardData(cardData);

        Destroy(targetCard.GetComponent<DraggableObject>());

        CardBehaviour playedCard = _hand.Play(targetCard);

        print("Playing card : " + cardData.name + " | battalion: " + cardData.battalion);

        BoardCardRow cardRow = cardRows.First(c => c.acceptedType == cardData.battalion);
        if (cardRow != null) {
            cardRow.Play(targetCard);
        } else {
            
        }
    }

    IEnumerator prout() {
        yield return null;
    }

    void Move(CardBehaviour card, Vector3 pos, float moveDuration) {
        StartCoroutine(CardAnimation.RotateTo(card, Quaternion.identity, moveDuration));
        StartCoroutine(CardAnimation.MoveTo(card, pos, moveDuration));
    }
}