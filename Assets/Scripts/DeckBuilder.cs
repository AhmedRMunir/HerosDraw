using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    // a hashtable for the player's current deck
    public Hashtable deck;

    // a hashtable for the player's complete collection of cards
    public Hashtable collection;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
