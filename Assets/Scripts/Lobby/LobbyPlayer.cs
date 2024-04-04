using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPlayer : NetworkBehaviour {

    public int playerIndex;

    CustomDropdown deckDropdown; 

    public void Start() {
        deckDropdown = GetComponentInChildren<CustomDropdown>();

        PopulateLoadDropdown();
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

    public override void OnNetworkSpawn() {
        print("Spawned player");
    }

    public override void OnNetworkObjectParentChanged(NetworkObject parentNetworkObject) {
        print("NetworkObjectParentChanged for player: " + playerIndex);
        transform.SetSiblingIndex(playerIndex);
        transform.localScale = new Vector3(1, 1, 1);
    }
}
