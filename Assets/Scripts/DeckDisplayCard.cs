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
    } 
}
