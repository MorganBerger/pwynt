using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StorageHandler {

    public static void SaveStringArray(string[] data, string fileName) {
        string FullFilePath = Path.Combine(Application.persistentDataPath, fileName + ".txt");
        File.WriteAllLines(FullFilePath, data);
    }

    public static string[] LoadStringArray(string fileName) {
        string FullFilePath = Path.Combine(Application.persistentDataPath, fileName + ".txt");
        
        if (File.Exists(FullFilePath)) {
            return File.ReadAllLines(FullFilePath);
        } else {
            Debug.LogWarning("File not found: " + FullFilePath);
            return new string[0]; // Return empty array if file does not exist
        }
    }

    public static void SaveData(object objectToSave, string fileName) {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";

        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(FullFilePath, FileMode.Create);

        Formatter.Serialize(fileStream, objectToSave);
        fileStream.Close();
    }

    public static object LoadData(string fileName) {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";

        Debug.Log("file: " + fileName);

        if (File.Exists(FullFilePath)) {
            Debug.Log("file exists");
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(FullFilePath, FileMode.Open);

            object obj = Formatter.Deserialize(fileStream);
            fileStream.Close();
            return obj;
        }
        else {
            Debug.Log("file does not exist");
            return null;
        }
    }
}

public class DeckStorageHandler {

    // static string deckSavePath = "Decks";
    static string deckSavePath = "DecksTest";

    public static object LoadDeck(string deckName) {
        return StorageHandler.LoadData(deckSavePath + "/" + deckName);
    }

    public static void SaveDeckString(string[] objToSave, string deckName) {
        Directory.CreateDirectory(Application.persistentDataPath + "/" + deckSavePath);
        StorageHandler.SaveStringArray(objToSave, deckSavePath + "/" + deckName);
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