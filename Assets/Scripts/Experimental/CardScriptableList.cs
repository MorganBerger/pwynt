using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class CardScriptableList: ScriptableObject {
    public List<CardData> content;

    public CardScriptableList() {
        content = new List<CardData>();
    }

    // public void Add(Card card) {
        // content.Add(new CardScriptable(card));
    // }
}