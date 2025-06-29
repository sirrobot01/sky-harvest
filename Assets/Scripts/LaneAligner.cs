using UnityEngine;

public class LaneAligner : MonoBehaviour
{
    [Header("Lane System")]
    public float[] laneYPositions = { -4f, -2f, 0f, 2f, 4f }; // Must match PlayerDragonController
    
    [Header("Alignment Settings")]
    public bool snapToNearestLane = true;
    public bool alignOnStart = true;
    public bool showDebugLines = true;
    
    void Start()
    {
        if (alignOnStart)
        {
            SnapToNearestLane();
        }
    }
    
    void Update()
    {
        if (snapToNearestLane)
        {
            SnapToNearestLane();
        }
    }
    
    public void SnapToNearestLane()
    {
        if (laneYPositions.Length == 0) return;
        
        float currentY = transform.position.y;
        float nearestLaneY = FindNearestLaneY(currentY);
        
        // Snap exactly to lane position
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, nearestLaneY, currentPos.z);
    }
    
    float FindNearestLaneY(float currentY)
    {
        float nearestLaneY = laneYPositions[0];
        float shortestDistance = Mathf.Abs(currentY - nearestLaneY);
        
        for (int i = 1; i < laneYPositions.Length; i++)
        {
            float distance = Mathf.Abs(currentY - laneYPositions[i]);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestLaneY = laneYPositions[i];
            }
        }
        
        return nearestLaneY;
    }
    
    public void SetToSpecificLane(int laneIndex)
    {
        if (laneIndex >= 0 && laneIndex < laneYPositions.Length)
        {
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(currentPos.x, laneYPositions[laneIndex], currentPos.z);
        }
    }
    
    public int GetCurrentLaneIndex()
    {
        float currentY = transform.position.y;
        
        for (int i = 0; i < laneYPositions.Length; i++)
        {
            if (Mathf.Approximately(currentY, laneYPositions[i]))
            {
                return i;
            }
        }
        
        return -1; // Not aligned with any lane
    }
    
    void OnDrawGizmosSelected()
    {
        if (!showDebugLines || laneYPositions.Length == 0) return;
        
        // Draw lane lines
        Gizmos.color = Color.yellow;
        float lineLength = 20f;
        
        for (int i = 0; i < laneYPositions.Length; i++)
        {
            Vector3 lineStart = new Vector3(transform.position.x - lineLength/2, laneYPositions[i], 0);
            Vector3 lineEnd = new Vector3(transform.position.x + lineLength/2, laneYPositions[i], 0);
            Gizmos.DrawLine(lineStart, lineEnd);
            
            // Draw lane labels
            Vector3 labelPos = new Vector3(transform.position.x + lineLength/2 + 1, laneYPositions[i], 0);
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(labelPos, $"Lane {i} ({laneYPositions[i]})");
            #endif
        }
        
        // Highlight current object position
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}