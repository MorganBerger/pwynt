using UnityEngine;
using TMPro;
using System.Collections;

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
        print("-- obj :'" + obj.name + "', drag from: " + from.name + ", to: " + to.name);

        if (DraggedOutOfHand(from)) {            
            var card = obj.GetComponent<Card>();
            if (card != null) {
                
                var hoverBehaviour = obj.GetComponent<HoverableObject>();
                if (hoverBehaviour) {
                    print("DISABLING HOVER FOR '" + obj.name + "'");
                    hoverBehaviour.isEnabled = false;
                }

                _players[0].hand.Play(card);
            }
        }

        var fromIsBoard = from.gameObject == board.gameObject;
        var toIsHand = to.gameObject == _players[0].hand.gameObject;

        if (toIsHand && fromIsBoard) {
            print("SHOULD GO BACK TO HAND");
        }
    }

    private bool DraggedOutOfHand(DragPlane from) {
        return from.gameObject == _players[0].hand.gameObject;
    }

    void DraggableWasDropped(DraggableObject obj, DragPlane plane) {
        print("-- dropped: '" + obj.name + "' on: " + plane.name);

        if (obj.canDrop) {
            DropCard(obj);
        } else if (!plane.Is(_players[0].hand)) {
            UndoPlay(obj);
        } else if (plane.Is(_players[0].hand)) {
            _players[0].hand.TidyUpHand();
        }
    }

    void DropCard(DraggableObject draggableCard) {
        draggableCard.Drop();
        draggableCard.draggingEnabled = false;

        foreach (var row in _players[0].cardRows) {
            row.Shines(false);

            var card = draggableCard.GetComponent<Card>();
            if (row.currentDraggedCard == card) {
                row.AddCard(card);
            }
        }
        if (cardUIPanel)
            cardUIPanel.Hide();
    }

    void UndoPlay(MonoBehaviour obj) {
        _players[0].hand.UndoPlay();
        StartCoroutine(ReenableHoverBehaviour(obj));
    }

    private IEnumerator ReenableHoverBehaviour(MonoBehaviour obj) {
        yield return new WaitForSeconds(.201f);

        var hoverBehaviour = obj.GetComponent<HoverableObject>();
        print("ENABLING HOVER FOR '" + obj.name + "'");
        hoverBehaviour.isEnabled = true;
        hoverBehaviour.hovered = false;
    }

    void SetupListeners() {
        foreach (Player player in _players) {
            player.hand.CardHovered.AddListener(CardInHandWasHovered);
            player.hand.CardUnhovered.AddListener(CardInHandWasUnhovered);
        }
        _dragManager.didReleaseObject.AddListener(DraggableWasDropped);
        _dragManager.didDragObject.AddListener(ObjectWasDragged);
    }

    void CardInHandWasHovered(Card card) {
        foreach (var row in  _players[0].cardRows)
            row.Shines(row.acceptedType == card.battalion);

        if (cardUIPanel)
            cardUIPanel.Show(card.texture2D);
    }

    void CardInHandWasUnhovered(Card card) {
        foreach (var row in _players[0].cardRows)
            row.Shines(false);

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
