using UnityEngine;

[CreateAssetMenu]
public class CardData: ScriptableObject {

    public string objName;

    [Tooltip("Full name")]
    public string fullName;
    
    public Faction faction;

    public string CARD_ID;
    public int productionID;

    public int level;
    public Battalion battalion;

    public string subtitle;

    public string description;
    public string abilityDescription;

    public bool isHero;
    
    public UnitAbility ability;

    public CardEffect effect;

    public int targetUpgraded;

    public Texture2D texture2D;
    public Texture2D thumbnail;

    public Material materialFront;
    public Material materialBack;

}