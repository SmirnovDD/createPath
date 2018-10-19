using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBasicFloor : MoveOnSwipe {

    public override void Move(RegisterSwipe.Swipes swipeDir)
    {
        if(swipeDir == RegisterSwipe.Swipes.right)
        {
            anim.SetBool("moveRight", true);
        }
        else if (swipeDir == RegisterSwipe.Swipes.left)
        {
            anim.SetBool("moveLeft", true);
        }

        obstacleSpawner.UpdateFirstObstacle();
    }
}
