using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSwipe : MonoBehaviour {
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public ObstaclesSpawner obstacleSpawner;
    // Use this for initialization
    void Start ()
    {
        obstacleSpawner = FindObjectOfType(typeof(ObstaclesSpawner)) as ObstaclesSpawner;
        anim = GetComponent<Animator>();
    }

    public virtual void Move(RegisterSwipe.Swipes swipeDir)
    {

    }
}
