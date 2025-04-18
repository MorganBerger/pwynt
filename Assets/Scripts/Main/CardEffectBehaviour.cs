using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardEffectBehaviour: MonoBehaviour {

    CardEffect effect;

    Faction targetFaction;

    List<CardBehaviour> cards;

    List<CardBehaviour> handCards;
    List<CardBehaviour> boardCards;

    public CardEffectBehaviour(CardBehaviour card) {
        List<CardBehaviour> targetCards = new List<CardBehaviour>();
        // card.effect = effect;

        // switch (effect) {
        //     case CardEffect.BuffLVL1:
        //         ActivateBuff(Faction.Menstrels);
        //         break;
        //     case CardEffect.NerfLVL1:
        //         ActivateNerf(Faction.Menstrels);
        //         break;
        //     case CardEffect.TylerRitual:
        //         ActivateTylerRitual();
        //         break;
        //     case CardEffect.UpgradeLVL2:
        //         ActivateUpgradeLVL2();
        //         break;
        //     case CardEffect.UpgradeLVL3:
        //         ActivateUpgradeLVL3();
        //         break;
        //     default:
        //         break;
        // }
    }

    void ActivateBuff(Faction faction)
    {

    }
    void ActivateNerf(Faction faction)
    {

    }
    void ActivateTylerRitual()
    {
        var hasHead = handCards.First(c => c.data.CARD_ID == "PW01-TY01") != null;
        var hasLeftLeg = handCards.First(c => c.data.CARD_ID == "PW01-TY02") != null;
        var hasRightLeg = handCards.First(c => c.data.CARD_ID == "PW01-TY03") != null;
        var hasThirdLeg = handCards.First(c => c.data.CARD_ID == "PW01-TY04") != null;

        if (hasHead && hasLeftLeg && hasRightLeg && hasThirdLeg) { 
            // Win game;
        }
    }
    void ActivateUpgradeLVL2()
    {

    }
    void ActivateUpgradeLVL3()
    {

    }

}