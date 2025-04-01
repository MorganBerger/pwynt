using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class CardInDeckRow : MonoBehaviour
{
    public GameObject imageGO;

    Image cardImage;
    Button button;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardLevel;

    [SerializeField]
    CardData card;

    public UnityEvent<CardInDeckRow> onClick;

    // Start is called before the first frame update
    void Awake() {
        cardImage = imageGO.GetComponent<Image>();
        button = GetComponent<Button>();
    }

    void Start() {
        button.onClick.AddListener(DidClickOnRow);
    }

    void DidClickOnRow() {
        onClick.Invoke(this);
    }

    public CardData GetCard() {
        return card;
    }

    public void SetCard(CardData card) {
        this.card = card;
        cardName.text = card.fullName;
        cardLevel.text = card.level + "";
        SetTexture(card.thumbnail);
    }

    // public void SetCard(CardObject card) {
    //     this.card = card;

    //     cardName.text = card.fullName;
    //     cardLevel.text = card.level + "";

    //     UpdateTexture();
    // }
     public void SetTexture(Texture2D texture) {
        // var texture = card.thumbnail;
        if (texture != null && cardImage.mainTexture != texture) {
            cardImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0.5f));
        }
    }
    
    public void UpdateTexture() {
        var texture = card.thumbnail;
        if (texture != null && cardImage.mainTexture != texture) {
            cardImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0.5f));
        }
    }
}
