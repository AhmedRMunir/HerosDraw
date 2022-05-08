using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    public GameController gm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void passiveAbility(string cardAbility, int[] abilityParams) {
        StartCoroutine(cardAbility, abilityParams);
    }

    public IEnumerator reinforce(int[] values) {
        Debug.Log("reinforce is called");
        for (int i = 0; i < gm.field.GetLength(1); i++) {
            GameObject player_card = gm.field[1,i];

            if (player_card != null) {
                player_card.GetComponent<CardBehavior>().updateStats(values[0], values[1]);
            }
        }
        yield return new WaitForSeconds(1f);
    }
}
