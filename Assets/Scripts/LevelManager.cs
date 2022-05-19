using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static int currentLevelID; // Use this when we implement the map
    public static string currentLevelName;
    public static List<int> clearedLevels = new List<int>();

    public static void loadNewLevel() {
        /*currentLevelName = levelName;
        SceneManager.LoadScene(levelName);*/
        switch (Conditions.levelsCompleted) {
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

    public static void saveGame()
    {
        PlayerPrefs.SetInt("LevelsCompleted", Conditions.levelsCompleted);
        PlayerPrefs.SetInt("Wins", Conditions.wins);
        PlayerPrefs.SetInt("Losses", Conditions.losses);
        //PlayerPrefs.SetInt("CurrentLevelID", currentLevelID);
        PlayerPrefs.SetString("CurrentLevelName", currentLevelName);
        PlayerPrefs.SetString("ClearedLevels", string.Join("/n", clearedLevels));
    }

    public static void loadGame()
    {
        Conditions.levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
        Conditions.wins = PlayerPrefs.GetInt("Wins");
        Conditions.wins = PlayerPrefs.GetInt("Losses");
        //currentLevelID = PlayerPrefs.GetInt("CurrentLevelID");
        currentLevelName = PlayerPrefs.GetString("CurrentLevelName");
        string[] clearedLevelsData = PlayerPrefs.GetString("ClearedLevels").Split("/n");
        for (int i = 0; i < clearedLevelsData.Length; i++)
        {
            clearedLevels.Add(int.Parse(clearedLevelsData[i]));
        }
        Debug.Log(Conditions.levelsCompleted + " " + currentLevelName + "clearedLevels");
    }
}
