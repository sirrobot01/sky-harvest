using UnityEngine;

public class ObjectAnimator : MonoBehaviour
{
    [Header("Float Animation")]
    public bool enableFloating = true;
    public float floatSpeed = 2f;
    public float floatHeight = 0.3f;
    public float floatOffset = 0f; // Random offset for variety
    
    [Header("Rotation Animation")]
    public bool enableRotation = false;
    public Vector3 rotationSpeed = new Vector3(0, 0, 50f); // degrees per second
    
    [Header("Pulse Animation")]
    public bool enablePulsing = false;
    public float pulseSpeed = 3f;
    public float pulseScale = 0.2f; // How much bigger/smaller it gets
    
    [Header("Bobbing Animation")]
    public bool enableBobbing = false;
    public float bobSpeed = 1.5f;
    public float bobHeight = 0.1f;
    
    [Header("Wobble Animation")]
    public bool enableWobble = false;
    public float wobbleSpeed = 4f;
    public float wobbleAmount = 5f; // degrees
    
    private Vector3 startPosition;
    private Vector3 startScale;
    private float timeOffset;
    
    void Start()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
        
        // Add random time offset so objects don't all animate in sync
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
        floatOffset = Random.Range(0f, 2f * Mathf.PI);
    }
    
    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        Vector3 newPosition = startPosition;
        Vector3 newScale = startScale;
        Vector3 newRotation = transform.eulerAngles;
        
        float time = Time.time + timeOffset;
        
        // Floating animation (smooth up and down)
        if (enableFloating)
        {
            float floatY = Mathf.Sin(time * floatSpeed + floatOffset) * floatHeight;
            newPosition.y = startPosition.y + floatY;
        }
        
        // Bobbing animation (different from floating - more subtle)
        if (enableBobbing)
        {
            float bobY = Mathf.Sin(time * bobSpeed) * bobHeight;
            newPosition.y = startPosition.y + bobY;
        }
        
        // Rotation animation
        if (enableRotation)
        {
            newRotation += rotationSpeed * Time.deltaTime;
        }
        
        // Pulse animation (scale up and down)
        if (enablePulsing)
        {
            float pulseMultiplier = 1f + Mathf.Sin(time * pulseSpeed) * pulseScale;
            newScale = Vector3.Scale(startScale, Vector3.one * pulseMultiplier);
        }
        
        // Wobble animation (slight rotation back and forth)
        if (enableWobble)
        {
            float wobble = Mathf.Sin(time * wobbleSpeed) * wobbleAmount;
            newRotation.z = wobble;
        }
        
        // Apply all transformations with safeguards
        transform.position = newPosition;
        
        // Ensure scale never goes to zero or negative
        if (newScale.x > 0.01f && newScale.y > 0.01f && newScale.z > 0.01f)
        {
            transform.localScale = newScale;
        }
        
        transform.eulerAngles = newRotation;
    }
    
    // Preset configurations for different object types
    [ContextMenu("Configure for Regular Fruit")]
    public void ConfigureForRegularFruit()
    {
        enableFloating = true;
        floatSpeed = 2f;
        floatHeight = 0.2f;
        
        enableRotation = false;
        enablePulsing = false;
        enableBobbing = false;
        enableWobble = false;
    }
    
    [ContextMenu("Configure for Golden Fruit")]
    public void ConfigureForGoldenFruit()
    {
        enableFloating = true;
        floatSpeed = 2.5f;
        floatHeight = 0.2f;
        
        enableRotation = true;
        rotationSpeed = new Vector3(0, 30f, 0);
        
        enablePulsing = true;
        pulseSpeed = 3f;
        pulseScale = 0.05f; // Much smaller scale change
        
        enableBobbing = false;
        enableWobble = false;
    }
    
    [ContextMenu("Configure for Heart Fruit")]
    public void ConfigureForHeartFruit()
    {
        enableFloating = true;
        floatSpeed = 1.5f;
        floatHeight = 0.2f;
        
        enablePulsing = true;
        pulseSpeed = 2f;
        pulseScale = 0.08f; // Smaller scale change
        
        enableRotation = false;
        enableBobbing = false;
        enableWobble = false;
    }
    
    [ContextMenu("Configure for Obstacle")]
    public void ConfigureForObstacle()
    {
        enableFloating = false;
        enablePulsing = false;
        
        enableRotation = true;
        rotationSpeed = new Vector3(0, 0, -45f);
        
        enableWobble = false;
        enableBobbing = false;
    }
    
    [ContextMenu("Configure for Building")]
    public void ConfigureForBuilding()
    {
        enableFloating = false;
        enableRotation = false;
        enablePulsing = false;
        
        enableWobble = true;
        wobbleSpeed = 0.5f;
        wobbleAmount = 1f; // Very subtle
        
        enableBobbing = false;
    }
}