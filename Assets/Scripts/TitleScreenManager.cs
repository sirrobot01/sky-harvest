using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Button quitButton;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    
    [Header("Difficulty Selection")]
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public TextMeshProUGUI difficultyDisplayText;
    public TextMeshProUGUI selectedDifficultyText;
    
    [Header("Control Guide")]
    public TextMeshProUGUI controlsText;
    public GameObject controlsPanel;
    
    [Header("Enhanced UI")]
    public bool useEnhancedUI = true;
    
    [Header("Audio")]
    public AudioSource backgroundMusic;

    void Start()
    {
        // Ensure DifficultyManager exists
        if (DifficultyManager.instance == null)
        {
            GameObject difficultyManagerObj = new GameObject("DifficultyManager");
            difficultyManagerObj.AddComponent<DifficultyManager>();
        }
        
        SetupTitleScreen();
        UpdateDifficultyDisplay();
        SetupControlsDisplay();
        
        // Play background music if available
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    void Update()
    {
        // Allow Enter or Space to start the game
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        
        // Allow Escape to quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    void SetupTitleScreen()
    {
        // Setup button listeners
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            
            // Upgrade to enhanced button if enabled
            if (useEnhancedUI)
            {
                EnhancedButton enhancedStart = startButton.gameObject.GetComponent<EnhancedButton>();
                if (enhancedStart == null)
                {
                    enhancedStart = startButton.gameObject.AddComponent<EnhancedButton>();
                    enhancedStart.buttonStyle = EnhancedButton.ButtonStyle.Primary;
                    enhancedStart.buttonSize = EnhancedButton.ButtonSize.Large;
                    enhancedStart.SetButtonText("START GAME");
                }
            }
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            
            // Upgrade to enhanced button if enabled
            if (useEnhancedUI)
            {
                EnhancedButton enhancedQuit = quitButton.gameObject.GetComponent<EnhancedButton>();
                if (enhancedQuit == null)
                {
                    enhancedQuit = quitButton.gameObject.AddComponent<EnhancedButton>();
                    enhancedQuit.buttonStyle = EnhancedButton.ButtonStyle.Secondary;
                    enhancedQuit.buttonSize = EnhancedButton.ButtonSize.Large;
                    enhancedQuit.SetButtonText("QUIT GAME");
                }
            }
        }
        
        // Setup difficulty buttons
        SetupDifficultyButtons();
        
        // Setup enhanced text displays
        if (useEnhancedUI)
        {
            SetupEnhancedText();
        }
    }

    public void StartGame()
    {
        // Play click sound if AudioManager exists
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySelectClick();
        }
        
        // Go directly to game
        LoadGameScene();
    }
    
    void LoadGameScene()
    {
        // Stop background music
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    
    public void QuitGame()
    {
        // Play click sound if AudioManager exists
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySelectClick();
        }
        
        Debug.Log("Quit Game - Works in built application");
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    void SetupEnhancedText()
    {
        // Setup title text
        if (titleText != null)
        {
            EnhancedTextDisplay titleDisplay = titleText.gameObject.GetComponent<EnhancedTextDisplay>();
            if (titleDisplay == null)
            {
                titleDisplay = titleText.gameObject.AddComponent<EnhancedTextDisplay>();
                titleDisplay.textStyle = EnhancedTextDisplay.TextStyle.Heading1;
                titleDisplay.textColor = EnhancedTextDisplay.TextColor.Primary;
                titleDisplay.enablePulse = true;
                titleDisplay.pulseSpeed = 1f;
                titleDisplay.pulseIntensity = 0.05f;
            }
        }
        
        // Setup subtitle text
        if (subtitleText != null)
        {
            EnhancedTextDisplay subtitleDisplay = subtitleText.gameObject.GetComponent<EnhancedTextDisplay>();
            if (subtitleDisplay == null)
            {
                subtitleDisplay = subtitleText.gameObject.AddComponent<EnhancedTextDisplay>();
                subtitleDisplay.textStyle = EnhancedTextDisplay.TextStyle.Heading3;
                subtitleDisplay.textColor = EnhancedTextDisplay.TextColor.Secondary;
            }
        }
    }
    
    // Apply theme to all UI elements
    [ContextMenu("Apply UI Theme")]
    public void ApplyUITheme()
    {
        UIThemeApplier themeApplier = GetComponent<UIThemeApplier>();
        if (themeApplier == null)
        {
            themeApplier = gameObject.AddComponent<UIThemeApplier>();
            themeApplier.applyOnStart = true;
            themeApplier.applyToChildren = true;
        }
        
        themeApplier.ApplyTheme();
    }
    
    void SetupDifficultyButtons()
    {
        // Setup Easy button
        if (easyButton != null)
        {
            easyButton.onClick.AddListener(() => SelectDifficulty(DifficultyManager.DifficultyLevel.Easy));
            
            if (useEnhancedUI)
            {
                EnhancedButton enhanced = easyButton.gameObject.GetComponent<EnhancedButton>();
                if (enhanced == null)
                {
                    enhanced = easyButton.gameObject.AddComponent<EnhancedButton>();
                    enhanced.buttonStyle = EnhancedButton.ButtonStyle.Success;
                    enhanced.buttonSize = EnhancedButton.ButtonSize.Medium;
                    enhanced.SetButtonText("EASY");
                }
            }
        }
        
        // Setup Normal button
        if (normalButton != null)
        {
            normalButton.onClick.AddListener(() => SelectDifficulty(DifficultyManager.DifficultyLevel.Normal));
            
            if (useEnhancedUI)
            {
                EnhancedButton enhanced = normalButton.gameObject.GetComponent<EnhancedButton>();
                if (enhanced == null)
                {
                    enhanced = normalButton.gameObject.AddComponent<EnhancedButton>();
                    enhanced.buttonStyle = EnhancedButton.ButtonStyle.Primary;
                    enhanced.buttonSize = EnhancedButton.ButtonSize.Medium;
                    enhanced.SetButtonText("NORMAL");
                }
            }
        }
        
        // Setup Hard button
        if (hardButton != null)
        {
            hardButton.onClick.AddListener(() => SelectDifficulty(DifficultyManager.DifficultyLevel.Hard));
            
            if (useEnhancedUI)
            {
                EnhancedButton enhanced = hardButton.gameObject.GetComponent<EnhancedButton>();
                if (enhanced == null)
                {
                    enhanced = hardButton.gameObject.AddComponent<EnhancedButton>();
                    enhanced.buttonStyle = EnhancedButton.ButtonStyle.Danger;
                    enhanced.buttonSize = EnhancedButton.ButtonSize.Medium;
                    enhanced.SetButtonText("HARD");
                }
            }
        }
    }
    
    void SelectDifficulty(DifficultyManager.DifficultyLevel difficulty)
    {
        // Play click sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySelectClick();
        }
        
        // Set difficulty
        if (DifficultyManager.instance != null)
        {
            DifficultyManager.instance.SetDifficulty(difficulty);
        }
        
        // Update display
        UpdateDifficultyDisplay();
        
        // Visual feedback on selected button
        HighlightSelectedDifficulty(difficulty);
    }
    
    void UpdateDifficultyDisplay()
    {
        if (DifficultyManager.instance == null) return;
        
        string difficultyName = DifficultyManager.instance.GetDifficultyName();
        Color difficultyColor = DifficultyManager.instance.GetDifficultyColor();
        float timeLimit = DifficultyManager.instance.GetTimeLimit();
        
        // Update main difficulty display
        if (difficultyDisplayText != null)
        {
            difficultyDisplayText.text = $"DIFFICULTY: {difficultyName}";
            difficultyDisplayText.color = difficultyColor;
        }
        
        // Update selected difficulty info
        if (selectedDifficultyText != null)
        {
            int minutes = Mathf.FloorToInt(timeLimit / 60f);
            int seconds = Mathf.FloorToInt(timeLimit % 60f);
            selectedDifficultyText.text = $"Time Limit: {minutes}:{seconds:00}";
            selectedDifficultyText.color = difficultyColor;
        }
    }
    
    void HighlightSelectedDifficulty(DifficultyManager.DifficultyLevel selectedDifficulty)
    {
        // Reset all button colors first
        ResetDifficultyButtonColors();
        
        // Highlight selected button
        Button selectedButton = null;
        switch (selectedDifficulty)
        {
            case DifficultyManager.DifficultyLevel.Easy: selectedButton = easyButton; break;
            case DifficultyManager.DifficultyLevel.Normal: selectedButton = normalButton; break;
            case DifficultyManager.DifficultyLevel.Hard: selectedButton = hardButton; break;
        }
        
        if (selectedButton != null)
        {
            EnhancedButton enhanced = selectedButton.GetComponent<EnhancedButton>();
            if (enhanced != null)
            {
                // Make the selected button brighter/more prominent
                Color highlightColor = DifficultyManager.instance.GetDifficultyColor();
                highlightColor.a = 1f;
                // You could add visual highlighting here
            }
        }
    }
    
    void ResetDifficultyButtonColors()
    {
        // Reset all difficulty buttons to normal appearance
        Button[] difficultyButtons = { easyButton, normalButton, hardButton };
        
        foreach (Button button in difficultyButtons)
        {
            if (button != null)
            {
                EnhancedButton enhanced = button.GetComponent<EnhancedButton>();
                if (enhanced != null)
                {
                    // Reset to default colors
                }
            }
        }
    }
    
    void SetupControlsDisplay()
    {
        if (controlsText != null)
        {
            // Set control instructions
            controlsText.text = "CONTROLS:\n\n" +
                               "↑↓ Arrow Keys - Change Lanes\n" +
                               "Space - Boost Speed\n" +
                               "Enter - Start Game\n" +
                               "Escape - Quit Game\n\n" +
                               "COLLECT:\n" +
                               "Fruits - Points\n" +
                               "Golden Fruits - Bonus Points\n" +
                               "Heart Fruits - Restore Health\n\n" +
                               "AVOID:\n" +
                               "Missiles & Cannonballs\n" +
                               "Buildings\n" +
                               "Thunder Clouds\n" +
                               "Rotten Fruits";
            
            // Setup enhanced text display for controls
            if (useEnhancedUI)
            {
                EnhancedTextDisplay controlsDisplay = controlsText.gameObject.GetComponent<EnhancedTextDisplay>();
                if (controlsDisplay == null)
                {
                    controlsDisplay = controlsText.gameObject.AddComponent<EnhancedTextDisplay>();
                    controlsDisplay.textStyle = EnhancedTextDisplay.TextStyle.Body;
                    controlsDisplay.enableShadow = true;
                }
            }
        }
        
        // Setup controls panel if it exists
        if (controlsPanel != null)
        {
            // Add theme applier to controls panel
            if (useEnhancedUI)
            {
                UIThemeApplier applier = controlsPanel.GetComponent<UIThemeApplier>();
                if (applier == null)
                {
                    applier = controlsPanel.AddComponent<UIThemeApplier>();
                    applier.applyOnStart = true;
                    applier.applyToChildren = true;
                }
            }
        }
    }
}