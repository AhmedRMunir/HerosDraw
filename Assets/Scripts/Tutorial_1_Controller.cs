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
    public GameObject indicatorArrow;
    public GameObject summonZone;
    public GameObject completeCard;
    public CardObject exampleCardIdentity;
    public PassTurn passTurn;

    public override IEnumerator gameStart()
    {
        if (Conditions.collectingData)
            StartCoroutine(LoadingController.LOGGER.LogLevelStart(levelID, "{ User entered tutorial 1 }"));

        player.maxMana = 1;
        player.mana = 1;
        enemy.maxMana = 1;
        enemy.mana = 1;

        player.health = 3;
        enemy.health = 3;

        ready_Button.GetComponent<ReadyForBattle>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        List<string> promptList = new List<string>();
        promptList.Add("Welcome to Hero's Draw.");
        promptList.Add("Your goal is to become the Card Hero, but first let's run you through how the battle works");
        dialogPrompt.Setup(promptList);

        yield return new WaitUntil(() => dialogPrompt.pressed);
        dialogPrompt.pressed = false;
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
                promptList.Add("This is your health.");
                dialogPrompt.Setup(promptList);
                // Highlight Health
                GameObject arrow = Instantiate(indicatorArrow, player_Avatar.transform.position, Quaternion.identity, dialogPrompt.transform);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                promptList.Add("The goal of the game is to reduce your opponent's health to 0.");
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // MANA DIALOG
                promptList.Add("Here is your Mana.\nMana is what allows you to play cards.");
                dialogPrompt.Setup(promptList);
                // Highlight Mana by accessing game objects in the hierarchy
                arrow.transform.position = player_Avatar.transform.parent.transform.parent.GetChild(2).transform.position;
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;


                // CARD DIALOG
                promptList.Add("This is a Pawn card.\nPawns are the cards you will summon to do battle with your opponent.");
                dialogPrompt.Setup(promptList);
                // Show an example card for several following dialogue prompts
                GameObject exampleCard = Instantiate(completeCard, summonZone.transform.position, Quaternion.identity, dialogPrompt.transform);
                exampleCard.GetComponent<CardBehavior>().cardIdentity = exampleCardIdentity;
                exampleCard.GetComponent<CardBehavior>().summoned = true;
                exampleCard.transform.localScale = new Vector2(1.3f, 1.3f);
                float cardOffset = exampleCard.GetComponent<RectTransform>().rect.width / 2f * 1.3f;
                arrow.transform.SetAsLastSibling();
                arrow.transform.position = new Vector2(summonZone.transform.position.x - cardOffset, summonZone.transform.position.y);
                arrow.transform.eulerAngles = new Vector3(0, 0, -225);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD MANA
                promptList.Add("Each card possesses a Mana cost.\nIt is the cost a player must pay to play the card.");
                // Highlight Card mana
                arrow.transform.position = exampleCard.GetComponent<CardBehavior>().costValue.transform.position;
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD ATTACK
                promptList.Add("This is the Pawn's Attack.\nIt is the damage the Pawn will deal in battle.");
                dialogPrompt.Setup(promptList);
                // Highlight Card Attack
                arrow.transform.position = exampleCard.GetComponent<CardBehavior>().attackValue.transform.position;
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD HEALTH
                promptList.Add("This is the Pawn's Health.\nOpposing Pawns deal their Attack to each others' Health.\nIf a Pawn's health drops to 0 or less, it is slain.");
                dialogPrompt.Setup(promptList);
                // Highlight Card Health
                arrow.transform.position = exampleCard.GetComponent<CardBehavior>().healthValue.transform.position;
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD Faction
                promptList.Add("This is the Pawn's Faction.\nIt indicates what group of Pawns it belongs to.\nThis Pawn belongs to the \"Knight\" faction");
                dialogPrompt.Setup(promptList);
                // Highlight Card Faction
                arrow.transform.position = exampleCard.GetComponent<CardBehavior>().factionIcon.transform.position;
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;

                // CARD Ability
                promptList.Add("This is the Pawn's Ability Section.\nIt can be a passive, active, or nothing.\nWe will get into each ability type later.");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Card Ability
                arrow.transform.position = exampleCard.GetComponent<CardBehavior>().descriptionText.transform.position;
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                Destroy(exampleCard);

                // LANES
                // TODO: Highlight Summoning Lanes
                arrow.transform.position = player_lanes.transform.GetChild(0).transform.position;
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
                arrow.transform.localScale = new Vector2(-1, 1);
                promptList.Add("Here are the summoning Lanes.");
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                promptList.Add("There are a maximum of 5 that your Pawns can occupy.");
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                promptList.Add("Once you click a Pawn you want to summon, it shows you where you can summon the card.");
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                promptList.Add("Once you summon a Pawn, it cannot be moved.");
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                promptList.Add("Select a Pawn and summon it to a lane!");
                dialogPrompt.Setup(promptList);
                // TODO: Highlight Summoning Lanes
                // Add a variable called card_summoned to the WaitUntil function
                // That way we can wait on the player to summon a card before we display more dialog
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                Destroy(arrow);
                player_lanes.transform.GetChild(0).gameObject.SetActive(false);
                player_lanes.transform.GetChild(1).gameObject.SetActive(false);
                player_lanes.transform.GetChild(3).gameObject.SetActive(false);
                player_lanes.transform.GetChild(4).gameObject.SetActive(false);
                player.hand[0].GetComponent<CardBehavior>().summoned = true;

                yield return new WaitUntil(() => player_has_summoned);

                // READY FOR BATTLE EXPLANATION
                promptList.Add("Congratulations! You have summoned your first Pawn!");
                promptList.Add("However, you are now out of Mana and can no longer play additional cards.");
                promptList.Add("When you are out of cards to play, your next move will be to ready for battle.");
                promptList.Add("When a player readies for battle, they can no longer play any more cards.");

                // BATTLE EXPLANATION
                promptList.Add("Once both players are ready for battle, the battle will commence and all Pawns will fight the opposing lane.");
                promptList.Add("However, if a card is unopposed, it will attack the player directly, depleting their health.");
                promptList.Add("Now, click Ready For Battle!");
                dialogPrompt.Setup(promptList);
                // Enable the Ready for Battle Button
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;;
                ready_Button.GetComponent<ReadyForBattle>().enabled = true;
                player.hand[0].GetComponent<CardBehavior>().summoned = false;
                break;
            case 2:
                if (turnNum == 1) {
                    // GOING FIRST STRATEGY

                    promptList.Add("After each battle, the player who makes the first move alternates.");
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    promptList.Add("Also, notice that your max Mana increases by 1 and replenishes every Battle.");
                    dialogPrompt.Setup(promptList);

                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    // Highlight Mana by accessing game objects in the hierarchy
                    GameObject arrow2 = Instantiate(indicatorArrow, player_Avatar.transform.parent.transform.parent.GetChild(2).transform.position, Quaternion.identity, dialogPrompt.transform);
                    
                    promptList.Add("The opponent went first last time, so now you will take the first turn.");
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    promptList.Add("Start by summoning the weaker Pawn to bait the opponent.");

                    dialogPrompt.Setup(promptList);
                    player.hand[1].GetComponent<CardBehavior>().summoned = true;
                    player_lanes.transform.GetChild(1).gameObject.SetActive(true);
                    completeCard.GetComponent<CardBehavior>().removeIndicators();
                    // Don't let player ready for battle or pass turn yet
                    ready_Button.GetComponent<ReadyForBattle>().enabled = false;
                    passTurn.enabled = false;

                    // Highlight the 0th index card and wait for player to play it before continuing
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    Destroy(arrow2);

                    yield return new WaitUntil(() => player_has_summoned);
                    passTurn.enabled = true;
                    player.hand[0].GetComponent<CardBehavior>().summoned = false;

                    // PASSING THE TURN
                    promptList.Add("If your opponent is not ready for battle, you may only summon one Pawn per turn.");
                    promptList.Add("From here you must pass the turn over");
                    dialogPrompt.Setup(promptList);
                    // Highlight the Pass Turn button
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    
                } else if (turnNum == 2) {
                    // CONTINUATION
                    promptList.Add("Now play your other card and then go to Battle!");
                    dialogPrompt.Setup(promptList);
                    yield return new WaitUntil(() => dialogPrompt.pressed);
                    dialogPrompt.pressed = false;
                    player_lanes.transform.GetChild(3).gameObject.SetActive(true);
                    completeCard.GetComponent<CardBehavior>().removeIndicators();
                    ready_Button.GetComponent<ReadyForBattle>().enabled = true;
                }
                break;
            case 3:
                // DRAW CARD WITH PASSIVE
                promptList.Add("You drew a Pawn with a Passive Ability. Passives typically apply when the Pawn is summoned.");
                promptList.Add("Passive abilities can quickly turn the tide of battle. The one you drew will strengthen your other Pawns.");
                promptList.Add("With it, defeat your opponent!");
                dialogPrompt.Setup(promptList);
                yield return new WaitUntil(() => dialogPrompt.pressed);
                dialogPrompt.pressed = false;
                player_lanes.transform.GetChild(0).gameObject.SetActive(true);
                player_lanes.transform.GetChild(4).gameObject.SetActive(true);
                completeCard.GetComponent<CardBehavior>().removeIndicators();
                ready_Button.GetComponent<ReadyForBattle>().enabled = false;
                passTurn.enabled = false;

                yield return new WaitUntil(() => player_has_summoned);

                ready_Button.GetComponent<ReadyForBattle>().enabled = true;
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
