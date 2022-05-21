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
    public int numInDeck;
    public int numInCollection;
    public CardObject card;
    
    public Text displayText;

    public Image image;


    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.Find("DeckBuilder").GetComponent<DeckBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Conditions.deck_collection.ContainsKey(cardName)) {
            numInDeck = Conditions.deck_collection[cardName].num;
        } else {
            numInDeck = 0;
        }

        if (Conditions.card_collection.ContainsKey(cardName)) {
            numInCollection = Conditions.card_collection[cardName].num;
        } else {
            numInCollection = 0;
        }

        if (inDeck) {
            displayText.text = cardName + " x" + numInDeck;
        } else {
            displayText.text = cardName + " x" + numInCollection;
            if (numInDeck == type) {
                image.color = Color.gray;
            } else {
                image.color = Color.white;
            }
        }
    }

    public void onClick() {
        if (inDeck && numInDeck > 0) {

            db.deck_size--;
            if (numInDeck > 1) {
                // reduce count in deck, increase count in collection
                Conditions.deck_collection[cardName].num--;
            } else {
                // remove entry from deck
                Conditions.deck_collection.Remove(cardName);
            }

            if (Conditions.card_collection.ContainsKey(cardName)) {
                // collection already has this card, increase count
                Conditions.card_collection[cardName].num++;
            } else {
                // collection does not have this card, make a new entry
                Conditions.card_collection.Add(cardName, new Conditions.info(card, type, 1));
            }

        } else if (!inDeck && numInCollection > 0 && numInDeck < type) {

            db.deck_size++;
            if (numInCollection > 1) {
                // reduce count in collection
                Conditions.card_collection[cardName].num--;
            } else {
                // remove entry from collection
                Conditions.card_collection.Remove(cardName);
            }

            if (Conditions.deck_collection.ContainsKey(cardName)) {
                // deck already has this card, increase count
                Conditions.deck_collection[cardName].num++;
            } else {
                // deck does not have this card, make a new entry
                Conditions.deck_collection.Add(cardName, new Conditions.info(card, type, 1));
            }
        }
    } 
}
