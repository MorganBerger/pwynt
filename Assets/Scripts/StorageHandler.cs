using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StorageHandler {

    public static void SaveData(object objectToSave, string fileName) {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";

        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(FullFilePath, FileMode.Create);

        Formatter.Serialize(fileStream, objectToSave);
        fileStream.Close();
    }

    public static object LoadData(string fileName) {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";

        if (File.Exists(FullFilePath)) {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(FullFilePath, FileMode.Open);

            object obj = Formatter.Deserialize(fileStream);
            fileStream.Close();
            return obj;
        }
        else {
            return null;
        }
    }
}

public class DeckStorageHandler {

    static string deckSavePath = "Decks";

    public static object LoadDeck(string deckName) {
        return StorageHandler.LoadData(deckSavePath + "/" + deckName);
    }

    public static void SaveDeck(object objToSave, string deckName) {
        Directory.CreateDirectory(Application.persistentDataPath + "/" + deckSavePath);
        StorageHandler.SaveData(objToSave, deckSavePath + "/" + deckName);
    }

    public static string[] ListSavedDecks() {
        string path = Application.persistentDataPath + "/" + deckSavePath;
        Directory.CreateDirectory(path);

        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();

        string[] result = new string[fileInfo.Length];

        for (int i = 0; i < fileInfo.Length; i++) {
            var file = fileInfo[i];
            result[i] = Path.GetFileNameWithoutExtension(file.Name);
        }
        return result;
    }
}