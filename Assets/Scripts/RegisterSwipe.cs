using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterSwipe : MonoBehaviour {
    private const float DEADZONE = 50f;
    public enum Swipes
    {
        right,
        left,
        up,
        down,
        tap,
        hold
    };

    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private MoveOnSwipe moveOnSwipeOfFirstObst;
    private bool tap, hold, swipeLeft, swipeRight, swipeDown, swipeUp;
    private Vector2 swipeDelta, startTouch;
	
	void Update ()
    {
        if (!moveOnSwipeOfFirstObst)
            return;
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
        #region Standalone Inputs
        if(Input.GetKeyDown(KeyCode.D))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.right);
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.left);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.down);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.tap);
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            moveOnSwipeOfFirstObst.Move(Swipes.hold);
        }
        #endregion
            #region Mobile Inputs
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                hold = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
                hold = false;
            }
        }
        #endregion
        // Calculate distance
        swipeDelta = Vector2.zero;
        if(startTouch != Vector2.zero)
        {
            if(Input.touchCount > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
        }
        if(swipeDelta.magnitude > DEADZONE)
        {
            // Confirmed swipe
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left of right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // Up or down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            startTouch = swipeDelta = Vector2.zero;
        }
        Swipe();
    }

    private void Swipe()
    {
        if (swipeRight)
        {
            moveOnSwipeOfFirstObst.Move(Swipes.right);
        }
        else if (swipeLeft)
        {
            moveOnSwipeOfFirstObst.Move(Swipes.left);
        }
        else if(swipeUp)
        {
            moveOnSwipeOfFirstObst.Move(Swipes.up);
        }
        else if(swipeDown)
        {
            moveOnSwipeOfFirstObst.Move(Swipes.down);
        }
        else if (tap)
        {
            moveOnSwipeOfFirstObst.Move(Swipes.tap);
        }
        else if (hold)
        {
            moveOnSwipeOfFirstObst.Move(Swipes.hold);
        }
    }
    public void UpdateFirstObstacle(MoveOnSwipe firstObstacleMoveScript)
    {
        moveOnSwipeOfFirstObst = firstObstacleMoveScript;
    }
}
