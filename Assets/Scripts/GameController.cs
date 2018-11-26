using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Button play;
    public Text score;
    public Text movedBlocksScore;
    public GameObject highScroreMark;
    public float scoreNum;
    public float nextLevel = 1;
    public float highScore;
    public float movedBlocks;

    private bool gameStarted;
    private float scoreIncrement = 3f;
    private PlayerMovement pm;
    private bool setHighScore;
	// Use this for initialization
	void Awake ()
    {
        Time.timeScale = 0;
	}
    private void Start()
    {
        pm = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        highScore = PlayerPrefs.GetFloat("highScore");
    }

    void Update ()
    {
		if(gameStarted && !pm.gameOver)
        {
            scoreNum += scoreIncrement * Time.deltaTime * 60;
            score.text = ((int)scoreNum).ToString();
            movedBlocksScore.text = movedBlocks.ToString();
            if(highScore > 0 && (scoreNum > highScore - 14 / pm.movementSpeed * 180) && !setHighScore) //1100
            {
                Instantiate(highScroreMark, new Vector3(0, 0, pm.gameObject.transform.position.z + 14), Quaternion.identity);
                setHighScore = true;
            }
        }
        else if(pm.gameOver)
        {
            if (scoreNum > highScore)
            {
                PlayerPrefs.SetFloat("highScore", scoreNum); //Сохраняем рекорд
            }
        }
	}

    public void Play()
    {
        gameStarted = true;
        play.gameObject.SetActive(false);
        score.gameObject.SetActive(true);
        movedBlocksScore.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
}
