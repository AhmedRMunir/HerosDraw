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

        // Enemy AI
        enemy_play_card_first_open_lane();
        // enemy_play_card_first_block_lane();
        // enemy_play_card_block_strongest_on_field();

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

    // Return a list of the indices of open lanes
    // 0 -> enemy
    // 1 -> player
    private List<int> get_open_lanes(int player) {
        List<int> enemy_lanes = new ArrayList<>();

        for (int i = 0; i < field.GetLength(1); i++) {
            if (field[player, i] == null) {
                enemy_lanes.Add(i);
            }
        }
        return enemy_open_lanes;
    }

    // Return a list of cards in the enemy hand that can be played
    private List<GameObject> get_playable_cards(int player) {
        List<GameObject> cards = new ArrayList<>();

        if (player == 0) {
            for (int i = 0; i < enemy.hand.Count; i++) {
                GameObject potential_card = enemy.hand[i];
                int potential_cost = getCardCost(potential_card);

                if (potential_cost < enemy.mana) {
                    cards.Add(potential_card);
                }
            }
        } else {
            for (int i = 0; i < player.hand.Count; i++) {
                GameObject potential_card = player.hand[i];
                int potential_cost = getCardCost(potential_card);

                if (potential_cost < player.mana) {
                    cards.Add(potential_card);
                }
            }
        }

        return cards;
    }

    private GameObject getStrongestCard(int player) {
        List<GameObject> playable_cards = get_playable_cards(player);
        int strength = -1;
        GameObject strongest_card = null;
        for (int i = 0; i < playable_cards.Count; i++) {
            if (getCardStrength(playable_cards[i]) > strength) {
                strength = getCardStrength(playable_cards[i]);
                strongest_card = playable_cards[i];
            }
        }
    }

    // returns the cost of a card
    private int getCardCost(GameObject card) {
        return card.GetComponent<CardBehavior>().getCost();
    }

    private int getCardAttack(GameObject card) {
        return card.GetComponent<CardBehavior>().getAttack();
    }

    private int getCardHealth(GameObject card) {
        return card.GetComponent<CardBehavior>().getHealth();
    }

    private int getCardStrength(GameObject card) {
        return getCardAttack(card) + getCardHealth(card);
    }

    // Summons the given card into the given lane
    // To do: Use a player parameter to allow to use the same function for player and enemy
    private void summon_card(int player, int lane, GameObject card) {

        if (player == 0) {
            enemy.hand.Remove(card_to_play);
            num_enemy_summoned_card++;
            card.GetComponent<CardBehavior>()
                .summonCard(enemy_lanes.transform.GetChild(lane_num).GetComponent<RectTransform>(), lane_num);

            Debug.Log("enemy plays " + card.GetComponent<CardBehavior>().nameText.text + " at lane " + lane_num); 
        } else {
            player.hand.Remove(card_to_play);
            num_player_summoned_card++;
            card.GetComponent<CardBehavior>()
                .summonCard(player_lanes.transform.GetChild(lane_num).GetComponent<RectTransform>(), lane_num);

            Debug.Log("player plays " + card.GetComponent<CardBehavior>().nameText.text + " at lane " + lane_num); 
        }
    }


    // Plays card into the first open lane
    public void enemy_play_card_first_open_lane() {

        // Enemy has a playable card
        // Enemy has an open lane 

        List<int> enemy_open_lanes = get_open_lanes(0);
        List<GameObject> enemy_playable_cards = get_playable_cards(0);

        if (enemy_open_lanes.Count == 0 || enemy_playable_cards.Count == 0) {
            return;
        }

        // play the first playable card into the first open space
        GameObject card_to_play = enemy.hand.Remove(enemy_playable_cards[0]);
        int lane_num = enemy_open_lanes[0];
        summon_card(0, lane_num, card_to_play);        
    }

    public void enemy_play_card_first_block_lane() {
        List<int> enemy_open_lanes = get_open_lanes(0);
        List<int> player_open_lanes = get_open_lanes(1);

        List<GameObject> enemy_playable_cards = get_playable_cards(0);

        if (enemy_open_lanes.Count == 0 || enemy_playable_cards.Count == 0) {
            return;
        }

        GameObject card_to_play = enemy.hand.Remove(enemy_playable_cards[0]);

        int lane_num = enemy_open_lanes[0];

        for (int i = 0; i < enemy_open_lanes.Count; i++) {
            if (player_open_lanes.Contains(enemy_open_lanes[i])) {
                lane_num = enemy_open_lanes[i];
                break;
            }
        }


        summon_card(0, lane_num, card_to_play); 
    }


    public void enemy_play_card_block_strongest_on_field() {
        List<int> enemy_open_lanes = get_open_lanes(0);
        List<int> player_open_lanes = get_open_lanes(1);

        List<GameObject> enemy_playable_cards = get_playable_cards(0);

        if (enemy_open_lanes.Count == 0 || enemy_playable_cards.Count == 0) {
            return;
        }

        List<int> shared_lanes = new ArrayList<>();

        for (int i = 0; i < enemy_open_lanes.Count; i++) {
            if (!player_open_lanes.Contains(enemy_open_lanes[i])) {
                shared_lanes.Add(enemy_open_lanes[i]);
            }
        }

        if (shared_lanes.Count == 0) {
            enemy_play_card_first_open_lane();
        }

        int strongest_player_lane = -1;
        int strongest_val = -1;

        for (int i = 0; i < shared_lanes.Count; i++) {
            int lane_num = shared_lanes[i];
            int card_strength = getCardStrength(field[1, lane_num]);
            if (card_strength > strongest_val) {
                strongest_val = card_strength;
                strongest_player_lane = shared_lanes[i];
            }
        }

        GameObject card_to_play = enemy.hand.Remove(enemy_playable_cards[0]);
        summon_card(0, strongest_player_lane, card_to_play);
    }
}
