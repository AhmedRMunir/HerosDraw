using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CardBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform cardTran;
    public float upAmount;
    public float upDuration;
    public RectTransform container;
    private Vector2 ogPosition;
    private PlayerController deck;
    public bool summoned;

    public CardObject cardIdentity;
    public Text nameText;
    public Image art;
    public Image cardBG;
    public Text descriptionText;
    public Text costValue;
    public Text healthValue;
    public Text attackValue;
    public Image factionIcon;
    public GameObject cardback;
    public GameObject highlight;
    //public GameObject cardAbility; // Check if equals null to check if card has ability
    public string cardAbility;
    public List<int> abilityParams;
    public bool hasUseableAbility;
    public string cardType;

    private int attackPoints;
    private int healthPoints;
    private int cost;
    private int faction;

    public bool isEnemy;
    public GameObject spawnLocation;
    public GameObject summonIndicator;
    public RectTransform playerHandholder;
    public GameObject cancelButton;

    private RectTransform playedCardSlot;
    public GameController gameController;

    private CardAbility ability;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
        playedCardSlot = GameObject.FindGameObjectWithTag("CardSlot").GetComponent<RectTransform>();
        //summoned = false;
        ogPosition = cardTran.anchoredPosition;
        upAmount = container.sizeDelta.y / 2;

        playerHandholder = GameObject.FindGameObjectWithTag("PlayerHand").GetComponent<RectTransform>();
        nameText.text = cardIdentity.cardName;
        if (isEnemy)
        {
            cardback.SetActive(true);
            deck = GameObject.FindGameObjectWithTag("EnemyDeck").GetComponent<PlayerController>();
            spawnLocation = GameObject.FindGameObjectWithTag("EnemyLanes");
        }
        else
        {
            deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<PlayerController>();
            spawnLocation = GameObject.FindGameObjectWithTag("PlayerLanes");
        }

        art.sprite = cardIdentity.art;
        descriptionText.text = cardIdentity.description;
        cost = cardIdentity.cost;
        costValue.text = "" + cost;
        attackPoints = cardIdentity.attack;
        healthPoints = cardIdentity.health;
        attackValue.text = "" + attackPoints;
        healthValue.text = "" + healthPoints;
        cardAbility = cardIdentity.cardAbility;
        cardType = cardIdentity.cardType;
        abilityParams = new List<int>(cardIdentity.abilityParams);
        ability = GameObject.FindGameObjectWithTag("Ability").GetComponent<CardAbility>();
        hasUseableAbility = false;

        // Update faction icon based on value of faction string, faction ids: 0 = knight, 1 = mage, 2 = vampire
        if (cardIdentity.faction == "Knight")
        {
            faction = 0;
            factionIcon.sprite = Resources.Load<Sprite>("Sprites/Knight");
        } else if (cardIdentity.faction == "Mage")
        {
            faction = 1;
            factionIcon.sprite = Resources.Load<Sprite>("Sprites/Mage");
        } else if (cardIdentity.faction == "Vampire") {
            faction = 2;
            factionIcon.sprite = Resources.Load<Sprite>("Sprites/Vampire");
        } else // Maybe if we wanna have factionless cards
        {
            faction = -1;
            factionIcon.enabled = false;
        }

        // Special graphics for hero and spell cards
        if (cardType == "Hero")
        {
            cardBG.sprite = Resources.Load<Sprite>("Sprites/HeroCard");
        } else if (cardType == "Spell")
        {

        } 

        if (nameText.text.Length > 10
            )
        {
            nameText.fontSize = 13;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (cardIdentity.hasActiveAbility) {
            if (cardIdentity.activeAbilityCost[0] < gameController.player.health && cardIdentity.activeAbilityCost[1] <= gameController.player.mana) {
                hasUseableAbility = true;
            } else {
                hasUseableAbility = false;
            }
        } else {
            hasUseableAbility = false;
        }

        if (isEnemy)
        {
            highlight.SetActive(false);
        } else if ((!gameController.player_has_summoned || (gameController.player_has_summoned && gameController.enemy_ready_for_battle)) && gameController.player_can_play && gameController.current_turn == GameController.turn.PLAYER && !summoned)
        {
            highlight.SetActive(true);
        } else if (summoned && hasUseableAbility && cardAbility != "" && gameController.current_turn == GameController.turn.PLAYER && !gameController.player_ready_for_battle)
        {
            highlight.SetActive(true);
        } else
        {
            highlight.SetActive(false);
        }

        if (cost > deck.mana && !summoned)
        {
            costValue.color = Color.red;
            highlight.SetActive(false);
        } else
        {
            costValue.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!summoned && !isEnemy)
        {
            Sequence hoverCard = DOTween.Sequence();
            hoverCard.Append(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y + upAmount), upDuration))
                .Join(container.DOScale(1.2f, upDuration))
                .PrependCallback(() => { transform.SetAsLastSibling(); })
                .Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!summoned && !isEnemy)
        {
            exitHover();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((!gameController.player_has_summoned || (gameController.player_has_summoned && gameController.enemy_ready_for_battle)) && summoned == false && gameController.current_turn == GameController.turn.PLAYER && gameController.player_can_play && isEnemy == false && deck.mana >= cost)
        {
            Conditions.actionsPerLevel++;
            exitHover();
            /*if (gameController.enemy_ready_for_battle == false )
            {
                gameController.player_has_summoned = true;
            }*/

            gameController.player_is_summoning = true;
            // Attach to Canvas
            GameObject cancel = Instantiate(cancelButton, gameObject.transform.parent.transform.parent.transform);
            cancel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1080 / 2.5f);
            cancel.GetComponent<CancelButton>().selectedCard = this;
            playerHandholder.DOAnchorPos(new Vector2(0, -upAmount * 1.5f), upDuration);
            Sequence enlarge = DOTween.Sequence();
            enlarge.Append(container.DOAnchorPos(new Vector2(playedCardSlot.anchoredPosition.x, upAmount * 1.5f), upDuration))
                .Join(container.DOScale(1.2f, upDuration))
                .Play();
            summoned = true;
            //gameController.player_can_play = false;

            foreach (RectTransform child in spawnLocation.transform)
            {
                if (child.childCount == 0)
                {
                    indicatePlayableLane(child);
                }
            }
        } else if (summoned == true && isEnemy == false && gameController.current_turn == GameController.turn.PLAYER && hasUseableAbility && !gameController.player_ready_for_battle) {
            if (Conditions.collectingData)
                LoadingController.LOGGER.LogActionWithNoLevel(52, "{ Player activated: " + cardAbility + " }");
            Conditions.actionsPerLevel++;
            ability.activeAbility(cardAbility, abilityParams.ToArray());
        }
        
    }

    public void indicatePlayableLane(RectTransform lane)
    {
        GameObject indicator = Instantiate(summonIndicator, lane);
        indicator.GetComponent<PlayableZone>().playerLane = lane;
        indicator.GetComponent<PlayableZone>().playedCard = gameObject;
    }

    public void summonCard(RectTransform spawn, int laneIndex)
    {
        cardback.SetActive(false);
        summoned = true;
        gameController.player_is_summoning = false;
        deck.mana -= cost;
        
        container.SetParent(spawn);
        Sequence activateCard = DOTween.Sequence();
        activateCard
            .AppendCallback(() =>
            {
                deck.hand.Remove(gameObject);
                if (isEnemy)
                {
                    gameController.field[0, laneIndex] = gameObject;
                    if (!cardAbility.Equals(""))
                    {
                        abilityParams[2] = 0;
                        abilityParams[3] = laneIndex;
                    }
                }
                else
                {
                    gameController.player_has_summoned = true;

                    gameController.field[1, laneIndex] = gameObject;
                    if (!cardAbility.Equals(""))
                    {
                        abilityParams[2] = 1;
                        abilityParams[3] = laneIndex;
                    }
                }

                //gameController.player_summoned_card[laneIndex] = cardIdentity;
                removeIndicators();
            })
            .Pause();

        if (cardType == "Hero") // Special animation for playing a hero card, shakes all cards on the field.
        {
            int rowIndex = 0;
            if (!isEnemy)
            {
                rowIndex = 1;
            }
            activateCard.Append(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y), upDuration))
                .Join(container.DOAnchorPos(new Vector2(0, 0), upDuration))
                .Append(container.DOScale(1.5f, upDuration * 2))
                .Append(container.DOScale(1f, upDuration / 2)).SetEase(Ease.Linear)
                .AppendCallback(() =>
                {
                    for (int i = 0; i < gameController.player_lanes.transform.childCount; i++)
                    {
                        if (i != laneIndex || rowIndex != 1)
                        {
                            Transform lane = gameController.player_lanes.transform.GetChild(i);
                            float shake = Random.Range(0.1f,0.5f);
                            activateCard.Append(lane.DOPunchScale(new Vector3(shake, shake, shake), 0.5f, 25, 0)).SetEase(Ease.OutQuad);
                        }
                    }
                    for (int i = 0; i < gameController.enemy_lanes.transform.childCount; i++)
                    {
                        if (i != laneIndex || rowIndex != 0)
                        {
                            Transform lane = gameController.enemy_lanes.transform.GetChild(i);
                            float shake = Random.Range(0.1f,0.5f);
                            activateCard.Append(lane.DOPunchScale(new Vector3(shake, shake, shake), 0.5f, 25, 0)).SetEase(Ease.OutQuad);
                        }
                    }
                });

        } else
        {
            activateCard.Append(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y), upDuration))
                .Join(container.DOAnchorPos(new Vector2(0, 0), upDuration))
                .Join(container.DOScale(1f, upDuration));
        }
        activateCard.Append(playerHandholder.DOAnchorPos(new Vector2(0, 0), upDuration))
            .AppendCallback(() => {
                if (cardIdentity.hasPassiveAbility)
                {
                    ability.passiveAbility(cardAbility, abilityParams.ToArray());
                }
            })
            .Play();


        //gameController.player_can_play = true;
        gameController.player_can_pass = true;
    }

    public void updateStats(int attack, int health)
    {
        attackPoints += attack;
        healthPoints += health;
        attackValue.text = "" + attackPoints;
        healthValue.text = "" + healthPoints;
    }

    public void exitHover()
    {
        Sequence exitHover = DOTween.Sequence();
        exitHover.Append(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y), upDuration))
                .Join(container.DOScale(0.85f, upDuration))
                .Play();
    }

    public void undo()
    {
        removeIndicators();
        Sequence returnToHand = DOTween.Sequence();
        returnToHand.Join(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y), 0))
                .Join(container.DOScale(0.85f, upDuration))
                .Join(playerHandholder.DOAnchorPos(new Vector2(0, 0), upDuration))
                .PrependCallback(() => { deck.shiftHand(deck.cardSpeed/2f);  })
                .AppendCallback(() => { summoned = false; gameController.player_has_summoned = false; })
                .Play();
    }

    public void removeIndicators()
    {
        GameObject[] indicators = GameObject.FindGameObjectsWithTag("Indicator");
        foreach (GameObject indiciator in indicators)
        {
            Destroy(indiciator);
        }
    }

    public int getAttack() {
        return attackPoints;
    }

    public int getHealth() {
        return healthPoints;
    }

    public int getCost() {
        return cost;
    }
    public int getFaction()
    {
        return faction;
    }
}
