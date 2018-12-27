using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Button play;
    public GameObject uiCoin;
    public Text score;
    public Text maxScore;
    public Text movedBlocksScore;
    public Text collectedCoins;
    public GameObject highScroreMark;
    public GameObject servicesPanel;
    public float scoreNum;
    public int nextLevel = 1;
    public float highScore;
    public int movedBlocks;
    public static int collectedCoinsNum;

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
        collectedCoinsNum = PlayerPrefs.GetInt("collectedCoins");
        collectedCoins.text = collectedCoinsNum.ToString();
        highScore = PlayerPrefs.GetFloat("highScore");
        maxScore.text = "BEST " + PlayerPrefs.GetInt("maxMovedBlocks", 0);
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
                Instantiate(highScroreMark, new Vector3(-1, 0, pm.gameObject.transform.position.z + 14), Quaternion.identity);
                setHighScore = true;
            }
        }
        else if(pm.gameOver)
        {
            PlayerPrefs.SetInt("collectedCoins", collectedCoinsNum);
            if(movedBlocks > PlayerPrefs.GetInt("maxMovedBlocks"))
            {
                PlayerPrefs.SetInt("maxMovedBlocks", movedBlocks);
                score.gameObject.SetActive(false);
                movedBlocksScore.gameObject.SetActive(false);
                maxScore.gameObject.SetActive(true);
                maxScore.text = "NEW RECORD!\n" + movedBlocks;
            }
            if (scoreNum > highScore)
            {
                PlayerPrefs.SetFloat("highScore", scoreNum); //Сохраняем рекорд
            }
        }
	}

    public void Play()
    {
        gameStarted = true;
        maxScore.gameObject.SetActive(false);
        uiCoin.SetActive(false);
        servicesPanel.SetActive(false);
        collectedCoins.gameObject.SetActive(false);
        play.gameObject.SetActive(false);
        score.gameObject.SetActive(true);
        movedBlocksScore.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
}
