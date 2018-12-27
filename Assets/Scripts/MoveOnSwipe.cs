using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSwipe : MonoBehaviour {
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public ObstaclesSpawner obstacleSpawner;
    public PlayerMovement playerMovement;
    public Rigidbody rigidB;
    public bool leftPos, rightPos, bottomPos, topPos;
    public bool moved;
    public bool neutralPosSpikedFloor;
    public bool trampoline;
    public bool playerInRange;
    public float iceMeltTime = 0f;
    // Use this for initialization
    void Start ()
    {
        obstacleSpawner = FindObjectOfType(typeof(ObstaclesSpawner)) as ObstaclesSpawner;
        anim = GetComponent<Animator>();
        playerMovement = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (gameObject.CompareTag("Barrel"))
            rigidB = GetComponent<Rigidbody>();

        if (transform.position.x == -1)
            leftPos = true;
        else if (transform.position.x == 1)
            rightPos = true;
        else if (transform.position.y == -1)
            bottomPos = true;
        else if (transform.position.y == 1)
            topPos = true;
        else if (transform.position.y == 0)
            neutralPosSpikedFloor = true;
        else
            trampoline = true;
    }

    public virtual void Move(RegisterSwipe.Swipes swipeDir)
    {

    }
    public void Throw(float force)
    {
        rigidB.useGravity = true;
        rigidB.AddForce(Vector3.right * force);
        rigidB.AddTorque(Vector3.right * force);
    }
    public void SetNeutralPosition()
    {
        neutralPosSpikedFloor = true;
    }
    public void SelfDestroy()
    {
        Destroy(gameObject, 10f);
    }
    public void LowerHeight()
    {
        transform.position += Vector3.down * 0.01f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
            anim.SetBool("ready", true);

            if (!obstacleSpawner.swipeHelpTr)
            {
                obstacleSpawner.swipeAnim.gameObject.SetActive(true);
                obstacleSpawner.swipeAnim.SetTrigger("tap");
                obstacleSpawner.swipeHelpTr = true;
            }
        }
    }
}
