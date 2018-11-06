using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Button play;
    public Text score;
    public float scoreNum;
    public float nextLevel = 1;
    private bool gameStarted;
    private float scoreIncrement = 3f;

    private PlayerMovement pm;
	// Use this for initialization
	void Awake ()
    {
        Time.timeScale = 0;
	}
    private void Start()
    {
        pm = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
    }
    // Update is called once per frame
    void Update ()
    {
		if(gameStarted && !pm.gameOver)
        {
            scoreNum += scoreIncrement * Time.deltaTime * 60;
            score.text = ((int)scoreNum).ToString();
        }
	}

    public void Play()
    {
        gameStarted = true;
        play.gameObject.SetActive(false);
        score.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
}
