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

    public int deck_size;

    public GameObject deckWindow;
    public GameObject collectionWindow;
    public Text deckSizeText;
    public Canvas canvas;

    public Dictionary<string, DeckDisplayCard> deckDisplay;
    private int deckDisplayLength;
    public Dictionary<string, DeckDisplayCard> collectionDisplay;
    private int collectionDisplayLength;

    public List<CardObject> testDeck;

    public EndPrompt warningPrompt;

    // Start is called before the first frame update
    void Start()
    {
        
        // comment below when done testing
        /*
        Conditions.deck = new List<CardObject>(testDeck); 
        Conditions.deck_collection = new Dictionary<string, Conditions.info>();
        Conditions.card_collection = new Dictionary<string, Conditions.info>();

        foreach (CardObject card in Conditions.deck) { 
            if (Conditions.deck_collection.ContainsKey(card.name)) {
                Conditions.deck_collection[card.name].num++;
            } else {
                int type = Conditions.REGULAR;
                if (card.cardType == "Hero")
                {
                    type = Conditions.REGULAR;
                }
                Conditions.deck_collection.Add(card.name, new Conditions.info(card, type, 1));
            }
        }

        foreach (CardObject card in testDeck) {
            if (!Conditions.card_collection.ContainsKey(card.name)) {
                int type = Conditions.REGULAR;
                if (card.cardType == "Hero")
                {
                    type = Conditions.REGULAR;
                }
                Conditions.card_collection.Add(card.name, new Conditions.info(card, type, 0));
            }
        }*/
        // comment above when done testing
        

        deckDisplayLength = Conditions.deck_collection.Count;
        collectionDisplayLength = Conditions.card_collection.Count;

        deck_size = 0;
        foreach (KeyValuePair<string, Conditions.info> cardInfo in Conditions.deck_collection) {
            deck_size += cardInfo.Value.num;
        }

        renderDisplay(Conditions.deck_collection, deckWindow, true);
        renderDisplay(Conditions.card_collection, collectionWindow, false);

    }

    // Update is called once per frame
    void Update()
    {
        if (deckDisplayLength != Conditions.deck_collection.Count) {
            deckDisplayLength = Conditions.deck_collection.Count;
            renderDisplay(Conditions.deck_collection, deckWindow, true);
        }
        if (collectionDisplayLength != Conditions.card_collection.Count)
        {
            collectionDisplayLength = Conditions.card_collection.Count;
            renderDisplay(Conditions.card_collection, collectionWindow, false);
        }

        deckSizeText.text = "Current Deck: " + deck_size + "/" + deck_max;
        if (deck_size > deck_max) {
            deckSizeText.color = Color.red;
        } else {
            deckSizeText.color = Color.white;
        }
    }

    private void clearCards(GameObject window)
    {
        foreach (Transform child in window.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void renderDisplay(Dictionary<string, Conditions.info> dict, GameObject window, bool inDeck) {
        clearCards(window);
        deckWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(0, deckDisplayLength * displayPrefab.GetComponent<RectTransform>().sizeDelta.y);
        collectionWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(0, collectionDisplayLength * displayPrefab.GetComponent<RectTransform>().sizeDelta.y);
        int i = 0;
        foreach (KeyValuePair<string, Conditions.info> cardInfo in dict) {
            GameObject displayedCard = displayCard(cardInfo, inDeck);
            displayedCard.transform.SetParent(window.transform);
            float cardHeight = displayedCard.GetComponent<RectTransform>().sizeDelta.y;
            Vector2 position = new Vector2(0, (- 0.5f * cardHeight) - (i * cardHeight));
            displayedCard.GetComponent<RectTransform>().anchoredPosition = position;
            i++;
        }
    }

    public GameObject displayCard(KeyValuePair<string, Conditions.info> cardInfo, bool inDeck) {
        GameObject displayedCard = Instantiate(displayPrefab, canvas.transform);
        DeckDisplayCard displayInfo = displayedCard.GetComponent<DeckDisplayCard>();
        displayInfo.inDeck = inDeck;
        displayInfo.card = cardInfo.Value.card;

        return displayedCard;
    }

    public void goToDeckBuilder() {
        SceneManager.LoadScene("Deck Builder");
    }

    public void goToTransitionScreen() {
        if (deck_size < deck_min)
        {
            List<string> promptList = new List<string>();
            promptList.Add("Your deck must have at least " + deck_min + " cards.");
            warningPrompt.Setup(promptList);
        } else if (deck_size > deck_max) {
            List<string> promptList = new List<string>();
            promptList.Add("Your deck must have no more than " + deck_max + " cards.");
            warningPrompt.Setup(promptList);
        } 
        else
        {
            Conditions.saveCards();
            Conditions.CollectionToDeck(Conditions.deck_collection);
            SceneManager.LoadScene("Transition Screen");
        }
    }

}
