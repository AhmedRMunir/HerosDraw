using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



/*
 Tutorial - 1 - Controller
 Set player and enemy HP to 3 HP

Enemy Deck:
 - 1A-1H
 - 1A-2H
 - Knight
 - Wizard


Player Deck:
 - 1C - 1A - 2H
 - Double Attack
 - Fireball
 - Knight
 - Champion
 
*/

public class Tutorial_2_Controller : GameController
{

    public override IEnumerator gameStart()
    {
        StartCoroutine(LoadingController.LOGGER.LogLevelStart(1, "{ User entered tutorial 2 }"));

        player.maxMana = 1;
        player.mana = 1;
        enemy.maxMana = 1;
        enemy.mana = 1;

        player.health = 3;
        enemy.health = 3;

        yield return new WaitForSeconds(0.5f);
        player.handSize = handStartSize;
        enemy.handSize = handStartSize;
        // player.shuffle();
        player.drawHand();
        // enemy.shuffle();
        enemy.drawHand();

        if (current_turn == turn.PLAYER) {
            next_player = turn.ENEMY;
        } else {
            next_player = turn.PLAYER;
        }

        yield return new WaitForSeconds(1f);

        if (current_turn == turn.PLAYER)
        {
            //StartCoroutine("playerTurn");
            StartCoroutine(indicateTurn("playerTurn"));
        }
        else
        {
            //StartCoroutine("enemyTurn");
            StartCoroutine(indicateTurn("enemyTurn"));
        }
    }

    // public override void displayDialog()
    // {
    //     switch(battleNum) {
    //         case 1:
    //             if (turnNum == 2)
    //             {
    //                 dialogPrompt.Setup("Time to Learn about Actives, but first play a good first card");
    //             }   
    //             break;
    //         case 2:
    //             if (turnNum == 1) {
    //                 dialogPrompt.Setup("You're allowed one free pass per Battle. Use it!");
    //                 for (int i = 0; i < player.handSize; i++)
    //                 {
    //                     player.hand[i].GetComponent<CardBehavior>().summoned = true;
    //                 }
                    
    //             } else if (turnNum == 2) {
    //                 player.hand[player.handSize - 1].GetComponent<CardBehavior>().summoned = false;
    //                 dialogPrompt.Setup("Play the Shaman and use its active");
    //             }
    //             break;
    //         case 3:
    //             if (turnNum == 2)
    //             {
    //                 dialogPrompt.Setup("See the game out!");
    //                 for(int i = 0; i < player.handSize; i++)
    //                 {
    //                     player.hand[i].GetComponent<CardBehavior>().summoned = false;
    //                 }
    //             }
    //             break;
    //         default:
    //             break;
    //     }
    // }

    // public override void enemy_play_card() {
    //     Debug.Log(battleNum + ", " + turnNum);
    //     switch(battleNum) {
    //         case 1:
    //             Debug.Log("playing card in 2nd position");
    //             summon_card(0, 2, get_playable_cards(0)[0]);
    //             enemy_ready_for_battle = true;
    //             break;
    //         case 2:
    //             switch(turnNum) {
    //                 case 1:
    //                     summon_card(0, 1, get_playable_cards(0)[0]);
    //                     break;
    //                 case 2:
    //                     if (get_playable_cards(0).Count != 0)
    //                     summon_card(0, 2, get_playable_cards(0)[0]);
    //                     break;
    //             }
    //             break;
    //         default:
    //             enemy_play_card_first_open_lane();
    //             break;
    //     }
    // }
}
