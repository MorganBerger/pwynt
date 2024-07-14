using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CardToJsonManager: MonoBehaviour {
    
    List<CardData> cards;

    private void Start() {
        cards = Globals.Cards;
    }

    public void SaveMomo() {
        CardDataContainer container = new CardDataContainer();
        var sorted = cards.OrderBy((c) => c.productionID).ToList();

        for (int i = 0; i < sorted.Count; i++) {
            CardData card = sorted[i];
            card.productionID = i;

            container.Add(new CardJSONObject(card));
        }
        
        string json = JsonUtility.ToJson(container, true);
    
        File.WriteAllText(Application.persistentDataPath + "/cards.json", json);
    }

    public static void SaveDataJSON(object objectToSave, string fileName) {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".json";

        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(FullFilePath, FileMode.Create);

        Formatter.Serialize(fileStream, objectToSave);
        fileStream.Close();
    }
}