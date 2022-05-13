using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



/*
 Tutorial - 3 - Full game
 
*/

public class Tutorial_3_Controller : GameController
{

    public override IEnumerator gameStart()
    {
        List<string> promptList = new List<string>();
        promptList.Add("You've come so far. I'm proud of you :')\nTime for a proper Battle!");
        promptList.Add("Win this battle to win your very first Hero card!");
        dialogPrompt.Setup(promptList);

        yield return new WaitUntil(() => dialogPrompt.pressed);
        dialogPrompt.pressed = false;
        yield return new WaitUntil(() => dialogPrompt.pressed);
        dialogPrompt.pressed = false;
        StartCoroutine(base.gameStart());
    }

}
