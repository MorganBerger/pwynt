using System;
using System.Collections.Generic;

[Serializable]
public class CardDataContainer {
    public List<CardJSONObject> content;

    public CardDataContainer() {
        content = new List<CardJSONObject>();
    }

    public void Add(CardJSONObject data) {
        content.Add(data);
    }
}