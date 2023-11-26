using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public enum UICardMode: int {
    Toggle,
    NumberSelected
}

public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    TextMeshProUGUI textMesh;
    Button button;
    Image image;
    GameObject panel;

    bool mouse_over = false;
    public void OnPointerEnter(PointerEventData eventData) {
        mouse_over = true;
        print("Hover: " + card.name);
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouse_over = false;
        Debug.Log("un-Hover: " + card.name);
    }

    public CardObject card;

    void Awake() {
        Setup();
    }
    void Update() {
        var text = "" + card.numberSelected + "";
        if (card.mode == UICardMode.Toggle) {
            text = "X";
        }
        textMesh.text = text;
        panel.SetActive(card.numberSelected > 0);

        UpdateTexture();
    }

    void OnMouseOver() {
        
    }

    void Setup() {
        foreach (Transform t in transform) {
            panel = t.gameObject;
            break;
        }
        textMesh = panel.GetComponentInChildren<TextMeshProUGUI>();

        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void UpdateTexture() {
        var texture = card.texture2D;
        if (texture != null && image.mainTexture != texture) {
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0.5f));
        }
    }

    public void onLeftClick() {
        if (card.numberSelected < card.limit) {
            card.numberSelected++;
        }
    }

    public void onRightClick() {
        var go = button.gameObject;
        var ped = new PointerEventData(EventSystem.current); 
        ExecuteEvents.Execute(go, ped, ExecuteEvents.pointerEnterHandler); ExecuteEvents.Execute(go, ped, ExecuteEvents.submitHandler); 

        if (card.numberSelected > 0) {
            card.numberSelected--;
        }
    }

}