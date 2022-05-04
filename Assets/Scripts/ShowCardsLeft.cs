using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowCardsLeft : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text cardsLeftText;
    public PlayerController deckController;

    // Start is called before the first frame update
    void Start()
    {
        cardsLeftText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardsLeftText.text = "" + deckController.deck.Count;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardsLeftText.text = "";
    }

}
