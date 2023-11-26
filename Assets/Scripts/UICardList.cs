using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class UICardList : MonoBehaviour { 

    Object uiCardPrefab;

    List<CardObject> cardList = new List<CardObject>();
    bool needsRefresh = false;

    List<GameObject> viewList = new List<GameObject>();

    /// <summary>
    /// LifeCycle.
    /// </summary>
    void Awake() {
        uiCardPrefab = Resources.Load("Prefabs/CardsUI/UICard", typeof(GameObject));
        AddGridLayout();
    }

    void Update() {
        if (needsRefresh) {
            needsRefresh = false;
            UpdateList();
        }
    }


    /// <summary>
    /// Private methods.
    /// </summary>
    void UpdateList() {
        viewList.RemoveAll(card => {
            Destroy(card);
            return true;
        });
        foreach(var card in cardList) {
            GameObject obj = Instantiate(Globals.prefabs) as GameObject;
            obj.name = card.name;
            var ui = obj.GetComponent<UICard>();
            ui.card = card;
            ui.UpdateTexture();

            SetObjTransform(obj, transform);

            viewList.Add(obj);
        }
    }

    void SetObjTransform(GameObject obj, Transform target) {     
        obj.transform.SetParent(target);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
    }

    void AddGridLayout() {
        var layout = gameObject.AddComponent<GridLayoutGroup>();
        layout.cellSize = new Vector2(100, 140);
        layout.spacing = new Vector2(10, 10);
        layout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layout.startAxis = GridLayoutGroup.Axis.Horizontal;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.constraint = GridLayoutGroup.Constraint.Flexible;

        var sizeFitter = gameObject.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }


    /// <summary>
    /// Public setters.
    /// </summary>
    public void SetCards(List<CardObject> list) {
        cardList = list;
        needsRefresh = true;
    }
}