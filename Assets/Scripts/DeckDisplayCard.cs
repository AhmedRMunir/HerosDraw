using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckDisplayCard : MonoBehaviour
{
    public DeckBuilder db;
    public string cardName;
    public string description;
    public int cost;
    public int attack;
    public int health;
    public string faction;
    public int type;
    public bool inDeck;
    public int num;
    public CardObject card;
    
    public Text displayText;


    // Start is called before the first frame update
    void Start()
    {
        displayText.text = cardName + " x" + num;
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = cardName + " x" + num;
    }

    public void onClick() {
        if (inDeck) {
            //int amount = db.deck_collection[cardName].num;
            //CardObject cardObject = db.deck_collection[cardName].card;
            //int cardType = db.deck_collection[cardName].type;

            if (num > 0) {
                // reduce count in deck, increase count in collection
                db.deck_collection[cardName].num--;
                num--;
            } else {
                // remove entry from deck
                db.deck_collection.Remove(cardName);
            }

            if (db.card_collection.ContainsKey(cardName)) {
                // collection already has this card, increase count
                db.card_collection[cardName].num++;
            } else {
                // collection does not have this card, make a new entry
                db.card_collection.Add(cardName, new DeckBuilder.info(card, type, 1));
                
            }

        } else {
            //int amount = db.card_collection[cardName].num;
            //CardObject cardObject = db.card_collection[cardName].card;
            //int cardType = db.card_collection[cardName].type;

            if (num > 0) {
                // reduce count in collection
                db.card_collection[cardName].num--;
                num--;
            } else {
                // remove entry from deck
                db.card_collection.Remove(cardName);
            }

            if (db.deck_collection.ContainsKey(cardName)) {
                // deck already has this card, increase count
                db.deck_collection[cardName].num++;
            } else {
                // deck does not have this card, make a new entry
                db.deck_collection.Add(cardName, new DeckBuilder.info(card, type, 1));
            }
        }
    } 
}
