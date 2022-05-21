using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartGame : MonoBehaviour, IPointerClickHandler
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
                Conditions.levelsCompleted = 0;
                Conditions.wins = 0;
                Conditions.losses = 0;
                LevelManager.loadNewLevel();
            });
        //SceneManager.LoadScene("Tutorial-1");
        //SceneManager.LoadScene("Battle");
    }
}
