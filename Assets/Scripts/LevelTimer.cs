using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [Tooltip("Time limit in seconds - can be set per level")]
    public float levelTimeLimit = 360f; // Default: 6 minutes (Normal mode)
    
    [Header("Variable Time Limits")]
    [Tooltip("Different time limits for different scenarios")]
    public float easyModeTimeLimit = 600f;   // 10 minutes
    public float normalModeTimeLimit = 360f; // 6 minutes  
    public float hardModeTimeLimit = 240f;   // 4 minutes
    public float speedRunTimeLimit = 60f;    // 1 minute
    
    [Header("Dynamic Time Adjustment")]
    public bool useScoreBasedTime = false;
    [Tooltip("Add extra time based on current score")]
    public float bonusTimePerThousandScore = 5f;
    
    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI rankingText;
    
    [Header("Enhanced UI")]
    public bool useEnhancedUI = true;
    
    private EnhancedTextDisplay timerDisplay;
    private EnhancedTextDisplay rankingDisplay;
    
    [Header("Ranking Thresholds")]
    [Tooltip("Percentage of time limit for Gold rank (0.33 = 33%)")]
    public float goldTimePercentage = 0.33f;
    [Tooltip("Percentage of time limit for Silver rank (0.5 = 50%)")]
    public float silverTimePercentage = 0.5f;
    
    // Calculated thresholds (auto-updated based on current time limit)
    private float goldTimeThreshold;
    private float silverTimeThreshold;
    
    public static LevelTimer instance;
    
    private float currentTime;
    private bool timerActive = false;
    private bool levelCompleted = false;
    
    public enum CompletionRank
    {
        Bronze,
        Silver, 
        Gold
    }

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
    }

    void Start()
    {
        SetupEnhancedUI();
        StartTimer();
    }
    
    void SetupEnhancedUI()
    {
        if (useEnhancedUI)
        {
            // Setup timer text display
            if (timerText != null)
            {
                timerDisplay = timerText.gameObject.GetComponent<EnhancedTextDisplay>();
                if (timerDisplay == null)
                {
                    timerDisplay = timerText.gameObject.AddComponent<EnhancedTextDisplay>();
                    timerDisplay.textStyle = EnhancedTextDisplay.TextStyle.Timer;
                    timerDisplay.textColor = EnhancedTextDisplay.TextColor.Timer;
                    timerDisplay.enableShadow = true;
                    timerDisplay.enableOutline = true;
                }
            }
            
            // Setup ranking text display
            if (rankingText != null)
            {
                rankingDisplay = rankingText.gameObject.GetComponent<EnhancedTextDisplay>();
                if (rankingDisplay == null)
                {
                    rankingDisplay = rankingText.gameObject.AddComponent<EnhancedTextDisplay>();
                    rankingDisplay.textStyle = EnhancedTextDisplay.TextStyle.Heading2;
                    rankingDisplay.textColor = EnhancedTextDisplay.TextColor.Success;
                    rankingDisplay.enablePulse = true;
                    rankingDisplay.pulseSpeed = 1.5f;
                    rankingDisplay.pulseIntensity = 0.1f;
                }
            }
        }
    }

    void Update()
    {
        if (timerActive && !GameManager.isLevelOver && !levelCompleted)
        {
            UpdateTimer();
        }
    }
    
    public void StartTimer()
    {
        // Calculate final time limit
        float finalTimeLimit = CalculateFinalTimeLimit();
        currentTime = finalTimeLimit;
        
        // Update ranking thresholds based on final time limit
        UpdateRankingThresholds(finalTimeLimit);
        
        timerActive = true;
        levelCompleted = false;
        UpdateTimerDisplay();
        
        if (rankingText != null)
        {
            rankingText.gameObject.SetActive(false);
        }
        
        Debug.Log($"Timer started with {finalTimeLimit} seconds. Gold: {goldTimeThreshold}s, Silver: {silverTimeThreshold}s");
    }
    
    float CalculateFinalTimeLimit()
    {
        float baseTime = levelTimeLimit;
        
        // Add score-based bonus time if enabled
        if (useScoreBasedTime && ScoreManager.instance != null)
        {
            int currentScore = ScoreManager.instance.GetScore();
            float bonusTime = (currentScore / 1000f) * bonusTimePerThousandScore;
            baseTime += bonusTime;
            Debug.Log($"Added {bonusTime:F1}s bonus time based on score {currentScore}");
        }
        
        return baseTime;
    }
    
    void UpdateRankingThresholds(float timeLimit)
    {
        goldTimeThreshold = timeLimit * goldTimePercentage;
        silverTimeThreshold = timeLimit * silverTimePercentage;
    }
    
    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();
        
        // Check if time ran out
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            TimeUp();
        }
    }
    
    void UpdateTimerDisplay()
    {
        if (useEnhancedUI && timerDisplay != null)
        {
            // Use enhanced timer display
            timerDisplay.UpdateTimer(currentTime);
        }
        else if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            
            // Change color as time runs low
            if (currentTime <= 30f) // Last 30 seconds
            {
                timerText.color = Color.red;
            }
            else if (currentTime <= 60f) // Last minute
            {
                timerText.color = Color.yellow;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }
    
    void TimeUp()
    {
        timerActive = false;
        GameManager.isLevelOver = true;
        
        Debug.Log("Time's up! Level failed.");
        
        // Find and disable player
        PlayerDragonController dragon = FindFirstObjectByType<PlayerDragonController>();
        if (dragon != null)
        {
            dragon.health = 0;
            dragon.isDead = true;
            dragon.SetToDead();
            dragon.enabled = false;
        }
        
        // Trigger game over screen
        GameEndManager.TriggerGameOver();
    }
    
    public void OnLevelCompleted()
    {
        if (levelCompleted) return; // Prevent multiple calls
        
        levelCompleted = true;
        timerActive = false;
        
        float completionTime = levelTimeLimit - currentTime;
        CompletionRank rank = CalculateRank(completionTime);
        
        
        Debug.Log($"Level completed in {completionTime:F1} seconds - Rank: {rank}");
    }
    
    CompletionRank CalculateRank(float completionTime)
    {
        if (completionTime <= goldTimeThreshold)
        {
            return CompletionRank.Gold;
        }
        else if (completionTime <= silverTimeThreshold)
        {
            return CompletionRank.Silver;
        }
        else
        {
            return CompletionRank.Bronze;
        }
    }
    
    // void ShowCompletionRank(CompletionRank rank, float completionTime)
    // {
    //     if (rankingText != null)
    //     {
    //         rankingText.gameObject.SetActive(true);
            
    //         string rankString = "";
    //         Color rankColor = Color.white;
            
    //         switch (rank)
    //         {
    //             case CompletionRank.Gold:
    //                 rankString = "★ GOLD RANK! ★";
    //                 rankColor = Color.yellow;
    //                 break;
    //             case CompletionRank.Silver:
    //                 rankString = "★ SILVER RANK ★";
    //                 rankColor = Color.gray;
    //                 break;
    //             case CompletionRank.Bronze:
    //                 rankString = "★ BRONZE RANK ★";
    //                 rankColor = new Color(0.8f, 0.5f, 0.2f); // Bronze color
    //                 break;
    //         }
            
    //         int minutes = Mathf.FloorToInt(completionTime / 60f);
    //         int seconds = Mathf.FloorToInt(completionTime % 60f);
            
    //         rankingText.text = $"{rankString}\nCompleted in: {minutes:00}:{seconds:00}";
    //         rankingText.color = rankColor;
    //     }
    // }
    
    public float GetRemainingTime()
    {
        return currentTime;
    }
    
    public float GetElapsedTime()
    {
        return levelTimeLimit - currentTime;
    }
    
    public bool IsTimerActive()
    {
        return timerActive;
    }
    
    // Public methods to set different time limits
    public void SetEasyMode()
    {
        levelTimeLimit = easyModeTimeLimit;
        Debug.Log($"Set to Easy Mode: {easyModeTimeLimit} seconds");
    }
    
    public void SetNormalMode()
    {
        levelTimeLimit = normalModeTimeLimit;
        Debug.Log($"Set to Normal Mode: {normalModeTimeLimit} seconds");
    }
    
    public void SetHardMode()
    {
        levelTimeLimit = hardModeTimeLimit;
        Debug.Log($"Set to Hard Mode: {hardModeTimeLimit} seconds");
    }
    
    public void SetSpeedRunMode()
    {
        levelTimeLimit = speedRunTimeLimit;
        Debug.Log($"Set to Speed Run Mode: {speedRunTimeLimit} seconds");
    }
    
    public void SetCustomTimeLimit(float seconds)
    {
        levelTimeLimit = seconds;
        Debug.Log($"Set custom time limit: {seconds} seconds");
    }
    
    // Context menu for testing different time limits
    [ContextMenu("Test Easy Mode (3 min)")]
    void TestEasyMode() { SetEasyMode(); }
    
    [ContextMenu("Test Normal Mode (2 min)")]
    void TestNormalMode() { SetNormalMode(); }
    
    [ContextMenu("Test Hard Mode (1.5 min)")]
    void TestHardMode() { SetHardMode(); }
    
    [ContextMenu("Test Speed Run (1 min)")]
    void TestSpeedRun() { SetSpeedRunMode(); }
}