using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GUIHoverController: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public UnityEvent<bool> onHover;

    public void OnPointerEnter(PointerEventData eventData) {
        onHover.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        onHover.Invoke(false);
    }
}