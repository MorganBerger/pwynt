using System;

[Serializable]
public struct CardJSONObject {
    public string fullName;
    
    public Faction faction;

    public string CARD_ID;
    public int productionID;

    public int level;
    public Battalion battalion;

    public string subtitle;

    public string description;
    public string abilityDescription;

    public bool isHero;
    
    public UnitAbility ability;

    public CardEffectData effect;

    public int targetUpgraded;

    public CardJSONObject(CardData card) {
        fullName = card.fullName;
        faction = card.faction;
        CARD_ID = card.CARD_ID;
        productionID = card.productionID;
        level = card.level;
        battalion = card.battalion;
        subtitle = card.subtitle;
        description = card.description;
        abilityDescription = card.abilityDescription;
        isHero = card.isHero;
        ability = card.ability;

        // effect = card.effect;
        if (card.effect != null) {
            effect = new CardEffectData(card.effect);
        } else {
            effect = new CardEffectData(Effect.None, Faction.None);
        }

        targetUpgraded = card.targetUpgraded;
    }
}