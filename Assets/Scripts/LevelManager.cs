using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static int currentLevelID;
    public static List<int> clearedLevels;

    public static void loadNewLevel(string levelName) {
        // front end stuff here
        SceneManager.LoadScene(levelName);
    }
}
