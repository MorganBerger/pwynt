using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CardInDeckRow : MonoBehaviour
{
    public GameObject imageGO;

    [SerializeField]
    Image cardImage;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardLevel;

    [SerializeField]
    CardObject card;

    // Start is called before the first frame update
    void Awake() {
        print("cardInDeckRow awake");
        cardImage = imageGO.GetComponent<Image>();
    }

    public void SetCard(CardObject card) {
        this.card = card;

        cardName.text = card.fullName;
        cardLevel.text = card.level + "";

        UpdateTexture();
    }

    public void UpdateTexture() {
        var texture = card.thumbnail;
        if (texture != null && cardImage.mainTexture != texture) {
            cardImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0.5f));
        }
    }
}
