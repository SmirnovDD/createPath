using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    public bool gameOver;
    public Vector3 startPos;
    public float movementSpeed;
    private ObstaclesSpawner obstSpawnScript;
    private Rigidbody rigidB;
    private float jumpDst;
    void Start ()
    {
        obstSpawnScript = FindObjectOfType(typeof(ObstaclesSpawner)) as ObstaclesSpawner;
        rigidB = GetComponent<Rigidbody>();
        startPos = transform.position;
	}
	
	void FixedUpdate ()
    {
        if (!gameOver)
        {
            movementSpeed = 1 / obstSpawnScript.spawnBlockTime;
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
	}
    public void PlayerJump(float dst)
    {
        jumpDst = dst;
        StartCoroutine(Jump());
    }
    IEnumerator Jump()
    {
        while (jumpDst - transform.position.z > 2.2)
        {
            transform.position = new Vector3(startPos.x, Mathf.LerpUnclamped(transform.position.y, 4, Time.deltaTime * 2), transform.position.z);
            yield return null;
        }
        float down = 0.01f * 1 / obstSpawnScript.spawnBlockTime;
        while (transform.position.y > startPos.y && transform.position.z <= jumpDst - 1)
        {
            transform.position -= Vector3.up * down;
            down *= 1.1f;
            yield return null;
        }
        transform.position = new Vector3(0, startPos.y, transform.position.z);
        yield break;
    }
    public void GameOver(int dir)
    {
        Invoke("RestartGame", 1.5f);
        gameOver = true;
        StopCoroutine(Jump());
        Rigidbody rigidB = GetComponent<Rigidbody>();
        rigidB.freezeRotation = true;
        rigidB.useGravity = true;
        if (dir == 0)
            rigidB.AddForce(Vector3.right * 400);
        else if (dir == 1)
            rigidB.AddForce(Vector3.left * 400);
        else if (dir == 2)
        {
            transform.position += Vector3.forward * 0.1f;
            rigidB.AddForce(Vector3.down * 500 + Vector3.forward * 200);
        }
        else if (dir == 3)
            rigidB.AddForce(Vector3.back * 80);
        else if (dir == 4)
            rigidB.velocity = Vector3.zero;
        Destroy(obstSpawnScript);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
