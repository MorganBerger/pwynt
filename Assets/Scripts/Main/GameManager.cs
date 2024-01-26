using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject cardUIPanelGO;
    CardPanel cardUIPanel;

    public GameObject cardNumberText;
    public int numberToDraw = 1;
    
    Player[] players;

    public Board board;

    public GameObject deckObject;

    // Start is called before the first frame update
    void Start() {
        print("GameManager starts");

        players = GetComponentsInChildren<Player>();
        print("Player count: " + players.Length);

        if (cardUIPanelGO) { 
            cardUIPanel = cardUIPanelGO.GetComponent<CardPanel>();
            cardUIPanelGO.SetActive(false);
        }

        SetupListeners();
        UdpateText();
    }

    void SetupListeners() {
        foreach (Player player in players) {
            // player.deck.OnDeckClick.AddListener(() => {
            //     OnDeckClickFor(player);
            // });
            player.hand.CardHovered.AddListener(CardInHandWasHovered);
            player.hand.UIShouldHide.AddListener(() => ShouldHideHandUI(player));
        }
    }

    void CardInHandWasHovered(Card card) {
        if (cardUIPanel)
            cardUIPanel.Show(card.texture2D);
        
        AnimateHandHover(card);
    }
    void ShouldHideHandUI(Player player) {
        UnhoverAllCards(player);

        if (cardUIPanel)
            cardUIPanel.Hide();
    }

    float hoverAnimationTime = .15f;
    void UnhoverAllCards(Player player) {
        var cards = player.hand.cardsInHand;
        foreach (var card in cards) {
            var pos = card.transform.localPosition;

            var draggable = card.GetComponent<DraggableObject>();
            if (!draggable.isDragging) {
                if (pos.z < 0f) {
                    var nextPost = new Vector3(pos.x, pos.y, 0f);
                    StartCoroutine(CardAnimation.MoveTo(card.transform, nextPost, hoverAnimationTime));
                }
            }
        }
    }

    void AnimateHandHover(Card card) {
        var pos = card.transform.localPosition;
        var nextPost = new Vector3(pos.x, pos.y, -.035f);

        var draggable = card.GetComponent<DraggableObject>();
        if (!draggable.isDragging)
            StartCoroutine(CardAnimation.MoveTo(card.transform, nextPost, hoverAnimationTime));
    }

    void OnDeckClickFor(Player player) {
        player.Draw(1);
    }

    public void Up() {
        numberToDraw++;
        UdpateText();
    }
    public void Down() {
        if (numberToDraw == 0) 
            return;
        numberToDraw--;
        UdpateText();
    }
    void UdpateText() {
        if (cardNumberText)
            cardNumberText.GetComponent<TextMeshProUGUI>().text = "" + numberToDraw;
    }

    public void DrawSome() {
        var nb = numberToDraw;
        print("Drawing " + nb + " cards!");
        players[0].Draw(numberToDraw);
    }
}
