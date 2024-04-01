using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardsInDeckList : MonoBehaviour
{
    public GameObject cardContainer;
    public GameObject loadPanel;

    TMP_Dropdown loadDeckDropDown;
    Button clearDeckButton;

    [SerializeField]
    List<CardInDeckRow> cardsInDeck = new List<CardInDeckRow>();

    ScrollRect scrollView;

    public UnityEvent<CardObject> didRemoveCardFromDeck;

    void Awake() {
        scrollView = GetComponentInChildren<ScrollRect>();

        loadDeckDropDown = loadPanel.GetComponentInChildren<TMP_Dropdown>();
        clearDeckButton = loadPanel.GetComponentInChildren<Button>();
    }

    void Start() {
        clearDeckButton.onClick.AddListener(ClearDeck);
        clearDeckButton.interactable = false;
    }

    public void AddCard(CardObject card) {
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

        StartCoroutine(ScrollToBottom());
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
    }

    public void Save() {

    }
    public void Load() {

    }

    public void ClearDeck() {
        cardsInDeck.RemoveAll(cardRow => {
            var card = cardRow.GetCard();
            Destroy(cardRow.gameObject);
            didRemoveCardFromDeck.Invoke(card);
            return true;
        });
        clearDeckButton.interactable = false;
    }
}
