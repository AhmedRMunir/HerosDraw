using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyDeckController : PlayerController
{

    private float cardWidth;

    // Start is called before the first frame update
    void Start()
    {
        cardWidth = completeCard.GetComponent<RectTransform>().sizeDelta.x * completeCard.GetComponent<RectTransform>().localScale.x;
        initEnemyDeck();
    }

    private void initEnemyDeck() {

        // Clear current deck
        deck = new List<CardObject>();   

        int deckCount = UnityEngine.Random.Range(15, 20);
        int allCardsCount = Conditions.enemyDeck.Count;
        for (int i = 0; i < deckCount; i++) {
            int newCardNum = UnityEngine.Random.Range(0, allCardsCount);
            deck.Add(Resources.Load<CardObject>("Cards/" + Conditions.enemyDeck[newCardNum]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (handSize != hand.Count)
        {
            handSize = hand.Count;
            shiftHand(cardSpeed / 4);
        }

        if (deck.Count == 0)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
    }

    public override void shiftCard(RectTransform rt, int i, float cardSpeed)
    {
        if (handSize % 2 == 0)
        {
            rt.DOAnchorPos(new Vector2((cardWidth / 2 + cardWidth * (handSize / 2 - 1) + cardGap / 2 + cardGap * (handSize / 2 - 1) - cardWidth * i - cardGap * i), 1080 /2.1f), cardSpeed);
        }
        else
        {
            rt.DOAnchorPos(new Vector2((cardWidth * (handSize - 1) / 2 + cardGap * (handSize - 1) / 2 - cardWidth * i - cardGap * i), 1080 / 2.1f), cardSpeed);
        }
    }

    public override void drawCard()
    {
        if (deck.Count > 0)
        {
            CardObject drawnCardIdentity = deck[0];
            deck.RemoveAt(0);
            GameObject drawnCard = Instantiate(completeCard, canvas.transform);
            drawnCard.transform.SetParent(handHolder.transform);
            drawnCard.GetComponent<CardBehavior>().cardIdentity = drawnCardIdentity;
            drawnCard.GetComponent<CardBehavior>().isEnemy = true;
            hand.Add(drawnCard);
            RectTransform rt = drawnCard.GetComponent<RectTransform>();
            rt.anchoredPosition = deckObject.GetComponent<RectTransform>().anchoredPosition;
        }

    }

    
}
