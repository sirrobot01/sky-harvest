using UnityEngine;

public class DragonMovementAnimator : MonoBehaviour
{
    [Header("Lane Movement")]
    public float laneTransitionSpeed = 8f;
    public AnimationCurve laneTransitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Speed Change Animation")]
    public float scaleTransitionSpeed = 5f;
    public Vector3 fastScaleMultiplier = new Vector3(1.1f, 0.9f, 1f); // Slightly wider, shorter when fast
    public Vector3 slowScaleMultiplier = new Vector3(0.9f, 1.1f, 1f); // Slightly narrower, taller when slow
    
    [Header("Idle Animation")]
    public bool enableIdleBob = true;
    public float idleBobSpeed = 2f;
    public float idleBobAmount = 0.05f;
    
    [Header("Wing Animation")]
    public bool enableWingRotation = true;
    public float wingFlapSpeed = 8f;
    public float wingFlapAmount = 10f;
    
    private PlayerDragonController dragonController;
    private Vector3 targetPosition;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private float laneTransitionProgress = 1f;
    private bool isTransitioning = false;
    private float idleTimeOffset;
    
    void Start()
    {
        dragonController = GetComponent<PlayerDragonController>();
        originalScale = transform.localScale;
        targetScale = originalScale;
        targetPosition = transform.position;
        
        // Random offset for idle animation so multiple dragons don't sync
        idleTimeOffset = Random.Range(0f, 2f * Mathf.PI);
    }
    
    void Update()
    {
        if (dragonController == null || dragonController.isDead) return;
        
        HandleLaneTransition();
        HandleSpeedAnimation();
        HandleIdleAnimation();
        HandleWingAnimation();
    }
    
    void HandleLaneTransition()
    {
        // Check if we need to start a new lane transition
        float[] lanePositions = dragonController.laneYPositions;
        int currentLaneIndex = dragonController.currentLaneIndex;
        
        if (currentLaneIndex >= 0 && currentLaneIndex < lanePositions.Length)
        {
            float targetY = lanePositions[currentLaneIndex];
            
            // If we're not at the target Y position, start transition
            if (Mathf.Abs(transform.position.y - targetY) > 0.01f && !isTransitioning)
            {
                targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
                isTransitioning = true;
                laneTransitionProgress = 0f;
            }
        }
        
        // Animate the transition
        if (isTransitioning)
        {
            laneTransitionProgress += Time.deltaTime * laneTransitionSpeed;
            
            if (laneTransitionProgress >= 1f)
            {
                laneTransitionProgress = 1f;
                isTransitioning = false;
            }
            
            // Use the curve for smooth easing
            float curveValue = laneTransitionCurve.Evaluate(laneTransitionProgress);
            Vector3 currentPos = transform.position;
            currentPos.y = Mathf.Lerp(currentPos.y, targetPosition.y, curveValue);
            transform.position = currentPos;
        }
    }
    
    void HandleSpeedAnimation()
    {
        Vector3 desiredScale = originalScale;
        
        // Determine target scale based on dragon speed
        float speedMultiplier = PlayerDragonController.scrollSpeedMultiplier;
        
        if (speedMultiplier > 1.5f) // Fast
        {
            desiredScale = Vector3.Scale(originalScale, fastScaleMultiplier);
        }
        else if (speedMultiplier < 0.8f) // Slow
        {
            desiredScale = Vector3.Scale(originalScale, slowScaleMultiplier);
        }
        
        // Smoothly transition to desired scale
        targetScale = Vector3.Lerp(targetScale, desiredScale, Time.deltaTime * scaleTransitionSpeed);
        
        // Apply the scale (will be modified by idle animation)
        Vector3 finalScale = targetScale;
        
        // Add idle bob to scale
        if (enableIdleBob && speedMultiplier > 0.1f) // Don't bob when stopped
        {
            float bobMultiplier = 1f + Mathf.Sin(Time.time * idleBobSpeed + idleTimeOffset) * idleBobAmount;
            finalScale.y *= bobMultiplier;
        }
        
        transform.localScale = finalScale;
    }
    
    void HandleIdleAnimation()
    {
        if (!enableIdleBob) return;
        
        // This creates a subtle floating effect
        float speedMultiplier = PlayerDragonController.scrollSpeedMultiplier;
        if (speedMultiplier > 0.1f) // Only when moving
        {
            float bobOffset = Mathf.Sin(Time.time * idleBobSpeed + idleTimeOffset) * idleBobAmount * 0.1f;
            
            if (!isTransitioning) // Don't interfere with lane transitions
            {
                Vector3 currentPos = transform.position;
                currentPos.y += bobOffset;
                transform.position = currentPos;
            }
        }
    }
    
    void HandleWingAnimation()
    {
        if (!enableWingRotation) return;
        
        float speedMultiplier = PlayerDragonController.scrollSpeedMultiplier;
        if (speedMultiplier > 0.1f) // Only when moving
        {
            // Create wing flap effect through slight rotation
            float wingFlap = Mathf.Sin(Time.time * wingFlapSpeed) * wingFlapAmount * speedMultiplier;
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.z = wingFlap;
            transform.eulerAngles = currentRotation;
        }
        else
        {
            // Return to neutral rotation when stopped
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.z = Mathf.LerpAngle(currentRotation.z, 0f, Time.deltaTime * 5f);
            transform.eulerAngles = currentRotation;
        }
    }
    
    // Call this when the dragon gets hit to add a quick shake
    public void PlayHitAnimation()
    {
        // You could add a quick shake or flash here
        // For now, we'll just interrupt any ongoing transitions
        laneTransitionProgress = 1f;
        isTransitioning = false;
    }
    
    // For debugging
    [ContextMenu("Test Lane Transition")]
    void TestLaneTransition()
    {
        if (dragonController != null)
        {
            // Simulate moving to a different lane
            int newLane = (dragonController.currentLaneIndex + 1) % dragonController.laneYPositions.Length;
            dragonController.currentLaneIndex = newLane;
        }
    }
}