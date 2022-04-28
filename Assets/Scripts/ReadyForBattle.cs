using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReadyForBattle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image playerReady;
    public Image enemyReady;
    private GameController battleController;

    // Start is called before the first frame update
    void Start()
    {
        battleController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (battleController.enemy_ready_for_battle)
        {
            enemyReady.color = Color.white;
        }
        else
        {
            enemyReady.color = Color.gray;
        }
        if (battleController.player_ready_for_battle)
        {
            playerReady.color = Color.white;
        }
        else
        {
            playerReady.color = Color.gray;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        battleController.player_ready_for_battle = true;
    }

}
