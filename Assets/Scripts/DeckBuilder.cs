using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{
    public class info {
        public CardObject card;
        public card_type type;
        public int num;

        public info(CardObject card, card_type type, int num) {
            this.card = card;
            this.type = type;
            this.num = num;
        }
    }

    // a hashtable for the player's current deck
    public Dictionary<string, info> deck_collection;

    // a hashtable for the player's complete collection of cards
    public Dictionary<string, info> card_collection;

    public GameObject displayPrefab;


    // different card types and their corresponding max limit in the deck
    public enum card_type {
        Regular = 5,
        Rare = 3,
        Champion = 1
    }

    // min size of the deck; can't go lower
    public int deck_min = 20;

    // max size of the deck; can't go higher
    public int deck_max = 40;

    // Start is called before the first frame update
    void Start()
    {
        foreach (CardObject card in Conditions.deck) {
            if (deck_collection.ContainsKey(card.name)) {
                deck_collection[card.name].num++;
            } else {
                deck_collection.Add(card.name, new info(card, card_type.Regular, 1));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToDeckBuilder() {
        SceneManager.LoadScene("Deck Builder");
    }

    public void goToTransitionScreen() {
        SceneManager.LoadScene("Transition Screen");
    }

    // Save deck collection and card collection. PlayerPrefs can only save strings, bools, and ints, so we need to convert the deck/collecton data into a string we can parse in loading.
    public void saveCards()
    {
        string deckString = "";
        string collectionString = "";
        foreach (KeyValuePair<string, info> cardInfo in deck_collection)
        {
            string cardName = cardInfo.Key;
            int quantity = cardInfo.Value.num;
            // card_type cannot be properly saved and loaded, will need to be replaced.
            card_type type = cardInfo.Value.type;
            // Each card entry will be its name and how many are in the deck.
            deckString += (cardName + ":" + type + ":" + quantity + "\n");
        }
        foreach (KeyValuePair<string, info> cardInfo in card_collection)
        {
            string cardName = cardInfo.Key;
            int cardQuantity = cardInfo.Value.num;
            card_type type = cardInfo.Value.type;
            // Each card entry will be its name and how many are in the collection.
            collectionString += (cardName + ":" + type + ":" + cardQuantity + "\n");
        }
        PlayerPrefs.SetString("PlayerDeck", deckString);
        PlayerPrefs.SetString("PlayerCollection", collectionString);
    }

    public void loadCards()
    {
        string deckString = PlayerPrefs.GetString("PlayerDeck");
        string[] cardData = deckString.Split("\n");
        foreach (string cardInfo in cardData)
        {
            string[] cardValues = cardInfo.Split(":");
            string cardName = cardValues[0];
            string type = cardValues[1];
            int cardQuantity = int.Parse(cardValues[2]);
            CardObject card = Resources.Load<CardObject>("Cards/" + cardName);
            // Replace card type with a string probably.
            deck_collection.Add(cardName, new info(card, card_type.Regular, cardQuantity));
        }

        string collectionString = PlayerPrefs.GetString("PlayerCollection");
        cardData = collectionString.Split("\n");
        foreach (string cardInfo in cardData)
        {
            string[] cardValues = cardInfo.Split(":");
            string cardName = cardValues[0];
            string type = cardValues[1];
            int cardQuantity = int.Parse(cardValues[2]);
            CardObject card = Resources.Load<CardObject>("Cards/" + cardName);
            // Replace card type with a string probably.
            card_collection.Add(cardName, new info(card, card_type.Regular, cardQuantity));
        }

        // Import deck_collection into the static deck variable.
        foreach (KeyValuePair<string, info> cardInfo in deck_collection)
        {
            info currentInfo = cardInfo.Value;
            for (int i = 0; i < currentInfo.num; i++)
            {
                Conditions.deck.Add(currentInfo.card);
            }
        }
    }
}
