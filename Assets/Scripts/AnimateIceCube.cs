public class AnimateIceCube : MoveOnSwipe {
    public override void Move(RegisterSwipe.Swipes swipeDir)
    {
        if (swipeDir == RegisterSwipe.Swipes.hold)
        {
            iceMeltTime += 0.025f;
            anim.SetFloat("touchHold", iceMeltTime);
            if (iceMeltTime >= 1f)
            {
                GetComponent<ChangePlayerPass>().PlayerPass();
                gameObject.SetActive(false);
            }
        }
    }
}
