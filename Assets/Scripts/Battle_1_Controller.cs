using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Battle_1_Controller : GameController
{

    public new IEnumerator enemyTurn() {
        passTurnSpinner.transform.DORotate(new Vector3(0, 0, 180f), 0.75f);
        if (!player_ready_for_battle) {
            yield return new WaitForSeconds(0.75f);
        }
        current_turn = turn.ENEMY;
        if (num_enemy_summoned_card == enemy_lanes.transform.childCount || enemy.hand.Count == 0) {
            // the field is full or the hand is empty
            enemy_has_summoned = true;

            // if cannot play, ready for battle
            enemy_ready_for_battle = true;

            if (player_ready_for_battle)
            {
                StartCoroutine("onBattle");
            } else
            {
                turnNum++;
                StartCoroutine("playerTurn");
            }
           
        } else
        {
            // Enemy AI
            // enemy_play_card_first_open_lane();
            // enemy_play_card_first_block_lane();
            // enemy_play_card_block_strongest_on_field();
            enemy_play_card();

            enemy_has_summoned = true;
            yield return new WaitForSeconds(0.5f);

            turnNum++;
            if (!player_ready_for_battle)
            {
                StartCoroutine(playerTurn());
            }
            else
            {
                StartCoroutine(enemyTurn());
            }
        }
        
    }

    private void enemy_play_card() {
        switch(battleNum) {
            case 1:
                summon_card(0, 2, get_playable_cards(0)[0]);
                break;
            case 2:
                switch (turnNum) {
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                } 
            break;
            default:
                enemy_play_card_first_open_lane();
                break;
        }
    }
}
