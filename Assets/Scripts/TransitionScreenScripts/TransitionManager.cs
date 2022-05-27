using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{   

    public Text levelCompleted;

    public Text wins;

    public Text losses;

    // Start is called before the first frame update
    void Start()
    {
        Conditions.saveCards();
    }

    // Update is called once per frame
    void Update()
    {
        wins.text = "Wins: " + Conditions.wins;
        losses.text = "Losses: " + Conditions.losses;
        levelCompleted.text = "Level Completed: " + Conditions.levelsCompleted;
    }
    
}
