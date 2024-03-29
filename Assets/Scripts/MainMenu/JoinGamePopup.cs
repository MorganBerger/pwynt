using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JoinGamePopup : MonoBehaviour
{
    TMP_InputField ipTextfield;

    public Button joinButton;
    public Button cancelButton;

    public UnityEvent onJoinGame;

    void Start() {
        print("Start JoinGamePopup");

        ipTextfield = GetComponentInChildren<TMP_InputField>();
        ipTextfield.onValueChanged.AddListener(onIPChanged);

        joinButton.onClick.AddListener(DidClickJoinButton);
        cancelButton.onClick.AddListener(DidClickCancelButton);
    }

    private void DidClickJoinButton() {
        var ipValue = ipTextfield.text;

        var ipArray = ipValue.Split(":");

        if (ipArray.Length == 2) {
            var ip = ipArray[0];
            var port = int.Parse(ipArray[1]);
            NetworkHelper.ip = ip;
            NetworkHelper.port = port;

            print("ip: " + NetworkHelper.ip);
            print("port: " + NetworkHelper.port);

            onJoinGame.Invoke();
        } else {
            print("Wrong ip formatting: missing `ip` or `port`");
        }
    }

    private void DidClickCancelButton() {
        ipTextfield.text = "";
    }

    private void onIPChanged(string value) {
        joinButton.interactable = value != "";
    }
}
