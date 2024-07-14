using System.Linq;
using UnityEngine;

public class Opponent: Player {

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

        var cardData = Globals.CardScriptableForID(cardID);
        targetCard.SetCardData(cardData);

        Destroy(targetCard.GetComponent<DraggableObject>());
        // Destroy(card.GetComponent<HoverableObject>());

        _hand.Play(targetCard);

        print("Playing card : " + cardData.name + " | battalion: " + cardData.battalion);

        BoardCardRow cardRow = cardRows.First(c => c.acceptedType == cardData.battalion);
        cardRow.Play(targetCard);
    }
}