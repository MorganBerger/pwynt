using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardDieStyleList: ScriptableObject {
    public List<CardDieStyle> styles = new List<CardDieStyle>();

    public CardDieStyle CardDestroyedStyle() {
        return styles[0];
    }
    public CardDieStyle MagicUsedStyle() {
        return styles[1];
    }
}