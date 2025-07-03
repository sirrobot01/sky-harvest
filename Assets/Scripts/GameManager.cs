using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevel = 1;
    public int scoreRequirement = 300; // From design doc: Level 1 needs 300 points
    
    public static bool isLevelOver = false;
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        InitializeLevel();
    }
    
    void InitializeLevel()
    {
        isLevelOver = false;
        Time.timeScale = 1f;
        
        
        // Apply difficulty settings if DifficultyManager exists
        if (DifficultyManager.instance != null)
        {
            DifficultyManager.instance.ApplyDifficultyToGame();
        }
        
        // Set score requirement on FinishLine if it exists (for endless mode checkpoints)
        FinishLine finishLine = FindFirstObjectByType<FinishLine>();
        if (finishLine != null)
        {
            finishLine.scoreRequirement = scoreRequirement;
            // Enable endless mode by default
            finishLine.endlessMode = true;
        }
        
        string difficulty = DifficultyManager.instance != null ? DifficultyManager.instance.GetDifficultyName() : "NORMAL";
        Debug.Log($"Level started - Difficulty: {difficulty}");
    }
    
    public void RestartLevel()
    {
        InitializeLevel();
        
        // Reset player
        PlayerDragonController dragon = FindFirstObjectByType<PlayerDragonController>();
        if (dragon != null)
        {
            dragon.health = 3;
            dragon.isDead = false;
            dragon.isInvincible = false;
            dragon.isStunned = false;
            dragon.enabled = true;
        }
        
        // Reset timer
        if (LevelTimer.instance != null)
        {
            LevelTimer.instance.StartTimer();
        }
        
        // Reset score
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.currentScore = 0;
        }
    }
    
    public void OnPlayerDeath()
    {
        if (!isLevelOver)
        {
            isLevelOver = true;
            
            TextManager textManager = FindFirstObjectByType<TextManager>();
            if (textManager != null)
            {
                textManager.ShowGameOver();
            }
            
            Time.timeScale = 0f;
        }
    }
}
