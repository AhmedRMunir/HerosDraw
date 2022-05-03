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
    private bool summoned;

    public CardObject cardIdentity;
    public Text nameText;
    public Image art;
    public Text descriptionText;
    public Text costValue;
    public Text healthValue;
    public Text attackValue;
    public Image factionIcon;
    public GameObject cardback;
    public GameObject cardAbility; // Check if equals null to check if card has ability
    public bool hasActivatedAbility;

    private int attackPoints;
    private int healthPoints;
    private int cost;

    public bool isEnemy;
    public GameObject spawnLocation;
    public GameObject summonIndicator;
    public RectTransform playerHandholder;
    public GameObject cancelButton;

    private RectTransform playedCardSlot;
    private GameController battleController;

    // Start is called before the first frame update
    void Start()
    {
        battleController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
        playedCardSlot = GameObject.FindGameObjectWithTag("CardSlot").GetComponent<RectTransform>();
        summoned = false;
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
            art.sprite = cardIdentity.art;
            deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<PlayerController>();
            spawnLocation = GameObject.FindGameObjectWithTag("PlayerLanes");
        }
        
        descriptionText.text = cardIdentity.description;
        cost = cardIdentity.cost;
        costValue.text = "" + cost;
        attackPoints = cardIdentity.attack;
        healthPoints = cardIdentity.health;
        attackValue.text = "" + attackPoints;
        healthValue.text = "" + healthPoints;
        cardAbility = cardIdentity.cardAbility;
        hasActivatedAbility = false;

        // Update faction icon based on value of faction string
        if (cardIdentity.faction == "Knight")
        {
            factionIcon.sprite = Resources.Load<Sprite>("Sprites/Knight");
        } else if (cardIdentity.faction == "Mage")
        {
            factionIcon.sprite = Resources.Load<Sprite>("Sprites/Mage");
        } else if (cardIdentity.faction == "Vampire") {
            factionIcon.sprite = Resources.Load<Sprite>("Sprites/Vampire");
        } else // Maybe if we wanna have factionless cards
        {
        }

    }

    // Update is called once per frame
    void Update()
    {

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
        if (!battleController.player_has_summoned && summoned == false && battleController.current_turn == GameController.turn.PLAYER && battleController.player_can_play) // Eventually add check if the card is an enemy card
        {
            exitHover();
            if (battleController.enemy_ready_for_battle == false )
            {
                battleController.player_has_summoned = true;
            }
            
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

            foreach (RectTransform child in spawnLocation.transform)
            {
                if (child.childCount == 0)
                {
                    indicatePlayableLane(child);
                }
            }
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
        //battleController.playerMana -= cost;
        
        container.SetParent(spawn);
        Sequence activateCard = DOTween.Sequence();
        activateCard
            .AppendCallback(() => {
                deck.hand.Remove(gameObject);
                if (isEnemy) {
                    battleController.field[0,laneIndex] = gameObject;
                } else {
                    battleController.field[1,laneIndex] = gameObject;
                }
                
                //battleController.player_summoned_card[laneIndex] = cardIdentity;
                removeIndicators();
            })
            .Append(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y), upDuration))
            .Join(container.DOAnchorPos(new Vector2(0, 0), upDuration))
            .Join(container.DOScale(1f, upDuration))
            .Append(playerHandholder.DOAnchorPos(new Vector2(0, 0), upDuration))
            .Play();
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
                .PrependCallback(() => { deck.shiftHand(deck.cardSpeed);  })
                .AppendCallback(() => { summoned = false; battleController.player_has_summoned = false; })
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
}
