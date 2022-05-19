using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SummonsLeft : MonoBehaviour
{
    private GameController gameController;
    public TMPro.TextMeshProUGUI text;
    public GameObject fadeToBlack;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
        GameObject fade = Instantiate(fadeToBlack, transform.parent);
        Image fadeBG = fade.GetComponent<Image>();
        fadeBG.color = Color.black;
        Sequence sceneTransition = DOTween.Sequence();
        sceneTransition.Append(fadeBG.DOFade(0f, 1f))
            .AppendCallback(() =>
            {
                Destroy(fade);
            });
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.current_turn == GameController.turn.PLAYER && gameController.enemy_ready_for_battle)
        {
            text.text = "Summons Left: \u221E";
        } else if (gameController.current_turn == GameController.turn.PLAYER && !gameController.player_has_summoned)
        {
            text.text = "Summons Left: 1";
        } else
        {
            text.text = "Summons Left: 0";
        }
    }

}
