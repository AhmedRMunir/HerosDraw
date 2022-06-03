using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void skipTutorial()
    {
        Conditions.levelsCompleted = 2;
        LevelManager.loadNewLevel();
    }

}
