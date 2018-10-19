using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterSwipe : MonoBehaviour {
    public enum Swipes
    {
        right,
        left
    };

    public delegate void Swipe(Swipes swipeDir);
    public static event Swipe OnSwipe;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.D))
        {
            if (OnSwipe != null)
                OnSwipe(Swipes.right);
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            if (OnSwipe != null)
                OnSwipe(Swipes.left);
        }
    }
}
