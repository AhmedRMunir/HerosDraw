using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



/*
 Tutorial - 1 - Controller
 Set player and enemy HP to 3 HP

Enemy Deck:
 - 1C - 0A - 1 H
 - 1C - 1A - 1 H
 - 1C - 1A - 2 H


Player Deck:
 - 1C - 0A - 1H
 - 1C - 1A - 1H
 - Double Attack Passive Card
 - Knight
 
*/

public class Tutorial_1_Controller : GameController
{

    public override IEnumerator gameStart()
    {
        if (Conditions.collectingData)
            StartCoroutine(LoadingController.LOGGER.LogLevelStart(1, "{ User entered tutorial 1 }"));

        player.maxMana = 1;
        player.mana = 1;
        enemy.maxMana = 1;
        enemy.mana = 1;

        player.health = 3;
        enemy.health = 3;

        yield return new WaitForSeconds(0.5f);

        List<string> promptList = new List<string>();
        promptList.Add("Welcome to Hero's Draw.\nLet's run you through the basics!");
        dialogPrompt.Setup(promptList);

        yield return new WaitUntil(() => dialogPrompt.pressed);
        dialogPrompt.pressed = false;

        // Draw Hand for Player
        player.handSize = handStartSize;
        player.drawHand();

        // Draw Hand for Enemy
        enemy.handSize = handStartSize;
        enemy.drawHand();


        // Declare the next player variable
        if (current_turn == turn.PLAYER) {
            next_player = turn.ENEMY;
        } else {
            next_player = turn.PLAYER;
        }

        // yield return new WaitForSeconds(1f);

        // Start with player or enemy turn
        if (current_turn == turn.PLAYER)
        {
            StartCoroutine(indicateTurn("playerTurn"));
        }
        else
        {
            StartCoroutine(indicateTurn("enemyTurn"));
        }       
    }

    public override IEnumerator displayDialog()
    {
        List<string> promptList = new List<string>();
        switch(battleNum) {
            case 1:
                // HEALTH DIALOG
                promptList.Add("Here are the players' Health");
                dialogPrompt.Setup(promptList);
                // TODO:Highlight Health
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                promptList.Add("If it goes down to 0 the other wins");

                // MANA DIALOG
                promptList.Add("Here is the mana.\nIt allows you to summon cards");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Mana
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;


                // CARD DIALOG
                promptList.Add("Click on the Highlighted card to Magnify it.");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight the 0th index card
                // Add a variable called card_clicked to the WaitUntil function
                // That way we can wait on the player to click on card before we display more dialog
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD MANA
                promptList.Add("This is the Card Mana\nIt's the cost it takes to play the card");
                // TODO: Highlight Card mana
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD ATTACK
                promptList.Add("This is the Card Attack\nIt's the damage the Card can deal");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Card Attack
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD HEALTH
                promptList.Add("This is the Card Health\nIt's the damage the Card can take");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Card Health
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD Faction
                promptList.Add("This is the Card Faction\nThis is the Type of the card");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Card Faction
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD Ability
                promptList.Add("This is the Card Ability Section\nCan be a passive, active, or nothing");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Card Ability
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // LANES
                promptList.Add("Here are the summoning Lanes");
                promptList.Add("Once you click a card, it shows you where you can summon the card");
                promptList.Add("Once you summon a card, it can not be moved");
                promptList.Add("Select a lane to summon your card!");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Summoning Lanes
                // Add a variable called card_summoned to the WaitUntil function
                // That way we can wait on the player to summon a card before we display more dialog
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                
                // BATTLE PHASE EXPLANATION
                promptList.Add("BATTLE PHASE EXPLANATION");
                promptList.Add("Each battle, players take turn to summon cards.");
                promptList.Add("A player can go to Battle if they are out of mana or cards to play");
                promptList.Add("Or whenever they want, but be careful!");
                promptList.Add("The other player can summon as much as their resources allow :|");
                promptList.Add("During the Battle Phase, cards attack each other");
                promptList.Add("If a card's health is reduced to 0, the card is destroyed");
                promptList.Add("If a card is unopposed, it will attack the player directly depleting their health");
                promptList.Add("Click Ready For Battle!");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight the Ready for Battle Button
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;;
                break;
            case 2:
                if (turnNum == 1) {
                    // GOING FIRST STRATEGY
                    promptList.Add("Cool. First Battle went well!");
                    promptList.Add("Let's try to win the second Battle as well!");
                    promptList.Add("Start by playing the card with the lower health to bait the opponent");
                    dialogPrompt.Setup(promptList);
                    // TODO: Highlight the 0th index card
                    // TODO: Highlight the 1st index lane
                    // Add a variable called card_summoned to the WaitUntil function
                    // That way we can wait on the player to click on card before we display more dialog
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;

                    // PASSING THE TURN
                    promptList.Add("Every time you summon a card, you must pass the turn over");
                    dialogPrompt.Setup(promptList);
                    // TODO: Highlight the Pass Turn button
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                } else if (turnNum == 2) {
                    // CONTINUATION
                    promptList.Add("Now play your other card and then go to Battle!");
                    dialogPrompt.Setup(promptList);
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                }
                break;
            case 3:
                // PLAY THE GAME OUT
                promptList.Add("Given what you know, strategize and win the game!");
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                break;
            default:
                break;
        }
    }

    public override void enemy_play_card() {
        Debug.Log(battleNum + ", " + turnNum);
        switch(battleNum) {
            case 1:
                Debug.Log("playing card in 2nd position");
                summon_card(0, 2, get_playable_cards(0)[0]);
                enemy_ready_for_battle = true;
                break;
            case 2:
                switch(turnNum) {
                    case 1:
                        summon_card(0, 1, get_playable_cards(0)[0]);
                        break;
                    case 2:
                        summon_card(0, 3, get_playable_cards(0)[0]);
                        break;
                }
                break;
            default:
                enemy_play_card_first_open_lane();
                break;
        }
    }
}
