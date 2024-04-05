using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkErrorPopup : MonoBehaviour
{
    public TextMeshProUGUI errorDescription;

    Button button;

    void Awake() {
        button = GetComponentInChildren<Button>();
    }

    void Start() {
        button.onClick.AddListener(ButtonClicked);
    }

    public void SetErrorDescription(string description) {
        errorDescription.text = "Description:" + description;
    }

    void ButtonClicked() {
        NetworkHelper.networkError = "";
    }
}
