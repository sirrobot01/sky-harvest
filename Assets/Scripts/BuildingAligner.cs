using UnityEngine;

public class BuildingAligner : MonoBehaviour
{
    [Header("Building Alignment")]
    public float groundLevel = -4f; // Ground lane position (buildings root here)
    public float[] laneYPositions = { -4f, -2f, 0f, 2f, 4f }; // Available lane heights
    public bool alignOnStart = true;
    public int targetLaneIndex = 1; // Which lane the building top should reach
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D buildingCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingCollider = GetComponent<Collider2D>();
        
        if (alignOnStart)
        {
            AlignBuildingToLane();
        }
    }
    
    public void AlignBuildingToLane()
    {
        if (spriteRenderer == null) return;
        
        // Choose a random lane height for the building top (excluding ground and top lane to leave space)
        if (targetLaneIndex <= 0 || targetLaneIndex >= laneYPositions.Length)
        {
            targetLaneIndex = Random.Range(1, laneYPositions.Length - 1); // Skip ground and top lane
        }
        
        float targetTopY = laneYPositions[targetLaneIndex];
        float desiredHeight = CalculateBuildingHeight(targetTopY);
        
        // Get original sprite height in world units
        float originalHeight = GetOriginalSpriteHeight();
        
        // Calculate scale needed to match desired height
        float scaleY = desiredHeight / originalHeight;
        
        // Apply scale (keep X scale, adjust Y scale)
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x, scaleY, currentScale.z);
        
        // Update collider to match new scale
        UpdateColliderSize(desiredHeight);
        
        // Position building so its bottom is at ground level
        float currentX = transform.position.x;
        float currentZ = transform.position.z;
        float newY = groundLevel + (desiredHeight / 2f); // Center at ground + half height
        
        transform.position = new Vector3(currentX, newY, currentZ);
        
        if (showDebugInfo)
        {
            Debug.Log($"Building aligned: DesiredHeight={desiredHeight:F2}, ScaleY={scaleY:F2}, Bottom={groundLevel:F2}, Top={targetTopY:F2}, Center={newY:F2}");
        }
    }
    
    float CalculateBuildingHeight(float targetTopY)
    {
        // Calculate height needed to reach from ground to target lane
        return targetTopY - groundLevel;
    }
    
    float GetBuildingHeight()
    {
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            // Get sprite bounds in world space
            Bounds spriteBounds = spriteRenderer.bounds;
            return spriteBounds.size.y;
        }
        else if (buildingCollider != null)
        {
            // Fallback to collider bounds
            return buildingCollider.bounds.size.y;
        }
        
        // Default fallback height
        return 2f;
    }
    
    float GetOriginalSpriteHeight()
    {
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            // Get sprite's original height in world units (before any scaling)
            Sprite sprite = spriteRenderer.sprite;
            return sprite.bounds.size.y;
        }
        
        // Fallback
        return 2f;
    }
    
    void UpdateColliderSize(float newHeight)
    {
        if (buildingCollider != null)
        {
            if (buildingCollider is BoxCollider2D boxCollider)
            {
                Vector2 currentSize = boxCollider.size;
                // Keep collider size matching the actual scaled sprite
                boxCollider.size = new Vector2(currentSize.x, newHeight);
                
                if (showDebugInfo)
                {
                    Debug.Log($"Collider updated: Height={newHeight:F2}");
                }
            }
        }
    }
    
    public void SetTargetLane(int laneIndex)
    {
        if (laneIndex > 0 && laneIndex < laneYPositions.Length)
        {
            targetLaneIndex = laneIndex;
            AlignBuildingToLane();
        }
    }
    
    public float GetBuildingTop()
    {
        return laneYPositions[targetLaneIndex];
    }
    
    public float GetBuildingBottom()
    {
        return groundLevel;
    }
    
    void OnDrawGizmosSelected()
    {
        if (!showDebugInfo) return;
        
        float targetTopY = (targetLaneIndex < laneYPositions.Length) ? laneYPositions[targetLaneIndex] : laneYPositions[1];
        float buildingHeight = CalculateBuildingHeight(targetTopY);
        
        // Draw building bounds
        Gizmos.color = Color.blue;
        Vector3 center = new Vector3(transform.position.x, groundLevel + (buildingHeight / 2f), 0);
        Vector3 size = new Vector3(1f, buildingHeight, 0.1f);
        Gizmos.DrawWireCube(center, size);
        
        // Draw ground line (red)
        Gizmos.color = Color.red;
        Vector3 groundStart = new Vector3(transform.position.x - 2f, groundLevel, 0);
        Vector3 groundEnd = new Vector3(transform.position.x + 2f, groundLevel, 0);
        Gizmos.DrawLine(groundStart, groundEnd);
        
        // Draw target lane line (green)
        Gizmos.color = Color.green;
        Vector3 topStart = new Vector3(transform.position.x - 2f, targetTopY, 0);
        Vector3 topEnd = new Vector3(transform.position.x + 2f, targetTopY, 0);
        Gizmos.DrawLine(topStart, topEnd);
        
        #if UNITY_EDITOR
        // Labels
        UnityEditor.Handles.Label(new Vector3(transform.position.x + 2.5f, groundLevel, 0), "Ground");
        UnityEditor.Handles.Label(new Vector3(transform.position.x + 2.5f, targetTopY, 0), $"Target Lane {targetLaneIndex}");
        UnityEditor.Handles.Label(new Vector3(transform.position.x, groundLevel - 0.5f, 0), $"Height: {buildingHeight:F2}");
        #endif
    }
}