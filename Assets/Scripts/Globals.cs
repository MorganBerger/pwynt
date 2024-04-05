using UnityEngine;
using Unity.Multiplayer.Playmode;
using System.Linq;

public static class Globals {
    public static GameObject cardUIPrefab = (GameObject)Resources.Load("Prefabs/CardsUI/UICard", typeof(GameObject));
    public static GameObject prefabsYO = (GameObject)Resources.Load("Prefabs/Test/Card", typeof(GameObject));
    public static Object[] AllCardsObjects = Resources.LoadAll("Prefabs/CardsObjects/success/", typeof(Card));

    public struct PlayerPrefsKey {
        #if UNITY_EDITOR

        public static string playerName = (CurrentPlayer.ReadOnlyTags().Contains("main") ? "main" : "second") + "playerName";

        #else

        public static string playerName = "playerName";
        
        #endif
    }
}
