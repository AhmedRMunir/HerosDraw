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

    public EndPrompt EndPrompt;

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
        StartCoroutine("gameStart");
       
    }

    public IEnumerator gameStart()
    {
        player.maxMana = 1;
        player.mana = 1;
        enemy.maxMana = 1;
        enemy.mana = 1;

        yield return new WaitForSeconds(0.5f);
        player.handSize = Conditions.handStartSize;
        enemy.handSize = Conditions.handStartSize;
        player.shuffle();
        player.drawHand();
        enemy.shuffle();
        enemy.drawHand();

        yield return new WaitForSeconds(1f);

        if (current_turn == turn.PLAYER)
        {
            StartCoroutine("playerTurn");
        }
        else
        {
            StartCoroutine("enemyTurn");
        }
    }

    public IEnumerator playerTurn()
    {
        passTurnSpinner.transform.DORotate(new Vector3(0, 0, 0), 0.75f);
        yield return new WaitForSeconds(0.75f);
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
                if (player_has_summoned)
                {
                    player_can_play = false;
                } else
                {
                    player_can_play = true;
                }
            }
        } else if (player_ready_for_battle)
        {
            StartCoroutine("enemyTurn");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
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
        //attack.Pause();
        

        for (int i = 0; i < field.GetLength(1); i++) {
            Debug.Log(i);
            GameObject player_card = field[1,i];
            GameObject enemy_card = field[0,i];


            // instead of manually updating health, should make a function
            // take into consideration of card's ability, e.g. double attack
            if (player_card == null && enemy_card == null)
            {
                continue;
            }
            else if (player_card == null && enemy_card != null)
            {
                Sequence attack = DOTween.Sequence();
                RectTransform enemyTran = enemy_card.GetComponent<RectTransform>();
                attack.Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y + 100), 0.25f))
                    .Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y - 100), 0.125f))
                    .AppendCallback(() => { updateHealth(player, -enemy_card.GetComponent<CardBehavior>().getAttack()); })
                    .Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y), 0.125f));

            }
            else if (player_card != null && enemy_card == null)
            {
                Sequence attack = DOTween.Sequence();
                RectTransform playerTran = player_card.GetComponent<RectTransform>();
                attack.Append(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y - 100), 0.25f))
                    .Append(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y + 100), 0.125f))
                    .AppendCallback(() => { updateHealth(enemy, -player_card.GetComponent<CardBehavior>().getAttack()); })
                    .Append(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y), 0.125f));

            }
            else
            {
                Sequence attack = DOTween.Sequence();
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
                    .Append(enemyTran.DOAnchorPos(new Vector2(enemyTran.anchoredPosition.x, enemyTran.anchoredPosition.y), 0.125f))
                    .Join(playerTran.DOAnchorPos(new Vector2(playerTran.anchoredPosition.x, playerTran.anchoredPosition.y), 0.125f));

            }
        }

        yield return new WaitForSeconds(0.5f);

        // second iteration; 
        for (int i = 0; i < field.GetLength(1); i++)
        {
            GameObject player_card = field[1, i];
            GameObject enemy_card = field[0, i];

            if (player_card != null && player_card.GetComponent<CardBehavior>().getHealth() <= 0)
            {
                num_player_summoned_card--;
                Destroy(field[1, i]);
            }

            if (enemy_card != null && enemy_card.GetComponent<CardBehavior>().getHealth() <= 0)
            {
                num_enemy_summoned_card--;
                Debug.Log("card is destoryed!");
                Destroy(field[0, i]);
            }
        }

        if (player.health <= 0 || enemy.health <= 0) {
            StartCoroutine("endGame");
        } else {
            StartCoroutine("newRound");
        }
    }

    public IEnumerator enemyTurn() {
        if (passTurnSpinner.transform.eulerAngles.z != 180f)
        {
            passTurnSpinner.transform.DORotate(new Vector3(0, 0, 180f), 0.75f);
            yield return new WaitForSeconds(0.75f);
        }
        
        current_turn = turn.ENEMY;
        if (num_enemy_summoned_card == enemy_lanes.transform.childCount || enemy.hand.Count == 0 || enemy_ready_for_battle) {
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
            enemy_play_card_block_strongest_on_field();

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

    public IEnumerator newRound()
    {
        player_ready_for_battle = false;
        enemy_ready_for_battle = false;
        player.drawCard();
        enemy.drawCard();
        battleNum++;
        turnNum = 1;
        resetMana(player);
        resetMana(enemy);

        yield return new WaitForSeconds(1f);

        if (battleNum % 2 == 0)
        {
            StartCoroutine("enemyTurn");
        }
        else
        {
            StartCoroutine("playerTurn");
        }
        StopCoroutine("newRound");
    }

    public IEnumerator endGame()
    {
        bool playerWin = enemy.health <= 0;
        EndPrompt.Setup(playerWin);

        StopAllCoroutines();
        yield return new WaitForSeconds(1f);
    }

    // Return a list of the indices of open lanes
    // 0 -> enemy
    // 1 -> player
    protected List<int> get_open_lanes(int player) {
        List<int> open_lanes = new List<int>();

        for (int i = 0; i < field.GetLength(1); i++) {
            if (field[player, i] == null) {
                open_lanes.Add(i);
            }
        }
        return open_lanes;
    }

    // Return a list of cards in the enemy hand that can be played
    protected List<GameObject> get_playable_cards(int player_num) {
        List<GameObject> cards = new List<GameObject>();

        if (player_num == 0) {
            for (int i = 0; i < enemy.hand.Count; i++) {
                GameObject potential_card = enemy.hand[i];
                int potential_cost = getCardCost(potential_card);

                if (potential_cost <= enemy.mana) {
                    cards.Add(potential_card);
                }
            }
        } else {
            for (int i = 0; i < player.hand.Count; i++) {
                GameObject potential_card = player.hand[i];
                int potential_cost = getCardCost(potential_card);

                if (potential_cost <= player.mana) {
                    cards.Add(potential_card);
                }
            }
        }

        return cards;
    }

    protected GameObject getStrongestCard(int player_num) {
        List<GameObject> playable_cards = get_playable_cards(player_num);
        int strength = -1;
        GameObject strongest_card = null;
        for (int i = 0; i < playable_cards.Count; i++) {
            if (getCardStrength(playable_cards[i]) > strength) {
                strength = getCardStrength(playable_cards[i]);
                strongest_card = playable_cards[i];
            }
        }
        return strongest_card;
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

    public int getCardStrength(GameObject card) {
        return getCardAttack(card) + getCardHealth(card);
    }

    // Summons the given card into the given lane
    // To do: Use a player parameter to allow to use the same function for player and enemy
    protected void summon_card(int player_num, int lane, GameObject card) {

        if (player_num == 0) {
            enemy.hand.Remove(card);
            num_enemy_summoned_card++;
            card.GetComponent<CardBehavior>()
                .summonCard(enemy_lanes.transform.GetChild(lane).GetComponent<RectTransform>(), lane);

            Debug.Log("enemy plays " + card.GetComponent<CardBehavior>().nameText.text + " at lane " + lane); 
        } else {
            player.hand.Remove(card);
            num_player_summoned_card++;
            card.GetComponent<CardBehavior>()
                .summonCard(player_lanes.transform.GetChild(lane).GetComponent<RectTransform>(), lane);

            Debug.Log("player plays " + card.GetComponent<CardBehavior>().nameText.text + " at lane " + lane); 
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
        int lane_num = enemy_open_lanes[0];
        summon_card(0, lane_num, enemy_playable_cards[0]);
        enemy.hand.Remove(enemy_playable_cards[0]);      
    }

    public void enemy_play_card_first_block_lane() {
        List<int> enemy_open_lanes = get_open_lanes(0);
        List<int> player_open_lanes = get_open_lanes(1);

        List<GameObject> enemy_playable_cards = get_playable_cards(0);

        if (enemy_open_lanes.Count == 0 || enemy_playable_cards.Count == 0) {
            return;
        }

        int lane_num = enemy_open_lanes[0];

        for (int i = 0; i < enemy_open_lanes.Count; i++) {
            if (!player_open_lanes.Contains(enemy_open_lanes[i])) {
                lane_num = enemy_open_lanes[i];
                break;
            }
        }


        summon_card(0, lane_num, enemy_playable_cards[0]); 
        enemy.hand.Remove(enemy_playable_cards[0]);
    }


    public void enemy_play_card_block_strongest_on_field() {
        List<int> enemy_open_lanes = get_open_lanes(0);
        List<int> player_open_lanes = get_open_lanes(1);

        List<GameObject> enemy_playable_cards = get_playable_cards(0);
        if (enemy_playable_cards.Count == 0)
        {
            enemy_ready_for_battle = true;
            return;
        }

        if (enemy_open_lanes.Count == 0 || enemy_playable_cards.Count == 0) {
            return;
        }

        List<int> shared_lanes = new List<int>();

        for (int i = 0; i < enemy_open_lanes.Count; i++) {
            if (!player_open_lanes.Contains(enemy_open_lanes[i])) {
                shared_lanes.Add(enemy_open_lanes[i]);
            }
        }

        if (shared_lanes.Count == 0) {
            enemy_play_card_first_open_lane();
            return;
        }

        int strongest_player_lane = shared_lanes[0];
        int strongest_val = -1;

        for (int i = 0; i < shared_lanes.Count; i++) {
            int lane_num = shared_lanes[i];
            int card_strength = getCardStrength(field[1, lane_num]);
            if (card_strength > strongest_val) {
                strongest_val = card_strength;
                strongest_player_lane = shared_lanes[i];
            }
        }

        summon_card(0, strongest_player_lane, enemy_playable_cards[0]);
        enemy.hand.Remove(enemy_playable_cards[0]);
    }

    public void resetMana(PlayerController player)
    {
        if (player.maxMana < Conditions.maxTotalMana)
        {
            player.maxMana++;
            player.mana = player.maxMana;
        }
    }
}
