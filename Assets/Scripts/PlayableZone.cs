using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayableZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform playerLane;
    public GameObject playedCard;
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Eventually add effects to show player is hovering over the playable zones
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Conditions.actionsPerLevel++;
        //gameController.num_player_summoned_card++;
        int laneIndex = playerLane.GetSiblingIndex();
        playedCard.GetComponent<CardBehavior>().summonCard(playerLane, laneIndex);
        GameObject cancelButton = GameObject.FindGameObjectWithTag("Cancel");
        Destroy(cancelButton);
    }
}
