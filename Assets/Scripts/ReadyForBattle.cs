using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReadyForBattle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image playerReady;
    public Image enemyReady;
    public GameObject highlight;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.current_turn == GameController.turn.PLAYER && (gameController.enemy_ready_for_battle == true || !gameController.playerHasPlayable()) && gameController.player_can_play)
        {
            highlight.SetActive(true);
        } else
        {
            highlight.SetActive(false);
        }

        if (gameController.enemy_ready_for_battle)
        {
            enemyReady.color = Color.white;
        }
        else
        {
            enemyReady.color = Color.gray;
        }
        if (gameController.player_ready_for_battle)
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
        if (gameController.current_turn == GameController.turn.PLAYER && gameController.player_can_play && !gameController.player_is_summoning)
        {
            if (Conditions.collectingData)
            {
                LoadingController.LOGGER.LogLevelAction(51, "{ Player readied for battle }");
                LoadingController.LOGGER.LogActionWithNoLevel(51, "{ Player readied for battle }");
            }
            Conditions.actionsPerLevel++;

            gameObject.GetComponent<Animator>().SetBool("isPushed", true);
            gameController.player_ready_for_battle = true;
            gameController.player_can_play = false;
            if (gameController.enemy_ready_for_battle)
            {
                gameController.StartCoroutine(gameController.indicateTurn("onBattle"));
            } else
            {
                gameController.StartCoroutine(gameController.enemyTurn());
            }
            
        }
        
    }

}
