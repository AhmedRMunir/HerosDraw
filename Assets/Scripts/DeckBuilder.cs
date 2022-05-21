using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{
    public GameObject displayPrefab;

    // min size of the deck; can't go lower
    public int deck_min = 20;

    // max size of the deck; can't go higher
    public int deck_max = 40;

    public GameObject deckWindow;
    public GameObject collectionWindow;
    public Text deckSizeText;
    public Canvas canvas;

    public List<CardObject> testDeck;

    // Start is called before the first frame update
    void Start()
    {
        foreach (CardObject card in testDeck) {
            if (Conditions.deck_collection.ContainsKey(card.name)) {
                Conditions.deck_collection[card.name].num++;
            } else {
                Conditions.deck_collection.Add(card.name, new Conditions.info(card, Conditions.REGULAR, 1));
            }
        }

        foreach (CardObject card in testDeck) {
            if (Conditions.card_collection.ContainsKey(card.name)) {
                Conditions.card_collection[card.name].num++;
            } else {
                Conditions.card_collection.Add(card.name, new Conditions.info(card, Conditions.REGULAR, 1));
            }
        }

        renderDisplay(Conditions.deck_collection, deckWindow);
        renderDisplay(Conditions.card_collection, collectionWindow);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void renderDisplay(Dictionary<string, Conditions.info> dict, GameObject window) {
        int i = 0;
        foreach (KeyValuePair<string, Conditions.info> cardInfo in dict) {
            GameObject displayedCard = displayDeckCard(cardInfo);
            displayedCard.transform.SetParent(window.transform);
            float cardHeight = displayedCard.GetComponent<RectTransform>().sizeDelta.y;
            Vector2 position = new Vector2(0, (- 0.5f * cardHeight) - (i * cardHeight));
            displayedCard.GetComponent<RectTransform>().anchoredPosition = position;
            i++;
        }
    }

    public GameObject displayDeckCard(KeyValuePair<string, Conditions.info> cardInfo) {
        GameObject displayedCard = Instantiate(displayPrefab, canvas.transform);
        DeckDisplayCard displayInfo = displayedCard.GetComponent<DeckDisplayCard>();
        displayInfo.cardName = cardInfo.Key;
        displayInfo.cost = cardInfo.Value.card.cost;
        displayInfo.attack = cardInfo.Value.card.attack;
        displayInfo.health = cardInfo.Value.card.health;
        displayInfo.description = cardInfo.Value.card.description;
        displayInfo.type = cardInfo.Value.card.type;
        displayInfo.inDeck = true;
        displayInfo.num = cardInfo.Value.num;
        displayInfo.card = cardInfo.Value.card;

        return displayedCard;
    }

    public GameObject displayCollectionCard(KeyValuePair<string, Conditions.info> cardInfo) {
        GameObject displayedCard = Instantiate(displayPrefab, canvas.transform);
        DeckDisplayCard displayInfo = displayedCard.GetComponent<DeckDisplayCard>();
        displayInfo.cardName = cardInfo.Key;
        displayInfo.cost = cardInfo.Value.card.cost;
        displayInfo.attack = cardInfo.Value.card.attack;
        displayInfo.health = cardInfo.Value.card.health;
        displayInfo.description = cardInfo.Value.card.description;
        displayInfo.type = cardInfo.Value.card.type;
        displayInfo.inDeck = false;
        displayInfo.num = cardInfo.Value.num;
        displayInfo.card = cardInfo.Value.card;
        return displayedCard;
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
        foreach (KeyValuePair<string, Conditions.info> cardInfo in Conditions.deck_collection)
        {
            string cardName = cardInfo.Key;
            int quantity = cardInfo.Value.num;
            // card_type cannot be properly saved and loaded, will need to be replaced.
            int type = cardInfo.Value.type;
            // Each card entry will be its name and how many are in the deck.
            deckString += (cardName + ":" + type + ":" + quantity + "\n");
        }
        foreach (KeyValuePair<string, Conditions.info> cardInfo in Conditions.card_collection)
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
            Conditions.deck_collection.Add(cardName, new Conditions.info(card, Conditions.REGULAR, cardQuantity));
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
            Conditions.card_collection.Add(cardName, new Conditions.info(card, Conditions.REGULAR, cardQuantity));
        }

        // Import deck_collection into the static deck variable.
        foreach (KeyValuePair<string, Conditions.info> cardInfo in Conditions.deck_collection)
        {
            Conditions.info currentInfo = cardInfo.Value;
            for (int i = 0; i < currentInfo.num; i++)
            {
                Conditions.deck.Add(currentInfo.card);
            }
        }
    }
}
