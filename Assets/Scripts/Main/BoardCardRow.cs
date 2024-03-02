using System.Collections.Generic;
using UnityEngine;

public class BoardCardRow : MonoBehaviour
{
    public Battalion acceptedType;

    [SerializeField]
    private GameObject hightlightPlane, canDropCardHerePlane, cardContainer;

    private List<Card> cardList = new List<Card>();

    void Awake() {
        hightlightPlane = transform.GetChild(0).gameObject;
        hightlightPlane.SetActive(false);

        canDropCardHerePlane = transform.GetChild(1).gameObject;
        canDropCardHerePlane.SetActive(false);

        cardContainer = transform.GetChild(2).gameObject;
    }

    public void AddCard(GameObject card) {
        card.transform.SetParent(transform);
    }

    public void AddCard(Card card) {
        card.transform.SetParent(cardContainer.transform);
    }

    public void Shines(bool shines) {
        canDropCardHerePlane.SetActive(shines);
    }

    public void TidyUp() {
        
    }

    public Card currentDraggedCard = null;
    void OnTriggerEnter(Collider other) {
        // print("Row '" + gameObject + "' has collided with '" + other.gameObject + "'");
        var card = other.gameObject.GetComponent<Card>();
		if (card != null && card.battalion == acceptedType) {
            currentDraggedCard = card;
			hightlightPlane.SetActive(true);
		}
	}
	
	void OnTriggerExit(Collider other) {
        // print("Row '" + gameObject + "' exited collision with '" + other.gameObject + "'");
        currentDraggedCard = null;
        hightlightPlane.SetActive(false);
	}
}
