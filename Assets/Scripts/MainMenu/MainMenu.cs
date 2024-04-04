using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // public GameObject startGameMenuPanel;
    // public GameObject settingsMenuPanel; 
    public GameObject joinGamePopupPanel;

    JoinGamePopup joinGamePopup;

    void Awake() {
        joinGamePopup = joinGamePopupPanel.GetComponentInChildren<JoinGamePopup>();
        joinGamePopup.onJoinGame.AddListener(JoinGame);
    }
    void Start() {
        print("Starting MainMenu");
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
