using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DeckCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform cardTran;
    public float upAmount;
    public float upDuration;
    public RectTransform container;
    private Vector2 ogPosition;
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

    // Start is called before the first frame update
    void Start()
    {
        ogPosition = cardTran.anchoredPosition;
        upAmount = container.sizeDelta.y / 2;
        nameText.text = cardIdentity.cardName;

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

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (Conditions.card_collection.ContainsKey(cardIdentity.cardName))
        {
            Conditions.card_collection[cardIdentity.cardName].num++;
        } else
        {
            Conditions.card_collection.Add(cardIdentity.cardName, new Conditions.info(cardIdentity, Conditions.card_type.Regular, 1));
        }
    }
}
