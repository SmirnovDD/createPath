using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSwipe : MonoBehaviour {
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public ObstaclesSpawner obstacleSpawner;
    public PlayerMovement playerMovement;

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

    public void SetNeutralPosition()
    {
        neutralPosSpikedFloor = true;
    }
    public void SelfDestroy()
    {
        Invoke("DestroyThis", 10f);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
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
