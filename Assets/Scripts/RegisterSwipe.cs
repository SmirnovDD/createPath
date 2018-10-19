using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterSwipe : MonoBehaviour {
    public enum Swipes
    {
        right,
        left
    };

    //private ObstaclesSpawner obstaclesSpawner;
    private MoveOnSwipe moveOnSwipeOfFirstObst;
	void Start ()
    {
        //obstaclesSpawner = FindObjectOfType(typeof(ObstaclesSpawner)) as ObstaclesSpawner;
	}
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.D))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.right);
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.left);
        }
    }

    public void UpdateFirstObstacle(MoveOnSwipe firstObstacleMoveScript)
    {
        moveOnSwipeOfFirstObst = firstObstacleMoveScript;
    }
}
