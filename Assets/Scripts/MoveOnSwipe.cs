using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSwipe : MonoBehaviour {
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public ObstaclesSpawner obstacleSpawner;

    public bool leftPos, rightPos, bottomPos, topPos;
    public bool moved;
    // Use this for initialization
    void Start ()
    {
        obstacleSpawner = FindObjectOfType(typeof(ObstaclesSpawner)) as ObstaclesSpawner;
        anim = GetComponent<Animator>();

        if (transform.position.x == -1)
            leftPos = true;
        else if (transform.position.x == 1)
            rightPos = true;
        else if (transform.position.y == -1)
            bottomPos = true;
        else if (transform.position.y == 1)
            topPos = true;
    }

    public virtual void Move(RegisterSwipe.Swipes swipeDir)
    {

    }
}
