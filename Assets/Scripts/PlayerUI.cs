using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerController player;
    public Text HP_Text;
    public Text Mana_Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HP_Text.text = player.health + "";
        Mana_Text.text = player.mana + "";
    }
}
