using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class PrefabFabric : MonoBehaviour {

    public GameObject textfieldContainer;
    string text;

    public GameObject dropDownContainer;
    TMP_Dropdown dropdown;

    public GameObject popupContainer;
    Popup popup;

    public GameObject objectsContainer;
    // List<Card> cardList = new List<Card>();

    Object[] cardsTextures;

    string outputPath = "Assets/Resources/Prefabs/CardsObjects"; 

    public void OnTextReturn() {
        var text = textfieldContainer.GetComponent<TMP_InputField>();
        print(text.text);
        this.text = text.text;
    }

    string selectedLoadName = "";
    public void DropdownValueChanged() {
        selectedLoadName = dropdown.options[dropdown.value].text;
    }

    public void Refresh() {
        SetDropdown();
    }

    void SetDropdown() {
        string path = Application.dataPath + "/Resources/Prefabs/CardsObjects/";

        dropdown.options.RemoveAll(obj => true);

        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();

        var options = fileInfo.Select((info) => {
            var filename = info.Name;
            filename = filename.Replace(".meta", "");
            return filename;
        }).ToList();

        options.Insert(0, "Blank");

        dropdown.AddOptions(options);
    }

    void CreateFromTexture() {
        // for (int i = 0; i < cardsTextures.Length; i++) {
        //     var texture = (Texture2D)cardsTextures[i];
        //     var obj = Instantiate(Globals.prefabsYO);
        //     var card = obj.GetComponent<Card>();

        //     int index = texture.name.IndexOf(".");
        //     string cardName = texture.name.Remove(0, index + 1).Replace(".", " ");
            
        //     obj.name = texture.name;
            
        //     card.productionID = i + 1;
        //     card.fullName = cardName ?? texture.name;
        //     card.texture2D = texture;

        //     cardList.Add(card);
        //     obj.transform.SetParent(objectsContainer.transform);
        // }
    }

    void ClearCurrentDeck() {
        //     cardList.RemoveAll(card => {
        //     Destroy(card.gameObject);
        //     return true;
        // });
    }

    void ClearLeftList() {

    }

    void CreateFromSave(string saveName) {
        // ClearCurrentDeck();
        
        // var dictInfo = new DirectoryInfo(outputPath + "/" + saveName);
        // var filesInfo = dictInfo.GetFiles();
        // foreach (var info in filesInfo) {
        //     var filename = info.FullName;
        //     if (filename.Contains(".meta")) { continue; }

        //     var targetStr = "Resources\\";
        //     var index = filename.IndexOf(targetStr);
        //     string resourceName = filename.Remove(0, index + targetStr.Length);
        //     resourceName = resourceName.Replace(".prefab", "");

        //     var prefab = (GameObject)Resources.Load(resourceName, typeof(GameObject));

        //     GameObject obj = Instantiate(prefab);
        //     var card = obj.GetComponent<Card>();

        //     obj.name = obj.name.Replace("(Clone)", "");
        //     // obj.name = card.fullName;

        //     cardList.Add(card);
        //     obj.transform.SetParent(objectsContainer.transform);
        // }
    }

    void Awake() {
        cardsTextures = Resources.LoadAll("Images/Cards2D/", typeof(Texture2D));

        popup = popupContainer.GetComponent<Popup>();
        popup.gameObject.SetActive(false);

        dropdown = dropDownContainer.GetComponent<TMP_Dropdown>();
        SetDropdown();

        CreateFromTexture();
    }

    
    public string CreateUniqueDir(string dirName) {
        var timeCloseToTheSecond = System.DateTime.UtcNow.ToString("ddMMyyyy_HHmmss");
        string dir = "CardsObjects_" + timeCloseToTheSecond;

        if (dirName != null) {
           // GOES HERE
            dir = dirName;
        }
        
        string fullPath = outputPath + "/" + dir;

        if (!Directory.Exists(fullPath)) {
            print("creating folder with: '" + outputPath + "', and: '" + dir + "'.");
            string createdFolder = AssetDatabase.CreateFolder(outputPath, dir);
            if (createdFolder.Length == 0) {
                print("Failed creating folder");
                return null;
            }
        }
        return fullPath;
    }

    public void Save() {     
        // string path; 
        // if (text == null || text.Length == 0) {
        //     path = CreateUniqueDir(null);
        // } else {
        //     path = CreateUniqueDir(text);
        // }
        
        // if (path != null) {
        //     foreach(var card in cardList) {
        //         card.SavePrefab(path);
        //     }
        //     SetDropdown();
        // }
    }

    public void Load() {
        if (selectedLoadName == "" || selectedLoadName == "Blank") { return; }
        CreateFromSave(selectedLoadName);
    }
}

#endif

// GOES THERE
 // var tempPath = path + "/" + dirName;
            // if (Directory.Exists(tempPath)) {
            //     print("dir exists: " + tempPath);
            //     string popupText = "Saving '" + dirName + "' will override the existing directory with the same name. Continue?";
            //     // ShowSaveConfirmPopup(popupText, () => {
            //         CleanDirectory(dirName);
            //         print("Directory cleaned");
            //         // Save();
            //     // });
            //     return null;
            // }

// void ShowSaveConfirmPopup(string text, UnityAction onYes) {
    //     popup.SetText(text);
    //     popup.onNo.AddListener(() => {
    //         popup.gameObject.SetActive(false);
    //         popup.onNo.RemoveAllListeners();
    //     });
    //     popup.onYes.AddListener(() => {
    //         onYes();
    //         popup.gameObject.SetActive(false);
    //         popup.onNo.RemoveAllListeners();
    //     });
    //     popup.gameObject.SetActive(true);
    // }

    // public void CleanDirectory(string dir) {
    //     string path = Application.dataPath + "/Resources/Prefabs/CardsObjects/" + dir;
    //     var info = new DirectoryInfo(path);
    //     var fileInfo = info.GetFiles();

    //     print("cleaning path: " + path);
    //     print("number of files" + fileInfo.Length);
    //     foreach (var item in fileInfo) {
    //         if (File.Exists(item.FullName)) {
    //             File.Delete(item.FullName);
    //         }
    //     }
    //     Directory.Delete(path);
    //     File.Delete(path + ".meta");
    // }