using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrefabCard : MonoBehaviour
{
    TextMeshProUGUI textComponent;

    void Awake() {
        textComponent = GetComponent<TextMeshProUGUI>();
    }
    public void SetText(string text) {
        textComponent.text = text;
    }
}
