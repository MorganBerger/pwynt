using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardCardRowSortButton : MonoBehaviour
{
    private Button button;

    public UnityEvent onClick;

    // Start is called before the first frame update
    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick.Invoke());
    }
}
