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

    // Start is called before the first frame update
    void Start()
    {
        
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
        int laneIndex = playerLane.GetSiblingIndex();
        playedCard.GetComponent<CardBehavior>().summonCard(playerLane, laneIndex);
        GameObject cancelButton = GameObject.FindGameObjectWithTag("Cancel");
        Destroy(cancelButton);
    }
}
