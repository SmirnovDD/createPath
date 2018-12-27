using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclesSpawner : MonoBehaviour {
    public float spawnBlockTime;
    public float speedIncrement;
    public float minSpawnBlockTime;
    public float regularBlocksBetweenTraps;
    public Image swipeIcon;
    public GameObject[] obstaclePrefab;
    public GameObject explosionPrefab;
    public GameObject coinPrefab;
    public Material basicFloorMat;
    public Material defaultFloorMat;
    public Material spikedFloorMat;
    public Material trampolineMaterial, trampolineBaseMaterial, iceCubeMaterial, barrelMaterial;
    public List<MoveOnSwipe> obstacleMoveScripts = new List<MoveOnSwipe>();
    [HideInInspector]
    public bool checkForPlayerPass, swipeHelpTr;
    [HideInInspector]
    public Animator swipeAnim;

    private GameController gc;
    private GameObject newObstacle;
    private GameObject player;
    private PlayerMovement playerMovement;
    private RegisterSwipe registerSwipeScript;
    private Vector3 spawnPos;
    private Color32 randomColor;
    private float timer;
    private float delay;
    private float posX = 0, posY = 0, oldPosX;
    private bool wasNotInMiddleX, wasNotInMiddleY;
    private float oldLevel = 1;
    private float obstacleType;
    private float noBlocksBetweenTraps;
    private float colorLerpTimer;
    private bool swipeHelpL, swipeHelpU, swipeHelpD, swipeHelpTap, swipeHelpSpike, swipeHelpBarrel, swipeHelpIce, allChecked;
    void Start ()
    {
        swipeAnim = swipeIcon.GetComponent<Animator>();
        barrelMaterial.color = basicFloorMat.color = defaultFloorMat.color;
        gc = FindObjectOfType(typeof(GameController)) as GameController;
        registerSwipeScript = FindObjectOfType(typeof (RegisterSwipe)) as RegisterSwipe;
        int randomFirstObst = Random.Range(-3, 2);
        if(randomFirstObst <= 0)
            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(-1, 0, 3), Quaternion.identity);
        else
        {
            swipeHelpBarrel = true;
            newObstacle = Instantiate(obstaclePrefab[5], new Vector3(0, 1, 3), Quaternion.identity);
            Instantiate(obstaclePrefab[0], new Vector3(0, 0, 3), Quaternion.identity);
        }
        spawnPos = newObstacle.transform.position;
        StartCoroutine(SwipeHelp(0));
        obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
        registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        timer = Time.time + spawnBlockTime;
    }

    void Update ()
    {

        if (Time.time >= timer)
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
            playerMovement.anim.speed += speedIncrement;
            colorLerpTimer = Time.time + 1;
            randomColor = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 1);
        }
        if (colorLerpTimer > Time.time)
        {
            basicFloorMat.color = Color.Lerp(basicFloorMat.color, randomColor, 1 - (colorLerpTimer - Time.time));
            spikedFloorMat.color = Color.Lerp(spikedFloorMat.color, randomColor, 1 - (colorLerpTimer - Time.time));
            barrelMaterial.color = iceCubeMaterial.color = trampolineBaseMaterial.color = trampolineMaterial.color = Color.Lerp(trampolineMaterial.color, randomColor, 1 - (colorLerpTimer - Time.time));
        }
    }
    public void SpawnNewBlock()
    {
        if (gc.scoreNum < 1200)
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
        else if (gc.scoreNum < 3500)
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
        else if (gc.scoreNum < 7000)
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
        else if (gc.scoreNum < 8000)
        {
            if (regularBlocksBetweenTraps < 6)
            {
                regularBlocksBetweenTraps++;
                newObstacle = Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
            }

            else
            {
                if (!swipeHelpSpike)
                {
                    newObstacle = Instantiate(obstaclePrefab[1], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                    StartCoroutine(SwipeHelp(3));
                    swipeHelpSpike = true;
                    regularBlocksBetweenTraps = 3;
                }
                else
                {
                    newObstacle = Instantiate(obstaclePrefab[1], new Vector3(Random.Range(-1, 2), 0, spawnPos.z + 1f), Quaternion.identity);
                    regularBlocksBetweenTraps = 3;
                }
            }
        }
        else if (gc.scoreNum < 11000)
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
        else if (gc.scoreNum < 12000)
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
            }
            else
            {
                newObstacle = Instantiate(obstaclePrefab[2], new Vector3(0, 0.55f, spawnPos.z + 1f), Quaternion.Euler(0, 90, 0));
                noBlocksBetweenTraps = 3;
                regularBlocksBetweenTraps = 0;
            }
        }
        else if (gc.scoreNum < 15000)
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

                        obstacleType = Random.Range(-50, 10);
                        if (obstacleType < 0)
                            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                        else
                            newObstacle = Instantiate(obstaclePrefab[1], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                    }
                }
            }
        }
        else if (gc.scoreNum < 16000)
        {
            if (noBlocksBetweenTraps > 0)
            {
                newObstacle = Instantiate(obstaclePrefab[3], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                noBlocksBetweenTraps--;
                regularBlocksBetweenTraps++;
            }
            else
            {
                if (regularBlocksBetweenTraps < 6)
                {
                    regularBlocksBetweenTraps++;
                    newObstacle = Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                }
                else
                {
                    if(!swipeHelpIce)
                    {
                        swipeHelpIce = true;
                        StartCoroutine(SwipeHelp(4));
                    }
                    newObstacle = Instantiate(obstaclePrefab[4], new Vector3(0, 1, spawnPos.z + 1f), Quaternion.identity);
                    Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                    regularBlocksBetweenTraps = 3;
                }
            }
        }
        else if (gc.scoreNum < 18000)
        {
            gc.nextLevel = 6;
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
                    int obst = Random.Range(0, 100);
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

                        obstacleType = Random.Range(-50, 50);
                        if (obstacleType > 0)
                            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                        else if (obstacleType > -35)
                            newObstacle = Instantiate(obstaclePrefab[1], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                        else
                        {
                            newObstacle = Instantiate(obstaclePrefab[4], new Vector3(0, 1, spawnPos.z + 1f), Quaternion.identity);
                            Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                        }
                    }
                }
            }
        }
        else if (gc.scoreNum < 19000)
        {
            if (regularBlocksBetweenTraps < 6)
            {
                regularBlocksBetweenTraps++;
                newObstacle = Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
            }

            else
            {
                if (!swipeHelpBarrel)
                {
                    swipeHelpBarrel = true;
                    StartCoroutine(SwipeHelp(0));
                }
                newObstacle = Instantiate(obstaclePrefab[5], new Vector3(0, 1, spawnPos.z + 1f), Quaternion.identity);
                Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                regularBlocksBetweenTraps = 3;
            }
        }
        else
        {
            gc.nextLevel = 7;
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
                    int obst = Random.Range(0, 100);
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

                        obstacleType = Random.Range(-60, 50);
                        if (obstacleType > 0)
                            newObstacle = Instantiate(obstaclePrefab[0], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                        else if (obstacleType > -20)
                        {
                            newObstacle = Instantiate(obstaclePrefab[5], new Vector3(0, 1, spawnPos.z + 1f), Quaternion.identity);
                            Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                        }
                        else if (obstacleType > -35)
                            newObstacle = Instantiate(obstaclePrefab[1], new Vector3(posX, posY, spawnPos.z + 1f), Quaternion.identity);
                        else
                        {
                            newObstacle = Instantiate(obstaclePrefab[4], new Vector3(0, 1, spawnPos.z + 1f), Quaternion.identity);
                            Instantiate(obstaclePrefab[0], new Vector3(0, 0, spawnPos.z + 1f), Quaternion.identity);
                        }
                    }
                }
            }
        }
        spawnPos = newObstacle.transform.position;

        if (newObstacle.transform.position.x != 0 || newObstacle.transform.position.y != 0)
        {
            obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
            if (obstacleMoveScripts.Count == 1)
            {
                registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
                if (!allChecked)
                {
                    if (posX > 0 && !swipeHelpL)
                    {
                        StopCoroutine(SwipeHelp(0));
                        StartCoroutine(SwipeHelp(1));
                        swipeHelpL = true;
                    }
                    else if (posY < 0 && !swipeHelpU)
                    {
                        StopCoroutine(SwipeHelp(0));
                        StartCoroutine(SwipeHelp(2));
                        swipeHelpU = true;
                    }
                    else if (posY > 0 && !swipeHelpD)
                    {
                        StopCoroutine(SwipeHelp(0));
                        StartCoroutine(SwipeHelp(3));
                        swipeHelpD = true;
                        allChecked = true;
                    }
                }
            }
        }
        else if(newObstacle.CompareTag("Spike"))
        {
            obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
            if (obstacleMoveScripts.Count == 1)
                registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        }
        else
        {
            int coinSpawn = Random.Range(-20, 1 + gc.nextLevel);
            if (coinSpawn > 0 && noBlocksBetweenTraps == 0)
                Instantiate(coinPrefab, newObstacle.transform.position + Vector3.up * 0.9f, Quaternion.Euler(90,0,0));
        }
    }
    public void RemoveFirstObstacle()
    {
        swipeAnim.SetTrigger("swiped");
        swipeIcon.gameObject.SetActive(false);
        checkForPlayerPass = false;
        obstacleMoveScripts.RemoveAt(0);
        if (obstacleMoveScripts.Count > 0)
        {
            registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
            if (!allChecked)
            {
                if (!swipeHelpL && obstacleMoveScripts[0].rightPos)
                {
                    StopCoroutine(SwipeHelp(0));
                    StartCoroutine(SwipeHelp(1));
                    swipeHelpL = true;
                }
                else if (!swipeHelpU && obstacleMoveScripts[0].bottomPos)
                {
                    StopCoroutine(SwipeHelp(0));
                    StartCoroutine(SwipeHelp(2));
                    swipeHelpU = true;
                }
                else if (!swipeHelpD && obstacleMoveScripts[0].topPos)
                {
                    StopCoroutine(SwipeHelp(0));
                    StartCoroutine(SwipeHelp(3));
                    swipeHelpD = true;
                }
            }
        }
        else
            registerSwipeScript.UpdateFirstObstacle(null);

        gc.movedBlocks++;
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
                    if (obstacleMoveScripts[0].CompareTag("Barrel"))
                    {
                        playerMovement.GameOver(5);
                        Instantiate(explosionPrefab, obstacleMoveScripts[0].gameObject.transform.position, Quaternion.identity);
                        obstacleMoveScripts[0].gameObject.SetActive(false);
                    }
                    else
                    {
                        playerMovement.GameOver(3);
                        swipeIcon.gameObject.SetActive(false);
                    }
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
                    swipeIcon.gameObject.SetActive(false);
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
                    swipeIcon.gameObject.SetActive(false);
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
                swipeIcon.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator SwipeHelp(int dir)
    {
        yield return new WaitForSeconds(spawnBlockTime);
        if (obstacleMoveScripts.Count > 0)
        {
            if (!swipeHelpBarrel)
            {
                if (dir == 0 && !obstacleMoveScripts[0].leftPos)
                {
                    yield break;
                }
                if (dir == 1 && !obstacleMoveScripts[0].rightPos)
                {
                    yield break;
                }
                else if (dir == 2 && !obstacleMoveScripts[0].bottomPos)
                {
                    yield break;
                }
                else if (dir == 3 && !obstacleMoveScripts[0].topPos && !obstacleMoveScripts[0].neutralPosSpikedFloor)
                {
                    yield break;
                }
            }
        }
        else
        {
            yield break;
        }
        swipeIcon.gameObject.SetActive(true);
        switch(dir)
        {
            case 0:
                swipeAnim.SetTrigger("right");
                break;
            case 1:
                swipeAnim.SetTrigger("left");
                break;
            case 2:
                swipeAnim.SetTrigger("up");
                break;
            case 3:
                swipeAnim.SetTrigger("down");
                break;
            case 4:
                swipeAnim.SetTrigger("hold");
                break;
        }

        yield return null;
    }
}
