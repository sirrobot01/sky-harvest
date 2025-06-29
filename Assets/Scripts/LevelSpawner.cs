using UnityEngine;
using System.Collections.Generic;

public class LevelSpawner : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    public GameObject applePrefab;
    public GameObject grapePrefab;
    public GameObject watermelonPrefab;
    public GameObject dragonfruitPrefab;
    public GameObject goldApplePrefab;
    public GameObject goldGrapePrefab;
    public GameObject goldWatermelonPrefab;
    public GameObject goldDragonfruitPrefab;
    public GameObject heartFruitPrefab;
    public GameObject rottenFruitPrefab;

    [Header("Obstacle Prefabs")]
    public GameObject[] obstaclePrefabs;
    public GameObject finishLinePrefab;
    public GameObject thunderCloudPrefab;

    [Header("Spawn Settings")]
    public float spawnRangeY = 8f;
    public Camera mainCamera;

    [Header("Fruit Spawning")]
    [Range(0, 100)] public float regularFruitChance = 60f;
    [Range(0, 100)] public float goldenFruitChance = 15f;
    [Range(0, 100)] public float heartFruitChance = 5f;
    [Range(0, 100)] public float rottenFruitChance = 10f;
    public int fruitsPerSegment = 3;

    [Header("Obstacle Spawning")]
    public float buildingChance = 30f;
    public float missileChance = 25f;
    public float cannonballChance = 20f;
    public int obstaclesPerSegment = 2;

    [Header("Thunder Spawning")]
    public float thunderCloudChance = 0f;
    public int maxThunderCloudsPerSegment = 2;

    [Header("Level Segmentation")]
    public float segmentLength = 20f; // Level divided into segments
    public float[] laneYPositions = { -4f, -2f, 0f, 2f, 4f }; // Must match PlayerDragonController lanes
    public float groundLevel = -4f; // Bottom lane for ground obstacles (buildings)
    public float minSpawnDistance = 3f; // Minimum X distance between spawned objects
    public float buildingSpacing = 8f; // Minimum X distance between buildings (they're wider)

    [Header("Progressive Difficulty")]
    public bool useProgressiveDifficulty = true;

    public enum ObstacleType { Building, Missile, Cannonball }

    // Tracking spawned positions to prevent overlap
    private List<Vector3> spawnedPositions = new List<Vector3>();
    
    // Track building X positions to prevent items spawning there
    private List<float> buildingXPositions = new List<float>();

    void Start()
    {
        if (mainCamera == null) 
            mainCamera = Camera.main;
            
        GenerateLevel();
    }
    
    void GenerateLevel()
    {
        Debug.Log("Generating level based on difficulty...");
        
        // Calculate finish line distance based on difficulty
        float finishDistance = GetFinishLineDistance();
        
        // Generate segments up to finish line
        float currentX = mainCamera.transform.position.x + 10f;
        int segmentCount = Mathf.CeilToInt(finishDistance / segmentLength);
        
        for (int i = 0; i < segmentCount; i++)
        {
            float segmentProgress = (float)i / segmentCount;
            GenerateSegment(currentX, segmentProgress);
            currentX += segmentLength;
        }
        
        // Place finish line at calculated distance
        if (finishLinePrefab != null)
        {
            Vector3 finishPos = new Vector3(finishDistance, 0f, 0f);
            Instantiate(finishLinePrefab, finishPos, Quaternion.identity);
            Debug.Log("Finish line placed at X: " + finishDistance + " for difficulty");
        }
    }
    
    float GetFinishLineDistance()
    {
        int difficulty = PlayerPrefs.GetInt("GameDifficulty", 1);
        
        switch (difficulty)
        {
            case 0: // Easy - 10 minutes
                return 900f;
            case 1: // Normal - 6 minutes  
                return 540f;
            case 2: // Hard - 4 minutes
                return 360f;
            default:
                return 540f;
        }
    }
    
    
    void GenerateSegment(float startX, float progressThroughLevel)
    {
        if (useProgressiveDifficulty)
        {
            ApplyProgressiveDifficulty(progressThroughLevel);
        }
        
        if (spawnedPositions.Count > 20)
        {
            spawnedPositions.RemoveRange(0, 10);
        }
        
        for (int i = 0; i < fruitsPerSegment; i++)
        {
            float xPos = startX + ((float)i / fruitsPerSegment) * segmentLength + Random.Range(1f, 3f);
            SpawnFruit(xPos, progressThroughLevel);
        }
        
        int buildingsToSpawn = Mathf.RoundToInt(obstaclesPerSegment * (buildingChance / 100f));
        for (int i = 0; i < buildingsToSpawn; i++)
        {
            float xPos = startX + (i * (segmentLength / Mathf.Max(buildingsToSpawn, 1))) + Random.Range(1f, 3f);
            SpawnSpecificObstacle(xPos, ObstacleType.Building);
        }
        
        int otherObstacles = obstaclesPerSegment - buildingsToSpawn;
        for (int i = 0; i < otherObstacles; i++)
        {
            float xPos = startX + Random.Range(5f, segmentLength - 2f);
            SpawnRandomNonBuildingObstacle(xPos);
        }

        for (int i = 0; i < maxThunderCloudsPerSegment; i++)
        {
            if (Random.Range(0f, 100f) < thunderCloudChance)
            {
                float xPos = startX + Random.Range(2f, segmentLength - 2f);
                SpawnThunderCloud(xPos);
            }
        }
    }
    
    void ApplyProgressiveDifficulty(float progress)
    {
        // More aggressive ramp-up
        if (progress < 0.05f) // First 5% - only fruits
        {
            obstaclesPerSegment = 0;
            fruitsPerSegment = 4;
            thunderCloudChance = 0f;
            goldenFruitChance = 0f;
            heartFruitChance = 0f;
            rottenFruitChance = 0f;
        }
        else if (progress < 0.20f) // Next 15% - introduce buildings and golden fruits
        {
            obstaclesPerSegment = 1;
            buildingChance = 100f; missileChance = 0f; cannonballChance = 0f;
            thunderCloudChance = 0f;
            goldenFruitChance = 20f;
            heartFruitChance = 0f;
            rottenFruitChance = 0f;
        }
        else if (progress < 0.40f) // Next 20% - introduce cannonballs and heart fruits
        {
            obstaclesPerSegment = 2;
            buildingChance = 50f; cannonballChance = 50f; missileChance = 0f;
            thunderCloudChance = 0f;
            goldenFruitChance = 15f;
            heartFruitChance = 5f;
            rottenFruitChance = 5f;
        }
        else // Final 60% - all hazards with increasing intensity
        {
            obstaclesPerSegment = Mathf.RoundToInt(2 + (progress * 4)); // Faster increase
            fruitsPerSegment = Mathf.RoundToInt(3 + (progress * 2));
            rottenFruitChance = 10f + (progress * 20f); // Faster increase
            goldenFruitChance = Mathf.Max(15f - (progress * 10f), 5f);
            thunderCloudChance = (progress - 0.4f) * 250f; // Starts earlier and ramps faster
        }
    }
    
    void SpawnFruit(float xPosition, float progressThroughLevel)
    {
        // Build the list of available regular fruits based on progress.
        List<GameObject> availableRegularFruits = new List<GameObject>();
        if (applePrefab != null) availableRegularFruits.Add(applePrefab);
        // Grapes now appear after 5% progress.
        if (grapePrefab != null && progressThroughLevel > 0.05f) availableRegularFruits.Add(grapePrefab);
        // Watermelons now appear after 20% progress.
        if (watermelonPrefab != null && progressThroughLevel > 0.20f) availableRegularFruits.Add(watermelonPrefab);
        // Dragonfruits now appear after 40% progress.
        if (dragonfruitPrefab != null && progressThroughLevel > 0.40f) availableRegularFruits.Add(dragonfruitPrefab);

        // Build the list of available golden fruits based on progress.
        List<GameObject> availableGoldenFruits = new List<GameObject>();
        if (goldApplePrefab != null && progressThroughLevel > 0.15f) availableGoldenFruits.Add(goldApplePrefab);
        if (goldGrapePrefab != null && progressThroughLevel > 0.30f) availableGoldenFruits.Add(goldGrapePrefab);
        if (goldWatermelonPrefab != null && progressThroughLevel > 0.50f) availableGoldenFruits.Add(goldWatermelonPrefab);
        if (goldDragonfruitPrefab != null && progressThroughLevel > 0.70f) availableGoldenFruits.Add(goldDragonfruitPrefab);

        if (availableRegularFruits.Count == 0) return;
        
        float randomValue = Random.Range(0f, 100f);
        GameObject fruitToSpawn = null;
        
        if (randomValue < heartFruitChance)
        {
            fruitToSpawn = heartFruitPrefab;
        }
        else if (randomValue < heartFruitChance + goldenFruitChance)
        {
            if (availableGoldenFruits.Count > 0)
            {
                fruitToSpawn = availableGoldenFruits[Random.Range(0, availableGoldenFruits.Count)];
            }
        }
        else if (randomValue < heartFruitChance + goldenFruitChance + rottenFruitChance)
        {
            fruitToSpawn = rottenFruitPrefab;
        }
        else
        {
            fruitToSpawn = availableRegularFruits[Random.Range(0, availableRegularFruits.Count)];
        }
        
        if (fruitToSpawn == null) return;

        Vector3 position = GetSpawnPositionWithOverlapCheck(xPosition, true);
        if (position == Vector3.zero) return;
        
        GameObject fruit = Instantiate(fruitToSpawn, position, Quaternion.identity);
        
        // Add component to destroy when offscreen to prevent memory buildup
        fruit.AddComponent<DestroyOffscreen>();
        
        spawnedPositions.Add(position);
    }
    
    void SpawnSpecificObstacle(float xPosition, ObstacleType obstacleType)
    {
        if (obstaclePrefabs.Length == 0) return;
        
        GameObject obstaclePrefab = GetObstaclePrefab(obstacleType);
        if (obstaclePrefab == null) return;
        
        Vector3 position = GetObstacleSpawnPosition(xPosition, obstacleType);
        if (position == Vector3.zero) return;
        
        GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
        
        // Add component to destroy when offscreen to prevent memory buildup
        obstacle.AddComponent<DestroyOffscreen>();
        
        if (obstacleType == ObstacleType.Building)
        {
            buildingXPositions.Add(xPosition);
            if (buildingXPositions.Count > 15)
            {
                buildingXPositions.RemoveRange(0, 5);
            }
        }
        
        spawnedPositions.Add(position);
    }
    
    void SpawnRandomNonBuildingObstacle(float xPosition)
    {
        if (obstaclePrefabs.Length == 0) return;
        
        float randomValue = Random.Range(0f, missileChance + cannonballChance);
        ObstacleType obstacleType;
        
        if (randomValue < missileChance)
        {
            obstacleType = ObstacleType.Missile;
        }
        else
        {
            obstacleType = ObstacleType.Cannonball;
        }
        
        SpawnSpecificObstacle(xPosition, obstacleType);
    }

    void SpawnThunderCloud(float xPosition)
    {
        if (thunderCloudPrefab == null) return;

        float yPosition = laneYPositions[laneYPositions.Length - 1] + 1.2f; 
        Vector3 position = new Vector3(xPosition, yPosition, 0);

        GameObject thunderCloud = Instantiate(thunderCloudPrefab, position, Quaternion.identity);
        
        // Add component to destroy when offscreen
        thunderCloud.AddComponent<DestroyOffscreen>();
    }
    
    Vector3 GetSpawnPositionWithOverlapCheck(float xPosition, bool isFruit)
    {
        int attempts = 0;
        int maxAttempts = 20;
        
        while (attempts < maxAttempts)
        {
            float yPosition;
            
            if (isFruit)
            {
                int airLaneIndex = Random.Range(1, laneYPositions.Length);
                yPosition = laneYPositions[airLaneIndex];
            }
            else
            {
                yPosition = laneYPositions[Random.Range(0, laneYPositions.Length)];
            }
            
            Vector3 candidatePosition = new Vector3(xPosition, yPosition, 0f);
            
            bool positionTaken = false;
            
            foreach (Vector3 existing in spawnedPositions)
            {
                if (Mathf.Approximately(existing.y, yPosition) && 
                    Mathf.Abs(existing.x - xPosition) < minSpawnDistance)
                {
                    positionTaken = true;
                    break;
                }
            }
            
            if (!positionTaken)
            {
                foreach (float buildingX in buildingXPositions)
                {
                    if (Mathf.Abs(xPosition - buildingX) < minSpawnDistance)
                    {
                        positionTaken = true;
                        break;
                    }
                }
            }
            
            if (!positionTaken)
            {
                return candidatePosition;
            }
            
            attempts++;
        }
        
        return Vector3.zero;
    }
    
    Vector3 GetObstacleSpawnPosition(float xPosition, ObstacleType obstacleType)
    {
        int attempts = 0;
        int maxAttempts = 15;
        
        while (attempts < maxAttempts)
        {
            float yPosition;
            
            switch (obstacleType)
            {
                case ObstacleType.Building:
                    yPosition = groundLevel;
                    break;
                    
                case ObstacleType.Missile:
                    yPosition = laneYPositions[Random.Range(0, laneYPositions.Length)];
                    break;
                    
                case ObstacleType.Cannonball:
                    yPosition = -6f;
                    break;
                    
                default:
                    yPosition = laneYPositions[Random.Range(0, laneYPositions.Length)];
                    break;
            }
            
            Vector3 candidatePosition = new Vector3(xPosition, yPosition, 0f);
            
            bool exactPositionTaken = false;
            foreach (Vector3 existing in spawnedPositions)
            {
                float requiredDistance = minSpawnDistance;
                
                if (obstacleType == ObstacleType.Building)
                {
                    requiredDistance = buildingSpacing;
                    
                    if (Mathf.Approximately(existing.y, groundLevel) && 
                        Mathf.Abs(existing.x - xPosition) < requiredDistance)
                    {
                        exactPositionTaken = true;
                        break;
                    }
                }
                else
                {
                    if (Mathf.Approximately(existing.y, yPosition) && 
                        Mathf.Abs(existing.x - xPosition) < requiredDistance)
                    {
                        exactPositionTaken = true;
                        break;
                    }
                }
            }
            
            if (!exactPositionTaken && obstacleType != ObstacleType.Building)
            {
                foreach (float buildingX in buildingXPositions)
                {
                    if (Mathf.Abs(xPosition - buildingX) < minSpawnDistance)
                    {
                        exactPositionTaken = true;
                        break;
                    }
                }
            }
            
            if (!exactPositionTaken)
            {
                return candidatePosition;
            }
            
            attempts++;
        }
        
        return Vector3.zero;
    }
    
    GameObject GetObstaclePrefab(ObstacleType type)
    {
        string typeName = type.ToString().ToLower();
        
        foreach (GameObject prefab in obstaclePrefabs)
        {
            if (prefab.name.ToLower().Contains(typeName))
            {
                return prefab;
            }
        }
        
        switch (type)
        {
            case ObstacleType.Building: return obstaclePrefabs.Length > 0 ? obstaclePrefabs[0] : null;
            case ObstacleType.Missile: return obstaclePrefabs.Length > 1 ? obstaclePrefabs[1] : null;
            case ObstacleType.Cannonball: return obstaclePrefabs.Length > 2 ? obstaclePrefabs[2] : null;
        }
        
        return obstaclePrefabs[0];
    }
}

// Helper component to destroy objects that go too far offscreen
public class DestroyOffscreen : MonoBehaviour
{
    public float destroyDistance = 20f;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        if (mainCamera != null)
        {
            float distanceFromCamera = transform.position.x - mainCamera.transform.position.x;
            if (distanceFromCamera < -destroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}

