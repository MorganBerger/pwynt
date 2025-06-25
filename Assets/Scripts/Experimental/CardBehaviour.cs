using System.Collections;
using UnityEngine;

public class CardBehaviour: MonoBehaviour { 
    
    [SerializeField]
    private CardData _data;

    [SerializeField]
    public CardData data {
        get { return _data; }
        set {
            if (data == null || _data.productionID != value.productionID) {
                _data = value;
                _needsUpdate = true;
            }
        }
    }
    
    void Awake() {
        // UpdateMaterials();
    }

    void Start() {
        // UpdateMaterials();
    }

    bool _needsUpdate = false;
    void Update() {
        if (_needsUpdate) {
            UpdateMaterials();
        }
    }

    void UpdateMaterials() {
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = new Material[2] { data.materialFront, data.materialBack };

        renderer.materials = materials;
        _needsUpdate = false;
    }

    public bool needsUpdate = false;
    public void SetCardData(CardData cardScriptable) {
        gameObject.name = cardScriptable.objName;
        data = cardScriptable;
        UpdateMaterials();
    }

    [SerializeField]
    private GameObject particlesContainer;
    public void UseMagic() {
        if (particlesContainer == null) {
            GameObject container = Resources.Load<GameObject>("Prefabs/Particles/ParticlesContainer");

            particlesContainer = Instantiate(container, transform, false);
            particlesContainer.transform.SetParent(transform);
        }
        ParticlesContainer particles = particlesContainer.GetComponent<ParticlesContainer>();
        particles.Play();

        // Die();
        StartCoroutine(DelayDie());
    }

    IEnumerator DelayDie() {
        yield return new WaitForSeconds(0.5f);
        Die();
    }

    public void Die() {
        CardDissolve dissolve = GetComponent<CardDissolve>();
        dissolve.Die();
    }
}