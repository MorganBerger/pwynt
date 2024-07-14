using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Dissolve : MonoBehaviour {
    // Start is called before the first frame update

    void Start() {
        SetupForDeath();
    }

    // Update is called once per frame
    void Update() {
        
    }

    Material front;
    Material back;

    Texture2D frontTexture;
    Texture2D backTexture;

    private Renderer _renderer;

    public void SetupForDeath() {
        print("Dissolve setup");

        _renderer = GetComponent<Renderer>();

        Material[] materials = _renderer.materials;

        front = materials[0];
        back = materials[1];

        frontTexture = (Texture2D)front.mainTexture;
        backTexture = (Texture2D)back.mainTexture;
    
        print("front: " + frontTexture.name + ", back:" + backTexture.name);
    }

    public void Die() {
        if (_dissolved) {
            print("solving");
            StartCoroutine(Solver());
        } else {
            print("dissolving");
            ReplaceMaterials();
            StartCoroutine(Dissolver());
        }
    }

    bool _dissolved = false;
    bool _isAnimating = false;

    bool _replaced = false;
    private void ReplaceMaterials() {
        if (_replaced) { return; }

        var newFrontMaterial = (Material)Resources.Load("VFX/DissolveVFX/cardMaterial");
        var newBackMaterial = (Material)Resources.Load("VFX/DissolveVFX/cardMaterial");

        Material[] materials = new Material[2];

        materials[0] = newFrontMaterial;
        materials[1] = newBackMaterial;

        materials[0].name = "new front";
        materials[0].name = "new back";

        print("Replaced materials | [0].texture: " + frontTexture + ", [1].texture: " + backTexture);
        
        _renderer.materials = materials;

        _renderer.materials[0].SetTexture("_texture", frontTexture);
        _renderer.materials[1].SetTexture("_texture", backTexture);
        _renderer.materials[1].SetInt("_inverted", 1);

        print(_renderer.materials[0].GetTexture("_texture").name);
        print(_renderer.materials[1].GetTexture("_texture").name);
        print(_renderer.materials[1].GetInt("_inverted"));

        _replaced = true;
    }

    public float dissolveDuration = 2.0f;
    public float _dissolveStr = 0.0f;

    public IEnumerator Solver() {
        if (_isAnimating || !_dissolved) { yield return null; }

        float elapsedTime = 0.0f;
        Material[] materials = _renderer.materials;

        _isAnimating = true;

        foreach (var m in materials) { m.SetFloat("_dissolveStr", _dissolveStr); }

        while (elapsedTime < dissolveDuration) {
            _dissolveStr = Mathf.Lerp(1, 0, elapsedTime / dissolveDuration);
            UpdateStrength();

            elapsedTime +=  Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        _dissolved = false;
        _isAnimating = false;
    }

    public IEnumerator Dissolver() {
        if (_isAnimating || _dissolved) { yield return null; }

        float elapsedTime = 0.0f;

        Material[] materials = _renderer.materials;

        Material front = materials[0];
        Material back = materials[1];

        _isAnimating = true;

        foreach (var material in materials) {
            material.SetFloat("_dissolveStr", _dissolveStr);
        }

        while (elapsedTime < dissolveDuration) {

            _dissolveStr = Mathf.Lerp(0, 1, elapsedTime / dissolveDuration);
            UpdateStrength();
    
            elapsedTime +=  Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }

        _dissolved = true;
        _isAnimating = false;

        // Destroy(_renderer.materials[0]);
        // Destroy(_renderer.materials[1]);
    }

    private void UpdateStrength() {
        foreach (var material in _renderer.materials) {
            material.SetFloat("_dissolveStr", _dissolveStr);
        }
    }
}
