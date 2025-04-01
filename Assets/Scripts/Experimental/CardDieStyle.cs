using UnityEngine;

[CreateAssetMenu]
public class CardDieStyle: ScriptableObject {
    [ColorUsageAttribute(true,true)]
    public Color colour;

    public float borderWidth;
    public float dissolveScale;
    public float dissolveDuration;
}