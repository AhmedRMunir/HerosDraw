using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndPrompt : MonoBehaviour
{
    public Text promptText;

    public List<string> promptList;
    public string levelName;
    public bool pressed;
    public GameObject fadeToBlack;
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
                GameObject fade = Instantiate(fadeToBlack, transform.parent);
                Image fadeBG = fade.GetComponent<Image>();
                Sequence sceneTransition = DOTween.Sequence();
                sceneTransition.Append(fadeBG.DOFade(1f, 1f))
                    .AppendCallback(() =>
                    {
                        LevelManager.loadNewLevel();
                        LevelManager.saveGame();
                    });

                
            }
        }
        pressed = true;
        Debug.Log("Button pressed is: " + pressed);        
    }

    private void Update()
    {
        
    }
}
