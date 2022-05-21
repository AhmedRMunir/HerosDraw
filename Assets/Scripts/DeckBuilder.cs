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
        Conditions.deck = new List<CardObject>(testDeck); // remove when done testing

        /* 
        foreach (CardObject card in Conditions.deck) { 
            if (Conditions.deck_collection.ContainsKey(card.name)) {
                Conditions.deck_collection[card.name].num++;
            } else {
                Conditions.deck_collection.Add(card.name, new Conditions.info(card, Conditions.REGULAR, 1));
            }
        }*/

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

}
