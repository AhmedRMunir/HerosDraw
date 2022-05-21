using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Conditions
{
    public static bool collectingData = false;
    public static int maxLanes = 5;
    public static int maxTotalMana = 10;
    public static int actionsPerLevel = 0;

    public static int wins = 0;
    public static int losses = 0;
    // Levels Completed
    public static int levelsCompleted = 0;
    public static List<CardObject> deck = new List<CardObject>();

    public class info
    {
        public CardObject card;
        public int type;
        public int num;

        public info(CardObject card, int type, int num)
        {
            this.card = card;
            this.type = type;
            this.num = num;
        }
    }

    // a hashtable for the player's current deck
    public static Dictionary<string, info> deck_collection = new Dictionary<string, info>();

    // a hashtable for the player's complete collection of cards
    public static Dictionary<string, info> card_collection = new Dictionary<string, info>();

    // different card types and their corresponding max limit in the deck
    public static int REGULAR = 5;
    public static int RARE = 3;
    public static int CHAMPION = 1;

    // Save deck collection and card collection. PlayerPrefs can only save strings, bools, and ints, so we need to convert the deck/collecton data into a string we can parse in loading.
    public static void saveCards()
    {
        string deckString = "";
        string collectionString = "";
        foreach (KeyValuePair<string, info> cardInfo in deck_collection)
        {
            string cardName = cardInfo.Key;
            int quantity = cardInfo.Value.num;
            // card_type cannot be properly saved and loaded, will need to be replaced.
            int type = cardInfo.Value.type;
            // Each card entry will be its name and how many are in the deck.
            deckString += (cardName + ":" + type + ":" + quantity + "\n");
        }
        foreach (KeyValuePair<string, info> cardInfo in card_collection)
        {
            string cardName = cardInfo.Key;
            int cardQuantity = cardInfo.Value.num;
            int type = cardInfo.Value.type;
            // Each card entry will be its name and how many are in the collection.
            collectionString += (cardName + ":" + type + ":" + cardQuantity + "\n");
        }
        Debug.Log("Deck save string: " + deckString);
        Debug.Log("Collection save string: " + collectionString);
        PlayerPrefs.SetString("PlayerDeck", deckString);
        PlayerPrefs.SetString("PlayerCollection", collectionString);
    }

    public static void loadCards()
    {
        string deckString = PlayerPrefs.GetString("PlayerDeck");
        string[] cardData = deckString.Split("\n");
        Debug.Log(cardData[0]);
        foreach (string cardInfo in cardData)
        {
            string[] cardValues = cardInfo.Split(":");
            if (cardValues.Length > 1)
            {
                Debug.Log(cardValues[0]);
                Debug.Log(cardValues[1]);
                Debug.Log(cardValues[2]);
                string cardName = cardValues[0];
                string type = cardValues[1];
                int cardQuantity = int.Parse(cardValues[2]);
                CardObject card = Resources.Load<CardObject>("Cards/" + cardName);
                // Replace card type with a string probably.
                deck_collection.Add(cardName, new info(card, REGULAR, cardQuantity));
            }
        }

        string collectionString = PlayerPrefs.GetString("PlayerCollection");
        cardData = collectionString.Split("\n");
        foreach (string cardInfo in cardData)
        {
            string[] cardValues = cardInfo.Split(":");
            if (cardValues.Length > 1)
            {
                string cardName = cardValues[0];
                string type = cardValues[1];
                int cardQuantity = int.Parse(cardValues[2]);
                CardObject card = Resources.Load<CardObject>("Cards/" + cardName);
                // Replace card type with a string probably.
                card_collection.Add(cardName, new info(card, REGULAR, cardQuantity));
            }
        }

        // Import deck_collection into the static deck variable.
        CollectionToDeck(deck_collection);
        Debug.Log(deck);
    }

    public static void CollectionToDeck(Dictionary<string, info> collection)
    {
        foreach (KeyValuePair<string, info> cardInfo in collection)
        {
            info currentInfo = cardInfo.Value;
            for (int i = 0; i < currentInfo.num; i++)
            {
                deck.Add(currentInfo.card);
            }
        }
    }

    public static void DeckToCollection(Dictionary<string, info> collection)
    {
        foreach (CardObject card in deck)
        {
            if (collection.ContainsKey(card.name))
            {
                collection[card.name].num++;
            }
            else
            {
                collection.Add(card.name, new info(card, REGULAR, 1));
            }
        }
        
    }
}
