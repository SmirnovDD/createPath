public class AnimateSpikedFloor : MoveOnSwipe {

    public override void Move(RegisterSwipe.Swipes swipeDir)
    {
        if(neutralPosSpikedFloor && swipeDir == RegisterSwipe.Swipes.down)
        {
            anim.SetBool("lowerSpikes", true);
        }

        if (swipeDir == RegisterSwipe.Swipes.right && leftPos)
        {
            anim.SetBool("moveRight", true);
            moved = true;
        }
        else if (swipeDir == RegisterSwipe.Swipes.left && rightPos)
        {
            anim.SetBool("moveLeft", true);
            moved = true;
        }
        else if (swipeDir == RegisterSwipe.Swipes.up && bottomPos)
        {
            anim.SetBool("moveUp", true);
            moved = true;
        }
        else if (swipeDir == RegisterSwipe.Swipes.down && topPos)
        {
            anim.SetBool("moveDown", true);
            moved = true;
        }
    }
}
