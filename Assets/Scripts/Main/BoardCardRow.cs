using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoardCardRow : MonoBehaviour
{
    public Battalion acceptedType;

    [SerializeField]
    private GameObject hightlightPlane, canDropCardHerePlane, cardContainer;

    private List<Card> cardList = new List<Card>();

    public TextMeshProUGUI rowText;

    void Awake() {
        hightlightPlane = transform.GetChild(0).gameObject;
        hightlightPlane.SetActive(false);

        canDropCardHerePlane = transform.GetChild(1).gameObject;
        canDropCardHerePlane.SetActive(false);

        cardContainer = transform.GetChild(2).gameObject;

        rowText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AddCard(Card card) {
        card.transform.SetParent(cardContainer.transform);
        cardList.Add(card);
        UpdateRowText();
    }

    void UpdateRowText() {
        var total = cardList.Sum(c => {
            return c.level;
        });
        rowText.text =  "" + total;
    }

    public void Shines(bool shines) {
        canDropCardHerePlane.SetActive(shines);
    }

    public void TidyUp() {
        
    }

    public Card currentDraggedCard = null;
    
    void OnTriggerEnter(Collider other) {
        var card = other.GetComponent<Card>();
		if (card != null && card.battalion == acceptedType) {
            currentDraggedCard = card;

            var draggable = card.GetComponent<DraggableObject>();
            draggable.canDrop = true;

			hightlightPlane.SetActive(true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.GetComponent<Card>()) {
            currentDraggedCard = null;

            var draggable = other.GetComponent<DraggableObject>();
            draggable.canDrop = false;

			hightlightPlane.SetActive(false);
		}
	}
}
