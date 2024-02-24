using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    Terrains
}

public enum Effect: int {
    None,
    Weather,
    BuffLVL1,
    NerfLVL1,
    TylerRitual,
    UpgradeLVL2,
    UpgradeLVL3,
}

public static class CardEffectMethods
{
    public static Faction targetFaction;
    public static Faction Faction(Faction faction) {
        targetFaction = faction;
        return faction;
    }
}

public enum Battalion: int {
    None,
    CloseCombat,
    Archery,
    Siege
}

public enum UnitAbility: int {
    None,
    Agile,
    Medic,
    Spy,
    TightBond,
    CanUpgradeToLVL2,
    CanUpgradeToLVL3,
}

public class Card : MonoBehaviour {
    public string fullName;
    public Faction faction;

    public string CARD_ID;
    public int cardProductionID;

    public int level;
    public Battalion battalion;

    public string subtitle;

    public string description;
    public string abilityDescription;

    public bool isHero;
    
    public int limitPerDeck;

    public UnitAbility ability;

    CardEffect effect;

    public GameObject UpgradedVersion;

    public Texture2D texture2D;

    void Awake() {
        effect = GetComponent<CardEffect>();
    }

    public void CopyCardData(Card card) {
        fullName = card.fullName;
        faction = card.faction;
        CARD_ID = card.CARD_ID;
        cardProductionID = card.cardProductionID;
        level = card.level;
        battalion = card.battalion;
        subtitle = card.subtitle;
        description = card.description;
        abilityDescription = card.abilityDescription;
        isHero = card.isHero;
        limitPerDeck = card.limitPerDeck;
        ability = card.ability;
        effect = card.effect;
        UpgradedVersion = card.UpgradedVersion;
        texture2D = card.texture2D;
    }

    public void SavePrefab(string path) {

        string localPath = path + "/" + gameObject.name + ".prefab";
        print(localPath);
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        
        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
    }

    // public bool hoverEnabled = false;
    // internal bool hovered = false;

    // [HideInInspector]
    // public UnityEvent<Card, bool> onHover;

    // void OnMouseOver() {
    //     if (!hovered && hoverEnabled) {
    //         onHover.Invoke(this, true);
    //         hovered = true;
    //     }
    // }

    // void OnMouseExit() {
    //     if (!hoverEnabled)
    //         return;
    //     onHover.Invoke(this, false);
    //     hovered = false;
    // }
}