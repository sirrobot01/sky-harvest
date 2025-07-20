using UnityEngine;

public class BuildingObstacle : MonoBehaviour
{
    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        transform.position += Vector3.left * 0.01f * PlayerDragonController.scrollSpeedMultiplier;
    }
}

