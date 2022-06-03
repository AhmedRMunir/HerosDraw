using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cse481.logging;

public class LoadingController : MonoBehaviour
{
    public static CapstoneLogger LOGGER;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        string skey = "1af01d188ea7d9060535afa83c2c1647";
        int gameId = 202205;
        string gameName = "herosdraw";
        // Increment this by 1 for each new official release, (2 = v1.0, 3 = v2.0, 4 = v2.1, 5 = final release mandatory tutorial, 6 = final release optional tutorial)
        int cid = Random.Range(5, 7);
        Debug.Log("cid: " + cid);
        CapstoneLogger logger = new CapstoneLogger(gameId, gameName, skey, cid);

        string userId = logger.GenerateUuid();
        StartCoroutine(logger.StartNewSession(userId));
        LoadingController.LOGGER = logger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        LoadingController.LOGGER.LogActionWithNoLevel(199, "press play");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
