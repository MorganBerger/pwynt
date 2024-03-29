using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public enum UICardMode: int {
    Toggle,
    NumberSelected
}

public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    Button button;
    Image image;

    CardObject card;
    public UnityEvent<CardObject> didClickOnCard;

    bool mouse_over = false;
    public void OnPointerEnter(PointerEventData eventData) {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouse_over = false;
    }

    void Awake() {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }

    void Update() {
        // UpdateTexture();
    }

    public void SetCard(CardObject card) {
        this.card = card;
        UpdateTexture();
    }

    public void UpdateTexture() {
        var texture = card.texture2D;
        if (texture != null && image.mainTexture != texture) {
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0.5f));
        }
    }

    public void onClick() {
        button.interactable = false;
        didClickOnCard.Invoke(card);
    }
}