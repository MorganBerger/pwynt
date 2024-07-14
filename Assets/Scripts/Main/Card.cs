using System;
// using UnityEditor;
// using UnityEngine;

[Serializable]
public enum Faction: int {
    None,
    Founders,
    Tylers,
    Bards,
    Rulers,
    Menstrels,
    Artists,
    Robots,
    Architects,
    Hybrids,
    Companions,
    Effects,
    Terrains,
    Creature,
    All
}

[Serializable]
public enum Effect: int {
    None,
    Weather,
    BuffLVL1,
    NerfLVL1,
    TylerRitual,
    UpgradeLVL2,
    UpgradeLVL3,
}

// public static class CardEffectMethods
// {
//     public static Faction targetFaction;
//     public static Faction Faction(Faction faction) {
//         targetFaction = faction;
//         return faction;
//     }
// }

[Serializable]
public enum Battalion: int {
    None,
    CloseCombat,
    Archery,
    Siege
}

[Serializable]
public enum UnitAbility: int {
    None,
    Agile,
    Medic,
    Spy,
    TightBond,
    CanUpgradeToLVL2,
    CanUpgradeToLVL3,
}

// public class Card : MonoBehaviour {
    
//     public bool animating = false;

//     public string fullName;
//     public Faction faction;

//     public string CARD_ID;
//     public int productionID;

//     public int level;
//     public Battalion battalion;

//     public string subtitle;

//     public string description;
//     public string abilityDescription;

//     public bool isHero;
    
//     public int limitPerDeck;

//     public UnitAbility ability;

//     public CardEffect effect;

//     public GameObject UpgradedVersion;
//     public int targetUpgraded = -1;

//     public Texture2D texture2D;
//     public Texture2D thumbnail;

//     void Awake() {
//         // animating = false;
//         effect = GetComponent<CardEffect>();
//     }

//     #if UNITY_EDITOR
//     public void SavePrefab(string path) {

//         string localPath = path + "/" + gameObject.name + ".prefab";
//         print(localPath);
//         localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        
//         bool prefabSuccess;
//         PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out prefabSuccess);
//         if (prefabSuccess == true)
//             Debug.Log("Prefab was saved successfully");
//         else
//             Debug.Log("Prefab failed to save" + prefabSuccess);
//     }
//     #endif
// }
