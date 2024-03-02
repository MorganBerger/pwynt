using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject cardUIPanelGO;
    CardPanel cardUIPanel;

    public GameObject cardNumberText;
    public int numberToDraw = 1;
    
    Player[] _players;

    public Board board;

    public GameObject deckObject;

    [SerializeField]
    private DraggingManager _dragManager;

    // Start is called before the first frame update
    void Awake() {
        
    }
    void Start() {
        print("GameManager start");

        _players = GetComponentsInChildren<Player>();
        print("Player count: " + _players.Length);

        if (cardUIPanelGO) {
            cardUIPanel = cardUIPanelGO.GetComponent<CardPanel>();
            cardUIPanelGO.SetActive(false);
        }

        _dragManager = DraggingManager.Instance;
        SetupListeners();
        UdpateText();
    }

    void ObjectWasDragged(DraggableObject obj, DragPlane from, DragPlane to) {
        if (DraggedOutOfHand(from)) {            
            var card = CardOutOfObject(obj);
            if (card != null) {
                // print("played card out of hand");
                var hoverBehaviour = card.GetComponent<HoverableObject>();
                hoverBehaviour.hoverEnabled = false;
            }
        }

        var fromIsBoard = from.gameObject == board.gameObject;
        var toIsHand = to.gameObject == _players[0].hand.gameObject;

        if (toIsHand && fromIsBoard) {
            print("SHOULD GO BACK TO HAND");
        }
        print("-- obj :'" + obj.name + "', drag from: " + from.name + ", to: " + to.name);
    }

    private Card CardOutOfObject(DraggableObject obj) {
        return obj.gameObject.GetComponent<Card>();
    }

    private bool DraggedOutOfHand(DragPlane from) {
        return from.gameObject == _players[0].hand.gameObject;
    }

    void DraggableWasDropped(DraggableObject obj, DragPlane plane) {
        print("-- dropped: '" + obj.name + "' on: " + plane.name);
        var card = CardOutOfObject(obj);
        print("card: " + card);
        print("plane: " + plane.name);
        if (plane.name == "red") {
            _players[0].hand.UndoPlay();
        }
    }

    void SetupListeners() {
        foreach (Player player in _players) {
            player.hand.CardHovered.AddListener(CardInHandWasHovered);
            player.hand.CardUnhovered.AddListener(() => ShouldHideHandUI(player));
        }
        _dragManager.didReleaseObject.AddListener(DraggableWasDropped);
        _dragManager.didDragObject.AddListener(ObjectWasDragged);
    }

    void CardInHandWasHovered(Card card) {
        // var cardRow = _players[0].GetCardRow(card.battalion);
        var cardRows = _players[0].cardRows;

        // if (cardRow) {
        //     cardRow.Shines(true);
        // }
        foreach (var row in cardRows)
            row.Shines(row.acceptedType == card.battalion);

        if (cardUIPanel)
            cardUIPanel.Show(card.texture2D);
        
        // AnimateHandHover(card);
    }
    void ShouldHideHandUI(Player player) {
        // UnhoverAllCards(player);

        if (cardUIPanel)
            cardUIPanel.Hide();
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
        _players[0].Draw(numberToDraw);
    }
}
