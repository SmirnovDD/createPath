
public class AnimateTrampoline : MoveOnSwipe {
    public override void Move(RegisterSwipe.Swipes swipeDir)
    {
        if (swipeDir == RegisterSwipe.Swipes.tap && playerInRange)
        {
            anim.SetBool("jump", true);
            playerMovement.PlayerJump(transform.position.z + 4);
            Invoke("SelfDestroy", 3f);
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
