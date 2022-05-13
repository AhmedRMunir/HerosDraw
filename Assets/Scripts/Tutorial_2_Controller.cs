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
    public PassTurn passTurn;
    public override IEnumerator gameStart()
    {
        if (Conditions.collectingData)
            StartCoroutine(LoadingController.LOGGER.LogLevelStart(levelID, "{ User entered tutorial 2 }"));

        player.maxMana = 1;
        player.mana = 1;
        enemy.maxMana = 1;
        enemy.mana = 1;

        player.health = 3;
        enemy.health = 3;

        yield return new WaitForSeconds(0.5f);


        List<string> promptList = new List<string>();
        promptList.Add("I see you won your first game!\nTime for another.");
        dialogPrompt.Setup(promptList);

        yield return new WaitUntil(() => dialogPrompt.pressed);
        dialogPrompt.pressed = false;

        player.handSize = handStartSize;
        enemy.handSize = handStartSize;
        player.drawHand();
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

    public override IEnumerator displayDialog() {
        List<string> promptList = new List<string>();
        switch(battleNum) {
            case 1:
                if (turnNum == 2) {
                    // Strategize with weaker card
                    promptList.Add("Time to strategise, but you have to trust me.");
                    promptList.Add("Play a Knight card in a lane where the enemy doesn't have a card.");
                    dialogPrompt.Setup(promptList);
                    // TODO: Highlight lanes index 1, 2, 3, 4
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    player_lanes.transform.GetChild(0).gameObject.SetActive(false);
                    ready_Button.GetComponent<ReadyForBattle>().enabled = false;

                    yield return new WaitUntil(() => player_has_summoned);

                    ready_Button.GetComponent<ReadyForBattle>().enabled = true;
                }   
                break;
            case 2:
                if (turnNum == 1) {
                    promptList.Add("See, it wasn't so bad. You can deal damage and take damage at the same time.");
                    // Free pass
                    promptList.Add("Time for a new mechanic.");
                    promptList.Add("Each Battle, you can pass your turn one time without summoning.");
                    promptList.Add("You can use this to counter your enemy's moves.");
                    promptList.Add("Pass your turn without playing a card, so we can see what the enemy is planning for us.");
                    dialogPrompt.Setup(promptList);
                    // TODO: Highlight Pass Turn Button
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    ready_Button.GetComponent<ReadyForBattle>().enabled = false;

                    for (int i = 0; i < player.handSize; i++)
                    {
                        player.hand[i].GetComponent<CardBehavior>().summoned = true;
                    }
                    // Shaman is the 2nd card in hand.
                    // Jank way to prevent player from interacting with the Shaman card in hand
                    player.hand[1].GetComponent<CardBehavior>().cardIdentity.hasActiveAbility = false;

                } else if (turnNum == 2) {
                                 
                    player.hand[1].GetComponent<CardBehavior>().summoned = false;
                    player.hand[1].GetComponent<CardBehavior>().cardIdentity.hasActiveAbility = true;
                    passTurn.enabled = false;
                    promptList.Add("Aha. The enemy is scared of taking more damage so we got blocked.");
                    promptList.Add("Let's carry on with our strategy and play in an open lane.");

                    // Active Card
                    promptList.Add("The Pawn we drew this turn has an Active Ability.");
                    promptList.Add("Unlike Passives, Actives must be activated by the player once the Pawn has been summoned.");
                    promptList.Add("Additionally, you can keep using Actives can be used as long as you can pay the cost.");
                    promptList.Add("Play the Shaman card in an open lane, then let's use it's Active Ability.");
                    dialogPrompt.Setup(promptList);
                    // TODO: Highlight Shaman Card
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;

                    yield return new WaitUntil(() => player_has_summoned);

                    // TODO: Wait on the Shaman card to be played.
                    promptList.Add("Click on it to use it's active to increase your HP at the cost of 1 Mana.");
                    dialogPrompt.Setup(promptList);
                    // TODO: Highlight Shaman Card
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;

                    yield return new WaitUntil(() => player.mana == 0);

                    // Go to battle
                    promptList.Add("Now that you're healed up a bit, let's go to Battle");
                    dialogPrompt.Setup(promptList);
                    // TODO: 
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;

                    player.hand[0].GetComponent<CardBehavior>().summoned = false;
                    ready_Button.GetComponent<ReadyForBattle>().enabled = true;
                }
                break;
            case 3:
                if (turnNum == 2)
                {
                    // Free pass
                    passTurn.enabled = true;
                    player_lanes.transform.GetChild(0).gameObject.SetActive(true);
                    player.hand[0].GetComponent<CardBehavior>().removeIndicators();
                    promptList.Add("Now that you have an advantage over the enemy\nSee the game out");
                    dialogPrompt.Setup(promptList);
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    

                    // for(int i = 0; i < player.handSize; i++)
                    // {
                    //     player.hand[i].GetComponent<CardBehavior>().summoned = false;
                    // }
                }
                break;
            default:
                break;
        }
    }

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
