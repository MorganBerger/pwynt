using System.Collections;
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

    private BoardCardRowSortButton sortButton;

    void Awake() {
        hightlightPlane = transform.GetChild(0).gameObject;
        hightlightPlane.SetActive(false);

        canDropCardHerePlane = transform.GetChild(1).gameObject;
        canDropCardHerePlane.SetActive(false);

        cardContainer = transform.GetChild(2).gameObject;

        rowText = GetComponentInChildren<TextMeshProUGUI>();

        sortButton = GetComponentInChildren<BoardCardRowSortButton>();
        sortButton.gameObject.SetActive(false);
        
        sortButton?.onClick.AddListener(TidyUp);
    }

    public void AddCard(Card card) {
        card.transform.SetParent(cardContainer.transform);
        cardList.Add(card);

        clean = false;
        sortButton.gameObject.SetActive(!clean);

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

    private bool clean = true;
    public void TidyUp() {
        print("Tidying up row '" + name + "'");

        if (cardList.Count <= 12) {
            cardStep = 0.1f;
        } else if (cardList.Count < 24) {
            cardStep = 0.05f;
        } else {
            cardStep = 0.025f;
        }

        StartCoroutine(UpdateCardsPos());
        
        clean = true;
        sortButton.gameObject.SetActive(!clean);
    }

    float cardStep = 0.1f;
    float cardDepthDiff = 0.0001f;
    IEnumerator UpdateCardsPos() {
        if (cardList.Count == 0)
            yield return null;
        
        float animDuration = 0.1f;
        var wait = new WaitForSeconds(0.05f);    

        var startPosX = (cardList.Count - 1) * cardStep / 2;

        for (int i = 0; i < cardList.Count; i++)
        {
            var card = cardList[i];

            var rigidbody = card.GetComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;

            var newPos = new Vector3(startPosX - cardStep * i, cardDepthDiff * i, 0);

            Move(card, newPos, animDuration);
            yield return wait;
        }
    }

    void Move(Card card, Vector3 pos, float moveDuration) {
        StartCoroutine(CardAnimation.RotateTo(card, Quaternion.identity, moveDuration));
        StartCoroutine(CardAnimation.MoveTo(card, pos, moveDuration));
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
