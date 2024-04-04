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


[Serializable]
public class CardObjectCereal {
    public string ID;
    public string name;
    public string fullName;
    public int level;
    public int numberInDeck;
    public int numberSelected;
    public int limitInDeck;
    public UICardMode mode;

    // public Texture2D texture2D;
    // public Texture2D thumbnail;

    public byte[] textureBytes;
    public byte[] thumbnailBytes;

    // public CardObjectCereal(string ID, string name, int numberInDeck, int numberSelected, int limitInDeck, UICardMode mode) {
    //     this.ID = ID;
    //     this.name = name;
    //     this.numberInDeck = numberInDeck;
    //     this.mode = mode;
    //     this.numberSelected = numberSelected;
    //     this.limitInDeck = limitInDeck;
    // }

    public CardObjectCereal(CardObject cardObj) {
        ID = cardObj.ID;
        name = cardObj.name;
        fullName = cardObj.fullName;
        level = cardObj.level;

        numberInDeck = cardObj.numberInDeck;
        mode = cardObj.mode;
        numberSelected = cardObj.numberSelected;
        limitInDeck = cardObj.limitInDeck;

        textureBytes = ImageConversion.EncodeToPNG(cardObj.texture2D);
        thumbnailBytes = ImageConversion.EncodeToPNG(cardObj.thumbnail);
    }
}

public class CardObject {
    public string ID;
    public string name;
    public string fullName;
    public int level;
    public int numberInDeck;
    public int numberSelected;
    public int limitInDeck;
    public UICardMode mode;
    public Texture2D texture2D;
    public Texture2D thumbnail;

    public int limit {
        get {
            int limit = limitInDeck - numberInDeck;
            if (mode == UICardMode.Toggle) {
                limit = 1;
            }
            return limit;
        }
    }

    public CardObject(CardObjectCereal cereal) {
        this.ID = cereal.ID != null ? cereal.ID : String.RandomString();

        // this.texture2D = texture2D;
        this.texture2D = new Texture2D(0, 0);
        ImageConversion.LoadImage(this.texture2D, cereal.textureBytes);

        this.thumbnail = new Texture2D(0, 0);
        ImageConversion.LoadImage(this.thumbnail, cereal.thumbnailBytes);

        this.name = cereal.name;

        this.fullName = cereal.fullName;
        this.level = cereal.level;

        this.numberInDeck = cereal.numberInDeck;
        this.mode = cereal.mode;
        this.numberSelected = cereal.numberSelected;
        this.limitInDeck = cereal.limitInDeck;
    }

    public CardObject(
        string ID, 
        string name,
        string fullName,
        int level,
        Texture2D texture2D,
        Texture2D thumbnail,
        int number,
        int numberSelected,
        int limitInDeck,
        UICardMode mode
    ) {
        this.ID = ID != null ? ID : String.RandomString();

        this.texture2D = texture2D;
        this.thumbnail = thumbnail;
        this.name = name;
        this.fullName = fullName;
        this.level = level;
        this.numberInDeck = number;
        this.mode = mode;
        this.numberSelected = numberSelected;
        this.limitInDeck = limitInDeck;
    }

    public void CopyData(CardObjectCereal cereal) {
        name = cereal.name;
        numberInDeck = cereal.numberInDeck;
        mode = cereal.mode;
        numberSelected = cereal.numberSelected;
        limitInDeck = cereal.limitInDeck;
    }
}
