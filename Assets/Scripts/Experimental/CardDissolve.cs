using System.Collections;
using UnityEngine;

public class CardDissolve : MonoBehaviour {

    [SerializeField] private CardDieStyle dieStyle;

    Texture2D _frontTexture;
    Texture2D _backTexture;

    private Renderer _renderer;

    void Start() {
        InitTextures();
    }

    public void SetDieStyle(CardDieStyle style) {
        dieStyle = style;
        SetupStyle();
    }

    void SetupStyle() {
        ReplaceMaterials();
        UpdateStyle();
    }

    public void InitTextures() {
        _renderer = GetComponent<Renderer>();

        Material[] materials = _renderer.materials;
        
        _frontTexture = (Texture2D)materials[0].mainTexture;
        _backTexture = (Texture2D)materials[1].mainTexture;
    }

    bool _dissolved = false;
    public void Die() {
        if (_dissolved) {
            StartCoroutine(Solver(false));
            _dissolved = false;
        } else {
            StartCoroutine(Solver(true));
            _dissolved = true;
        }
    }
    // bool _isAnimating = false;

    bool _replaced = false;
    private void ReplaceMaterials() {
        if (_replaced) { return; }

        var newFrontMaterial = (Material)Resources.Load("VFX/DissolveVFX/dissolvematerialRound");
        var newBackMaterial = (Material)Resources.Load("VFX/DissolveVFX/dissolvematerialRound");

        Material[] materials = new Material[2];

        materials[0] = newFrontMaterial;
        materials[1] = newBackMaterial;
        
        _renderer.materials = materials;

        _renderer.materials[0].SetTexture("_texture", _frontTexture);

        _renderer.materials[1].SetTexture("_texture", _backTexture);
        _renderer.materials[1].SetInt("_inverted", 1);

        _replaced = true;
    }

    public void UpdateStyle() {
        foreach(Material mat in _renderer.materials) {
            mat.SetFloat("_dissolveBorderWidth", dieStyle.borderWidth);

            mat.SetFloat("_dissolveStr", -dieStyle.borderWidth);

            mat.SetFloat("_dissolveScale", dieStyle.dissolveScale);
            mat.SetColor("_dissolveColor", dieStyle.colour);
        }
    }
    
    public IEnumerator Solver(bool dissolve) {

        var from = dissolve ? -dieStyle.borderWidth : 1;
        var to = dissolve ? 1 : -dieStyle.borderWidth;

        float elapsedTime = 0.0f;
        while (elapsedTime < dieStyle.dissolveDuration) {

            float dissolveStr = Mathf.Lerp(from, to, elapsedTime / dieStyle.dissolveDuration);

            for (int i = 0; i < _renderer.materials.Length; i++) {
                Material material = _renderer.materials[i];
                material.SetFloat("_dissolveStr", dissolveStr);
            }

            elapsedTime +=  Time.deltaTime;
            yield return new WaitForSeconds(0.002f);
        }
    }
}
