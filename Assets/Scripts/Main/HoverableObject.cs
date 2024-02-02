using UnityEngine;
using UnityEngine.Events;

class HoverableObject: MonoBehaviour {
    
    public bool hoverEnabled = false;
    internal bool hovered = false;

    [HideInInspector]
    public UnityEvent<GameObject, bool> onHover;

    void OnMouseOver() {
        if (!hovered && hoverEnabled) {
            onHover.Invoke(gameObject, true);
            hovered = true;
        }
    }

    void OnMouseExit() {
        if (!hoverEnabled)
            return;
        onHover.Invoke(gameObject, false);
        hovered = false;
    }
}