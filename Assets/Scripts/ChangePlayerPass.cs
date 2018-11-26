using UnityEngine;

public class ChangePlayerPass : MonoBehaviour {
    private ObstaclesSpawner obstSp;
    private void Start()
    {
        obstSp = FindObjectOfType(typeof(ObstaclesSpawner)) as ObstaclesSpawner;
    }
    public void PlayerPass()
    {
        if (obstSp != null)
        {
            obstSp.checkForPlayerPass = true;
            obstSp.RemoveFirstObstacle();
        }
    }
}
