using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float speedMultiplier = 1f; // Affects vertical speed
    private Vector3 startPosition;
    private float timer = 0f;
    private float duration = 5f;

    private PlayerDragonController dragon; // Reference to dragon

    void Start()
    {
        startPosition = transform.position;

        // Look for the PlayerDragonController in the scene
        dragon = FindFirstObjectByType<PlayerDragonController>();

        if (dragon == null)
        {
            Debug.LogError("Cannonball can't find the PlayerDragonController in the scene!");
        }
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        if (dragon != null && dragon.isStunned) return;

        if (dragon.isDead) return;

        timer += Time.deltaTime;

        // Vertical movement (up)
        float yMovement = 0.01f * speedMultiplier;

        // Horizontal movement (left)
        float xMovement = 0.01f * PlayerDragonController.scrollSpeedMultiplier;

        transform.position += new Vector3(-xMovement, yMovement, 0);

        if (timer >= duration)
        {
            // Reset Y position only
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            timer = 0f;
        }
    }
}




