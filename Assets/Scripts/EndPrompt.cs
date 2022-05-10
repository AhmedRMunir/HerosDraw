using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPrompt : MonoBehaviour
{
    public Text promptText;
    public string levelName;
    public void Setup(string text) {
        gameObject.SetActive(true);
        promptText.text = text;
        // if (win) {
        //     promptText.text = "YOU WIN :)";
        // } else {
        //     promptText.text = "YOU LOSE :(";
        // }
    }

    public void ContinueButton() {
        if (levelName == "none") {
            gameObject.SetActive(false);
        } else {
            SceneManager.LoadScene(levelName);
        }
    }
}
