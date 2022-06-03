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

    /* values:  idx 0 - attack of recruit
                idx 1 - health of recruit
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
                idx 4 - faction id (0 = knight, 1 = mage, 2 = vampire)
                idx 5 - mana cost
    */
    public IEnumerator recruit(int[] values)
    {
        if (gm.num_player_summoned_card != gm.field.GetLength(1))
        {
            gm.player.mana -= values[5];
            GameObject card = gm.field[values[2], values[3]];
            GameObject cardCopy = Instantiate(card, GameObject.FindGameObjectWithTag("PlayerHand").transform);
            CardBehavior newCard = cardCopy.GetComponent<CardBehavior>();
            if (values[4] == 0)
            {
                newCard.cardIdentity = Resources.Load<CardObject>("Cards/Recruit");
            } else if (values[4] == 5) // Teacher
            {
                newCard.cardIdentity = Resources.Load<CardObject>("Cards/Apprentice");
            }
            yield return new WaitForEndOfFrame();
            Debug.Log(values[0]);
            newCard.setCost(0);
            newCard.updateStats(values[0], values[1]);
            newCard.enterSelection(false);
        }

        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 - attack of recruit
                idx 1 - health of recruit
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
                idx 4 - faction id (0 = knight, 1 = mage, 2 = vampire, 10 = tiamat)
    */
    public IEnumerator army(int[] values)
    {
        if (gm.num_player_summoned_card != gm.field.GetLength(1) || values[4] == 10)
        {
            GameObject card = gm.field[values[2], values[3]];

            GameObject playerlanes; 
            if (values[2] == 0) // enemy card
            {
                playerlanes = gm.enemy_lanes;
            } else // player card
            {
                playerlanes = gm.player_lanes;
            }

            // Tiamat destroys your board and then summons heads
            if (values[4] == 10)
            {
                for (int i = 0; i < gm.field.GetLength(1); i++)
                {
                    GameObject curr_card = gm.field[values[2], i];
                    if (curr_card != null && values[2] == 1 && i != values[3])
                    {
                        gm.num_player_summoned_card--;
                        Destroy(gm.field[1, i]);
                    } else if (curr_card != null && values[2] == 0 && i != values[3])
                    {
                        gm.num_enemy_summoned_card--;
                        Destroy(gm.field[0, i]);
                    }

                }
            }
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < gm.field.GetLength(1); i++)
            {
                
                if (gm.field[values[2], i] == null)
                {
                    GameObject cardCopy = Instantiate(card, GameObject.FindGameObjectWithTag("PlayerHand").transform);
                    CardBehavior newCard = cardCopy.GetComponent<CardBehavior>();
                    if (values[4] == 0)
                    {
                        newCard.cardType = "";
                        newCard.cardBG.sprite = Resources.Load<Sprite>("Sprites/Card");
                        newCard.cardIdentity = Resources.Load<CardObject>("Cards/Recruit");
                    } else if (values[4] == 10)
                    {
                        newCard.cardType = "";
                        newCard.cardIdentity = Resources.Load<CardObject>("Cards/Head of Tiamat");
                    }
                    yield return new WaitForEndOfFrame();
                    newCard.updateStats(values[0], values[1]);

                    newCard.summonCard(playerlanes.transform.GetChild(i).GetComponent<RectTransform>(), i);
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
    public IEnumerator slifer(int[] values)
    {
        GameObject card = gm.field[values[2], values[3]];
        CardBehavior cardInfo = card.GetComponent<CardBehavior>();
        int cardsInHand = 0;
        if (values[2] == 0) // Enemy card
        {
            cardsInHand = gm.enemy.hand.Count;
        } else // Player card
        {
            cardsInHand = gm.player.hand.Count;
        }

        cardInfo.updateStats(cardsInHand, cardsInHand);

        yield return new WaitForEndOfFrame();
    }

    /* Destroy all other cards on the board
     * values:  idx 0 
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator extinction(int[] values)
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < gm.field.GetLength(1); i++)
        {
        GameObject player_card = gm.field[1, i];
        GameObject enemy_card = gm.field[0, i];
            if (player_card != null && (values[2] != 1 || i != values[3]))
            {
                gm.num_player_summoned_card--;
                Destroy(gm.field[1, i]);
            }

            if (enemy_card != null && (values[2] != 0 || i != values[3]))
            {
                gm.num_enemy_summoned_card--;
                Destroy(gm.field[0, i]);
            }
            
        }
    }

    /* values:  idx 0 - faction id (0 = knight, 1 = mage, 2 = vampire)
                idx 1 - attack/health updates
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator twinBoost(int[] values) {

        yield return new WaitForEndOfFrame();
        Debug.Log(values[2]);
        bool found = false;
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
                if (currCard.GetComponent<CardBehavior>().getName() == card.GetComponent<CardBehavior>().getName()) {
                    found = true;
                    currCard.GetComponent<CardBehavior>().updateStats(1, 1);
                }
            }
        }
        /*if (found) {
            card.GetComponent<CardBehavior>().updateStats(1, 1);
        }*/
        yield return new WaitForEndOfFrame();

    }

    /* values:  idx 0 - health updates
                idx 1 - mana cost
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator increaseMana(int[] values) {
        yield return new WaitForEndOfFrame();
        //GameObject card = gm.field[values[2], values[3]];
        gm.player.mana -= values[1];
        if (values[2] == 1) // Player
        {
            gm.updateMana(gm.player, values[0]);
        } else // Enemy
        {
            gm.updateMana(gm.enemy, values[0]);
        }

        if (values[2] == 1)
        {
            gm.player_can_pass = gm.playerHasPlayable();
        }

        yield return new WaitForEndOfFrame();
    }
}
