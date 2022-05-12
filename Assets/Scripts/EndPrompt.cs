using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPrompt : MonoBehaviour
{
    public Text promptText;

    public List<string> promptList;
    public string levelName;
    public bool pressed;
    public void Setup(List<string> promptList) {

        this.promptList = promptList; 
        promptText.text = promptList[0];
        gameObject.SetActive(true);
        promptList.Remove(promptList[0]);
    }

    public void ContinueButton() {

        if (promptList.Count > 0) {
            promptText.text = promptList[0];
            promptList.Remove(promptList[0]);
        } else {
            if (levelName == "none") {
                gameObject.SetActive(false);
            } else {
                SceneManager.LoadScene(levelName);
            }
        }
        pressed = true;
        Debug.Log("Button pressed is: " + pressed);        
    }

    private void Update()
    {
        
    }
}
