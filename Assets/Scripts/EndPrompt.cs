using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPrompt : MonoBehaviour
{
    public Text promptText;
    public string levelName;
    public void Setup(bool win) {
        gameObject.SetActive(true);
        if (win) {
            promptText.text = "YOU WIN :)";
        } else {
            promptText.text = "YOU LOSE :(";
        }
    }

    public void ContinueButton() {
        SceneManager.LoadScene(levelName);
    }
}
