
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StorageHandler {
    public void SaveData(object objectToSave, string fileName) {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";

        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(FullFilePath, FileMode.Create);

        Formatter.Serialize(fileStream, objectToSave);
        fileStream.Close();
    }

    public object LoadData(string fileName) {
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