using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ResumeGame : MonoBehaviour, IPointerClickHandler
{
    public GameObject fadeToBlack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject fade = Instantiate(fadeToBlack, transform.parent);
        Image fadeBG = fade.GetComponent<Image>();
        Sequence sceneTransition = DOTween.Sequence();
        sceneTransition.Append(fadeBG.DOFade(1f, 1f))
            .AppendCallback(() =>
            {
                switch (Conditions.levelsCompleted)
                {
                    case 0:
                        SceneManager.LoadScene("Tutorial-1");
                        break;
                    case 1:
                        SceneManager.LoadScene("Tutorial-2");
                        break;
                    case 2:
                        SceneManager.LoadScene("Tutorial-3");
                        break;
                    default:
                        SceneManager.LoadScene("Transition Screen");
                        break;
                }
            });
        
        //SceneManager.LoadScene("Tutorial-1");
        //SceneManager.LoadScene("Battle");
    }
}
