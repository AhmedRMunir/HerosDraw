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
        gm = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
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

    /* values:  idx 0 - attack updates
                idx 1 - health updates
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
                idx 4 - faction id (0 = knight, 1 = mage, 2 = vampire)
    */
    public IEnumerator reinforceOther(int[] values)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(values[2]);
        for (int i = 0; i < gm.field.GetLength(1); i++)
        {
            GameObject card = gm.field[values[2], i];
            if (card != null && i != values[3] && card.GetComponent<CardBehavior>().getFaction() == values[4])
            {
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

    /* values:  idx 0 - faction id (0 = knight, 1 = mage, 2 = vampire)
                idx 1 - attack/health updates
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator solidarity(int[] values)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(values[2]);
        int boost = 0;
        GameObject card = gm.field[values[2], values[3]];
        for (int i = 0; i < gm.field.GetLength(1); i++)
        {
            if (i == values[3])
            {
                continue;
            }
            GameObject currCard = gm.field[values[2], i];
            if (currCard != null)
            {
                if (currCard.GetComponent<CardBehavior>().getFaction() == values[0])
                    boost += values[1];
            }
        }
        card.GetComponent<CardBehavior>().updateStats(boost, boost);
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 - cards to draw
                idx 1 - 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index // unused
    */
    public IEnumerator draw(int[] values)
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < values[0]; i++)
        {
            if (values[2] == 0) // Enemy draw
            {
                gm.enemy.drawCard();
            } else // Player draw
            {
                gm.player.drawCard();
            }
        }
        if (values[2] == 1)
        {
            gm.player_can_pass = gm.playerHasPlayable(); 
        }
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 - cards to draw
                idx 1 - mana cost
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index // unused
    */
    public IEnumerator drawActive(int[] values)
    {
        yield return new WaitForEndOfFrame();
        gm.player.mana -= values[1];
        for (int i = 0; i < values[0]; i++)
        {
            if (values[2] == 0) // Enemy draw
            {
                gm.enemy.drawCard();
            }
            else // Player draw
            {
                gm.player.drawCard();
            }
        }
        if (values[2] == 1)
        {
            gm.player_can_pass = gm.playerHasPlayable();
        }
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator pierce(int[] values)
    {
        yield return new WaitForEndOfFrame();
        GameObject card = gm.field[values[2], values[3]];
        int enemyRow = Mathf.Abs(values[2] - 1);
        if (gm.field[enemyRow, values[3]] != null)
        {
            GameObject enemyCard = gm.field[enemyRow, values[3]];
            int playerAttack = card.GetComponent<CardBehavior>().getAttack();
            Debug.Log(playerAttack);
            int enemyHealth = enemyCard.GetComponent<CardBehavior>().getHealth();
            Debug.Log(enemyHealth);
            if (playerAttack > enemyHealth)
            {
                int damage = playerAttack - enemyHealth;
                if (values[2] == 0) // enemy card ability
                {
                    gm.updateHealth(gm.player, -damage);
                } else // player card ability
                {
                    gm.updateHealth(gm.enemy, -damage);
                }
            }
        }
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator lifesteal(int[] values)
    {
        yield return new WaitForEndOfFrame();
        GameObject card = gm.field[values[2], values[3]];
        int attack = card.GetComponent<CardBehavior>().getAttack();
        if (values[2] == 0) // enemy card ability
        {
            gm.updateHealth(gm.enemy, attack);
        }
        else // player card ability
        {
            gm.updateHealth(gm.player, attack);
        }
        yield return new WaitForEndOfFrame();
    }
}
