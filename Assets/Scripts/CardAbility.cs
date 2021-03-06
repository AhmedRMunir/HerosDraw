using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
            newCard.updateCard();
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

    /* values:  idx 0 - mana updates
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

    /* values:  idx 0 - cards to draw
                idx 1 - 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index // unused
    */
    public IEnumerator drawTwins(int[] values)
    {
        
        CardObject twinMage = Resources.Load<CardObject>("Cards/Twin Mage");
        if (values[2] == 0)
        {
            gm.enemy.deck.Insert(0, twinMage);
            gm.enemy.deck.Insert(0, twinMage);
        } else
        {
            gm.player.deck.Insert(0, twinMage);
            gm.player.deck.Insert(0, twinMage);
        }
        
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

    /* values:  idx 0
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator mutate(int[] values) {
        GameObject card = gm.field[values[2], values[3]];
        GameObject cardCopy = Instantiate(card, gm.player.transform);
        CardBehavior newCard = cardCopy.GetComponent<CardBehavior>();
        newCard.isEnemy = !newCard.isEnemy;
        newCard.cardIdentity = Resources.Load<CardObject>("Cards/Golem");

        GameObject playerlanes; 
            if (values[2] == 0) // enemy card
            {
                playerlanes = gm.enemy_lanes;
            } else // player card
            {
                playerlanes = gm.player_lanes;
            }
        
        newCard.summonCard(playerlanes.transform.GetChild(values[3]).GetComponent<RectTransform>(), values[3]);
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 - max reflection
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator reflection(int[] values) {
        GameObject player_card = gm.field[values[2], values[3]];
        GameObject enemy_card = gm.field[1 - values[2], values[3]];
        if (enemy_card != null) {
            int enemyAttack = enemy_card.GetComponent<CardBehavior>().getAttack();
            int attack = Mathf.Min(enemyAttack, values[0]);
            enemy_card.GetComponent<CardBehavior>().updateStats(0, -attack);
        }
        yield return new WaitForEndOfFrame();
    }
    
    /* values:  idx 0 - Damage
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator scorch(int[] values)
    {
        int enemyRow = Mathf.Abs(values[2] - 1);
        GameObject player_card = gm.field[values[2], values[3]];
        float battleAnimTime = 1f;
        /*Sequence battleAnim = DOTween.Sequence();
        float battleAnimTime = 1f;

        if (enemy_card == null) {
            gm.attackAvatar(player_card, gm.enemy, gm.enemy_Avatar, -100, battleAnim);
        } else {

            gm.attackPawns(player_card, enemy_card, battleAnim);

            Debug.Log(enemy_card.GetComponent<CardBehavior>().getHealth());
        }

        yield return new WaitForSeconds(battleAnimTime);

        if (gm.enemy.health <= 0) {
            StartCoroutine(gm.endGame());
        }

        if (enemy_card != null && enemy_card.GetComponent<CardBehavior>().getHealth() <= 0)
        {
            gm.num_enemy_summoned_card--;
            Debug.Log("card is destoryed!");
            Destroy(gm.field[0, values[3]]);
            player_card.GetComponent<CardBehavior>().updateStats(0, enemy_card.GetComponent<CardBehavior>().getHealth());
        }*/
        gm.oneWayAttackPawn(player_card, enemyRow, values[3], values[0]);
        yield return new WaitForSeconds(battleAnimTime);
    }

    /* values:  idx 0 - health curse value
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator healthcurse(int[] values){
        yield return new WaitForSeconds(1f);
        Sequence battleAnim = DOTween.Sequence();
        GameObject pawn = gm.field[values[2], values[3]];
        Transform pawnTran = pawn.transform.GetChild(0).GetChild(7).gameObject.transform;
        CardBehavior card = pawn.GetComponent<CardBehavior>();
        battleAnim
              .Append(gm.player_Avatar.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f, 10, 1))
              .Join(gm.enemy_Avatar.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f, 10, 1))
              .AppendCallback(() => {
                  gm.updateHealth(gm.player, -values[0]);
                  gm.updateHealth(gm.enemy, -values[0]);
              })
              .Append(gm.player_Avatar.transform.DOScale(1f, 0.2f))
              .Join(gm.enemy_Avatar.transform.DOScale(1f, 0.2f));
              
              
        yield return new WaitForSeconds(1f);
    }

    /* values:  idx 0
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator revive(int[] values)
    {
        GameObject card = gm.field[values[2], values[3]];
        CardObject cardCopy = card.GetComponent<CardBehavior>().cardIdentity;
        if (values[2] == 0) // enemy card
        {
            gm.enemy.deck.Insert(0, cardCopy);
            gm.enemy.drawCard();

        } else // player card
        {
            gm.player.deck.Insert(0, cardCopy);
            gm.player.drawCard();
        }
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 - number of copies
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator divide(int[] values)
    {
        CardObject cardCopy = Resources.Load<CardObject>("Cards/Slime");
        if (values[2] == 0) // enemy card
        {
            for (int i = 0; i < values[0]; i++)
            {
                gm.enemy.deck.Insert(0, cardCopy);
                gm.enemy.drawCard();
            }

        }
        else // player card
        {
            for (int i = 0; i < values[0]; i++)
            {
                gm.player.deck.Insert(0, cardCopy);
                gm.player.drawCard();
            }
        }
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator mulligan(int[] values)
    {
        int cardsToDraw = 0;
        if (values[2] == 0) // enemy card
        {
            while (gm.enemy.hand.Count > 0)
            {
                GameObject card = gm.enemy.hand[0];
                gm.enemy.deck.Insert(0, card.GetComponent<CardBehavior>().cardIdentity);
                cardsToDraw++;
                gm.enemy.hand.Remove(card);
            }
            foreach (Transform child in gm.enemy.handHolder.transform)
            {
                child.gameObject.GetComponent<CardBehavior>().hasOnDestroy = false;
                Destroy(child.gameObject);
            }
            gm.enemy.shuffle();
            for (int i = 0; i < cardsToDraw; i++)
            {
                gm.enemy.drawCard();
            }
            gm.enemy.shiftHand(gm.enemy.cardSpeed);

        }
        else // player card
        {
            while (gm.player.hand.Count > 0)
            {
                GameObject card = gm.player.hand[0];
                gm.player.deck.Insert(0, card.GetComponent<CardBehavior>().cardIdentity);
                cardsToDraw++;
                gm.player.hand.Remove(card);
            }
            foreach (Transform child in gm.player.handHolder.transform)
            {
                child.gameObject.GetComponent<CardBehavior>().hasOnDestroy = false;
                Destroy(child.gameObject);
            }
            gm.player.shuffle();
            for (int i = 0; i < cardsToDraw; i++)
            {
                gm.player.drawCard();
            }
            gm.player.shiftHand(gm.player.cardSpeed);
            gm.player_can_pass = gm.playerHasPlayable();
        }
        yield return new WaitForEndOfFrame();
    }

    /* values:  idx 0 
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator deathTouch(int[] values)
    {
        GameObject enemyCard = gm.field[1 - values[2], values[3]];
        if (enemyCard != null)
        {
            yield return new WaitForEndOfFrame();
            CardBehavior cardInfo = enemyCard.GetComponent<CardBehavior>();
            cardInfo.updateStats(0, -cardInfo.getHealth());
        }
        yield return new WaitForEndOfFrame();
    }

    /* 
     * values:  idx 0 - Siphon value
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator siphon(int[] values)
    {
        GameObject card = gm.field[values[2], values[3]];
        int boost = 0;
        for (int i = 0; i < gm.field.GetLength(1); i++)
        {
            GameObject player_card = gm.field[1, i];
            GameObject enemy_card = gm.field[0, i];
            if (player_card != null && (values[2] != 1 || i != values[3]))
            {
                player_card.GetComponent<CardBehavior>().updateStats(0, -values[0]);
                boost += values[0];
            }

            if (enemy_card != null && (values[2] != 0 || i != values[3]))
            {
                enemy_card.GetComponent<CardBehavior>().updateStats(0, -values[0]);
                boost += values[0];
            }

        }

        card.GetComponent<CardBehavior>().updateStats(boost, boost);
        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < gm.field.GetLength(1); i++)
        {
            GameObject player_card = gm.field[1, i];
            GameObject enemy_card = gm.field[0, i];

            if (player_card != null && player_card.GetComponent<CardBehavior>().getHealth() <= 0)
            {
                gm.num_player_summoned_card--;
                Destroy(gm.field[1, i]);
            }

            if (enemy_card != null && enemy_card.GetComponent<CardBehavior>().getHealth() <= 0)
            {
                gm.num_enemy_summoned_card--;
                Debug.Log("card is destoryed!");
                Destroy(gm.field[0, i]);
            }
        }

        yield return new WaitForEndOfFrame();
    }
    public IEnumerator trick(int[] values){
        GameObject player_card = gm.field[values[2], values[3]];
        GameObject enemy_card = gm.field[1 - values[2], values[3]];
        if (enemy_card != null) {
            CardBehavior player_cardbehavior = player_card.GetComponent<CardBehavior>();
            CardBehavior enemy_cardbehavior = enemy_card.GetComponent<CardBehavior>();
            CardObject player_cardIdentity = player_cardbehavior.cardIdentity;
            CardObject enemy_cardIdentity = enemy_cardbehavior.cardIdentity;
            List<int> player_abilityParams = new List<int>(player_cardbehavior.abilityParams);
            List<int> enemy_abilityParams = new List<int>(enemy_cardbehavior.abilityParams);
            /*int playerAttack = player_cardbehavior.getAttack();
            int playerHealth = player_cardbehavior.getHealth();
            int enemyAttack = enemy_cardbehavior.getAttack();
            int enemyHealth = enemy_cardbehavior.getHealth();
            player_cardbehavior.updateStats(enemyAttack - playerAttack, enemyHealth - playerHealth);
            enemy_cardbehavior.updateStats(playerAttack - enemyAttack, playerHealth - enemyHealth);*/
            player_cardbehavior.cardIdentity = enemy_cardIdentity;
            enemy_cardbehavior.cardIdentity = player_cardIdentity;
            player_cardbehavior.updateCard();
            enemy_cardbehavior.updateCard();
            player_cardbehavior.abilityParams = new List<int>(player_abilityParams);
            enemy_cardbehavior.abilityParams = new List<int>(enemy_abilityParams);

        }
        yield return new WaitForEndOfFrame();
    }

    /* Deal damage to all pawns
     * values:  idx 0 - damage
                idx 1 
                idx 2 - field row; 0 if enemy, 1 if player
                idx 3 - lane index
    */
    public IEnumerator explode(int[] values)
    {
        for (int i = 0; i < gm.field.GetLength(1); i++)
        {
            GameObject player_card = gm.field[1, i];
            GameObject enemy_card = gm.field[0, i];
            if (player_card != null)
            {
                player_card.GetComponent<CardBehavior>().updateStats(0, -values[0]);
            }

            if (enemy_card != null)
            {
                enemy_card.GetComponent<CardBehavior>().updateStats(0, -values[0]);
            }

        }

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < gm.field.GetLength(1); i++)
        {
            GameObject player_card = gm.field[1, i];
            GameObject enemy_card = gm.field[0, i];

            if (player_card != null && player_card.GetComponent<CardBehavior>().getHealth() <= 0)
            {
                gm.num_player_summoned_card--;
                Destroy(gm.field[1, i]);
            }

            if (enemy_card != null && enemy_card.GetComponent<CardBehavior>().getHealth() <= 0)
            {
                gm.num_enemy_summoned_card--;
                Destroy(gm.field[0, i]);
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
