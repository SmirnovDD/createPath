using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    public bool gameOver;
    private float movementSpeed;
    private ObstaclesSpawner obstSpawnScript;
    private Rigidbody rigidB;
	void Start ()
    {
        obstSpawnScript = FindObjectOfType(typeof(ObstaclesSpawner)) as ObstaclesSpawner;
        rigidB = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        if (!gameOver)
        {
            movementSpeed = 1 / obstSpawnScript.spawnBlockTime;
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
	}

    public void GameOver(int dir)
    {
        Invoke("RestartGame", 1.5f);
        gameOver = true;
        Rigidbody rigidB = GetComponent<Rigidbody>();
        rigidB.freezeRotation = true;
        rigidB.useGravity = true;
        if (dir == 0)
            rigidB.AddForce(Vector3.right * 200);
        else if (dir == 1)
            rigidB.AddForce(Vector3.left * 200);
        else if (dir == 2)
            rigidB.AddForce(Vector3.down * 40 + Vector3.forward * 50);
        else if (dir == 3)
            rigidB.AddForce(Vector3.back * 50);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
