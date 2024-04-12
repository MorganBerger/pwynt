using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyPlayer : NetworkBehaviour {

    public int playerIndex;

    CustomDropdown deckDropdown;

    Toggle readyToggle;

    public TextMeshProUGUI playerNameLabel;

    NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>();
    public NetworkVariable<FixedString64Bytes> DeckName = new NetworkVariable<FixedString64Bytes>();

    public NetworkVariable<bool> IsReady = new NetworkVariable<bool>();

    public void Start() {
        deckDropdown = GetComponentInChildren<CustomDropdown>();

        readyToggle = GetComponentInChildren<Toggle>();
        readyToggle.interactable = false;

        if (IsOwner) {
            PopulateLoadDropdown();

            deckDropdown.onValueChanged.AddListener(DeckValueChanged);
            readyToggle.onValueChanged.AddListener(OnReadyToggleValueChanged);

            SetName();
        } else {
            deckDropdown.interactable = false;
        }
    }

    void OnReadyToggleValueChanged(bool value) {
        SubmitIsReadyRpc(value);
        if (IsOwner) {
            deckDropdown.interactable = !value;
        }
    }

    public override void OnNetworkSpawn() {}

    void DeckValueChanged(int index) {
        readyToggle.interactable = true;

        var value = index - 1;
        var decks = DeckStorageHandler.ListSavedDecks();

        if (value >= 0 && value < decks.Length) {
            var deck = decks[value];

            GameHelper.chosenDeck = deck;

            SubmitDeckChangeRpc(deck);
        }
    }

    public override void OnNetworkObjectParentChanged(NetworkObject parentNetworkObject) {
        print("NetworkObjectParentChanged for player: " + playerIndex);
        transform.SetSiblingIndex(playerIndex);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void ClearDropdown() {
        deckDropdown.options.Clear();
        deckDropdown.RemoveAllCustomization();
    }

    void PopulateLoadDropdown() {
        ClearDropdown();

        List<TMP_Dropdown.OptionData> optionsData = new List<TMP_Dropdown.OptionData>();

        var decks = DeckStorageHandler.ListSavedDecks();

        optionsData.Add(new TMP_Dropdown.OptionData("none"));

        if (decks.Length == 0) {
            var option = new TMP_Dropdown.OptionData("No saved decks...");
            optionsData.Add(option);
            deckDropdown.AddCustomization(0, false, false, FontStyles.Italic);
        }
        
        foreach (string deck in decks) {
            var option = new TMP_Dropdown.OptionData(deck);
            optionsData.Add(option);
        }
        deckDropdown.AddOptions(optionsData);
    }

    public void SetName() {
        var key = Globals.PlayerPrefsKey.playerName;
        SubmitNameChangeRpc(PlayerPrefs.GetString(key));
    }

    [Rpc(SendTo.Server)]
    void SubmitIsReadyRpc(bool ready, RpcParams rpcParams = default) {
        IsReady.Value = ready;
    }

    [Rpc(SendTo.Server)]
    void SubmitDeckChangeRpc(string deck, RpcParams rpcParams = default) {
        DeckName.Value = deck;
    }

    [Rpc(SendTo.Server)]
    void SubmitNameChangeRpc(string playerName, RpcParams rpcParams = default) {
        playerNameLabel.text = playerName;
        PlayerName.Value = playerName;
    }

    void Update() {
        playerNameLabel.text = PlayerName.Value.ToString();

        var deckName = DeckName.Value.ToString();
        if (deckName.Length > 0) {
            deckDropdown.ShowMainLabel(true);
            deckDropdown.captionText.text = deckName;
        }

        readyToggle.SetIsOnWithoutNotify(IsReady.Value);
    }

    public void StartGame() {
        // print("GAME START!");
        // GameHelper.chosenDeck = DeckName.Value.ToString();
        // print("chosen deck = " + GameHelper.chosenDeck);
        SceneManager.LoadScene("SampleScene");
    }
}
