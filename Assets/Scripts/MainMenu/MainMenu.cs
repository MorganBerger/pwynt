using Unity.Multiplayer.Playmode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject startButtonGO;
    MenuButton startButton;

    public GameObject popupPanel;
    JoinGamePopup joinGamePopup;

    PlayerNamePopup playerNamePopup;

    [SerializeField]
    NetworkErrorPopup networkErrorPopup;

    public GameObject pophover;

    void Awake() {
        startButton = startButtonGO.GetComponent<MenuButton>();

        joinGamePopup = popupPanel.GetComponentInChildren<JoinGamePopup>(true);
        playerNamePopup = popupPanel.GetComponentInChildren<PlayerNamePopup>(true);
        networkErrorPopup = popupPanel.GetComponentInChildren<NetworkErrorPopup>(true);
    }

    bool startButtonEnabled = false;
    void Start() {
        startButtonEnabled = DeckStorageHandler.ListSavedDecks().Length > 0;
        startButton.SetEnabled(startButtonEnabled);
        startButton.wasHovered.AddListener(StartButtonWasHovered);

        joinGamePopup.onJoinGame.AddListener(JoinGame);

        CheckForPlayername();
        CheckForNetworkError();
    }

    void CheckForPlayername() {
        // PlayerPrefs.DeleteAll();

        var key = Globals.PlayerPrefsKey.playerName;
        var name = PlayerPrefs.GetString(key);

        print("player name: '" + name + "'");

        if (name == null || name.Length == 0) {
            popupPanel.SetActive(true);
            playerNamePopup.gameObject.SetActive(true);
        }
    }

    void CheckForNetworkError() {
        if (NetworkHelper.networkError.Length > 0) {
            networkErrorPopup.SetErrorDescription(NetworkHelper.networkError);

            popupPanel.SetActive(true);
            networkErrorPopup.gameObject.SetActive(true);
        }
    }

    bool buttonHovered = false;
    void StartButtonWasHovered(bool hovered) {
        if (startButtonEnabled) { return; }

        buttonHovered = hovered;
        pophover.SetActive(hovered);
    }

    void LateUpdate() {
        if (buttonHovered) {
            var mouse = Input.mousePosition;
            mouse.x = mouse.x + 245;
            mouse.y = mouse.y - 50;
            pophover.transform.position = new Vector3(mouse.x, mouse.y, mouse.z);
        }
    }

    public void CreateGame() {
        NetworkHelper.isHost = true;
        SceneManager.LoadScene("LobbyScene");
    }

    public void JoinGame() {
        NetworkHelper.isHost = false;
        SceneManager.LoadScene("LobbyScene");
    }

    public void BuildDeck() {
        SceneManager.LoadScene("DeckMaker");
    }
    
    public void QuitGame() {
        Application.Quit();
    }
}
