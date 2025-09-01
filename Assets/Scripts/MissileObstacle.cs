using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Missile Behavior")]
    public float trackingTime = 2f; // How long missile tracks player
    public float trackingSpeed = 2f; // How fast missile moves toward player
    public float normalSpeed = 4f;
    
    private PlayerDragonController target;
    private float trackingTimer = 0f;
    private bool isTracking = true;
    private Vector3 startPosition;
    
    void Start()
    {
        target = FindFirstObjectByType<PlayerDragonController>();
        startPosition = transform.position;
        
        if (target == null)
        {
            Debug.LogError("Missile can't find PlayerDragonController!");
        }
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        if (target == null || target.isDead)
        {
            // Just move left if no target
            transform.position += Vector3.left * normalSpeed * PlayerDragonController.scrollSpeedMultiplier * Time.deltaTime;
            return;
        }
        
        if (isTracking && trackingTimer < trackingTime)
        {
            // Track the player for a limited time
            TrackPlayer();
            trackingTimer += Time.deltaTime;
        }
        else
        {
            // Stop tracking, just move left
            isTracking = false;
            transform.position += Vector3.left * normalSpeed * PlayerDragonController.scrollSpeedMultiplier * Time.deltaTime;
        }
    }
    
    void TrackPlayer()
    {
        // Move toward player's current position
        Vector3 targetPosition = target.transform.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        // Combine tracking movement with leftward scroll
        Vector3 trackingMovement = direction * trackingSpeed * Time.deltaTime;
        Vector3 scrollMovement = Vector3.left * normalSpeed * PlayerDragonController.scrollSpeedMultiplier * Time.deltaTime;
        
        transform.position += trackingMovement + scrollMovement;
        
        // Optional: Rotate missile to face target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
