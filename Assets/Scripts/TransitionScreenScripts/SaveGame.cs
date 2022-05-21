using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveGame : MonoBehaviour, IPointerClickHandler
{

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
        //LevelManager.saveGame();
        //Conditions.saveCards();
        LevelManager.loadNewLevel("Title Screen");
    }
}
