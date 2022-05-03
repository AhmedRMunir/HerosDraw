using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    // Player
    //public Player player;
    public PlayerController player;
    public GameObject player_lanes;
    public int num_player_summoned_card;
    public bool player_has_summoned;
    public bool player_ready_for_battle;

    // animation flag
    public bool player_can_play;

    // AI
    //public Player enemy;
    public PlayerController enemy;
    public GameObject enemy_lanes;
    public int num_enemy_summoned_card;
    public bool enemy_ready_for_battle;
    public bool enemy_has_summoned;

    // 2d field
    public GameObject[,] field;

    public int turnNum;
    public int battleNum;

    public GameObject passTurnSpinner;

    public enum turn {
        PLAYER, ENEMY
    }

    public turn current_turn;
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        * Instantiate the players
        * Function for drawing one card
        * Update Health
        * Update Mana
        * Update Field
        */

        field = new GameObject[2, Conditions.maxLanes];
        if (current_turn == turn.PLAYER)
        {
            StartCoroutine(playerTurn());
        } else
        {
            StartCoroutine(enemyTurn());
        }
       
    }

    public IEnumerator playerTurn()
    {
        passTurnSpinner.transform.DORotate(new Vector3(0, 0, 0), 1f);
        yield return new WaitForSeconds(1f);
        current_turn = turn.PLAYER;
        player_has_summoned = (num_player_summoned_card == field.GetLength(1));
        yield return new WaitForEndOfFrame();

        if (player_ready_for_battle == false && current_turn == turn.PLAYER)
        {
            if (enemy_ready_for_battle)
            {
                player_can_play = true;
            } else
            {
                if (player_has_summoned || player.mana == 0)
                {
                    player_can_play = false;
                } else
                {
                    player_can_play = true;
                }
            }
        } else if (player_ready_for_battle)
        {
            StartCoroutine(enemyTurn());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (current_turn == turn.PLAYER) {

            if (player_ready_for_battle == true)
            {
                player_can_play = false;
                current_turn = turn.ENEMY;
                return;
            }

            if (player_has_summoned == true) {
                // highlight pass turn
            }

            if (num_player_summoned_card == player_lanes.transform.childCount) {
                player_has_summoned = true;
                // highlight Ready for Battle, disable Pass Turn
            }

            enemy_has_summoned = false;*/

            // player is allowed to click a card to an available lane
            // code for moving the card to the lane
            // update the flag array

            // CardBehavior.cs onSummon function set player_has_played to true.
            // CardBehavior.cs should check player_has_played; if true, cannot summon.
            
            // loop through player_summoned_card array, highlight all cards that have active abilities.
            // in CardBehavior.cs, check if player has enough cost to use active ability.

            /*  if (player_has_played = true) {
                    highlight Pass Turn button
                }*/

            // if (player_summoned_card.length() == player_lanes.transform.childCount()) {
                // player_has_played = true;
                // highlight Ready For Battle, and disable Pass Turn
            //}

            // enemy_has_played = false;
        //}

        // separate script for button "Pass Turn"
        // onPointerClick() -> set current_turn = ENEMY

        // separate script for button "Ready For Battle"
        // onPointerClick() -> set player_ready_for_battle = true

        /*if (current_turn == turn.ENEMY && enemy_ready_for_battle != true) {
            enemyTurn();

            player_has_summoned = false;
            // enemy makes move; returned by enemy AI script
            // do the necessary updates according to the move

            // after enemy made move, current_turn = PLAYER

            // if enemy cannot move, enemy_ready_for_battle = true;

            // at end of enemy turn, set player_has_played = false
        }*/

        // after enemy has made a move, set current_turn = PLAYER

        /*if (player_ready_for_battle && enemy_ready_for_battle) {
            StartCoroutine(onBattle());
        }*/
    }

    public void updateHealth(PlayerController player, int change) {
        player.health += change;
    }

    public void updateMana(PlayerController player, int change) {
        player.mana += change;
    }

    public IEnumerator onBattle() {
        // iterate through cards on the field
        // update card values post damage
        // update player and enemy avatar health
        Sequence attack = DOTween.Sequence();
        //attack.Pause();

        for (int i = 0; i < field.GetLength(1); i++) {
            Debug.Log(i);
            GameObject player_card = field[1,i];
            GameObject enemy_card = field[0,i];
            

            // instead of manually updating health, should make a function
            // take into consideration of card's ability, e.g. double attack
            if (player_card == null && enemy_card == null) {
                continue;
            } else if (player_card == null && enemy_card != null) {
                RectTransform enemyTran = enemy_card.GetComponent<RectTransform>();
                attack.Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y + 100), 0.25f))
                    .Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y - 100), 0.125f))
                    .AppendCallback(() => { updateHealth(player, -enemy_card.GetComponent<CardBehavior>().getAttack()); })
                    .Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y), 0.125f));
                
            } else if (player_card != null && enemy_card == null) {
                RectTransform playerTran = player_card.GetComponent<RectTransform>();
                attack.Append(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y - 100), 0.25f))
                    .Append(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y + 100), 0.125f))
                    .AppendCallback(() => { updateHealth(enemy, -player_card.GetComponent<CardBehavior>().getAttack()); })
                    .Append(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y), 0.125f));
           
            } else {
                RectTransform enemyTran = enemy_card.GetComponent<RectTransform>();
                RectTransform playerTran = player_card.GetComponent<RectTransform>();
                attack.Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y + 100), 0.25f))
                    .Join(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y - 100), 0.25f))
                    .Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y - 100), 0.125f))
                    .Join(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y + 100), 0.125f))
                    .AppendCallback(() =>
                    {
                        player_card.GetComponent<CardBehavior>().updateStats(0, -enemy_card.GetComponent<CardBehavior>().getAttack());
                        enemy_card.GetComponent<CardBehavior>().updateStats(0, -player_card.GetComponent<CardBehavior>().getAttack());
                    })
                    .AppendCallback(() => { updateHealth(player, -enemy_card.GetComponent<CardBehavior>().getAttack()); })
                    .Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y), 0.125f))
                    .Join(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y), 0.125f));
                
            }
        }

        attack.AppendCallback(() =>
        {
            // second iteration; 
            for (int i = 0; i < field.GetLength(1); i++)
            {
                GameObject player_card = field[1, i];
                GameObject enemy_card = field[0, i];

                if (player_card != null && player_card.GetComponent<CardBehavior>().getHealth() <= 0)
                {
                    Destroy(field[1, i]);
                }

                if (enemy_card != null && enemy_card.GetComponent<CardBehavior>().getHealth() <= 0)
                {
                    Debug.Log("card is destoryed!");
                    Destroy(field[0, i]);
                }
            }

            battleNum += 1;
            turnNum = 1;
            
        });

        yield return new WaitForSeconds(1f);

        StartCoroutine(newRound());
        
    }

    public IEnumerator enemyTurn() {
        passTurnSpinner.transform.DORotate(new Vector3(0, 0, 180f), 1f);
        if (!player_ready_for_battle) {
            yield return new WaitForSeconds(1f);
        }
        current_turn = turn.ENEMY;
        if (num_enemy_summoned_card == enemy_lanes.transform.childCount || enemy.hand.Count == 0) {
            // the field is full or the hand is empty
            enemy_has_summoned = true;

            // if cannot play, ready for battle
            enemy_ready_for_battle = true;

            if (player_ready_for_battle)
            {
                StartCoroutine(onBattle());
            } else
            {
                turnNum++;
                StartCoroutine(playerTurn());
            }
           
        }

        for (int i = 0; i < enemy.hand.Count; i++) {
            GameObject card = enemy.hand[i];
            Debug.Log("First loop, index " + i);
            if (card.GetComponent<CardBehavior>().getCost() < enemy.mana) {
                // play the card onto the empty lane
                for (int j = 0; j < field.GetLength(1); j++) {
                    Debug.Log("Second loop, index " + j);
                    if (field[0, j] == null) {
                        field[0, j] = card;
                        enemy.hand.Remove(card);
                        num_enemy_summoned_card ++;
                        card.GetComponent<CardBehavior>().summonCard(enemy_lanes.transform.GetChild(j).GetComponent<RectTransform>(), j);
                        Debug.Log("enemy plays " + card.GetComponent<CardBehavior>().nameText.text + " at lane " + j);
                        break;
                    }
                }
                break;
            }
        }

        enemy_has_summoned = true;
        yield return new WaitForSeconds(1f);

        turnNum++;
        if (!player_ready_for_battle)
        {
            StartCoroutine(playerTurn());
        } else
        {
            StartCoroutine(enemyTurn());
        }
        
    }

    public IEnumerator newRound()
    {
        player_ready_for_battle = false;
        enemy_ready_for_battle = false;
        //player.drawCard();
        //enemy.drawCard();

        yield return new WaitForSeconds(1f);

        if (battleNum % 2 == 0)
        {
            StartCoroutine(enemyTurn());
        }
        else
        {
            StartCoroutine(playerTurn());
        }
    }

}
