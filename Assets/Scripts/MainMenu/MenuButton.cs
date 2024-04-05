using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Color colorEnabled;
    public Color colorDisabled;

    TextMeshProUGUI label;
    Button button;

    public UnityEvent<bool> wasHovered;

    void Awake() {
        label = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        wasHovered.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        wasHovered.Invoke(false);
    }

    public void SetEnabled(bool enable) {
        label.color = enable ? colorEnabled : colorDisabled;
        button.interactable = enable;
    }
}
