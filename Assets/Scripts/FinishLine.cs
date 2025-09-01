using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("Finish Line Settings")]
    public float scrollSpeed = 4f;
    
    [Header("Finish Line Mode")]
    [Tooltip("If true, finish line acts as checkpoint instead of ending game")]
    public bool endlessMode = false;
    [Tooltip("Score bonus when reaching finish line in endless mode")]
    public int checkpointBonus = 500;
    
    [Header("Traditional Level Mode")]
    [Tooltip("Score requirement to complete level (only used if not endless mode)")]
    public int scoreRequirement = 100;
    
    private PlayerDragonController dragon;
    private bool hasTriggered = false;

    void Start()
    {
        dragon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDragonController>();
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        if (!dragon.isDead)
        {
            float scrollAmount = scrollSpeed * PlayerDragonController.scrollSpeedMultiplier * Time.deltaTime;
            transform.position -= new Vector3(scrollAmount, 0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            
            // Traditional mode: Check score requirement and end game
            HandleLevelCompletion();
        }
    }
    
    // HandleEndlessCheckpoint removed - using traditional level completion only
    
    void HandleLevelCompletion()
    {
        if (ScoreManager.instance.currentScore >= scoreRequirement)
        {
            Debug.Log("Level Complete! 🎉");
            GameManager.isLevelOver = true;
            dragon.enabled = false;
            
            // Trigger timer completion and ranking
            if (LevelTimer.instance != null)
            {
                LevelTimer.instance.OnLevelCompleted();
            }
            
            // Trigger level complete screen
            GameEndManager.TriggerLevelComplete();
            
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("Score too low. Game Over.");
            GameManager.isLevelOver = true;
            dragon.health = 0;
            dragon.isDead = true;
            dragon.SetToDead();
            dragon.enabled = false;
            
            // Trigger game over screen
            GameEndManager.TriggerGameOver();
        }
    }
    
    // All endless mode methods removed - using traditional level completion only
}
