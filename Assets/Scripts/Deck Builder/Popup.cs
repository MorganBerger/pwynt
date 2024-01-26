
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Popup: MonoBehaviour {

    TextMeshProUGUI text;

    public UnityEvent onYes;
    public UnityEvent onNo;

    void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Yes() {
        onYes.Invoke();
    }
    public void No() {
        onNo.Invoke();
    }

    public void SetText(string txt) {
        text.text = txt;
    }
}