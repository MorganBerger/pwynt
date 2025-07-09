using UnityEngine;
using TMPro;
using System.Collections;
using System;
using NUnit.Framework.Constraints;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject cardUIPanelGO;
    CardPanel cardUIPanel;

    public GameObject cardNumberText;
    public int numberToDraw = 1;
    
    public Player _player;
    public Player _opponent;

    public Board board;

    public GameObject deckObject;

    [SerializeField]
    private DraggingManager _dragManager;

    public CardScriptableList cardsList;

    void Awake() {
        Globals.cardsList = cardsList;
    }

    void Start() {        
        if (cardUIPanelGO) {
            cardUIPanel = cardUIPanelGO.GetComponent<CardPanel>();
            cardUIPanelGO.SetActive(false);
        }

        _dragManager = DraggingManager.Instance;
        SetupListeners();
        UdpateText();
    }

    public void OpponentPlays(int cardID) {

        System.Random rand = new System.Random();
        int randID = rand.Next(1, 72);

        _opponent.Play(randID);
    }

    void ObjectWasDragged(DraggableObject obj, DragPlane from, DragPlane to) {
        print("-- obj :'" + obj.name + "', drag from: " + from.name + ", to: " + to.name);

        // if (DraggedBackToHand(obj, to)) {
            
        //     // _player.hand.UndoPlay();
        // }

        CardBehaviour playedCard = null;
        if (DraggedOutOfHand(from)) {            
            var card = obj.GetComponent<CardBehaviour>();
            if (card != null) {
                
                // Disabling hover behaviour when card is dragged on board.
                var hoverBehaviour = obj.GetComponent<HoverableObject>();
                if (hoverBehaviour) {
                    hoverBehaviour.isEnabled = false;
                }

                playedCard = _player._hand.Play(card);
            }
        }
        var fromBoard = from.gameObject == board.gameObject;
        var toHand = to.gameObject == _player._hand.gameObject;

        if (toHand && fromBoard) {
            UndoPlay(obj, false);
        }

        // var fromHand = from.gameObject == _player._hand.gameObject;
        // var toMagicPlane = to.gameObject == magicDragPlaneGO;

        // if (fromHand && toMagicPlane && playedCard != null) {
        //     var dissolve = playedCard.GetComponent<CardDissolve>();
        //     CardDieStyle style = Resources.Load<CardDieStyle>("Scriptables/DieStyleScriptables/MagicUsed");
        //     dissolve.SetDieStyle(style);
        //     playedCard.UseMagic();
        // }
    }

    bool DraggedBackToHand(DraggableObject obj, DragPlane to) {
        return obj.originPlane == to && to.gameObject == _player._hand.gameObject;
    }

    private bool DraggedOutOfHand(DragPlane from) {
        return from.gameObject == _player._hand.gameObject;
    }

    void DraggableWasDropped(DraggableObject obj, DragPlane plane) {
        print("-- dropped: '" + obj.name + "' on: " + plane.name);
        stepsDragged = 0;

        if (plane.gameObject == board.gameObject) {
            if (obj.canDrop) {
                DropCard(obj);
            } else if (!plane.Is(_player._hand)) {
                UndoPlay(obj);
            } else if (plane.Is(_player._hand)) {
                _player._hand.TidyUpHand();
            }
        } else if (plane.gameObject == magicDragPlaneGO) {
            var playedCard = obj.GetComponent<CardBehaviour>();
            var dissolve = playedCard.GetComponent<CardDissolve>();
            CardDieStyle style = Resources.Load<CardDieStyle>("Scriptables/DieStyleScriptables/MagicUsed");
            dissolve.SetDieStyle(style);
            playedCard.UseMagic();
        }

        StartCoroutine(DelaysShit());
    }

    IEnumerator DelaysShit() {
        yield return new WaitForSeconds(0.205f);
        _player._hand.hoverEnabled = true;
    }

    void DropCard(DraggableObject draggableCard) {
        draggableCard.Drop();
        draggableCard.draggingEnabled = false;

        foreach (var row in _player.cardRows) {
            row.Shines(false);

            var card = draggableCard.GetComponent<CardBehaviour>();
            if (row.currentDraggedCard == card) {
                row.AddCard(card);
            }
        }
        if (cardUIPanel)
            cardUIPanel.Hide();
    }

    void UndoPlay(MonoBehaviour obj, bool animated = true) {
        _player._hand.UndoPlay(animated);

        // Re-enabling hover behaviour on card when it goes back to hand.
        StartCoroutine(ReenableHoverBehaviour(obj));
    }

    private IEnumerator ReenableHoverBehaviour(MonoBehaviour obj) {
        yield return new WaitForSeconds(.205f);

        var hoverBehaviour = obj.GetComponent<HoverableObject>();
        print("ENABLING HOVER FOR '" + obj.name + "'");
        hoverBehaviour.isEnabled = true;
        hoverBehaviour.hovered = false;
    }

    void SetupListeners() {
        _player._hand.CardHovered.AddListener(CardInHandWasHovered);
        _player._hand.CardUnhovered.AddListener(CardInHandWasUnhovered);

        _dragManager.didReleaseObject.AddListener(DraggableWasDropped);
        _dragManager.objectDidChangePlane.AddListener(ObjectWasDragged);
        _dragManager.didStartDragging.AddListener(DidStartDragging);
        _dragManager.onDrag.AddListener(OnDrag);
    }

    public int stepsDragged = 0;
    private int lastIndex = -1;
    private void OnDrag(DraggableObject obj, Vector3 distance) {
        
        var pos = obj.transform.position;
        var localPos = obj.transform.localPosition;

        var card = obj.GetComponent<CardBehaviour>();

        var index = _player._hand.cardsInHand.IndexOf(card);

        if (index < 0) return;

        int tmpSteps;
        double numberOfSteps = distance.x / _player._hand.cardStep;

        if ((int)numberOfSteps > 0) {
            tmpSteps = -(int)Math.Floor(numberOfSteps);
        } else if ((int)numberOfSteps < 0) {
            tmpSteps = -(int)Math.Ceiling(numberOfSteps);
        } else {
            tmpSteps = 0;
        }
    
        if (tmpSteps != stepsDragged) {
            print("steps changed from: " + stepsDragged + ", to: " + tmpSteps);
            var yo = 0;
            if (tmpSteps > stepsDragged) {
                yo = tmpSteps - stepsDragged;
            } else if (tmpSteps < stepsDragged) {
                yo = -(stepsDragged - tmpSteps);
            }
            _player._hand.MoveCard(obj.GetComponent<CardBehaviour>(), yo);
        }
        stepsDragged = tmpSteps;
    }

    public GameObject magicDragPlaneGO;
    void DidStartDragging(DraggableObject obj) {
        stepsDragged = 0;

        var card = obj.GetComponent<CardBehaviour>();
        bool isMagicCard = card.data.battalion == Battalion.None;

        magicDragPlaneGO.SetActive(isMagicCard);

        _player._hand.hoverEnabled = false;
    }

    void CardInHandWasHovered(CardBehaviour card) {
        foreach (var row in  _player.cardRows)
            row.Shines(row.acceptedType == card.data.battalion);

        if (cardUIPanel)
            cardUIPanel.Show(card.data.texture2D);
    }

    void CardInHandWasUnhovered(CardBehaviour card) {
        foreach (var row in _player.cardRows)
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
        _player.Draw(numberToDraw);
        _opponent.Draw(numberToDraw);
    }
}