using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardScriptableList: ScriptableObject {
    public List<CardData> content = new List<CardData>();

    // public CardScriptableList() {
        // content = new List<CardData>();
    // }

    // public void Add(Card card) {
        // content.Add(new CardScriptable(card));
    // }
}