using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Player
    public Player player;
    public DeckController player_deck;

    public GameObject player_lanes;

    public CardObject[] player_summoned_card;

    public bool player_has_summoned;

    // animation flag
    public bool player_can_play;

    public bool player_ready_for_battle;

    // AI
    public Player enemy;
    public DeckController enemy_deck;
    public GameObject enemy_lanes;
    public CardObject[] enemy_summoned_card;
    public bool enemy_ready_for_battle;
    public bool enemy_has_summoned;

    // an array of flags indicating the field
    public bool[][] field;

    public int turnNum;
    public int battleNum;

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

        /*
        Make button for passing turns and ready for battle
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (current_turn == turn.PLAYER && player_ready_for_battle != true) {

            if (player_has_summoned == true) {
                // highlight pass turn
            }

            if (player_summoned_card.Length == player_lanes.transform.childCount) {
                player_has_summoned = true;
                // highlight Ready for Battle, disable Pass Turn
            }

            enemy_has_summoned = false;

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
        }

        // separate script for button "Pass Turn"
        // onPointerClick() -> set current_turn = ENEMY

        // separate script for button "Ready For Battle"
        // onPointerClick() -> set player_ready_for_battle = true

        if (current_turn == turn.ENEMY && enemy_ready_for_battle != true) {
            // enemy makes move; returned by enemy AI script
            // do the necessary updates according to the move

            // after enemy made move, current_turn = PLAYER

            // if enemy cannot move, enemy_ready_for_battle = true;

            // at end of enemy turn, set player_has_played = false
        }

        // after enemy has made a move, set current_turn = PLAYER

        if (player_ready_for_battle && enemy_ready_for_battle) {
            onBattle();
            player_ready_for_battle = false;
            enemy_ready_for_battle = false;
        }
    }

    private int updateHealth(Player player, int change) {
        player.health += change;
        return player.health;
    }

    private int updateMana(Player player, int change) {
        player.mana += change;
        return player.mana;
    }

    private void onBattle() {
        // iterate through cards on the field
        // update card values post damage
        // update player and enemy avatar health
        for (int i = 0; i < player_summoned_card.Length; i++) {
            CardObject player_card = player_summoned_card[i];
            CardObject enemy_card = enemy_summoned_card[i];

            // instead of manually updating health, should make a function
            // take into consideration of card's ability, e.g. double attack
            if (player_card == null) {
                player.health -= enemy_card.attack;
            } else {
                player_card.health -= enemy_card.health;
            }
            
            if (enemy_card == null) {
                enemy.health -= player_card.attack;
            } else {
                enemy_card.health -= player_card.health;
            }
        }

        // second iteration; 
        for (int i = 0; i < player_summoned_card.Length; i++) {
            CardObject player_card = player_summoned_card[i];
            CardObject enemy_card = enemy_summoned_card[i];

            if (player_card.health <= 0) {
                Destroy(player_card);
            }

            if (enemy_card.health <= 0) {
                Destroy(enemy_card);
            }
        }

        battleNum += 1;

        // need a boolean to check which player should start next turn
        // set current_turn to that player accordingly
    }

}
