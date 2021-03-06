using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public int levelID;

    // Player
    //public Player player;
    public PlayerController player;
    public GameObject player_lanes;
    public int num_player_summoned_card;
    public bool player_has_summoned;
    public bool player_ready_for_battle;
    public GameObject player_Avatar;
    public GameObject ready_Button;

    public bool player_free_pass;
    public bool player_can_pass;
    public bool player_has_playable_card;

    public bool player_is_summoning;

    // animation flag
    public bool player_can_play;

    // AI
    //public Player enemy;
    public PlayerController enemy;
    public GameObject enemy_lanes;
    public int num_enemy_summoned_card;
    public bool enemy_ready_for_battle;
    public bool enemy_has_summoned;
    public GameObject enemy_Avatar;

    // 2d field
    public GameObject[,] field;

    public int handStartSize;
    public int turnNum;
    public int battleNum;
    public float slideSpeed;

    public GameObject passTurnSpinner;
    public GameObject phaseIndicator;

    public EndPrompt EndPrompt;

    public EndPrompt dialogPrompt;

    public GameObject ManaRegenOverlay;

    public GameObject obtainableCard;
    public GameObject obtainCardText;

    public CardAbility ability;

    public enum turn {
        PLAYER, ENEMY
    }

    public turn current_turn;
    public turn next_player;


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

        // Get levelID from level manager
        field = new GameObject[2, Conditions.maxLanes];
        ability = GameObject.FindGameObjectWithTag("Ability").GetComponent<CardAbility>();
        // Levels past tutorial
        if (levelID > 3) {
            // Randomize player who goes first
            int startPlayer = Random.Range(0, 2);
            levelID = Conditions.levelsCompleted + 1;
            if (startPlayer == 0) // Player start
            {
                current_turn = turn.PLAYER;
                next_player = turn.ENEMY;
            } else // Enemy start
            {
                current_turn = turn.ENEMY;
                next_player = turn.PLAYER;
            }
            // Load player deck
            player.deck = new List<CardObject>(Conditions.deck);
        }
        StartCoroutine("gameStart");
    }

    public virtual IEnumerator gameStart()
    {
        if (Conditions.collectingData)
            StartCoroutine(LoadingController.LOGGER.LogLevelStart(levelID, "{ User entered battle" + levelID + " }"));

        player.maxMana = 1;
        player.mana = 1;
        enemy.maxMana = 1;
        enemy.mana = 1;

        player_free_pass = true;
        player_can_pass = true;

        yield return new WaitForSeconds(0.5f);
        player.handSize = handStartSize;
        enemy.handSize = handStartSize;
        player.shuffle();
        player.drawHand();
        enemy.shuffle();
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

    public IEnumerator indicateTurn(string phase)
    {
        Debug.Log("Call " + phase);
        phaseIndicator.SetActive(true);
        phaseIndicator.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Phases/" + phase);
        RectTransform phaseTransform = phaseIndicator.GetComponent<RectTransform>();
        phaseTransform.anchoredPosition = new Vector2(0, Screen.height);
        yield return new WaitForEndOfFrame();
        Sequence slideIn = DOTween.Sequence();
        phaseTransform.DOAnchorPos(new Vector2(0, 0), slideSpeed);

        yield return new WaitForSeconds(slideSpeed + 0.5f);

        Sequence slideOut = DOTween.Sequence();
        slideOut.Append(phaseTransform.DOAnchorPos(new Vector2(0, Screen.height), slideSpeed))
            .AppendCallback(() => {
                StartCoroutine(phase);
                phaseIndicator.SetActive(false);
            });

    }

    public IEnumerator playerTurn()
    {
        // if (passTurnSpinner.transform.eulerAngles.z != 0)
        // {
        //     passTurnSpinner.transform.DORotate(new Vector3(0, 0, 0), 0.75f);
        //     yield return new WaitForSeconds(0.75f);
        // }

        bool spin = spinPassTurn(0f);
        if (spin)
            yield return new WaitForSeconds(0.75f);

        current_turn = turn.PLAYER;
        player_has_summoned = (num_player_summoned_card == field.GetLength(1));
        yield return new WaitForEndOfFrame();

        StartCoroutine(displayDialog());
        if (!player_free_pass) {
            player_can_pass = false;
        }

        //List<GameObject> playableCards = get_playable_cards(1);
        if (!playerHasPlayable())
        {
            player_can_pass = false;
        }

        if (player_ready_for_battle == false && current_turn == turn.PLAYER)
        {
            /*&if (enemy_ready_for_battle)
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
            }*/
            player_can_play = true;
        } else if (player_ready_for_battle)
        {
            StartCoroutine("enemyTurn");
        }
        
    }

    public virtual IEnumerator displayDialog() {
        yield return new WaitForEndOfFrame(); 
    }
    
    public bool playerHasPlayable()
    {
        List<GameObject> playableCards = get_playable_cards(1);
        if (playableCards.Count == 0 || num_player_summoned_card == field.GetLength(1))
        {
            return false;
        }

        return true;
    }


    // Update is called once per frame
    void Update()
    {
        if (!playerHasPlayable())
        {
            player_can_pass = false;
        }
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
        Sequence battleAnim = DOTween.Sequence();
        float battleAnimTime = 0;

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
                attackAvatar(enemy_card, player, player_Avatar, 100, battleAnim);
            }
            else if (player_card != null && enemy_card == null)
            {
                attackAvatar(player_card, enemy, enemy_Avatar, -100, battleAnim);
            }
            else
            {
                attackPawns(player_card, enemy_card, battleAnim);
            }
            battleAnimTime += 1f;

        }

        yield return new WaitForSeconds(battleAnimTime);

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

        yield return new WaitForSeconds(0.5f);

        if (player.health <= 0 || enemy.health <= 0) {
            StartCoroutine("endGame");
        } else {
            StartCoroutine("newRound");
        }
    }

    public void attackAvatar(GameObject pawn, PlayerController target, GameObject targetAvatar, int animationParam, Sequence battleAnim) {
        Transform pawnTran = pawn.transform.GetChild(0).GetChild(7).gameObject.transform;
        CardBehavior card = pawn.GetComponent<CardBehavior>();
        battleAnim.Append(pawnTran.DOMove(new Vector2(pawnTran.position.x, pawnTran.position.y + animationParam), 0.1f))
              .Join(pawnTran.DOScale(1.5f, 0.1f))
              .Append(pawnTran.DOMove(targetAvatar.transform.position, 0.25f))
              .AppendCallback(() => { 
                  updateHealth(target, -pawn.GetComponent<CardBehavior>().getAttack()); 
                  if (card.cardIdentity.hasBattleAbility)
                  {
                      ability.passiveAbility(card.cardAbility, card.abilityParams.ToArray());
                  }
              })
              .Append(pawnTran.DOMove(new Vector2(pawnTran.position.x, pawnTran.position.y), 0.3f))
              .Join(targetAvatar.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f, 10, 1))
              .Join(pawnTran.DOScale(1f, 0.3f))
              .Append(targetAvatar.transform.DOScale(1f, 0.2f));
    }

    public void attackPawns(GameObject player_card, GameObject enemy_card, Sequence battleAnim) {
        Transform enemyTran = enemy_card.transform.GetChild(0).GetChild(7).gameObject.transform;
        Transform enemyHP = enemy_card.transform.GetChild(0).GetChild(6).gameObject.transform;
        Transform playerTran = player_card.transform.GetChild(0).GetChild(7).gameObject.transform;
        Transform playerHP = player_card.transform.GetChild(0).GetChild(6).gameObject.transform;
        CardBehavior playerCardInfo = player_card.GetComponent<CardBehavior>();
        CardBehavior enemyCardInfo = enemy_card.GetComponent<CardBehavior>();
        battleAnim.Append(enemyTran.DOMove(new Vector2(enemyTran.position.x, enemyTran.position.y + 100), 0.1f))
            .Join(playerTran.DOMove(new Vector2(playerTran.position.x, playerTran.position.y - 100), 0.1f))
            .Join(enemyTran.DOScale(1.5f, 0.1f))
            .Join(enemyHP.DOScale(1.5f, 0.1f))
            .Join(playerTran.DOScale(1.5f, 0.1f))
            .Join(playerHP.DOScale(1.5f, 0.1f))
            .Append(enemyTran.DOMove(playerHP.transform.position, 0.25f))
            .Join(playerTran.DOMove(enemyHP.transform.position, 0.25f))
            .AppendCallback(() =>
            {
                if (playerCardInfo.cardIdentity.hasBattleAbility)
                {
                    ability.passiveAbility(playerCardInfo.cardAbility, playerCardInfo.abilityParams.ToArray());
                }
                if (enemyCardInfo.cardIdentity.hasBattleAbility)
                {
                    ability.passiveAbility(enemyCardInfo.cardAbility, enemyCardInfo.abilityParams.ToArray());
                }
            })
            .AppendCallback(() =>
            {
                player_card.GetComponent<CardBehavior>().updateStats(0, -enemy_card.GetComponent<CardBehavior>().getAttack());
                enemy_card.GetComponent<CardBehavior>().updateStats(0, -player_card.GetComponent<CardBehavior>().getAttack());
            })
            .Join(enemyHP.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f, 10, 1))
            .Join(playerHP.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f, 10, 1))
            .Join(enemyTran.DOMove(new Vector2(enemyTran.position.x, enemyTran.position.y), 0.3f))
            .Join(playerTran.DOMove(new Vector2(playerTran.position.x, playerTran.position.y), 0.3f))
            .Join(enemyTran.DOScale(1f, 0.3f))
            .Join(playerTran.DOScale(1f, 0.3f))
            .Append(enemyHP.DOScale(1f, 0.2f))
            .Join(playerHP.DOScale(1f, 0.2f));
    }

    // rowIndex and laneIndex are the location of the lane being attacked.
    public void oneWayAttackPawn(GameObject attacking_card, int rowIndex, int laneIndex, int damage)
    {
        Sequence battleAnim = DOTween.Sequence();
        Transform enemyHP;
        Transform attackTran = attacking_card.transform.GetChild(0).GetChild(7).gameObject.transform;
        GameObject defending_card = field[rowIndex, laneIndex];
        int attackType; // 0 = attack pawn, 1 = direct attack player, 2 = direct attack enemy
        if (defending_card == null) // Direct attack
        {
            if (rowIndex == 0) // Player card attacking
            {
                enemyHP = enemy_Avatar.transform;
                attackType = 1;
            }
            else // Enemy card attacking
            {
                enemyHP = player_Avatar.transform;
                attackType = 2;
            }
        } else // Attack opposing Pawn.
        {
            enemyHP = defending_card.transform.GetChild(0).GetChild(6).gameObject.transform;
            attackType = 0;
        }

        battleAnim.Append(attackTran.DOMove(new Vector2(enemyHP.position.x, enemyHP.position.y + 100), 0.1f))
            .Join(enemyHP.DOScale(1.5f, 0.1f))
            .Join(attackTran.DOScale(1.5f, 0.1f))
            .Append(attackTran.DOMove(enemyHP.transform.position, 0.25f))
            .AppendCallback(() =>
            {
                if (attackType == 0)
                {
                    defending_card.GetComponent<CardBehavior>().updateStats(0, -damage);
                }
                else if (attackType == 1)
                {
                    updateHealth(enemy, -damage);
                }
                else if (attackType == 2)
                {
                    updateHealth(player, -damage);
                }

            })
            .Join(enemyHP.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f, 10, 1))
            .Join(attackTran.DOMove(new Vector2(attackTran.position.x, attackTran.position.y), 0.3f))
            .Join(attackTran.DOScale(1f, 0.3f))
            .Append(enemyHP.DOScale(1f, 0.2f))
            .AppendCallback(() =>
            {
                if (defending_card != null && defending_card.GetComponent<CardBehavior>().getHealth() <= 0)
                {
                    if (rowIndex == 0) // Enemy card destroyed
                    {
                        num_enemy_summoned_card--;
                    }
                    else // Player card destroyed
                    {
                        num_player_summoned_card--;
                    }
                    Destroy(field[rowIndex, laneIndex]);
                }

                if (player.health <= 0 || enemy.health <= 0)
                {
                    StartCoroutine("endGame");
                }
            });
    }

    private bool spinPassTurn(float angle) {
        // if (passTurnSpinner.transform.eulerAngles.z != 180f) {
        //     passTurnSpinner.transform.DORotate(new Vector3(0, 0, 180f), 0.75f);
        // } else if (passTurnSpinner.transform.eulerAngles.z != 0) {
        //     passTurnSpinner.transform.DORotate(new Vector3(0, 0, 0), 0.75f);
        // }

        if (passTurnSpinner.transform.eulerAngles.z != angle) {
            passTurnSpinner.transform.DORotate(new Vector3(0, 0, angle), 0.75f);
            return true;
        }
        return false;
    }

    public IEnumerator enemyTurn() {
        // if (passTurnSpinner.transform.eulerAngles.z != 180f)
        // {
        //     passTurnSpinner.transform.DORotate(new Vector3(0, 0, 180f), 0.75f);
        //     yield return new WaitForSeconds(0.75f);
        // }

        bool spin = spinPassTurn(180f);
        if (spin) 
            yield return new WaitForSeconds(0.75f);
        
        current_turn = turn.ENEMY;
        if (num_enemy_summoned_card == enemy_lanes.transform.childCount || enemy.hand.Count == 0 || enemy_ready_for_battle || enemy.mana == 0) {
            // the field is full or the hand is empty
            enemy_has_summoned = true;

            // if cannot play, ready for battle
            enemy_ready_for_battle = true;

            if (player_ready_for_battle)
            {
                StartCoroutine(indicateTurn("onBattle"));
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
            List<GameObject> enemy_playable_cards = get_playable_cards(0);
            enemy_play_card();

            enemy_has_summoned = true;
            if (enemy_playable_cards.Count > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
            

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
        Instantiate(ManaRegenOverlay, phaseIndicator.transform.parent);
        player_ready_for_battle = false;
        enemy_ready_for_battle = false;
        player.drawCard();
        enemy.drawCard();
        battleNum++;
        turnNum = 1;
        resetMana(player);
        resetMana(enemy);
        ready_Button.GetComponent<Animator>().SetBool("isPushed", false);

        player_free_pass = true;
        player_can_pass = true;

        // Reset actives
        foreach (Transform lane in player_lanes.transform)
        {
            if (!lane.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (lane.childCount > 0)
            {
                GameObject card = lane.GetChild(0).gameObject;
                card.GetComponent<CardBehavior>().abilityUsed = false;
            }

        }

        yield return new WaitForSeconds(0.5f);

        if (next_player == turn.ENEMY)
        {
            next_player = turn.PLAYER;
            StartCoroutine(indicateTurn("enemyTurn"));
            //StartCoroutine("enemyTurn");
        }
        else
        {
            next_player = turn.ENEMY;
            StartCoroutine(indicateTurn("playerTurn"));
            //StartCoroutine("playerTurn");
        }
        StopCoroutine("newRound");
    }

    public IEnumerator endGame()
    {
        
        bool playerWin = enemy.health <= 0;
        if (Conditions.collectingData)
        {
            LoadingController.LOGGER.LogLevelEnd("{ Player Won: " + playerWin + ", Number of battles: " + battleNum + ", Actions taken: " + Conditions.actionsPerLevel + " }");
            LoadingController.LOGGER.LogActionWithNoLevel(55, "{ Player stats: levels completed: " + Conditions.levelsCompleted + ", wins: " + Conditions.wins + ", losses: " + Conditions.losses + " }");
        }

        Conditions.levelsCompleted++;
        //Conditions.actionsPerLevel = 0;
        List<string> promptList = new List<string>();
        List<string> dialogPromptList = new List<string>();

        if (playerWin) {
            LevelManager.clearedLevels.Add(levelID);
            Conditions.wins++;
            List<CardObject> cards = new List<CardObject>();
            if (levelID == 3)
            {
                dialogPromptList.Add("Congratulations! You are now ready for Hero's Draw.");
                dialogPromptList.Add("As promised, here is your first Hero card!");
                dialogPromptList.Add("You may only have 1 of each Hero card in your deck, so build wisely.");
                dialogPromptList.Add("You can obtain a new Hero card every 5 wins, so do your best!");
                dialogPrompt.Setup(dialogPromptList);
                cards.Add(Resources.Load<CardObject>("Cards/Arthur, King of Legend"));
                obtainCard(cards);
                Conditions.wins = 0; // We only care about wins after the tutorial levels.
            } else if (levelID > 3)
            {
                string cardType = "BASIC";
                if (Conditions.wins % 5 == 0)
                {
                    cardType = "HERO";
                }
                List<CardObject> obtainableCards = findObtainableCards(cardType);
                if (obtainableCards.Count == 0)
                {
                    dialogPromptList.Add("All "+ cardType + " cards have been obtained.");
                    dialogPrompt.promptText.fontSize = 25;
                } else
                {
                    dialogPromptList.Add("VICTORY");
                    dialogPrompt.promptText.fontSize = 70;
                }
                dialogPrompt.Setup(dialogPromptList);
                int numCardsToShow = Mathf.Min(3, obtainableCards.Count);
                Debug.Log(numCardsToShow);
                for (int i = 0; i < numCardsToShow; i++)
                {
                    int cardDBIndex = Random.Range(0, obtainableCards.Count);
                    Debug.Log("Random card: " + obtainableCards[cardDBIndex]);
                    CardObject chosenCard = obtainableCards[cardDBIndex];
                    while(cards.Contains(chosenCard))
                    {
                        cardDBIndex = Random.Range(0, obtainableCards.Count);
                        chosenCard = obtainableCards[cardDBIndex];
                    }
                    cards.Add(chosenCard);
                }
                obtainCard(cards);
            }
            
            promptList.Add("VICTORY");
        } else {
            Conditions.losses++;
            promptList.Add("DEFEAT");
        }
        EndPrompt.Setup(promptList);
        
        StopAllCoroutines();
        yield return new WaitForSeconds(1f);
    }

    public List<CardObject> findObtainableCards(string cardType)
    {
        List<CardObject> obtainableCards = new List<CardObject>();
        List<string> cardList;
        if (cardType == "HERO")
        {
            cardList = new List<string>(CardDatabase.heroList);
        } else
        {
            cardList = new List<string>(CardDatabase.cardList);
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            CardObject currCard = Resources.Load<CardObject>("Cards/" + cardList[i]);
            int numOwned = 0;
            Conditions.info collectionInfo;
            if (Conditions.card_collection.TryGetValue(currCard.cardName, out collectionInfo))
            {
                int maxCardsAllowed = collectionInfo.type;
                numOwned += collectionInfo.num;
                Conditions.info deckInfo;
                if (Conditions.deck_collection.TryGetValue(currCard.cardName, out deckInfo))
                {
                    numOwned += deckInfo.num;
                }
                if (numOwned < maxCardsAllowed)
                {
                    obtainableCards.Add(currCard);
                }
            } else
            {
                obtainableCards.Add(currCard);
            }

        }
        return obtainableCards;
    }

    public void obtainCard(List<CardObject> cards)
    {
        if (cards.Count == 0)
        {
            return;
        }
        Instantiate(obtainCardText, EndPrompt.transform);
        int i = 0;
        if (cards.Count == 1)
        {
            i = 1;
        }
        foreach (CardObject card in cards)
        {
            GameObject newCard = Instantiate(obtainableCard, EndPrompt.transform);
            newCard.GetComponent<ObtainableCard>().cardIdentity = card;     
            float scale = newCard.GetComponent<RectTransform>().localScale.x;
            float cardWidth = newCard.GetComponent<RectTransform>().sizeDelta.x * scale * 1.1f;
            newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-cardWidth + i * cardWidth, 0);
            newCard.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 0), 2f);
            i++;
        }
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
            //num_enemy_summoned_card++;
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
    

    public virtual void enemy_play_card() {
        
        int nextMove = Random.Range(0,4);

        switch (nextMove) {
            case 0:
                enemy_play_card_first_block_lane();
                break;
            case 1:
                enemy_play_card_first_open_lane();
                break;
            case 2:
                enemy_play_card_block_strongest_on_field();
                break;
            default:
                enemy_play_strongest_card_open_lane();
                break;
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

    // Plays card into the first open lane
    public void enemy_play_strongest_card_open_lane() {

        // Enemy has a playable card
        // Enemy has an open lane 

        List<int> enemy_open_lanes = get_open_lanes(0);
        List<GameObject> enemy_playable_cards = get_playable_cards(0);

        if (enemy_open_lanes.Count == 0 || enemy_playable_cards.Count == 0) {
            return;
        }

        GameObject strongest_enemy_card = getStrongestCard(0);

        List<int> player_open_lanes = get_open_lanes(1);

        List<int> shared_open_lanes = new List<int>();

        for (int i = 0; i < enemy_open_lanes.Count; i++) {
            if (player_open_lanes.Contains(enemy_open_lanes[i])) {
                shared_open_lanes.Add(enemy_open_lanes[i]);
            }
        }

        int lane_num = enemy_open_lanes[0];

        if (shared_open_lanes.Count != 0) {
            lane_num = shared_open_lanes[0];
        }

        // play the first playable card into the first open space
        
        summon_card(0, lane_num, strongest_enemy_card);
        enemy.hand.Remove(strongest_enemy_card);              
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
        }
        player.mana = player.maxMana;
    }
}
