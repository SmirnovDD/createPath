using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSwipe : MonoBehaviour {
    [HideInInspector]
    public Vector3 startPos;

    private void OnEnable()
    {
         RegisterSwipe.OnSwipe += Move;
    }
    private void OnDisable()
    {
        RegisterSwipe.OnSwipe -= Move;
    }
    // Use this for initialization
    void Start ()
    {
        startPos = transform.position; // в будущем убрать
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Move(RegisterSwipe.Swipes swipeDir)
    {
        if (swipeDir == RegisterSwipe.Swipes.right)
        {
            transform.position = transform.position + Vector3.right;
        }
        else if (swipeDir == RegisterSwipe.Swipes.left)
        {
            transform.position = transform.position + Vector3.left;
        }
    }
}
