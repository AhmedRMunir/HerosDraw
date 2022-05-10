using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    public GameController gm;

    /*
    A class that houses all card abilities.
    passiveAbility() is invoked when the card is summoned, triggering the card's passive
    activeAbility() is invoked when the card is selected during a player's turn, if the card's active is usable

    for all abilityParams, the last two index will always be: 
        - field row num (0 for enemy, 1 for player)
        - lane index
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void passiveAbility(string cardAbility, int[] abilityParams) {
        StartCoroutine(cardAbility, abilityParams);
    }

    public void activeAbility(string cardAbility, int[] abilityParams) {
        StartCoroutine(cardAbility, abilityParams);
    }

    /* values:  idx 0 - attack updates
                idx 1 - health updates
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator reinforce(int[] values) {
        yield return new WaitForEndOfFrame();
        Debug.Log(values[2]);
        for (int i = 0; i < gm.field.GetLength(1); i++) {
            GameObject card = gm.field[values[2],i];
            if (card != null) {
                Debug.Log(card.GetComponent<CardBehavior>().nameText.text);
                card.GetComponent<CardBehavior>().updateStats(values[0], values[1]);
            }
        }
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 - health updates
                idx 1 - mana cost
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator invigorate(int[] values) {
        yield return new WaitForEndOfFrame();
        //GameObject card = gm.field[values[2], values[3]];
        gm.player.mana -= values[1];
        if (values[2] == 1) // Player
        {
            gm.updateHealth(gm.player, values[0]);
        } else // Enemy
        {
            gm.updateHealth(gm.enemy, values[0]);
        }
        
        yield return new WaitForEndOfFrame();
    }
}
