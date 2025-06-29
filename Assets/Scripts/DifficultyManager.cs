using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Difficulty Settings")]
    public DifficultyLevel selectedDifficulty = DifficultyLevel.Normal;
    
    [Header("Easy Mode")]
    public float easyTimeLimit = 600f;      // 10 minutes
    public float easySpawnRate = 0.8f;      // Slower spawning
    public int easyDragonHealth = 5;        // More health
    
    [Header("Normal Mode")]
    public float normalTimeLimit = 360f;    // 6 minutes
    public float normalSpawnRate = 1.0f;    // Normal spawning
    public int normalDragonHealth = 3;      // Normal health
    
    [Header("Hard Mode")]
    public float hardTimeLimit = 240f;      // 4 minutes
    public float hardSpawnRate = 1.3f;      // Faster spawning
    public int hardDragonHealth = 2;        // Less health
    
    public static DifficultyManager instance;
    
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }
    
    void Awake()
    {
        // Persist across scene changes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetDifficulty(DifficultyLevel difficulty)
    {
        selectedDifficulty = difficulty;
        Debug.Log($"Difficulty set to: {difficulty}");
        
        // Save to PlayerPrefs for persistence
        PlayerPrefs.SetInt("GameDifficulty", (int)difficulty);
        PlayerPrefs.Save();
    }
    
    public void SetEasyDifficulty() { SetDifficulty(DifficultyLevel.Easy); }
    public void SetNormalDifficulty() { SetDifficulty(DifficultyLevel.Normal); }
    public void SetHardDifficulty() { SetDifficulty(DifficultyLevel.Hard); }
    
    void Start()
    {
        // Load saved difficulty
        if (PlayerPrefs.HasKey("GameDifficulty"))
        {
            selectedDifficulty = (DifficultyLevel)PlayerPrefs.GetInt("GameDifficulty");
        }
    }
    
    public float GetTimeLimit()
    {
        switch (selectedDifficulty)
        {
            case DifficultyLevel.Easy: return easyTimeLimit;
            case DifficultyLevel.Normal: return normalTimeLimit;
            case DifficultyLevel.Hard: return hardTimeLimit;
            default: return normalTimeLimit;
        }
    }
    
    public float GetSpawnRateMultiplier()
    {
        switch (selectedDifficulty)
        {
            case DifficultyLevel.Easy: return easySpawnRate;
            case DifficultyLevel.Normal: return normalSpawnRate;
            case DifficultyLevel.Hard: return hardSpawnRate;
            default: return normalSpawnRate;
        }
    }
    
    public int GetDragonHealth()
    {
        switch (selectedDifficulty)
        {
            case DifficultyLevel.Easy: return easyDragonHealth;
            case DifficultyLevel.Normal: return normalDragonHealth;
            case DifficultyLevel.Hard: return hardDragonHealth;
            default: return normalDragonHealth;
        }
    }
    
    public string GetDifficultyName()
    {
        return selectedDifficulty.ToString().ToUpper();
    }
    
    public Color GetDifficultyColor()
    {
        switch (selectedDifficulty)
        {
            case DifficultyLevel.Easy: return new Color(0.2f, 0.8f, 0.2f); // Green
            case DifficultyLevel.Normal: return new Color(1f, 0.8f, 0f); // Yellow
            case DifficultyLevel.Hard: return new Color(0.8f, 0.2f, 0.2f); // Red
            default: return Color.white;
        }
    }
    
    // Apply difficulty settings to game systems
    public void ApplyDifficultyToGame()
    {
        // Apply to timer
        if (LevelTimer.instance != null)
        {
            LevelTimer.instance.levelTimeLimit = GetTimeLimit();
            Debug.Log($"Applied {selectedDifficulty} time limit: {GetTimeLimit()} seconds");
        }
        
        // Apply to dragon health
        PlayerDragonController dragon = FindFirstObjectByType<PlayerDragonController>();
        if (dragon != null)
        {
            dragon.health = GetDragonHealth();
            Debug.Log($"Applied {selectedDifficulty} dragon health: {GetDragonHealth()}");
        }
        
        // Apply to spawners (you can extend this)
        ApplyDifficultyToSpawners();
    }
    
    void ApplyDifficultyToSpawners()
    {
        // Apply spawn rate to fruit spawner
        FruitSpawner fruitSpawner = FindFirstObjectByType<FruitSpawner>();
        if (fruitSpawner != null)
        {
            // Adjust spawn timing based on difficulty
            float baseSpawnTime = 2f; // Default spawn time
            float adjustedSpawnTime = baseSpawnTime / GetSpawnRateMultiplier();
            
            // You might need to expose spawn timing in FruitSpawner for this to work
            Debug.Log($"Applied {selectedDifficulty} spawn rate: {GetSpawnRateMultiplier()}x");
        }
    }
}