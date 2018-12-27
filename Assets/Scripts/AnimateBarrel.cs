public class AnimateBarrel : MoveOnSwipe {
    public override void Move(RegisterSwipe.Swipes swipeDir)
    {
        if (swipeDir == RegisterSwipe.Swipes.right)
        {
            Throw(200);
            GetComponent<ChangePlayerPass>().PlayerPass();
            Destroy(gameObject, 2f);
        }
        else if(swipeDir == RegisterSwipe.Swipes.left)
        {
            Throw(-200);
            GetComponent<ChangePlayerPass>().PlayerPass();
            Destroy(gameObject, 2f);
        }
    }


}
