using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour {
    public GameObject[] obstaclePrefab;
    public List<MoveOnSwipe> obstacleMoveScripts = new List<MoveOnSwipe>();

    private GameObject firstObstacle;
    private RegisterSwipe registerSwipeScript;
    private Vector3 spawnPos;
	// Use this for initialization
	void Start ()
    {
        registerSwipeScript = FindObjectOfType(typeof (RegisterSwipe)) as RegisterSwipe;
        firstObstacle = Instantiate(obstaclePrefab[0], Vector3.forward * 3, Quaternion.identity);
        spawnPos = firstObstacle.transform.position;
        obstacleMoveScripts.Add(firstObstacle.GetComponent<MoveOnSwipe>());
        registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdateFirstObstacle()
    {
        obstacleMoveScripts.RemoveAt(0);
        firstObstacle = Instantiate(obstaclePrefab[0], spawnPos + Vector3.forward, Quaternion.identity);
        spawnPos = firstObstacle.transform.position;
        obstacleMoveScripts.Add(firstObstacle.GetComponent<MoveOnSwipe>());
        registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
    }
}
