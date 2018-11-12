using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour {
    public float spawnBlockTime;
    public float speedIncrement;
    public float minSpawnBlockTime;
    public float regularBlocksBetweenTraps;
    public GameObject[] obstaclePrefab;
    public List<MoveOnSwipe> obstacleMoveScripts = new List<MoveOnSwipe>();
    public bool checkForPlayerPass;

    private GameController gc;
    private GameObject newObstacle;
    private GameObject player;
    private PlayerMovement playerMovement;
    private RegisterSwipe registerSwipeScript;
    private Vector3 spawnPos;
    private float timer;
    private float delay;
    private float posX = 0, posY = 0, oldPosX;
    private bool wasNotInMiddleX, wasNotInMiddleY;
    private float oldLevel = 1;
    private float obstacleType;
    private float noBlocksBetweenTraps;
    private bool canContinue;
    void Start ()
    {
        gc = FindObjectOfType(typeof(GameController)) as GameController;
        registerSwipeScript = FindObjectOfType(typeof (RegisterSwipe)) as RegisterSwipe;
        newObstacle = Instantiate(obstaclePrefab[0], new Vector3(-1, 0, 3), Quaternion.identity);
        spawnPos = newObstacle.transform.position;
        obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
        registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        timer = Time.time + spawnBlockTime;
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
        if (gc.scoreNum < 2500)
        {
            if (regularBlocksBetweenTraps > 0)
            {
                posX = 0;
                regularBlocksBetweenTraps--;
            }
            else
            {
                posX = Random.Range(-100, 100);
                if (posX < 0)
                    posX = -1;
                else
                    posX = 1;

                regularBlocksBetweenTraps = 2;
            }

            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, 0, spawnPos.z + 1f), Quaternion.identity);

        }
        else if (gc.scoreNum < 6000)
        {
            gc.nextLevel = 2;
            if (regularBlocksBetweenTraps > 0)
            {
                posX = 0;
                posY = 0;
                regularBlocksBetweenTraps--;
            }
            else
            {
                posX = Random.Range(-1, 2);
                if (posX == 0)
                    posY = -1;

                regularBlocksBetweenTraps = 2;
            }
            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
        }
        else if (gc.scoreNum < 15000)
        {
            gc.nextLevel = 3;
            if (regularBlocksBetweenTraps > 0)
            {
                posX = 0;
                posY = 0;
                regularBlocksBetweenTraps--;
            }
            else
            {
                posX = Random.Range(-1, 2);
                if (posX == 0)
                {
                    posY = Random.Range(-100, 100);

                    if (posY < 0)
                        posY = -1;
                    else
                        posY = 1;
                }

                regularBlocksBetweenTraps = 1;
            }
            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
        }
        else if (gc.scoreNum < 17000)
        {
            if (regularBlocksBetweenTraps < 6)
            {
                posX = 0;
                regularBlocksBetweenTraps++;
                newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, 0, spawnPos.z + 1f), Quaternion.identity);
            }

            else
            {
                newObstacle = Instantiate(obstaclePrefab[1], new Vector3(Random.Range(-1, 2), 0, spawnPos.z + 1f), Quaternion.identity);
                regularBlocksBetweenTraps = 3;
            }
        }
        else if (gc.scoreNum < 22000)
        {
            gc.nextLevel = 4;
            if (regularBlocksBetweenTraps > 0)
            {
                posX = 0;
                posY = 0;
                regularBlocksBetweenTraps--;
                newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
            }
            else
            {
                posX = Random.Range(-1, 2);
                if (posX == 0)
                {
                    posY = Random.Range(-100, 100);

                    if (posY < 0)
                        posY = -1;
                    else
                        posY = 1;
                }

                regularBlocksBetweenTraps = 1;

                obstacleType = Random.Range(-50, 20);
                if (obstacleType < 0)
                    newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                else
                    newObstacle = Instantiate(obstaclePrefab[1], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
            }
        }
        else if (gc.scoreNum < 23000)
        {

            if (noBlocksBetweenTraps > 0)
            {
                newObstacle = Instantiate(obstaclePrefab[3], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                noBlocksBetweenTraps--;
                regularBlocksBetweenTraps++;
            }
            else if (regularBlocksBetweenTraps < 6)
            {
                posX = 0;
                regularBlocksBetweenTraps++;
                newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, 0, spawnPos.z + 1f), Quaternion.identity);
                canContinue = true;
            }
            else
            {
                newObstacle = Instantiate(obstaclePrefab[2], new Vector3(0, 0.55f, spawnPos.z + 1f), Quaternion.Euler(0, 90, 0));
                noBlocksBetweenTraps = 3;
                regularBlocksBetweenTraps = 0;
            }
        }
        else if (gc.scoreNum < 30000 && canContinue)
        {
            gc.nextLevel = 5;
            if (noBlocksBetweenTraps > 0)
            {
                newObstacle = Instantiate(obstaclePrefab[3], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                noBlocksBetweenTraps--;
                regularBlocksBetweenTraps++;
            }
            else
            {
                if (regularBlocksBetweenTraps > 0)
                {
                    posX = 0;
                    posY = 0;
                    regularBlocksBetweenTraps--;
                    newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                }
                else
                {
                    int obst = Random.Range(0, 70);
                    if (obst < 10)
                    {
                        newObstacle = Instantiate(obstaclePrefab[2], new Vector3(0, 0.55f, spawnPos.z + 1f), Quaternion.Euler(0, 90, 0));
                        noBlocksBetweenTraps = 3;
                        regularBlocksBetweenTraps = 1;
                    }
                    else
                    {
                        posX = Random.Range(-1, 2);
                        if (posX == 0)
                        {
                            posY = Random.Range(-100, 100);

                            if (posY < 0)
                                posY = -1;
                            else
                                posY = 1;
                        }

                        regularBlocksBetweenTraps = 1;

                        obstacleType = Random.Range(-50, 20);
                        if (obstacleType < 0)
                            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                        else
                            newObstacle = Instantiate(obstaclePrefab[1], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                    }
                }
            }
        }

        spawnPos = newObstacle.transform.position;

        if (newObstacle.transform.position.x != 0 || newObstacle.transform.position.y != 0)
        {
            obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
            if (obstacleMoveScripts.Count == 1)
                registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        }
        else if(newObstacle.CompareTag("Spike"))
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
                if (!checkForPlayerPass)
                {
                    playerMovement.GameOver(3);
                }
            }
        }
        else if (obstacleMoveScripts[0].neutralPosSpikedFloor)
        {
            if (Mathf.Abs(player.transform.position.z - obstacleMoveScripts[0].gameObject.transform.position.z) < 0.5f)
            {
                if (!checkForPlayerPass)
                {
                    playerMovement.GameOver(4); //ADD ANIMATION
                }
            }
        }
        else if (obstacleMoveScripts[0].trampoline)
        {
            if (player.transform.position.z - obstacleMoveScripts[0].gameObject.transform.position.z > 0.75f) //ОСТОРОЖНО!
            {
                if (!checkForPlayerPass)
                {
                    playerMovement.GameOver(2);
                }
            }
        }
        else if (Mathf.Abs(player.transform.position.z - obstacleMoveScripts[0].gameObject.transform.position.z) < 0.5f)
        {
            if (!checkForPlayerPass)
            {
                if (obstacleMoveScripts[0].leftPos && obstacleMoveScripts[0].moved)
                    playerMovement.GameOver(0);
                else if(obstacleMoveScripts[0].rightPos && obstacleMoveScripts[0].moved)
                    playerMovement.GameOver(1);
                else
                    playerMovement.GameOver(2);
            }
        }
    }
}
