using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour {
    public float spawnBlockTime;
    public float speedIncrement;
    public float minSpawnBlockTime;
    public GameObject[] obstaclePrefab;
    public List<MoveOnSwipe> obstacleMoveScripts = new List<MoveOnSwipe>();
    public static bool checkForPlayerPass;

    private GameController gc;
    private GameObject newObstacle;
    private GameObject player;
    private RegisterSwipe registerSwipeScript;
    private Vector3 spawnPos;
    private float timer;
    private float delay;
    private float posX = 0, posY = 0, oldPosX;
    private bool wasNotInMiddleX, wasNotInMiddleY;
    private float oldLevel = 1;
    void Start ()
    {
        gc = FindObjectOfType(typeof(GameController)) as GameController;
        registerSwipeScript = FindObjectOfType(typeof (RegisterSwipe)) as RegisterSwipe;
        newObstacle = Instantiate(obstaclePrefab[0], Vector3.forward * 3 + Vector3.right, Quaternion.identity);
        spawnPos = newObstacle.transform.position;
        obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
        registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        player = GameObject.FindGameObjectWithTag("Player");
        timer = spawnBlockTime;
    }

    void Update ()
    {
		if(Time.time >= timer)
        {
            SpawnNewBlock();
            delay = Time.time - timer;
            timer = Time.time + spawnBlockTime - delay;
        }
        CheckForPlayerPass();
        if (gc.nextLevel != oldLevel)
        {
            spawnBlockTime = (spawnBlockTime > minSpawnBlockTime) ? spawnBlockTime - speedIncrement : spawnBlockTime;
            oldLevel = gc.nextLevel;
        }
    }
    public void SpawnNewBlock()
    {
        if (false)//(gc.scoreNum < 2500)
        {
            posX = Random.Range(-1, 2);
            if(wasNotInMiddleX)
            {
                posX = 0;
                wasNotInMiddleX = false;
            }
            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, 0, spawnPos.z + 1f), Quaternion.identity);
            if (posX != 0)
                wasNotInMiddleX = true;
        }
        else if (false)//(gc.scoreNum < 6000)
        {
            gc.nextLevel = 2;
            posX = Random.Range(-1, 2);
            if(posX == 0)
                posY = Random.Range(-1, 1);
            if (wasNotInMiddleX)
            {
                posX = 0;
                wasNotInMiddleX = false;
            }
            if (wasNotInMiddleY || oldPosX == 1)
            {
                posY = 0;
                wasNotInMiddleY = false;
            }
            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
            if (posX != 0)
                wasNotInMiddleX = true;
            if (posY != 0)
                wasNotInMiddleY = true;

            oldPosX = posX;
        }
        else //if (gc.scoreNum < 15000)
        {
            gc.nextLevel = 3;
            posX = Random.Range(-1, 2);
            if (posX == 0)
                posY = Random.Range(-1, 2);
            if (wasNotInMiddleX)
            {
                posX = 0;
                wasNotInMiddleX = false;
            }
            if (wasNotInMiddleY || oldPosX == 1)
            {
                posY = 0;
                wasNotInMiddleY = false;
            }
            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
            if (posX != 0)
                wasNotInMiddleX = true;
            if (posY != 0)
                wasNotInMiddleY = true;

            oldPosX = posX;
        }
        spawnPos = newObstacle.transform.position;
        if (newObstacle.transform.position.x != 0 || newObstacle.transform.position.y != 0)
        {
            obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
            if (obstacleMoveScripts.Count == 1)
                registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        }
    }
    public void RemoveFirstObstacle()
    {
        checkForPlayerPass = false;
        obstacleMoveScripts.RemoveAt(0);
        if(obstacleMoveScripts.Count > 0)
            registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        else
            registerSwipeScript.UpdateFirstObstacle(null);
    }

    public void CheckForPlayerPass()
    {
        if (obstacleMoveScripts.Count == 0)
        {
            return;
        }
        if (obstacleMoveScripts[0].topPos)
        {
            if (Mathf.Abs(player.transform.position.z - obstacleMoveScripts[0].gameObject.transform.position.z) < 0.72f)
            {
                player.GetComponent<PlayerMovement>().GameOver(3);                
            }
        }
        else if (Mathf.Abs(player.transform.position.z - obstacleMoveScripts[0].gameObject.transform.position.z) < 0.5f)
        {
            if (!checkForPlayerPass)
            {
                if(obstacleMoveScripts[0].leftPos && obstacleMoveScripts[0].moved)
                    player.GetComponent<PlayerMovement>().GameOver(0);
                else if(obstacleMoveScripts[0].rightPos && obstacleMoveScripts[0].moved)
                    player.GetComponent<PlayerMovement>().GameOver(1);
                else
                    player.GetComponent<PlayerMovement>().GameOver(2);
            }
        }
    }
}
