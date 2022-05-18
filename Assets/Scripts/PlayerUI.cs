using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerController player;
    public Text HP_Text;
    public Text Mana_Text;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        HP_Text.text = player.health + "";
        if (player.isPlayer)
        {
            if (player.prevMana != player.mana)
            {
                player.prevMana = player.mana;
                if (!gameController.playerHasPlayable())
                {
                    gameController.player_can_pass = false;
                } 
            }
        }
        Mana_Text.text = player.mana + "/" + player.maxMana;
    }
}
