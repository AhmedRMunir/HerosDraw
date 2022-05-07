using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class PassTurn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private GameController gameController;
    public Image passButton;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.current_turn != GameController.turn.PLAYER || gameController.enemy_ready_for_battle == true || !gameController.player_can_play)
        {
            passButton.color = new Color(0.6235294f, 0.6235294f, 0.6235294f);
        } else
        {
            passButton.color = Color.white;
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
        if (gameController.current_turn == GameController.turn.PLAYER && !gameController.enemy_ready_for_battle && gameController.player_can_play)
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
