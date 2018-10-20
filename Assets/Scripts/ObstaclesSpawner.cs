using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour {
    public float spawnBlockTime;
    public GameObject[] obstaclePrefab;
    public List<MoveOnSwipe> obstacleMoveScripts = new List<MoveOnSwipe>();

    private GameObject newObstacle;
    private RegisterSwipe registerSwipeScript;
    private Vector3 spawnPos;
    private float timer;
	void Start ()
    {
        registerSwipeScript = FindObjectOfType(typeof (RegisterSwipe)) as RegisterSwipe;
        newObstacle = Instantiate(obstaclePrefab[0], Vector3.forward * 3, Quaternion.identity);
        spawnPos = newObstacle.transform.position;
        obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
        registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
	}
	
	void Update ()
    {
		if(Time.time > timer)
        {
            SpawnNewBlock();
            timer = Time.time + spawnBlockTime;
        }
	}
    public void SpawnNewBlock()
    {
        newObstacle = Instantiate(obstaclePrefab[0], spawnPos + Vector3.forward, Quaternion.identity);
        spawnPos = newObstacle.transform.position;
        obstacleMoveScripts.Add(newObstacle.GetComponent<MoveOnSwipe>());
        if(obstacleMoveScripts.Count == 1)
            registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
    }
    public void UpdateFirstObstacle()
    {
        obstacleMoveScripts.RemoveAt(0);
        if(obstacleMoveScripts.Count > 0)
            registerSwipeScript.UpdateFirstObstacle(obstacleMoveScripts[0]);
        else
            registerSwipeScript.UpdateFirstObstacle(null);
    }
}
