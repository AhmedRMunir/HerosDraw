using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static int currentLevelID;
    public static List<int> clearedLevels;

    public static void loadNewLevel() {
        // front end stuff here
        switch (Conditions.levelCompleted) {
            case 0:
                SceneManager.LoadScene("Tutorial-1");
                break;
            case 1:
                SceneManager.LoadScene("Tutorial-2");
                break;
            case 2:
                SceneManager.LoadScene("Tutorial-3");
                break;
            // case 3:
            //     SceneManager.LoadScene("Tutorial-3");
            //     break;
            default:
                SceneManager.LoadScene("Battle");
                break;
        }
    }
}
