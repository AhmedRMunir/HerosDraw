using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyDeckController : DeckController
{

    private float cardWidth;

    // Start is called before the first frame update
    void Start()
    {
        handSize = handSizeStart;
        cardWidth = completeCard.GetComponent<RectTransform>().sizeDelta.x * completeCard.GetComponent<RectTransform>().localScale.x;

        shuffle();
        for (int i = 0; i < handSize; i++)
        {
            CardObject drawnCardIdentity = deck[0];
            deck.RemoveAt(0);
            GameObject drawnCard = Instantiate(completeCard, canvas.transform);
            drawnCard.GetComponent<CardBehavior>().cardIdentity = drawnCardIdentity;
            drawnCard.GetComponent<CardBehavior>().isEnemy = true;
            hand.Add(drawnCard);
            RectTransform rt = drawnCard.GetComponent<RectTransform>();
            rt.anchoredPosition = deckObject.GetComponent<RectTransform>().anchoredPosition;
        }
        shiftHand(cardSpeed);
        Conditions.canPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (handSize != hand.Count)
        {
            handSize = hand.Count;
            shiftHand(cardSpeed / 4);
        }
    }

    public override void shiftCard(RectTransform rt, int i, float cardSpeed)
    {
        if (handSize % 2 == 0)
        {
            rt.DOAnchorPos(new Vector2((cardWidth / 2 + cardWidth * (handSize / 2 - 1) + cardGap / 2 + cardGap * (handSize / 2 - 1) - cardWidth * i - cardGap * i), Screen.height/2.1f), cardSpeed);
        }
        else
        {
            rt.DOAnchorPos(new Vector2((cardWidth * (handSize - 1) / 2 + cardGap * (handSize - 1) / 2 - cardWidth * i - cardGap * i), Screen.height/ 2.1f), cardSpeed);
        }
    }

    public override void drawCard()
    {
        if (deck.Count > 0)
        {
            CardObject drawnCardIdentity = deck[0];
            deck.RemoveAt(0);
            GameObject drawnCard = Instantiate(completeCard, canvas.transform);
            drawnCard.GetComponent<CardBehavior>().cardIdentity = drawnCardIdentity;
            drawnCard.GetComponent<CardBehavior>().isEnemy = true;
            hand.Add(drawnCard);
            RectTransform rt = drawnCard.GetComponent<RectTransform>();
            rt.anchoredPosition = deckObject.GetComponent<RectTransform>().anchoredPosition;
        }

    }
}
