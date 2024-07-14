
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Networking.Transport.Error;
using UnityEditor;
using UnityEngine;

public class CardScriptableHolder: MonoBehaviour {

    static string outputPath = "Assets/Resources/Scriptables"; 

    public CardScriptableList list;

    void Start () {
        Globals.cardsList = list;

        // var yo = GetComponentInChildren<CardBehaviour>();
        // print("yo: " + yo);
        // print(list.content.Count);
        // var card = list.content[2];
        // print(card);
        // yo.SetCardData(card);

        // list = new CardScriptableList();
        // CreateScriptables();
        // Save();
    }

    void CreateScriptables() {
        // List<Card> cardList = new List<Card>(); //Globals.CardsObjects().OrderBy(c => c.name).ToList();
        // List<Texture2D> textures = Resources.LoadAll<Texture2D>("Blender Files/Cards/Textures/Rectos")
        //                             .OrderBy(t => int.Parse(t.name))
        //                             .ToList();

        // List<Material> materials = Resources.LoadAll<Material>("Blender Files/Cards/Materials/").ToList();
        // // materials.RemoveAll(c => c.name.Contains("back."));

        // cardList.RemoveAt(0);
        // for (int i = 0; i < cardList.Count; i++) {
        //     var card = cardList[i];

        //     card.productionID = i + 1;
                
        //     card.texture2D = textures[i];

        //     var cardScriptable = new CardScriptable(card);
            
        //     Material frontMaterial = materials.First(m => { 
        //         int number = int.Parse(m.name.Split(".")[1]);

        //         bool contains = m.name.Contains("front.");
        //         bool rightMaterial = number == card.productionID;

        //         return contains && rightMaterial;
        //     });
        //     Material backMaterial = materials.First(m => {
        //         return m.name.Contains("back.");
        //     });

        //     cardScriptable.materialFront = frontMaterial;
        //     cardScriptable.materialBack = backMaterial;
        //     list.content.Add(cardScriptable);

        // }
    }
    void SaveList() {
        SavePrefab(list, outputPath, "AllCardsList");
    }
    void SaveAllCards() {
        var path = CreateUniqueDir("CardsScriptables");
        foreach (CardData card in list.content) {
            SavePrefab(card, path, card.objName);
        }
    }

    public string CreateUniqueDir(string dirName) {
        var timeCloseToTheSecond = System.DateTime.UtcNow.ToString("ddMMyyyy_HHmmss");
        string dir = "cards_" + timeCloseToTheSecond;

        if (dirName != null) {
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

        SaveList();
        SaveAllCards();


        // string path;
        // path = CreateUniqueDir(null);
        
        // if (path != null) {
        //     SavePrefab(path);
        // }
    }

    #if UNITY_EDITOR
    public void SavePrefab(Object obj, string path, string name) {
        string localPath = path + "/" + name + ".asset";
        print(localPath);
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        AssetDatabase.CreateAsset(obj, localPath);
    }
    #endif
}