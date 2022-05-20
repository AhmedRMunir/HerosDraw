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
    public GameObject highlight;
    public Image freePass;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.current_turn != GameController.turn.PLAYER || gameController.enemy_ready_for_battle == true || !gameController.player_can_play || !gameController.player_can_pass)
        {
            highlight.SetActive(false);
            passButton.color = new Color(0.6235294f, 0.6235294f, 0.6235294f);
        } else
        {
            if (!gameController.player_ready_for_battle)
            {
                highlight.SetActive(true);
            }
            passButton.color = Color.white;
        }

        if (gameController.player_can_pass)
        {
            freePass.color = Color.white;
        } else
        {
            freePass.color = new Color(0.6235294f, 0.6235294f, 0.6235294f);
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
        if (gameController.current_turn == GameController.turn.PLAYER && !gameController.enemy_ready_for_battle && gameController.player_can_play && gameController.player_can_pass && !gameController.player_is_summoning)
        {
            if (Conditions.collectingData)
            {
                LoadingController.LOGGER.LogLevelAction(50, "{ Player passed turn }");
                LoadingController.LOGGER.LogActionWithNoLevel(50, "{ Player passed turn }");    
            }
            Conditions.actionsPerLevel++;


            gameController.player_can_play = false;

            if (gameController.player_free_pass && !gameController.player_has_summoned) {
                gameController.player_free_pass = false;
            }

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
