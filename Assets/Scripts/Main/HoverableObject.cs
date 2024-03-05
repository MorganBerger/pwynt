using UnityEngine;
using UnityEngine.Events;

class HoverableObject: MonoBehaviour {
    
    internal bool hovered = false;
    public bool isEnabled = true;

    [HideInInspector]
    public UnityEvent<GameObject, bool> onHover;

    void OnMouseOver() {
        if (!isEnabled)
            return;

        if (!hovered) {
            print("Hoverable object '" + name + "' has been hovered.");
            hovered = true;
            onHover.Invoke(gameObject, true);
        }
    }

    // public void TriggerOnMouseExit() {
    //     OnMouseExit();
    // }
    void OnMouseExit() {
        if (!isEnabled)
            return;

        print("Hoverable object '" + name + "' has been unhovered.");
        onHover.Invoke(gameObject, false);
        hovered = false;
    }
}