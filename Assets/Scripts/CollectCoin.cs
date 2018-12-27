using UnityEngine;

public class CollectCoin : MonoBehaviour {
    public Animator anim;
    private bool picked;
    private void OnTriggerEnter(Collider other)
    {
        if (!picked)
        {
            anim.SetTrigger("pick");
            GameController.collectedCoinsNum++;
        }
        picked = true;
        Destroy(gameObject, 1f);
    }
}
