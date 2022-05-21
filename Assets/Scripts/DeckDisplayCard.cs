using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckDisplayCard : MonoBehaviour
{
    public DeckBuilder db;
    public Image image;
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
        if (inDeck) {
            if (Conditions.deck_collection.ContainsKey(cardName)) {
                num = Conditions.deck_collection[cardName].num;
            } else {
                num = 0;
            }
        } else {
            if (Conditions.card_collection.ContainsKey(cardName)) {
                num = Conditions.card_collection[cardName].num;
            } else {
                num = 0;
            }
        }
        displayText.text = cardName + " x" + num;
        if (num == 0) {
            image.color = Color.grey;
        } else {
            image.color = Color.white;
        }
    }

    public void onClick() {
        if (inDeck && num > 0) {
            //int amount = db.deck_collection[cardName].num;
            //CardObject cardObject = db.deck_collection[cardName].card;
            //int cardType = db.deck_collection[cardName].type;

            if (num > 0) {
                // reduce count in deck, increase count in collection
                Conditions.deck_collection[cardName].num--;
            } else {
                // remove entry from deck
                Conditions.deck_collection.Remove(cardName);
                Destroy(gameObject);
            }

            if (Conditions.card_collection.ContainsKey(cardName)) {
                // collection already has this card, increase count
                Conditions.card_collection[cardName].num++;
            } else {
                // collection does not have this card, make a new entry
                Conditions.card_collection.Add(cardName, new Conditions.info(card, type, 1));
            }

        } else if (!inDeck && num > 0) {
            //int amount = db.card_collection[cardName].num;
            //CardObject cardObject = db.card_collection[cardName].card;
            //int cardType = db.card_collection[cardName].type;

            if (num > 0) {
                // reduce count in collection
                Conditions.card_collection[cardName].num--;
            } else {
                // remove entry from deck
                Conditions.card_collection.Remove(cardName);
                Destroy(gameObject);
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
