using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPrompt : MonoBehaviour
{
    public Text promptText;
    public void Setup(bool win) {
        gameObject.SetActive(true);
        if (win) {
            promptText.text = "YOU WIN :)";
        } else {
            promptText.text = "YOU LOSE :(";
        }
    }

    public void ContinueButton() {
        SceneManager.LoadScene("Tutorial-1");
    }
}
