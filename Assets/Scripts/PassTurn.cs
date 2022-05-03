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
            Sequence passSpin = DOTween.Sequence();
            passSpin.Append(transform.DORotate(new Vector3(0, 0, transform.GetComponent<RectTransform>().eulerAngles.z - 180f), 1f))
                .AppendCallback(() =>
                {
                    if (gameController.current_turn == GameController.turn.PLAYER)
                    {
                        gameController.current_turn = GameController.turn.ENEMY;
                    }
                    else
                    {
                        gameController.current_turn = GameController.turn.PLAYER;
                    }

                    gameController.turnNum += 1;
                })
                .Play();
        }
    }
}
