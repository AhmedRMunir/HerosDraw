using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.loadGame();
        Conditions.loadCards();
    }

    // Update is called once per frame
    void Update()
    {
        if (Conditions.levelsCompleted > 0)
        {
            continueButton.SetActive(true);
        }
    }
}
