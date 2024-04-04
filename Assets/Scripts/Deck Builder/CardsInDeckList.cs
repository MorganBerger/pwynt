using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardsInDeckList : MonoBehaviour
{
    public GameObject cardContainer;
    public GameObject loadPanel;
    public GameObject savePanel;

    CustomDropdown loadDeckDropDown;
    Button clearDeckButton;

    TMP_InputField deckNameTextfield;
    Button saveDeckButton;

    [SerializeField]
    List<CardInDeckRow> cardsInDeck = new List<CardInDeckRow>();

    ScrollRect scrollView;

    public UnityEvent<CardObject[]> didLoadDeck;
    public UnityEvent<CardObject> didRemoveCardFromDeck;

    void Awake() {
        scrollView = GetComponentInChildren<ScrollRect>();

        loadDeckDropDown = loadPanel.GetComponentInChildren<CustomDropdown>();
        clearDeckButton = loadPanel.GetComponentInChildren<Button>();

        deckNameTextfield = savePanel.GetComponentInChildren<TMP_InputField>();
        saveDeckButton = savePanel.GetComponentInChildren<Button>();
    }

    void Start() {
        PopulateLoadDropdown();

        loadDeckDropDown.onValueChanged.AddListener(OnDeckLoadValueChanged);

        clearDeckButton.onClick.AddListener(ClearDeck);
        clearDeckButton.interactable = false;

        deckNameTextfield.onValueChanged.AddListener(OnDeckNameChanged);
        saveDeckButton.interactable = false;
        saveDeckButton.onClick.AddListener(SaveDeck);
    }

    void OnDeckLoadValueChanged(int value) {
        var decks = DeckStorageHandler.ListSavedDecks();
        
        var realValue = value - 1;
        if (realValue >= 0 && realValue < decks.Length) {
            var selectedDeck = decks[realValue];
            LoadDeck(selectedDeck);
        }
    }

    void LoadDeck(string deck) {
        var cerealsList = (CardObjectCereal[])DeckStorageHandler.LoadDeck(deck);
        RemoveAllCards();

        foreach (var cardCereal in cerealsList) {
            var card = new CardObject(cardCereal);
            AddCard(card, false);
        }
        didLoadDeck.Invoke(cardsInDeck.Select(cardRow => { return cardRow.GetCard(); }).ToArray());

        deckNameTextfield.text = deck;
    }

    void ClearDropdown() {
        loadDeckDropDown.options.Clear();
        loadDeckDropDown.RemoveAllCustomization();
    }

    void PopulateLoadDropdown() {
        ClearDropdown();

        List<TMP_Dropdown.OptionData> optionsData = new List<TMP_Dropdown.OptionData>();

        var decks = DeckStorageHandler.ListSavedDecks();

        optionsData.Add(new TMP_Dropdown.OptionData("none"));

        if (decks.Length == 0) {
            var option = new TMP_Dropdown.OptionData("No saved decks...");
            optionsData.Add(option);
            loadDeckDropDown.AddCustomization(0, false, false, FontStyles.Italic);
        }
        
        foreach (string deck in decks) {
            var option = new TMP_Dropdown.OptionData(deck);
            optionsData.Add(option);
        }
        loadDeckDropDown.AddOptions(optionsData);
    }

    void OnDeckNameChanged(string value) {
        UpdateSaveButton();
    }

    void UpdateSaveButton() {
        saveDeckButton.interactable = deckNameTextfield.text.Length > 0 && cardsInDeck.Count > 0;
    }

    public void AddCard(CardObject card, bool shouldScroll = true) {
        GameObject cardIndeckPrefab = (GameObject)Resources.Load("Prefabs/CardsUI/CardInDeckRow", typeof(GameObject));
        
        GameObject cardInDeckObject = Instantiate(cardIndeckPrefab);
        CardInDeckRow cardInDeckRow = cardInDeckObject.GetComponent<CardInDeckRow>();

        cardInDeckObject.transform.SetParent(cardContainer.transform);
        cardInDeckObject.transform.localScale = Vector3.one;
        cardInDeckObject.transform.localPosition = Vector3.zero;

        cardInDeckRow.SetCard(card);

        cardsInDeck.Add(cardInDeckRow);

        clearDeckButton.interactable = cardsInDeck.Count > 0;

        cardInDeckRow.onClick.AddListener(DidClickOnRow);

        if (shouldScroll)
            StartCoroutine(ScrollToBottom());

        UpdateSaveButton();
    }

    IEnumerator ScrollToBottom() {
        yield return new WaitForEndOfFrame();
        scrollView.verticalNormalizedPosition = 0;
    }

    void DidClickOnRow(CardInDeckRow row) {
        RemoveCard(row);
    }

    void RemoveCard(CardInDeckRow row) {
        var card = row.GetCard();

        cardsInDeck.Remove(row);
        Destroy(row.gameObject);

        didRemoveCardFromDeck.Invoke(card);

        clearDeckButton.interactable = cardsInDeck.Count > 0;
        
        UpdateSaveButton();
    }

    void SaveDeck() {
        var deckName = deckNameTextfield.text;

        var array = cardsInDeck.Select(row => {
            var card = row.GetCard();
            return new CardObjectCereal(card);
        }).ToArray();

        DeckStorageHandler.SaveDeck(array, deckName);

        saveDeckButton.interactable = false;
        PopulateLoadDropdown();
        loadDeckDropDown.ResetState();
    }

    public void ClearDeck() {
        RemoveAllCards();
        clearDeckButton.interactable = false;
        loadDeckDropDown.ResetState();
    }

    void RemoveAllCards() {
        cardsInDeck.RemoveAll(cardRow => {
            var card = cardRow.GetCard();
            Destroy(cardRow.gameObject);
            didRemoveCardFromDeck.Invoke(card);
            return true;
        });
    }
}
