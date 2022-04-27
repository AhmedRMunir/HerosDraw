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
    private DeckController deck;
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

    public bool isEnemy;
    public GameObject spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        summoned = false;
        ogPosition = cardTran.anchoredPosition;
        upAmount = container.sizeDelta.y / 2;

        nameText.text = cardIdentity.cardName;
        if (isEnemy)
        {
            cardback.SetActive(true);
            deck = GameObject.FindGameObjectWithTag("EnemyDeck").GetComponent<DeckController>();
            spawnLocation = GameObject.FindGameObjectWithTag("EnemyLanes");
        }
        else
        {
            art.sprite = cardIdentity.art;
            deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckController>();
            spawnLocation = GameObject.FindGameObjectWithTag("PlayerLanes");
        }
        
        descriptionText.text = cardIdentity.description;
        costValue.text = "" + cardIdentity.cost;
        healthValue.text = "" + cardIdentity.health;
        attackValue.text = "" + cardIdentity.attack;

        // Update faction icon based on value of faction string
        if (cardIdentity.faction == "Knight")
        {

        } else if (cardIdentity.faction == "Mage")
        {

        } else if (cardIdentity.faction == "Vampire") {

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
            Sequence exitHover = DOTween.Sequence();
            exitHover.Append(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y), upDuration))
                    .Join(container.DOScale(0.85f, upDuration))
                    .Play();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Conditions.canPlay && summoned == false)
        {
            summonCard();
        }
        
    }

    public void summonCard()
    {
        Debug.Log("clicked");
        cardback.SetActive(false);
        summoned = true;
        Conditions.canPlay = false;
        RectTransform spawn = null;
        if (isEnemy)
        {
            spawn = (RectTransform)spawnLocation.transform.GetChild(Conditions.enemyLanesOccupied);
            Conditions.enemyLanesOccupied++;
        } else
        {
            spawn = (RectTransform)spawnLocation.transform.GetChild(Conditions.playerLanesOccupied);
            Conditions.playerLanesOccupied++;
        }
        
        container.SetParent(spawn);
        Sequence activateCard = DOTween.Sequence();
        activateCard.AppendCallback(() => { deck.hand.Remove(gameObject); })
            .Append(cardTran.DOAnchorPos(new Vector2(ogPosition.x, ogPosition.y), upDuration))
            .Join(container.DOAnchorPos(new Vector2(0, 0), upDuration))
            .Join(container.DOScale(1f, upDuration))
            .AppendCallback(() => { Conditions.canPlay = true; })
            .Play();
    }
}
