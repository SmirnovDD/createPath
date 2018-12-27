public class AnimateIceCube : MoveOnSwipe {
    public override void Move(RegisterSwipe.Swipes swipeDir)
    {
        if (swipeDir == RegisterSwipe.Swipes.hold)
        {
            iceMeltTime += 0.03f;
            anim.SetFloat("touchHold", iceMeltTime);
            LowerHeight();
            if (iceMeltTime >= 1f)
            {
                GetComponent<ChangePlayerPass>().PlayerPass();
                gameObject.SetActive(false);
            }
        }
    }
}
