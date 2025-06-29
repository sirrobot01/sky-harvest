using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    public GameObject regularFruitPrefab;
    public GameObject goldenFruitPrefab;
    public GameObject heartFruitPrefab;
    public GameObject rottenFruitPrefab;
    
    [Header("Spawn Settings")]
    public float spawnRate = 2f; // Fruits per second
    public float spawnRangeY = 8f; // Vertical spawn range
    public float spawnOffsetX = 15f; // How far right to spawn
    
    [Header("Fruit Probabilities (0-100)")]
    [Range(0, 100)] public float regularFruitChance = 70f;
    [Range(0, 100)] public float goldenFruitChance = 15f;
    [Range(0, 100)] public float heartFruitChance = 5f;
    [Range(0, 100)] public float rottenFruitChance = 10f;
    
    [Header("Lane Positions")]
    public float[] laneYPositions = { -4f, -2f, 0f, 2f, 4f };
    
    private float nextSpawnTime;
    private Camera mainCamera;
    
    // Regular fruit types for variety
    private Fruit.FruitType[] regularTypes = {
        Fruit.FruitType.Apple,
        Fruit.FruitType.Grape,
        Fruit.FruitType.Watermelon,
        Fruit.FruitType.Dragonfruit
    };
    
    // Golden fruit types
    private Fruit.FruitType[] goldenTypes = {
        Fruit.FruitType.GoldApple,
        Fruit.FruitType.GoldGrape,
        Fruit.FruitType.GoldWatermelon,
        Fruit.FruitType.GoldDragonfruit
    };

    void Start()
    {
        mainCamera = Camera.main;
        nextSpawnTime = Time.time + (1f / spawnRate);
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnFruit();
            nextSpawnTime = Time.time + (1f / spawnRate) + Random.Range(-0.3f, 0.3f);
        }
    }
    
    void SpawnFruit()
    {
        // Determine fruit type based on probabilities
        float randomValue = Random.Range(0f, 100f);
        GameObject fruitPrefab;
        Fruit.FruitType fruitType;
        
        if (randomValue < heartFruitChance)
        {
            // Heart fruit (rarest)
            fruitPrefab = heartFruitPrefab != null ? heartFruitPrefab : regularFruitPrefab;
            fruitType = Fruit.FruitType.HeartFruit;
        }
        else if (randomValue < heartFruitChance + goldenFruitChance)
        {
            // Golden fruit (rare)
            fruitPrefab = goldenFruitPrefab != null ? goldenFruitPrefab : regularFruitPrefab;
            fruitType = goldenTypes[Random.Range(0, goldenTypes.Length)];
        }
        else if (randomValue < heartFruitChance + goldenFruitChance + rottenFruitChance)
        {
            // Rotten fruit (uncommon)
            fruitPrefab = rottenFruitPrefab != null ? rottenFruitPrefab : regularFruitPrefab;
            fruitType = Fruit.FruitType.RottenApple;
        }
        else
        {
            // Regular fruit (common)
            fruitPrefab = regularFruitPrefab;
            fruitType = regularTypes[Random.Range(0, regularTypes.Length)];
        }
        
        // Choose spawn position
        Vector3 spawnPosition = GetSpawnPosition();
        
        // Spawn the fruit
        GameObject fruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
        
        // Set the fruit type
        Fruit fruitScript = fruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.fruitType = fruitType;
            
            // Set appropriate sprite based on fruit type
            SetFruitSprite(fruit, fruitType);
            Debug.Log("Spawned fruit of type " + fruitType + " with script attached.");
        }
        else
        {
            Debug.LogError("Fruit script missing on prefab: " + fruitPrefab.name);
        }
        
        // Auto-destroy fruit after 15 seconds if not collected
        Destroy(fruit, 15f);
    }
    
    Vector3 GetSpawnPosition()
    {
        // Choose random lane or free position
        bool useLane = Random.Range(0f, 1f) < 0.7f; // 70% chance to spawn in lane
        
        float yPosition;
        if (useLane && laneYPositions.Length > 0)
        {
            yPosition = laneYPositions[Random.Range(0, laneYPositions.Length)];
        }
        else
        {
            yPosition = Random.Range(-spawnRangeY / 2f, spawnRangeY / 2f);
        }
        
        float xPosition = mainCamera.transform.position.x + spawnOffsetX;
        
        return new Vector3(xPosition, yPosition, 0f);
    }
    
    void SetFruitSprite(GameObject fruit, Fruit.FruitType fruitType)
    {
        SpriteRenderer renderer = fruit.GetComponent<SpriteRenderer>();
        if (renderer == null) return;
        
        // Load appropriate sprite from Resources or assign manually
        // This assumes you have the sprites loaded properly in your prefabs
        // You may need to adjust this based on your sprite setup
        
        string spriteName = GetSpriteNameForFruit(fruitType);
        Sprite fruitSprite = Resources.Load<Sprite>("Sprites/Fruits/" + spriteName);
        
        if (fruitSprite != null)
        {
            renderer.sprite = fruitSprite;
        }
        else
        {
            Debug.LogWarning($"Could not find sprite for {fruitType}: {spriteName}");
        }
    }
    
    string GetSpriteNameForFruit(Fruit.FruitType fruitType)
    {
        switch (fruitType)
        {
            case Fruit.FruitType.Apple: return "apple";
            case Fruit.FruitType.Grape: return "grape";
            case Fruit.FruitType.Watermelon: return "watermelon";
            case Fruit.FruitType.Dragonfruit: return "dragonfruit";
            
            case Fruit.FruitType.GoldApple: return "gold apple";
            case Fruit.FruitType.GoldGrape: return "gold grape";
            case Fruit.FruitType.GoldWatermelon: return "gold watermelon";
            case Fruit.FruitType.GoldDragonfruit: return "gold dragonfruit";
            
            case Fruit.FruitType.HeartFruit: return "heart fruit";
            case Fruit.FruitType.RottenApple: return "rotten apple";
            
            default: return "apple";
        }
    }
    
    public void AdjustDifficulty(int level)
    {
        // Increase spawn rate and change probabilities based on level
        spawnRate = Mathf.Min(2f + (level * 0.3f), 4f); // Cap at 4 fruits per second
        
        // Make golden fruits rarer and rotten fruits more common in higher levels
        goldenFruitChance = Mathf.Max(15f - (level * 2f), 5f); // Min 5%
        rottenFruitChance = Mathf.Min(10f + (level * 3f), 25f); // Max 25%
        heartFruitChance = Mathf.Max(5f - level, 2f); // Min 2%
        
        // Adjust regular fruit chance to maintain 100% total
        regularFruitChance = 100f - goldenFruitChance - rottenFruitChance - heartFruitChance;
    }
}