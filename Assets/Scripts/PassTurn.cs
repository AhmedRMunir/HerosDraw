using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassTurn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private GameController battleController;
    // Start is called before the first frame update
    void Start()
    {
        battleController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
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
        if (battleController.current_turn == GameController.turn.PLAYER) {
            battleController.current_turn = GameController.turn.ENEMY;
        } else {
            battleController.current_turn = GameController.turn.PLAYER;
        }
    }
}
