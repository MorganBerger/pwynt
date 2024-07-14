using UnityEngine;

[ExecuteInEditMode]
public class CardBehaviour: MonoBehaviour { 
    
    [SerializeField]
    private CardData _data;

    [SerializeField]
    public CardData data {
        get { return _data; }
        set {
            if (data == null || _data.productionID != value.productionID) {
                _data = value;
                needsUpdate = true;
            }
        }
    }

    void Update() {
        if (needsUpdate) {
            UpdateMaterials();
            needsUpdate = false;
        }
    }

    void UpdateMaterials() {
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = new Material[2] { data.materialFront, data.materialBack };

        renderer.materials = materials;
    }

    public bool needsUpdate = false;
    public void SetCardData(CardData cardScriptable) {
        gameObject.name = cardScriptable.objName;
        data = cardScriptable;
        needsUpdate = true;
    }
}