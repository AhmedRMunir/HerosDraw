using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDisplayCard : MonoBehaviour
{
    public DeckBuilder db;
    public string cardName;
    public Sprite art;
    public string description;
    public int cost;
    public int attack;
    public int health;
    public string faction;
    public GameObject cardAbility; // We'll probably have to write prefabs with the card ability and instantiate them on activation
    public DeckBuilder.card_type type;
    public bool inDeck;
    public int num;
    // Other potential stats


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
            int amount = db.deck_collection[cardName].num;
            CardObject cardObject = db.deck_collection[cardName].card;
            DeckBuilder.card_type cardType = db.deck_collection[cardName].type;

            if (amount > 0) {
                // reduce count in deck, increase count in collection
                db.deck_collection[cardName].num--;
            } else {
                // remove entry from deck
                db.deck_collection.Remove(cardName);
            }

            if (db.card_collection.ContainsKey(cardName)) {
                // collection already has this card, increase count
                db.card_collection[cardName].num++;
            } else {
                // collection does not have this card, make a new entry
                db.card_collection.Add(cardName, new DeckBuilder.info(cardObject, cardType, 1));
            }

        } else {
            int amount = db.card_collection[cardName].num;
            CardObject cardObject = db.card_collection[cardName].card;
            DeckBuilder.card_type cardType = db.card_collection[cardName].type;

            if (amount > 0) {
                // reduce count in collection
                db.card_collection[cardName].num--;
            } else {
                // remove entry from deck
                db.card_collection.Remove(cardName);
            }

            if (db.deck_collection.ContainsKey(cardName)) {
                // deck already has this card, increase count
                db.deck_collection[cardName].num++;
            } else {
                // deck does not have this card, make a new entry
                db.deck_collection.Add(cardName, new DeckBuilder.info(cardObject, cardType, 1));
            }
        }
    } 
}
