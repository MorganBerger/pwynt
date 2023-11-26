using UnityEngine;

public static class Globals {
    public static GameObject prefabs = (GameObject)Resources.Load("Prefabs/CardsUI/UICard", typeof(GameObject));
    public static GameObject prefabsYO = (GameObject)Resources.Load("Prefabs/Test/Card", typeof(GameObject));
    public static Object[] AllCardsObjects = Resources.LoadAll("Prefabs/CardsObjects/success/", typeof(Card));
}
