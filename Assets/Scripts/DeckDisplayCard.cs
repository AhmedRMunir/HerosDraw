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
    
    public Text displayText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick() {
        if (inDeck) {
            int amount = Conditions.deck_collection[cardName].num;
            CardObject cardObject = Conditions.deck_collection[cardName].card;
            int cardType = Conditions.deck_collection[cardName].type;

            if (amount > 0) {
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
                Conditions.card_collection.Add(cardName, new Conditions.info(cardObject, cardType, 1));
            }

        } else {
            int amount = Conditions.card_collection[cardName].num;
            CardObject cardObject = Conditions.card_collection[cardName].card;
            int cardType = Conditions.card_collection[cardName].type;

            if (amount > 0) {
                // reduce count in collection
                Conditions.card_collection[cardName].num--;
            } else {
                // remove entry from deck
                Conditions.card_collection.Remove(cardName);
            }

            if (Conditions.deck_collection.ContainsKey(cardName)) {
                // deck already has this card, increase count
                Conditions.deck_collection[cardName].num++;
            } else {
                // deck does not have this card, make a new entry
                Conditions.deck_collection.Add(cardName, new Conditions.info(cardObject, cardType, 1));
            }
        }
    } 
}
