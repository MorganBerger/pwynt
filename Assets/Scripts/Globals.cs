using UnityEngine;
using Unity.Multiplayer.Playmode;
using System.Linq;
using System.Collections.Generic;

public static class Globals {
    public static GameObject cardUIPrefab = (GameObject)Resources.Load("Prefabs/CardsUI/UICard", typeof(GameObject));
   
   
    public static GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/CardsObjects/Card");
    public static CardBehaviour cardPrefabBehaviour = cardPrefab.GetComponent<CardBehaviour>();

    public static CardScriptableList cardsList;

    public static List<CardData> Cards {
        get {
            return cardsList.content;
        }
    }

    public static List<CardData> playableCards {
        get {
            return cardsList.content.Where(card => card.productionID != 0).ToList();
        }
    }

    public static CardData CardDataForID(int id) {
        return Cards.First(c => c.productionID == id);
    }

    public static List<CardData> CardDatasForIDs(int[] IDs) {
        return Cards.FindAll(c => IDs.Contains(c.productionID));
    }

    public struct PlayerPrefsKey {
        #if UNITY_EDITOR

        public static string playerName = (CurrentPlayer.ReadOnlyTags().Contains("main") ? "main" : "second") + "playerName";

        #else

        public static string playerName = "playerName";
        
        #endif
    }
}
