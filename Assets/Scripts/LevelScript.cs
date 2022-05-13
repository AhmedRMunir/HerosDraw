using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelScript : MonoBehaviour
{
    public int levelID;
    public string levelName;
    public List<int> prerequistieLevels;
    private GameObject playerPiece;

    // Start is called before the first frame update
    void Start()
    {
        playerPiece = GameObject.FindGameObjectWithTag("PlayerPiece");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            Sequence movePlayer = DOTween.Sequence();
            movePlayer.Append(playerPiece.transform.DOMove(transform.position, 0.5f))
                .AppendCallback(() =>
                {
                    //SceneManager.LoadScene(levelName);
                });
        }
    }
}
