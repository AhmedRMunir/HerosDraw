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

    public bool player_ready_for_battle;

    // AI
    public Player enemy;
    public DeckController enemy_deck;
    public GameObject enemy_lanes;
    public CardObject[] enemy_summoned_card;
    public bool enemy_ready_for_battle;

    // in memory copy of the field
    // 2 rows x 3 cols array

    public int turnNum;
    public int battleNum;

    private enum turn {
        PLAYER, ENEMY
    }

    private turn playerTurn;
    
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
        if (player_ready_for_battle && enemy_ready_for_battle) {
            onBattle();
            player_ready_for_battle = false;
            enemy_ready_for_battle = false;
            // possibly a helper function to update ready_for_battle
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

        // call clean up method to remove dead cards
    }

}
