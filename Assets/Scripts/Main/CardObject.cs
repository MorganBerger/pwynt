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
    public int numberInDeck;
    public int numberSelected;
    public int limitInDeck;
    public UICardMode mode;

    public CardObjectCereal(string ID, string name, int numberInDeck, int numberSelected, int limitInDeck, UICardMode mode) {
        this.ID = ID;
        this.name = name;
        this.numberInDeck = numberInDeck;
        this.mode = mode;
        this.numberSelected = numberSelected;
        this.limitInDeck = limitInDeck;
    }
}

public class CardObject {
    public string ID;
    public string name;
    public int numberInDeck;
    public int numberSelected;
    public int limitInDeck;
    public UICardMode mode;
    public Texture2D texture2D;

    public int limit {
        get {
            int limit = limitInDeck - numberInDeck;
            if (mode == UICardMode.Toggle) {
                limit = 1;
            }
            return limit;
        }
    }

    public CardObject(string ID, string name, Texture2D texture, int number, int numberSelected, int limitInDeck, UICardMode mode) {
        if (ID != null) {
            this.ID = ID;
        } else {
            this.ID = String.RandomString();
        }
        this.texture2D = texture;
        this.name = name;
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
