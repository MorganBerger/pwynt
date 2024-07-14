using System;
using UnityEngine;

struct String {
    public static string RandomString() {
        const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";
        string myString = "";
        int charAmount = UnityEngine.Random.Range(15, 20); //set those to the minimum and maximum length of your string
        for(int i=0; i<charAmount; i++) {
            myString += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
        }   
        return myString;
    }
}

public class CardObject {
    public int ID;
    public string name;
    public string fullName;
    public int level;
    
    public Texture2D texture2D;
    public Texture2D thumbnail;

    public CardObject(
        int ID,
        string name,
        string fullName,
        int level,
        Texture2D texture2D,
        Texture2D thumbnail
    ) {
        this.ID = ID > 0 ? ID : 0;

        this.texture2D = texture2D;
        this.thumbnail = thumbnail;
        this.name = name;
        this.fullName = fullName;
        this.level = level;
    }
}
