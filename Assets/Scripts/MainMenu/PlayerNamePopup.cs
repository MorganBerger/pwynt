using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerNamePopup : MonoBehaviour {
    TMP_InputField nameTextfield;

    public Button confirmButton;

    public UnityEvent didChooseName;

    void Awake() {
        nameTextfield = GetComponentInChildren<TMP_InputField>();
    }

    // Start is called before the first frame update
    void Start() { 
        nameTextfield.onValueChanged.AddListener(NameDidChange);
        confirmButton.onClick.AddListener(DidClickConfirm);
    }

    void NameDidChange(string name) {
        confirmButton.interactable = name.Length > 0;
    }

    void DidClickConfirm() {
        PlayerPrefs.SetString(Globals.PlayerPrefsKey.playerName, nameTextfield.text);
        didChooseName.Invoke();
    }


}
