using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameEndManager : MonoBehaviour
{
    [Header("Game End Screens")]
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;
    
    [Header("Game Over Elements")]
    public TextMeshProUGUI gameOverTitle;
    public TextMeshProUGUI gameOverScore;
    public TextMeshProUGUI gameOverTime;
    public Button gameOverRetryButton;
    public Button gameOverMenuButton;
    
    [Header("Level Complete Elements")]
    public TextMeshProUGUI levelCompleteTitle;
    public TextMeshProUGUI levelCompleteScore;
    public TextMeshProUGUI levelCompleteTime;
    public TextMeshProUGUI levelCompleteRank;
    public Button levelCompleteRetryButton;
    public Button levelCompleteMenuButton;
    
    [Header("Animation Settings")]
    public float fadeInDuration = 1f;
    public float delayBeforeShow = 1f;
    
    [Header("Enhanced UI")]
    public bool useEnhancedUI = true;
    
    public static GameEndManager instance;
    
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
        
        // Hide panels initially
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        
        SetupButtons();
    }
    
    void SetupButtons()
    {
        // Game Over buttons
        if (gameOverRetryButton != null)
        {
            gameOverRetryButton.onClick.AddListener(RestartLevel);
            SetupEnhancedButton(gameOverRetryButton, EnhancedButton.ButtonStyle.Primary, "TRY AGAIN");
        }
        if (gameOverMenuButton != null)
        {
            gameOverMenuButton.onClick.AddListener(GoToMainMenu);
            SetupEnhancedButton(gameOverMenuButton, EnhancedButton.ButtonStyle.Secondary, "MAIN MENU");
        }
        
        // Level Complete buttons
        if (levelCompleteRetryButton != null)
        {
            levelCompleteRetryButton.onClick.AddListener(RestartLevel);
            SetupEnhancedButton(levelCompleteRetryButton, EnhancedButton.ButtonStyle.Success, "PLAY AGAIN");
        }
        if (levelCompleteMenuButton != null)
        {
            levelCompleteMenuButton.onClick.AddListener(GoToMainMenu);
            SetupEnhancedButton(levelCompleteMenuButton, EnhancedButton.ButtonStyle.Secondary, "MAIN MENU");
        }
        
        // Setup enhanced text displays
        if (useEnhancedUI)
        {
            SetupEnhancedTextDisplays();
        }
    }
    
    void SetupEnhancedButton(Button button, EnhancedButton.ButtonStyle style, string buttonText)
    {
        if (!useEnhancedUI || button == null) return;
        
        EnhancedButton enhanced = button.gameObject.GetComponent<EnhancedButton>();
        if (enhanced == null)
        {
            enhanced = button.gameObject.AddComponent<EnhancedButton>();
            enhanced.buttonStyle = style;
            enhanced.buttonSize = EnhancedButton.ButtonSize.Medium;
            enhanced.SetButtonText(buttonText);
        }

        TextMeshProUGUI buttonTextComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonTextComponent != null)
        {
            SetupEnhancedText(buttonTextComponent, EnhancedTextDisplay.TextStyle.Button, EnhancedTextDisplay.TextColor.Primary);
        }
    }
    
    void SetupEnhancedTextDisplays()
    {
        // Game Over text displays
        SetupEnhancedText(gameOverTitle, EnhancedTextDisplay.TextStyle.Heading1, EnhancedTextDisplay.TextColor.Danger);
        SetupEnhancedText(gameOverScore, EnhancedTextDisplay.TextStyle.Button, EnhancedTextDisplay.TextColor.Primary);
        SetupEnhancedText(gameOverTime, EnhancedTextDisplay.TextStyle.Button, EnhancedTextDisplay.TextColor.Secondary);
        
        // Level Complete text displays
        SetupEnhancedText(levelCompleteTitle, EnhancedTextDisplay.TextStyle.Heading1, EnhancedTextDisplay.TextColor.Success);
        SetupEnhancedText(levelCompleteScore, EnhancedTextDisplay.TextStyle.Button, EnhancedTextDisplay.TextColor.Primary);
        SetupEnhancedText(levelCompleteTime, EnhancedTextDisplay.TextStyle.Button, EnhancedTextDisplay.TextColor.Secondary);
        SetupEnhancedText(levelCompleteRank, EnhancedTextDisplay.TextStyle.Heading2, EnhancedTextDisplay.TextColor.Success);
    }
    
    void SetupEnhancedText(TextMeshProUGUI textComponent, EnhancedTextDisplay.TextStyle style, EnhancedTextDisplay.TextColor color)
    {
        if (textComponent == null) return;
        
        EnhancedTextDisplay enhanced = textComponent.gameObject.GetComponent<EnhancedTextDisplay>();
        if (enhanced == null)
        {
            enhanced = textComponent.gameObject.AddComponent<EnhancedTextDisplay>();
            enhanced.textStyle = style;
            enhanced.textColor = color;
            enhanced.enableShadow = true;
            
            // Add special effects for titles
            if (style == EnhancedTextDisplay.TextStyle.Heading1)
            {
                enhanced.enablePulse = true;
                enhanced.pulseSpeed = 0.8f;
                enhanced.pulseIntensity = 0.08f;
            }
        }
    }
    
    public void ShowGameOver()
    {
        StartCoroutine(ShowGameOverCoroutine());
    }
    
    public void ShowLevelComplete()
    {
        StartCoroutine(ShowLevelCompleteCoroutine());
    }
    
    IEnumerator ShowGameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(delayBeforeShow);
        
        if (gameOverPanel == null) yield break;
        
        // Get current game stats
        int finalScore = ScoreManager.instance != null ? ScoreManager.instance.currentScore : 0;
        float elapsedTime = LevelTimer.instance != null ? LevelTimer.instance.GetElapsedTime() : 0f;
        
        // Update game over text
        UpdateGameOverText(finalScore, elapsedTime);
        
        // Show panel
        gameOverPanel.SetActive(true);
        
        // Animate fade in
        yield return StartCoroutine(FadeInPanel(gameOverPanel));
        
        // Play sound effect
        if (AudioManager.instance != null)
        {
            // Could add game over sound here
        }
    }
    
    IEnumerator ShowLevelCompleteCoroutine()
    {
        yield return new WaitForSecondsRealtime(delayBeforeShow);
        
        if (levelCompletePanel == null) yield break;
        
        // Get current game stats
        int finalScore = ScoreManager.instance != null ? ScoreManager.instance.currentScore : 0;
        float elapsedTime = LevelTimer.instance != null ? LevelTimer.instance.GetElapsedTime() : 0f;
        string rank = GetCompletionRank(elapsedTime);
        
        // Update level complete text
        UpdateLevelCompleteText(finalScore, elapsedTime, rank);
        
        // Show panel
        levelCompletePanel.SetActive(true);
        
        // Animate fade in
        yield return StartCoroutine(FadeInPanel(levelCompletePanel));
        
        // Play completion sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayPowerUp(); // Success sound
        }
    }
    
    void UpdateGameOverText(int score, float time)
    {
        if (gameOverTitle != null)
        {
            gameOverTitle.text = "GAME OVER";
        }
        
        if (gameOverScore != null)
        {
            gameOverScore.text = $"Final Score: {score:N0}";
        }
        
        if (gameOverTime != null)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            gameOverTime.text = $"Survived: {minutes:00}:{seconds:00}";
        }
    }
    
    void UpdateLevelCompleteText(int score, float time, string rank)
    {
        if (levelCompleteTitle != null)
        {
            levelCompleteTitle.text = "LEVEL COMPLETE!";
        }
        
        if (levelCompleteScore != null)
        {
            levelCompleteScore.text = $"Final Score: {score:N0}";
        }
        
        if (levelCompleteTime != null)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            levelCompleteTime.text = $"Time: {minutes:00}:{seconds:00}";
        }
        
        if (levelCompleteRank != null)
        {
            levelCompleteRank.text = rank;
            
            // Set rank color
            switch (rank.ToLower())
            {
                case "Gold rank!":
                    levelCompleteRank.color = Color.yellow;
                    break;
                case "Silver rank!":
                    levelCompleteRank.color = Color.gray;
                    break;
                case "Bronze rank!":
                    levelCompleteRank.color = new Color(0.8f, 0.5f, 0.2f);
                    break;
                default:
                    levelCompleteRank.color = Color.white;
                    break;
            }
        }
    }
    
    string GetCompletionRank(float completionTime)
    {
        // Use LevelTimer's ranking logic if available
        if (LevelTimer.instance != null)
        {
            if (completionTime <= 40f) // Gold threshold
            {
                return "★ GOLD RANK! ★";
            }
            else if (completionTime <= 60f) // Silver threshold
            {
                return "★ SILVER RANK ★";
            }
            else
            {
                return "★ BRONZE RANK ★";
            }
        }
        
        return "COMPLETED";
    }
    
    IEnumerator FadeInPanel(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        float elapsed = 0f;
        
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    public void RestartLevel()
    {
        // Play click sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySelectClick();
        }
        
        // Reset time scale
        Time.timeScale = 1f;
        
        // Reload current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    public void GoToMainMenu()
    {
        // Play click sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySelectClick();
        }
        
        // Reset time scale
        Time.timeScale = 1f;
        
        // Load title screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }
    
    // Called by other systems when game ends
    public static void TriggerGameOver()
    {
        if (instance != null)
        {
            instance.ShowGameOver();
        }
    }
    
    public static void TriggerLevelComplete()
    {
        if (instance != null)
        {
            instance.ShowLevelComplete();
        }
    }
}