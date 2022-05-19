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
}
