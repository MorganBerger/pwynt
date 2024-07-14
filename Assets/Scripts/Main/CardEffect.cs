using System;
using UnityEngine;

[Serializable]
public struct CardEffectData {
    public Effect effect;
    public Faction targetFaction;

    public CardEffectData(CardEffect obj) {
        effect = obj.effect;
        targetFaction = obj.targetFaction;
    }

    public CardEffectData(Effect e, Faction target) {
        effect = e;
        targetFaction = target;
    }
}

public class CardEffect : MonoBehaviour {
    public Effect effect;
    public Faction targetFaction;
}