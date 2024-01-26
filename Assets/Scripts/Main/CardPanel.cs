using UnityEngine;
using UnityEngine.UI;

public class CardPanel : MonoBehaviour
{
    Image image;

    void Awake() {
        foreach(Transform t in transform) {
            image = t.gameObject.GetComponent<Image>();
            break;
        }     
        Hide();
    }

    public void Hide() {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public void Show(Texture2D texture) {
        SetImage(texture);
        if (!gameObject.activeSelf)
            gameObject.SetActive(true); 
    }

    void SetImage(Texture2D texture) {
        image.sprite = Sprite.Create(
            texture, 
            new Rect(0, 0, texture.width, texture.height), 
            new Vector2(0f, 0.5f)
        );
    }
}
