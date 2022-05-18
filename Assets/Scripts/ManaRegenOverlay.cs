using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegenOverlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            gameObject.GetComponent<Animator>().SetBool("isOver", true);
        }
    }

    public void animationEnd()
    {
        Destroy(gameObject);
    }
}
