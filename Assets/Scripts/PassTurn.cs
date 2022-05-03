using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PassTurn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameController.current_turn == GameController.turn.PLAYER && gameController.enemy_ready_for_battle == false && gameController.player_can_play == true)
        {
            gameController.player_can_play = false;
            if (gameController.current_turn == GameController.turn.PLAYER)
            {
                //gameController.current_turn = GameController.turn.ENEMY;
                gameController.StartCoroutine(gameController.enemyTurn());
            }
            else
            {
                //gameController.current_turn = GameController.turn.PLAYER;
                gameController.StartCoroutine(gameController.playerTurn());
            }
        }
    }
}
